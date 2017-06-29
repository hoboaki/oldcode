/**
 * @file
 * @brief BinCommonHeader���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /**
     * @brief �o�C�i���f�[�^���ʂ̃w�b�_�B
     *
     * i386 or ppc���ň�����悤�ɒl��ێ����Ă���B
     */
    struct BinCommonHeader
    {
        U32 signature; ///< SGBN��4�����B
        U16 version;   ///< �o�[�W�����ԍ��B
        U16 kind;      ///< �t�@�C���̎�ށBBinKind.hpp�Q�ƁB
    };

}
//------------------------------------------------------------
// EOF
