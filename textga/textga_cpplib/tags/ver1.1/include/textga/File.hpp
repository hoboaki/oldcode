/**
 * @file
 * @brief File�̃��b�p�[�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <cstdio>

//------------------------------------------------------------
namespace textga {

    /**
     * @brief File�̃��b�p�[�N���X�B
     * �R���X�g���N�^��fopen�A�f�X�g���N�^��fclose������B
     */
    class File
    {
    public:
        File( const char* filePath , const char* option );
        ~File();
        
        FILE* fp();
        
    private:
        File( const File& ); // noncopyable
        
        FILE* fp_;
    };

}
//------------------------------------------------------------
// EOF
