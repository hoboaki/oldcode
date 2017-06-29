using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        // ユニークなフルパス。Rootには無い。
        public string GetUniqueFullPath()
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
            aTracer.WriteName("RootSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var node in mNodeList)
                {
                    node.Trace(aTracer);
                }
            }
        }

        //------------------------------------------------------------
        // ノードの追加。
        public void AddNode(ISymbolNode aNode)
        {
            // Rootに追加できるのはnamespaceかmodule。
            Assert.Check(
                aNode.GetNodeKind() == SymbolNodeKind.NameSpace
                || aNode.GetNodeKind() == SymbolNodeKind.Module
                );
            mNodeList.Add(aNode);
        }

        //============================================================
        SymbolNodeList mNodeList;
    }
}
