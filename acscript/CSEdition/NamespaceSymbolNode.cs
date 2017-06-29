using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 名前空間となるノード。
    /// </summary>
    class NamespaceSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public NamespaceSymbolNode(ISymbolNode aParent, Identifier aIdent)
        {
            mParent = aParent;
            mIdent = aIdent;
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // 自分自身のIdent。
        public Identifier GetIdentifier()
        {
            return mIdent;
        }

        //------------------------------------------------------------
        // ノードの種類。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.NameSpace;
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
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aCmd)
        {
            foreach (ISymbolNode node in mNodeList)
            {
                if (!node.SymbolExpand(aHolder, aCmd))
                {
                    return false;
                }
            }
            return true;
        }

        //------------------------------------------------------------
        // ノードの追加。
        public void AddNode(ISymbolNode aNode)
        {
            System.Diagnostics.Debug.Assert(
                aNode.GetNodeKind() == SymbolNodeKind.NameSpace
                || aNode.GetNodeKind() == SymbolNodeKind.Module
                );
            mNodeList.Add(aNode);
        }

        //============================================================
        ISymbolNode mParent;
        Identifier mIdent;
        SymbolNodeList mNodeList;
    }
}
