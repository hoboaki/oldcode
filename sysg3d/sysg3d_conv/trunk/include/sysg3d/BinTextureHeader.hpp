/**
 * @file
 * @brief BinTextureHeader���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/BinTexturePixelFormat.hpp>
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /// �e�N�X�`�����L�^�����o�C�i���f�[�^�̃w�b�_�B
    struct BinTextureHeader
    {
        BinDataOffset bdoName;          ///< ���O�����񂪊i�[����Ă���A�h���X�ւ̃f�[�^�I�t�Z�b�g�l�B
        BinTexturePixelFormat pixelFormat; ///< �s�N�Z���t�H�[�}�b�g�B
        /**
         * @name �e�N�X�`���̃s�N�Z�����B
         * �����Ɋi�[����Ă���͈̂ꖇ�ڂ̃C���[�W�̃s�N�Z�����B
         * �񖇖ڈȍ~�́A2�ׂ̂����傤�i�K�Ō����Ă����B�i�~�b�v�}�b�v�ɂ��̂܂ܓK�p�ł���j
         */
        //@{
        U16 width;  ///< �e�N�X�`���̉��s�N�Z�����B
        U16 height; ///< �e�N�X�`���̏c�s�N�Z�����B
        //@}
        U32 imageCount; ///< �S�C���[�W���B
        BinDataOffset imageBDOArray; ///< �C���[�W�̐擪�A�h���X�̔z��B
    };

}
//------------------------------------------------------------
// EOF
