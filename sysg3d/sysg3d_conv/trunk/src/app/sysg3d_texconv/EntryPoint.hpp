/**
 * @file
 * @brief エントリーポイントを記述する。
 */
#pragma once

//------------------------------------------------------------
namespace app {
    class Argument;
}

//------------------------------------------------------------
namespace app {
namespace sysg3d_texconv {

    /// エントリーポイント関数。
    struct EntryPoint
    {
        static int run( const Argument& );
    };

}}
//------------------------------------------------------------
// EOF
