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
        static ::textga::u16 swapU16BE( ::textga::u16 value );
        static ::textga::u32 swapU32BE( ::textga::u32 value );
    };
    
}
//------------------------------------------------------------
// EOF
