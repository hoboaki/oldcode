/**
 * @file
 * @brief エンディアンに関するユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

    /// エンディアンに関するユーティリティ関数群。
    class EndianUtil
    {
    public:
        // BigEndian環境でのみスワップする。
        static u16 swapU16BE( u16 value );
        static u32 swapU32BE( u32 value );
    };
    
}
//------------------------------------------------------------
// EOF
