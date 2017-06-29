/**
 * @file
 * @brief BinMeshHeaderを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /// メッシュを記録したバイナリデータのヘッダ。
    struct BinMeshHeader
    {
        BinDataOffset bdoName;          ///< 名前文字列が格納されているアドレスへのデータオフセット値。
        U32 meshKind;                   ///< メッシュの種類。BinMeshKind.hppを参照。
        U32 primitiveCount;             ///< プリミティブの数。要素数配列の長さ。
        BinDataOffset bdoCount;         ///< インデックス配列の各配列に対応する長さ（要素数）配列へのデータオフセット値。
        BinDataOffset bdoIndexBDO;      ///< インデックス配列へのデータオフセット値。BDOがprimitiveCount数並んでいる。
        BinDataOffset bdoPos;           ///< 頂点位置配列へのデータオフセット値。xyzの順、f32型で並んでいる。
        BinDataOffset bdoNormal;        ///< 頂点法線配列へのデータオフセット値。xyzの順、f32型で並んでいる。
        BinDataOffset bdoColor;         ///< 頂点カラー配列へのデータオフセット値。rgbaの順、f32型で並んでいる。
        //BinDataOffset bdoTexCoordBDO;   ///< テクスチャ座標配列へのデータオフセット値。stの順。f32型で並んでいる。
    };

}
//------------------------------------------------------------
// EOF
