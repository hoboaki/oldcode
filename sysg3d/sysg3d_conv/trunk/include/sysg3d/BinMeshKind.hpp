/**
 * @file
 * @brief BinMeshKind�^���L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace sysg3d {

    /// ���b�V���̎�ށB
    enum BinMeshKind
    {
        BinMeshKind_Triangles ///< Triangles�B
        , BinMeshKind_Polylist ///< Polylist�B
        // term
        , BinMeshKind_Terminate
        , BinMeshKind_Begin = 0
        , BinMeshKind_End = BinMeshKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
