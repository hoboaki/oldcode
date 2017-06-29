/**
 * @file
 * @brief �G���[�\���֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// �G���[�\���֐��Q�B
    class ErrorReport
    {
    public:
        static void argumentNotEnoughError();
        static void argumentTooMuchError();
        static void argumentUnknownOptionError( const char* optionName );
        static void fileOpenError( const char* fileName );
        static void notSupportedTGAError( const char* fileName );
        static void notTexTargaError( const char* fileName );
        static void fileWriteError();
    };

}
//------------------------------------------------------------
// EOF
