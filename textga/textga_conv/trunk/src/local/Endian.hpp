/**
 * @file
 * @brief エンディアンを取得する関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    class Endian
    {
    public:
        static bool isLittleEndian();
        static bool isBigEndian();
    };
}
//------------------------------------------------------------
// EOF
