/**
 * @file
 * @brief アルファチャンネルをマージする関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class ArgumentDataIterator;
    
    /// アルファチャンネルをマージする関数。
    class Merge
    {
    public:
        static bool execute( const ArgumentDataIterator& );
    };
    
}
//------------------------------------------------------------
// EOF
