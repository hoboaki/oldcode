using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ノードのリスト。
    /// </summary>
    class SymbolNodeList : IEnumerable<ISymbolNode>
    {
       //------------------------------------------------------------
        // コンストラクタ。
        public SymbolNodeList()
        {
            mNodes = new List<ISymbolNode>();
        }

        //------------------------------------------------------------
        // ノードを追加する。
        public void Add(ISymbolNode node)
        {
            mNodes.Add(node);
        }

        //------------------------------------------------------------
        // 指定のIdentのNodeを取得する。
        public ISymbolNode FindNode(Identifier aIdent)
        {
            foreach (ISymbolNode node in mNodes)
            {
                if (node.GetIdentifier().IsSame(aIdent))
                {
                    return node;
                }
            }
            return null;
        }

        //------------------------------------------------------------
        // IEnumerableの実装。
        public IEnumerator<ISymbolNode> GetEnumerator()
        {
            return mNodes.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mNodes.GetEnumerator();
        }
        
        //============================================================
        List<ISymbolNode> mNodes;

    }
}
