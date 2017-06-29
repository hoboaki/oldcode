using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �m�[�h�̃��X�g�B
    /// </summary>
    class SymbolNodeList : IEnumerable<ISymbolNode>
    {
       //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SymbolNodeList()
        {
            mNodes = new List<ISymbolNode>();
        }

        //------------------------------------------------------------
        // �m�[�h��ǉ�����B
        public void Add(ISymbolNode node)
        {
            mNodes.Add(node);
        }

        //------------------------------------------------------------
        // �w���Ident��Node���擾����B
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
        // IEnumerable�̎����B
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
