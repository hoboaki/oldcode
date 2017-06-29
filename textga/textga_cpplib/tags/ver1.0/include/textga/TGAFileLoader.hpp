/**
 * @file
 * @brief TGA�t�@�C�������[�h����N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>
#include <textga/FileLoader.hpp>
#include <textga/TGADataAccessor.hpp>

//------------------------------------------------------------
namespace textga {
    
    /// TGA�t�@�C�������[�h����N���X���L�q����B
    class TGAFileLoader
    {
    public:
        TGAFileLoader( const char* filePath );
    
        /// �t�@�C�����ǂݍ��߂����B
        bool isLoaded()const;
        
        /**
         * @brief TGAData�A�N�Z�T���擾����B
         * isLoaded() == true �̂Ƃ��̂݌Ăׂ�B
         * �A�N�Z�T���������������A����TGAFileLoader��delete���Ă͂����Ȃ��B
         */
        TGADataAccessor tgaDataAccessor()const;
        
        /// �t�@�C���̎Q�Ƃ�Ԃ��B�B
        const FileLoader& file()const;
        
    private:
        FileLoader file_;
    };
    
}
//------------------------------------------------------------
// EOF
