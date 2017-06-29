/**
 * @file
 * @brief TexTargaの情報を取得する関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class ArgumentDataIterator;
    
    /// TexTargaの情報を取得する関数。
    class Information
    {
    public:
        static bool execute( const ArgumentDataIterator& );
    };

}
//------------------------------------------------------------
// EOF
