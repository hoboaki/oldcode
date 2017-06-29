/**
 * @file
 * @brief BinKihd.hpp�̎������L�q����B
 */
#pragma once

//------------------------------------------------------------
namespace sysg3d {

    /// �o�C�i���f�[�^�̎�ނ������l�B
    enum BinKind
    {
        BinKind_Unknown ///< �s���ȃf�[�^�B
        , BinKind_Mesh ///< Mesh�f�[�^�B
        , BinKind_Texture ///< Texture�f�[�^�B
        // term
        , BinKind_Terminate
        , BinKind_Begin = 0
        , BinKind_End = BinKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
