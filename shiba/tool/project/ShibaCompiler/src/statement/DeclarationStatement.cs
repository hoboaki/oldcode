using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 宣言文。
    /// </summary>
    class DeclarationStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public DeclarationStatement(VariableDecl aVariableDecl, bool aIsConst)
        {
            mVariableDecl = aVariableDecl;
            mIsConst = aIsConst;
        }

        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // TypeInfo作成
            TypeInfo typeInfo = aComp.CreateTypeInfo(mVariableDecl.TypePath, mIsConst, false);

            // シンボルノードを作成
            VariableSymbolNode symbolNode = new VariableSymbolNode(
                aComp.PrevSymbolNode()
                , mVariableDecl.Ident
                , typeInfo
                );
            aComp.AddSymbolNode(symbolNode);

            // 評価ノードを作成
            var evaluateNode = new EvaluateNode(symbolNode,mVariableDecl.Expression());

            // 評価準備
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

            // 評価実行
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

            // 親の評価終了 
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

            // 割り当て済みシンボルノードとして登録
            aComp.AddEvaluatedSymbolNode(new EvaluatedSymbolNode(symbolNode, evaluateNode));

            // Scope終了イベントのために追加
            aComp.AddEvaluateNode(evaluateNode);
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("DeclarationStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                mVariableDecl.Trace(aTracer, "mVariableDecl");
                aTracer.WriteValue("mIsConst", mIsConst.ToString());
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(VariableSymbolNode aSymbol, IExpression aExpr)
            {
                mSymbol = aSymbol;
                mExpr = aExpr;
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
                    case EvaluateNodeEventKind.InsertOpCodeOnScopeLeave: eventInsertOpCodeOnScopeLeave(aComp); break;
                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            VariableSymbolNode mSymbol;
            IExpression mExpr;
            IEvaluateNode mExprNode;
            EvaluateInfo mEvaluateInfo;
            
            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // 型情報取得
                TypeInfo typeInfo = mSymbol.GetTypeInfo();
                
                // todo: いろんな型の対応
                // - 現時点はint,boolのみ対応
                if (typeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                    || (typeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && typeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, typeInfo.Symbol.GetToken());
                }

                // 評価情報作成
                mEvaluateInfo = EvaluateInfo.CreateAsValue(typeInfo);

                // 初期化式があるか
                if (mExpr != null)
                {// 初期化式の結果でロード命令を挟む
                    // 作成
                    mExprNode = mExpr.CreateEvaluateNode();

                    // 評価
                    mExprNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                }
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // レジスタ確保　
                mEvaluateInfo.SR = aComp.SRReserve();

                // 初期化式があるか否かの分岐
                if (mExpr == null)
                {// 初期化子がないので既定値で初期化する

                    // todo: 組み込み型以外の対応
                    if (mEvaluateInfo.TypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn)
                    {// 
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, mEvaluateInfo.TypeInfo.Symbol.GetToken());
                    }

                    // 組み込み型ならゼロ初期化
                    aComp.BCFunction.AddOPCode_SReg(
                        BCOpCode.OpType.LDSRZR
                        , mEvaluateInfo.SR
                        );
                }
                else
                {// 初期化式の結果でロード命令を挟む
                    // 評価情報を伝達
                    aComp.TransferredEvaluateInfoSet(mEvaluateInfo);

                    // 評価
                    mExprNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                    // 伝達リセット
                    aComp.TransferredEvaluateInfoReset();

                    // todo:
                    // 暗黙の変換の対応

                    // 型確認
                    Assert.Check(mExprNode.GetEvaluateInfo().Kind == EvaluateInfo.InfoKind.Value);
                    EvaluateInfo exprEI = mExprNode.GetEvaluateInfo();
                    TypeInfo exprTypeInfo = exprEI.TypeInfo;
                    if (exprTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                        || (exprTypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && exprTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                        )
                    {// todo: 今はboolとintしかサポートしない
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, exprTypeInfo.Symbol.GetToken());
                    }

                    // 型が同じか確認
                    if (mEvaluateInfo.TypeInfo.Symbol.GetBuiltInType() != exprTypeInfo.Symbol.GetBuiltInType())
                    {// 違う型
                        // エラーメッセージ
                        throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                            SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                            , aComp.TypeSymbolNode.ModuleContext()
                            , mSymbol.GetIdentifier().Token
                            , exprTypeInfo
                            , mEvaluateInfo.TypeInfo
                            ));
                    }

                    // レジスタが異なればロード命令を追加する
                    if (!mEvaluateInfo.SR.IsSame(exprEI.SR))
                    {
                        aComp.BCFunction.AddOPCode_SReg1_SReg2(
                            BCOpCode.OpType.LDSRSR
                            , mEvaluateInfo.SR
                            , exprEI.SR
                            );
                    }
                    // 親の評価が終わったことを告げる
                    mExprNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                }
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // structのデストラクタ呼び出し

                // レジスタ解放
                aComp.SRRelease(mEvaluateInfo.SR);
            }
            
            //------------------------------------------------------------
            // スコープ離脱命令を挿入。
            void eventInsertOpCodeOnScopeLeave(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // structのデストラクタ呼び出し
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち
        VariableDecl mVariableDecl;
        bool mIsConst;
    }
}
