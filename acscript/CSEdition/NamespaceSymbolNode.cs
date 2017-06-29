using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ���O��ԂƂȂ�m�[�h�B
    /// </summary>
    class NamespaceSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public NamespaceSymbolNode(ISymbolNode aParent, Identifier aIdent)
        {
            mParent = aParent;
            mIdent = aIdent;
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // �������g��Ident�B
        public Identifier GetIdentifier()
        {
            return mIdent;
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ށB
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.NameSpace;
        }

        //------------------------------------------------------------
        // �e�m�[�h�B
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // �w���Ident�̃m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return mNodeList.FindNode(aIdent);
        }

        //------------------------------------------------------------
        // �W�J���߁B
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
        // �m�[�h�̒ǉ��B
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
