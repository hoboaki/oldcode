using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
            ,ModuleContext aModuleContext
            ,MemberFunctionDecl aFunctionDecl
            )
        {
            mParent = aParent;
            mModuleContext = aModuleContext;
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
        // TypeInfo生成
        static TypeInfo createTypeInfo(
            TypePath aTP
            , bool aIsConst
            , bool aIsIn
            , bool aIsRef
            )
        {
            BuiltInType builtInType = aTP.BuiltInType;
            TypeInfo.TypeKind kind = TypeInfo.TypeKind.VALUE;
            if (builtInType == BuiltInType.Void)
            {
               kind = TypeInfo.TypeKind.UNKNOWN;
            }
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
                new TypeInfo.TypeSymbol(builtInType)
                , kind
                , new TypeInfo.TypeAttribute(aIsConst, aIsRef)
                );
        }

        //------------------------------------------------------------
        // 識別子の取得。
        public Identifier GetIdentifier()
        {
            return mFunctionDecl.Ident;
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
        // シンボルを展開する。
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aCmdKind)
        {
            if (aCmdKind != SymbolExpandCmdKind.FunctionNodeImpl)
            {// 関数実装以外なら何もしない
                return true;
            }

            mLocalVariableSymbolNodes = new List<VariableSymbolNode>();
            ISymbolNode prevNode = this;

            {// 引数
                foreach (ArgTypeInfo argTypeInfo in mArgTypeInfos)
                {
                    VariableSymbolNode node = new VariableSymbolNode(
                        prevNode
                        , argTypeInfo.Ident
                        , argTypeInfo.TypeInfo
                        );
                    prevNode = node;
                    mLocalVariableSymbolNodes.Add(node);
                }
            }

            {// Statement解析
                SemanticAnalyzeComponent comp = new SemanticAnalyzeComponent(
                    aHolder
                    , mModuleContext
                    , mParent
                    , new SemanticAnalyzeComponent.OnSymbolNodeCreate(onSymbolNodeCreate)
                    , prevNode
                    );
                if (!mFunctionDecl.Statement().SemanticAnalyze(comp))
                {
                    return false;
                }
            }

            return true;
        }

        //============================================================
        TypeSymbolNode mParent;
        ModuleContext mModuleContext;
        MemberFunctionDecl mFunctionDecl;
        TypeInfo mReturnTypeInfo;
        List<ArgTypeInfo> mArgTypeInfos;
        List<VariableSymbolNode> mLocalVariableSymbolNodes;

        //------------------------------------------------------------
        // シンボルノードを作成する。
        private bool onSymbolNodeCreate(VariableSymbolNode aNode)
        {
            // 重複チェック
            // ...

            // 追加
            mLocalVariableSymbolNodes.Add(aNode);

            return true;
        }


    }
}
