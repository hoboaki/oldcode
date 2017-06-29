using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �V���{���m�[�h�̃C���^�[�t�F�[�X�N���X�B
    /// </summary>
    interface ISymbolNode
    {
        //------------------------------------------------------------
        // ���ʎq�̎擾�B
        Identifier GetIdentifier();

        //------------------------------------------------------------
        // �m�[�h�̎�ނ��擾�B
        SymbolNodeKind GetNodeKind();

        //------------------------------------------------------------
        // �e�m�[�h���擾�B
        ISymbolNode ParentNode();

        //------------------------------------------------------------
        // �w��̎��ʎq�̎q�m�[�h��T���B
        ISymbolNode FindChildNode(Identifier aIdent);

        //------------------------------------------------------------
        // �V���{����W�J����B
        bool SymbolExpand( SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aKind);
    }
}
