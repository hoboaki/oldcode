/**
 * @file
 * @brief BinConstant���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /// �o�C�i���萔�Q�B
    struct BinConstant
    {
        static const U32 SIGNATURE_LE = 0x4E424753; ///< �V�O�l�`���̒l�BSGBN(SysG3dBiNary)��LE�ŕ��ׂ����́B
        static const U32 SIGNATURE_BE = 0x5347424E; ///< �V�O�l�`���̒l�BSGBN(SysG3dBiNary)��BE�ŕ��ׂ����́B
        static const U8 VERSION_MAJOR = 0; ///< ���W���[�o�[�W�����ԍ��B
        static const U8 VERSION_MINOR = 6; ///< �}�C�i�[�o�[�W�����ԍ��B
        static const U16 VERSION = VERSION_MAJOR * 0x100 + VERSION_MINOR; ///< �o�[�W�����ԍ��B
    };

}
//------------------------------------------------------------
// EOF
