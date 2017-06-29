/**
 * @file
 * @brief �G���[�`�F�b�N�֐��Q���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace textga {
    class TGAFileLoader;
}

//------------------------------------------------------------
namespace local {

    /// �G���[�`�F�b�N�֐��Q�B
    class ErrorCheck
    {
    public:
        static bool supportedTgaCheck( const textga::TGAFileLoader& , const char* filePath);
        static bool texTargaCheck( const textga::TGAFileLoader&  , const char* filePath );
    };

}
//------------------------------------------------------------
// EOF
