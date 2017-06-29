/**
 * @file
 * @brief ������֌W�̃��[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>

//------------------------------------------------------------
namespace local {

    /// ������֌W�̃��[�e�B���e�B�֐��B
    class StringUtil
    {
    public:
        /// ���������B
        static bool equals( const char* str1 , const char* str2 );
        
        /// �啶���A�������֌W�Ȃ����������B
        static bool caseEquals( const char* str1 , const char* str2 );
        
        /// �t�@�C���p�X����g���q���O�����t�@�C�������擾����B
        static std::string baseName( const char* filepath );
    };

}
//------------------------------------------------------------
// EOF
