using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Variable�m�[�h�B
    /// </summary>
    class VariableSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public VariableSymbolNode(
            ISymbolNode aParent
            , Identifier aIdent
            , TypeInfo aTypeInfo
            )
        {
            mParent = aParent;
            mIdent = aIdent;
            mTypeInfo = aTypeInfo;
        }

        //------------------------------------------------------------
        // ���ʎq�̎擾�B
        public Identifier GetIdentifier()
        {
            return mIdent;
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ނ̎擾�B
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Variable;
        }

        //------------------------------------------------------------
        // �e�m�[�h�̎擾�B
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // �q�m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return null; // �q�m�[�h�͂Ȃ��B
        }

        //------------------------------------------------------------
        // �V���{����W�J����B
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aCmdKind)
        {
            // todo:impl
            return true;
        }

        //============================================================
        ISymbolNode mParent;
        Identifier mIdent;
        TypeInfo mTypeInfo;
    }
}
