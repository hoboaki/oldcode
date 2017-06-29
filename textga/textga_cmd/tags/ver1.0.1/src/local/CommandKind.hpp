/**
 * @file
 * @brief �R�}���h�̎�ނ��������ʎq���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// �R�}���h�̎�ނ��������ʎq�B
    enum CommandKind
    {
        CommandKind_Invalid         ///< �s���Ȉ����B
        
        ,CommandKind_Convert        ///< �ϊ��B
        ,CommandKind_Clear          ///< �g�����̍폜�B
        ,CommandKind_Help           ///< �w���v�̕\���B
        ,CommandKind_Information    ///< ���B
        ,CommandKind_Merge          ///< �A���t�@�`�����l���̃}�[�W�B
        ,CommandKind_Revert         ///< ���ɖ߂��B
        
        ,CommandKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
