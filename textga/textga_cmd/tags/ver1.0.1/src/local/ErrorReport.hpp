/**
 * @file
 * @brief エラー表示関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// エラー表示関数群。
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
