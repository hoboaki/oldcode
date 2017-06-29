using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        // ユニークなフルパス。
        public string GetUniqueFullPath()
        {
            return SymbolNodeUtil.FullPath(this);
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
        public void SymbolExpand(SymbolExpandCmdKind aCmd)
        {
            foreach (ISymbolNode node in mNodeList)
            {
                node.SymbolExpand(aCmd);
            }
        }

        //------------------------------------------------------------
        // トレースする。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(),"NamespaceSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var entry in mNodeList)
                {
                    entry.Trace(aTracer);
                }
            }
        }

        //------------------------------------------------------------
        // ノードの追加。
        public void AddNode(ISymbolNode aNode)
        {
            Assert.Check(
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
