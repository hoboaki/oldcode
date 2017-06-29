/**
 * @file
 * @brief ������֌W�̃��[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

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
        
        /**
         * @brief �o�C�i���f�[�^���當������擾����B
         * @return �����񂪎擾�ł���΃|�C���^�����̂܂ܕԂ��B�擾�ł��Ȃ����0��Ԃ��B
         * @param data �f�[�^�̃A�h���X�B
         * @param size �f�[�^�̃T�C�Y�B
         */
        static const char* findString( const ::textga::byte* data , size_t size );
    };

}
//------------------------------------------------------------
// EOF
