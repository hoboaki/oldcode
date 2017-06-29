/**
 * @file
 * @brief 拡張情報をクリアする関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class ArgumentDataIterator;
    
    /// 拡張情報をクリアする関数。
    class Clear
    {
    public:
        static bool execute( const ArgumentDataIterator& );
    };
    
}
//------------------------------------------------------------
// EOF
