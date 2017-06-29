/**
 * @file
 * @brief BinConstantを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /// バイナリ定数群。
    struct BinConstant
    {
        static const U32 SIGNATURE_LE = 0x4E424753; ///< シグネチャの値。SGBN(SysG3dBiNary)をLEで並べたもの。
        static const U32 SIGNATURE_BE = 0x5347424E; ///< シグネチャの値。SGBN(SysG3dBiNary)をBEで並べたもの。
        static const U8 VERSION_MAJOR = 0; ///< メジャーバージョン番号。
        static const U8 VERSION_MINOR = 6; ///< マイナーバージョン番号。
        static const U16 VERSION = VERSION_MAJOR * 0x100 + VERSION_MINOR; ///< バージョン番号。
    };

}
//------------------------------------------------------------
// EOF
