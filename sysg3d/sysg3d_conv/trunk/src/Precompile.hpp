/**
 * @file
 * @brief �v���R���p�C���w�b�_�B
 */
#pragma once

//------------------------------------------------------------
#include <assert.h>
#include <boost/static_assert.hpp>
#include <cstdio>
#include <dae.h>
#include <dom/domCOLLADA.h>
#include <sysg3d/SysG3D.hpp>
#include "Types.hpp"

/// �A�T�[�g�B
#define PJ_ASSERT assert

/// �W���o�́B
#define PJ_COUT std::printf

/// �R���p�C�����̃A�T�[�g�B
#define PJ_COMPILE_ASSERT( cmd ) BOOST_STATIC_ASSERT( cmd )

/// �z��̒������擾����}�N���B
#define PJ_ARRAY_LENGTH( obj ) ( sizeof(obj)/sizeof(obj[0]) )

/// �w��̔z��̒������A�w��̒������`�F�b�N����B
#define PJ_ARRAY_LENGTH_CHECK( arr , len ) PJ_COMPILE_ASSERT( PJ_ARRAY_LENGTH( arr ) == len )

//------------------------------------------------------------
// EOF
