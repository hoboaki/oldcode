using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Functionノード。
    /// </summary>
    class FunctionSymbolNode : ISymbolNode
    {
        /// <summary>
        /// 引数の型情報。
        /// </summary>
        class ArgTypeInfo
        {
            public readonly Identifier Ident;
            public readonly TypeInfo TypeInfo;

            //------------------------------------------------------------
            // コンストラクタ。
            public ArgTypeInfo(Identifier aIdent, TypeInfo aTypeInfo)
            {
                Ident = aIdent;
                TypeInfo = aTypeInfo;
            }
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionSymbolNode(
            TypeSymbolNode aParent
            ,BCObjectType aBCObjectType
            ,MemberFunctionDecl aFunctionDecl
            )
        {
            // メンバ初期化
            mParent = aParent;
            mBCObjectType = aBCObjectType;
            mFunctionDecl = aFunctionDecl;

            // TypeInfo生成
            {// 戻り値
                FunctionReturnValueDecl retValDecl = aFunctionDecl.ReturnValueDecl;
                mReturnTypeInfo = createTypeInfo(
                    retValDecl.TypePath
                    , retValDecl.IsConst
                    , false // isIn
                    , retValDecl.IsRef
                    );
            }
            {// 引数
                mArgTypeInfos = new List<ArgTypeInfo>();
                foreach (FunctionArgumentDecl argDecl in aFunctionDecl.ArgDeclList)
                {
                    mArgTypeInfos.Add(new ArgTypeInfo(argDecl.Ident, createTypeInfo(
                        argDecl.TypePath
                        , argDecl.IsConst
                        , argDecl.IsIn
                        , argDecl.IsRef
                        )));
                }
            }
        }

        //------------------------------------------------------------
        // 識別子の取得。
        public Identifier GetIdentifier()
        {
            return mFunctionDecl.Ident;
        }

        //------------------------------------------------------------
        // ユニークなフルパス。
        public string GetUniqueFullPath()
        {
            // 普通のパス
            string path = SymbolNodeUtil.FullPath(this);

            // 引数
            path += "(";
            foreach (var entry in mArgTypeInfos)
            {
                // todo:
            }
            path += ")";

            // const
            if (mFunctionDecl.IsConst())
            {
                path += "const";
            }

            // 結果を返す
            return path;
        }

        //------------------------------------------------------------
        // ノードの種類に取得。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Function;
        }

        //------------------------------------------------------------
        // 親ノードを取得。
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // 子ノードを検索する。
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return null;
        }

        //------------------------------------------------------------
        // 戻り値のTypeInfoを取得する。
        public TypeInfo ReturnTypeInfo()
        {
            return mReturnTypeInfo;
        }

        //------------------------------------------------------------
        // シンボルを展開する。
        public void SymbolExpand(SymbolExpandCmdKind aCmdKind)
        {
            if (aCmdKind != SymbolExpandCmdKind.FunctionNodeImpl)
            {// 関数実装以外なら何もしない
                return;
            }

            // BCFunction作成
            mBCFunction = mBCObjectType.GenerateFunction(this);

            // コンポーネント
            SemanticAnalyzeComponent comp = new SemanticAnalyzeComponent(
                mBCFunction
                , mParent
                , this
                );

            // コピーするレジスタの数をメモする変数
            byte copyRegCount = 0;

            {// 関数内部のスコープ
                // スコープ追加
                comp.ScopeEnter();

                // 戻り値対応
                if (mReturnTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                    || mReturnTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Void
                    )
                {// void以外なら。

                    // todo: いろんな型の対応
                    // 組み込み型のint,boolしか対応していません。
                    if (mReturnTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                        || (mReturnTypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && mReturnTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                        )
                    {
                        comp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, mReturnTypeInfo.Symbol.GetToken());
                    }

                    // EI作成
                    var returnEI = EvaluateInfo.CreateAsValue(mReturnTypeInfo);

                    // SRを確保
                    returnEI.SR = comp.SRReserve();

                    // 戻り値として登録　
                    comp.ReturnEvaluateInfoSet(returnEI);

                    // コピーするレジスタカウントアップ
                    ++copyRegCount;
                }

                // this
                if (!mFunctionDecl.IsStatic())
                {
                    // TypeInfo作成
                    var ti = comp.CreateTypeInfo(
                        new TypePath(new IdentPath(mParent))
                        , mFunctionDecl.IsConst()
                        , true
                        );

                    // ダミートークンを作成
                    var token = new Token();
                    token.Value = Token.Kind.KeyThis;
                    token.pos = GetIdentifier().Token.pos;
                    token.posColumn = GetIdentifier().Token.posColumn;
                    token.posLine = GetIdentifier().Token.posLine;

                    // シンボルノードを作成
                    VariableSymbolNode symbolNode = new VariableSymbolNode(
                        comp.PrevSymbolNode()
                        , new Identifier(token)
                        , ti
                        );

                    // ノードを追加
                    comp.AddSymbolNode(symbolNode);

                    // 評価ノードを作成
                    var evaluateNode = new EvaluateNode(symbolNode);

                    // 評価イベント送信
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Analyze);
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Evaluate);

                    // 追加
                    comp.AddEvaluatedSymbolNode(new EvaluatedSymbolNode(symbolNode, evaluateNode));

                    // コピーするレジスタカウントアップ
                    ++copyRegCount;
                }

                // 引数
                foreach (var arg in mArgTypeInfos)
                {
                    // シンボルノードを作成
                    VariableSymbolNode symbolNode = new VariableSymbolNode(
                        comp.PrevSymbolNode()
                        , arg.Ident
                        , arg.TypeInfo
                        );

                    // ノードを追加
                    comp.AddSymbolNode(symbolNode);

                    // 評価ノードを作成
                    var evaluateNode = new EvaluateNode(symbolNode);

                    // 評価イベント送信
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Analyze);
                    evaluateNode.SendEvent(comp, EvaluateNodeEventKind.Evaluate);

                    // 追加
                    comp.AddEvaluatedSymbolNode(new EvaluatedSymbolNode(symbolNode, evaluateNode));

                    // コピーするレジスタカウントアップ
                    ++copyRegCount;
                }

                {// Statement
                    // スコープに入る
                    comp.ScopeEnter();

                    // Returnラベル確保・登録
                    BCLabel labelReturn = comp.BCFunction.LabelCreate();
                    comp.RegisterLabelReturn(labelReturn);

                    // 解析
                    mFunctionDecl.Statement().SemanticAnalyze(comp);

                    // スコープから出る
                    comp.ScopeLeave();

                    // Returnラベル挿入
                    comp.BCFunction.LabelInsert(labelReturn);
                }

                // スコープ終了
                comp.ScopeLeave();
            }

            // 関数命令を追加
            // todo: レジスタ使いすぎチェック
            mBCFunction.PushFrontOPCode_CU1_CU1(BCOpCode.OpType.FENTER, (byte)comp.SRPeakCount(), copyRegCount);
            mBCFunction.AddOPCode_CU1(BCOpCode.OpType.FLEAVE, (byte)comp.SRPeakCount());

            // ラベル解決
            mBCFunction.LabelResolve();
        }

        //------------------------------------------------------------
        // トレースする。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(),"FunctionSymbolNode");
        }

        //============================================================

        //------------------------------------------------------------
        // 戻り値、this、引数用評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(VariableSymbolNode aSymbol)
            {
                mSymbol = aSymbol;
            }

            //------------------------------------------------------------
            // TypeInfoを取得する。
            public EvaluateInfo GetEvaluateInfo()
            {
                return mEvaluateInfo;
            }

            //------------------------------------------------------------
            // 評価イベントを送信する。
            public void SendEvent(SemanticAnalyzeComponent aComp, EvaluateNodeEventKind aEventKind)
            {
                switch (aEventKind)
                {
                    case EvaluateNodeEventKind.Analyze: eventAnalyze(aComp); break;
                    case EvaluateNodeEventKind.Evaluate: eventEvaluate(aComp); break;
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;
                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            VariableSymbolNode mSymbol;
            EvaluateInfo mEvaluateInfo;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // todo:いろいろ対応してません･･･。

                // 型情報取得
                TypeInfo typeInfo = mSymbol.GetTypeInfo();

                // 評価情報作成
                mEvaluateInfo = EvaluateInfo.CreateAsValue(typeInfo);
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // レジスタ確保　
                mEvaluateInfo.SR = aComp.SRReserve();

                // 再利用禁止
                mEvaluateInfo.DisableReuseSR();
            }


            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // レジスタ解放
                aComp.SRRelease(mEvaluateInfo.SR);
            }
        };

        //------------------------------------------------------------
        TypeSymbolNode mParent;
        BCObjectType mBCObjectType;
        MemberFunctionDecl mFunctionDecl;
        TypeInfo mReturnTypeInfo;
        List<ArgTypeInfo> mArgTypeInfos;
        BCFunction mBCFunction;

        //------------------------------------------------------------
        // TypeInfo生成
        static TypeInfo createTypeInfo(
            TypePath aTP
            , bool aIsConst
            , bool aIsIn
            , bool aIsRef
            )
        {
            // ビルトインタイプ
            BuiltInType builtInType = aTP.BuiltInType;
            Assert.Check(builtInType != BuiltInType.Unknown); // todo: 組み込み型以外の対応

            // inキーワード対策
            if (aIsIn)
            {
                if (aTP.BuiltInType == BuiltInType.Unknown)
                {
                    aIsConst = true;
                    aIsRef = true;
                }
                else
                {
                    aIsConst = true;
                }
            }
            return new TypeInfo(
                new TypeInfo.TypeSymbol(aTP.BuiltInToken, builtInType)
                , new TypeInfo.TypeAttribute(aIsConst, aIsRef)
                );
        }
    }
}
