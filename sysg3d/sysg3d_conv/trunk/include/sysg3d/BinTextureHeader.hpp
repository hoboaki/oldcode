/**
 * @file
 * @brief BinTextureHeaderを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/BinTexturePixelFormat.hpp>
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /// テクスチャを記録したバイナリデータのヘッダ。
    struct BinTextureHeader
    {
        BinDataOffset bdoName;          ///< 名前文字列が格納されているアドレスへのデータオフセット値。
        BinTexturePixelFormat pixelFormat; ///< ピクセルフォーマット。
        /**
         * @name テクスチャのピクセル数。
         * ここに格納されているのは一枚目のイメージのピクセル数。
         * 二枚目以降は、2のべきじょう段階で減っていく。（ミップマップにそのまま適用できる）
         */
        //@{
        U16 width;  ///< テクスチャの横ピクセル数。
        U16 height; ///< テクスチャの縦ピクセル数。
        //@}
        U32 imageCount; ///< 全イメージ数。
        BinDataOffset imageBDOArray; ///< イメージの先頭アドレスの配列。
    };

}
//------------------------------------------------------------
// EOF
