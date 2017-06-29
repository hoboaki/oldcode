/**
 * @file
 * @brief BinMeshKind型を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace sysg3d {

    /// メッシュの種類。
    enum BinMeshKind
    {
        BinMeshKind_Triangles ///< Triangles。
        , BinMeshKind_Polylist ///< Polylist。
        // term
        , BinMeshKind_Terminate
        , BinMeshKind_Begin = 0
        , BinMeshKind_End = BinMeshKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
