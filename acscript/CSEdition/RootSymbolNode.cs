using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ルートとなるノード。
    /// </summary>
    class RootSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public RootSymbolNode()
        {
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // 自分自身のIdent。Rootには無い。
        public Identifier GetIdentifier()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // ノードの種類。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Root;
        }

        //------------------------------------------------------------
        // 親ノード。Rootには無い。
        public ISymbolNode ParentNode()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // 指定のIdentのノードを探す。
        public ISymbolNode FindChildNode(Identifier ident)
        {
            return mNodeList.FindNode(ident);
        }

        //------------------------------------------------------------
        // 展開命令
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aErrorHolder, SymbolExpandCmdKind aCmd)
        {
            foreach (ISymbolNode node in mNodeList)
            {
                if (!node.SymbolExpand(aErrorHolder,aCmd))
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
            // Rootに追加できるのはnamespaceかmodule。
            System.Diagnostics.Debug.Assert(
                aNode.GetNodeKind() == SymbolNodeKind.NameSpace
                || aNode.GetNodeKind() == SymbolNodeKind.Module
                );
            mNodeList.Add(aNode);
        }

        //============================================================
        SymbolNodeList mNodeList;
    }
}
