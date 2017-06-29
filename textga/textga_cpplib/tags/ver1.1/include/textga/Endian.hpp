/**
 * @file
 * @brief エンディアンを取得する関数を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace textga {

    /// エンディアンを取得する関数を記述する。
    class Endian
    {
    public:
        static bool isLittleEndian(); ///< LEか。
        static bool isBigEndian();    ///< BEか。
    };
    
}
//------------------------------------------------------------
// EOF
