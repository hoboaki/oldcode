/**
 * @file
 * @brief BinMeshHeader���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace sysg3d {

    /// ���b�V�����L�^�����o�C�i���f�[�^�̃w�b�_�B
    struct BinMeshHeader
    {
        BinDataOffset bdoName;          ///< ���O�����񂪊i�[����Ă���A�h���X�ւ̃f�[�^�I�t�Z�b�g�l�B
        U32 meshKind;                   ///< ���b�V���̎�ށBBinMeshKind.hpp���Q�ƁB
        U32 primitiveCount;             ///< �v���~�e�B�u�̐��B�v�f���z��̒����B
        BinDataOffset bdoCount;         ///< �C���f�b�N�X�z��̊e�z��ɑΉ����钷���i�v�f���j�z��ւ̃f�[�^�I�t�Z�b�g�l�B
        BinDataOffset bdoIndexBDO;      ///< �C���f�b�N�X�z��ւ̃f�[�^�I�t�Z�b�g�l�BBDO��primitiveCount������ł���B
        BinDataOffset bdoPos;           ///< ���_�ʒu�z��ւ̃f�[�^�I�t�Z�b�g�l�Bxyz�̏��Af32�^�ŕ���ł���B
        BinDataOffset bdoNormal;        ///< ���_�@���z��ւ̃f�[�^�I�t�Z�b�g�l�Bxyz�̏��Af32�^�ŕ���ł���B
        BinDataOffset bdoColor;         ///< ���_�J���[�z��ւ̃f�[�^�I�t�Z�b�g�l�Brgba�̏��Af32�^�ŕ���ł���B
        //BinDataOffset bdoTexCoordBDO;   ///< �e�N�X�`�����W�z��ւ̃f�[�^�I�t�Z�b�g�l�Bst�̏��Bf32�^�ŕ���ł���B
    };

}
//------------------------------------------------------------
// EOF
