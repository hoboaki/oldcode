/**
 * @file
 * @brief BinKihd.hppの実装を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace sysg3d {

    /// バイナリデータの種類を示す値。
    enum BinKind
    {
        BinKind_Unknown ///< 不明なデータ。
        , BinKind_Mesh ///< Meshデータ。
        , BinKind_Texture ///< Textureデータ。
        // term
        , BinKind_Terminate
        , BinKind_Begin = 0
        , BinKind_End = BinKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
