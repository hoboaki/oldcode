using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ���[�g�ƂȂ�m�[�h�B
    /// </summary>
    class RootSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public RootSymbolNode()
        {
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // �������g��Ident�BRoot�ɂ͖����B
        public Identifier GetIdentifier()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ށB
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Root;
        }

        //------------------------------------------------------------
        // �e�m�[�h�BRoot�ɂ͖����B
        public ISymbolNode ParentNode()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // �w���Ident�̃m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier ident)
        {
            return mNodeList.FindNode(ident);
        }

        //------------------------------------------------------------
        // �W�J����
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
        // �m�[�h�̒ǉ��B
        public void AddNode(ISymbolNode aNode)
        {
            // Root�ɒǉ��ł���̂�namespace��module�B
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
