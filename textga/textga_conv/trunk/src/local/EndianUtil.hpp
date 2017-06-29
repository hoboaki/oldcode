/**
 * @file
 * @brief エンディアンに関するユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// エンディアンに関するユーティリティ関数群。
    class EndianUtil
    {
    public:
        // BigEndian環境でのみスワップする。
        static short swapS16BE( short value );
        static int   swapS32BE( int value );
    };
}
//------------------------------------------------------------
// EOF
