using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Typeノード。
    /// </summary>
    class TypeSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public TypeSymbolNode(ISymbolNode aParent, ModuleContext aModuleContext, StaticTypeDef aStaticTypeDef)
        {
            mParent = aParent;
            mModuleContext = aModuleContext;
            mStaticTypeDef = aStaticTypeDef;
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // 自分自身のIdent。
        public Identifier GetIdentifier()
        {
            return mStaticTypeDef.Ident;
        }

        //------------------------------------------------------------
        // ノードの種類。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Type;
        }

        //------------------------------------------------------------
        // 親ノード。
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // 指定のIdentのノードを探す。
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return mNodeList.FindNode(aIdent);
        }

        //------------------------------------------------------------
        // 展開命令。
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aErrorHolder, SymbolExpandCmdKind aCmd)
        {
            switch (aCmd)
            {
                case SymbolExpandCmdKind.TypeNode:
                    return expandTypeNode(aErrorHolder);

                case SymbolExpandCmdKind.FunctionNodeDecl:
                    return expandFunctionNodeDecl(aErrorHolder);

                default:
                    return true;
            }
        }

        //============================================================
        ISymbolNode mParent;
        ModuleContext mModuleContext;
        StaticTypeDef mStaticTypeDef;
        SymbolNodeList mNodeList;

        //------------------------------------------------------------
        // TypeNode展開。
        private bool expandTypeNode(SymbolTree.ErrorInfoHolder aErrorHolder)
        {
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.StaticTypeDef != null)
                {
                    if (checkIdentDuplication(aErrorHolder, symbol.StaticTypeDef.Ident))
                    {
                        return false;
                    }
                    addNode(new TypeSymbolNode(this, mModuleContext, symbol.StaticTypeDef));
                }
            }
            return true;
        }

        //------------------------------------------------------------
        // FunctionNode宣言展開。
        private bool expandFunctionNodeDecl(SymbolTree.ErrorInfoHolder aErrorHolder)
        {
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.MemberFunctionDecl != null)
                {
                    if (checkIdentDuplication(aErrorHolder, symbol.MemberFunctionDecl.Ident))
                    {
                        return false;
                    }
                    addNode(new FunctionSymbolNode(this, mModuleContext, symbol.MemberFunctionDecl));
                }
            }
            return true;
        }

        //------------------------------------------------------------
        // Ident重複チェック。trueなら重複している。
        private bool checkIdentDuplication(SymbolTree.ErrorInfoHolder aErrorHolder, Identifier aIdent)
        {
            if (FindChildNode(aIdent) != null)
            {
                aErrorHolder.Set(new SymbolTree.ErrorInfo(SymbolTree.ErrorKind.NODE_NAME_IS_ALREADY_EXIST, mModuleContext, aIdent.Token));
                return true;
            }
            return false;
        }

        //------------------------------------------------------------
        // ノードの追加。
        private void addNode(ISymbolNode aNode)
        {
            System.Diagnostics.Debug.Assert(
                aNode.GetNodeKind() == SymbolNodeKind.Type
                || aNode.GetNodeKind() == SymbolNodeKind.Variable
                || aNode.GetNodeKind() == SymbolNodeKind.Function
                );
            mNodeList.Add(aNode);
        }       
    }
}
