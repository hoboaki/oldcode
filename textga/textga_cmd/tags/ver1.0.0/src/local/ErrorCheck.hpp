/**
 * @file
 * @brief エラーチェック関数群を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace textga {
    class TGAFileLoader;
}

//------------------------------------------------------------
namespace local {

    /// エラーチェック関数群。
    class ErrorCheck
    {
    public:
        static bool supportedTgaCheck( const textga::TGAFileLoader& , const char* filePath);
        static bool texTargaCheck( const textga::TGAFileLoader&  , const char* filePath );
    };

}
//------------------------------------------------------------
// EOF
