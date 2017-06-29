/**
 * @file
 * @brief BinCommonHeaderを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /**
     * @brief バイナリデータ共通のヘッダ。
     *
     * i386 or ppc環境で扱えるように値を保持している。
     */
    struct BinCommonHeader
    {
        U32 signature; ///< SGBNの4文字。
        U16 version;   ///< バージョン番号。
        U16 kind;      ///< ファイルの種類。BinKind.hpp参照。
    };

}
//------------------------------------------------------------
// EOF
