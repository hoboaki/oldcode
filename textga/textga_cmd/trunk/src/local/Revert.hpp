/**
 * @file
 * @brief TexTargaのSrcDataを使って元に戻る関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class ArgumentDataIterator;
    
    /// TexTargaのSrcDataを使って元に戻る関数。
    class Revert
    {
    public:
        static bool execute( const ArgumentDataIterator& );
    };

}
//------------------------------------------------------------
// EOF
