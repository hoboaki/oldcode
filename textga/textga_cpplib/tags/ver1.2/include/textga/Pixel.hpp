/**
 * @file
 * @brief １つのピクセルを表す構造体を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

    /// １つのピクセルを表す構造体。
    struct Pixel
    {
        ::textga::u8 r;
        ::textga::u8 g;
        ::textga::u8 b;
        ::textga::u8 a;
    };
    
}
//------------------------------------------------------------
// EOF
