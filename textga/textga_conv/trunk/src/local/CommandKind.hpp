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
        CommandKind_Invalid         ///< �s���ȃR�}���h�B
        ,CommandKind_PrintUseage    ///< �w���v�̕\���B
        ,CommandKind_Convert        ///< �ϊ��B
        ,CommandKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
