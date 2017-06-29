/**
 * @file
 * @brief ��{�^���`����B
 */
#pragma once

//------------------------------------------------------------

//=================================================
/// @name ���������^
//@{
typedef unsigned char           UInt8;
typedef unsigned short int      UInt16;
typedef unsigned int            UInt32;
typedef unsigned long long int  UInt64;
typedef unsigned char           U8;
typedef unsigned short int      U16;
typedef unsigned int            U32;
typedef unsigned long long int  U64;
typedef UInt32                  UInt; ///< 32bit���������s���ɍœK�����ꂽ�����Ȃ��̌^�B
//@}

//=================================================
/// @name �����L��^
//@{
typedef signed char             SInt8;
typedef signed short int        SInt16;
typedef signed int              SInt32;
typedef signed long long int    SInt64;
typedef signed char             S8;
typedef signed short int        S16;
typedef signed int              S32;
typedef signed long long int    S64;
typedef SInt32                  SInt; ///< 32bit���������s���ɍœK�����ꂽ��������̌^�B
//@}

//=================================================
/// @name ���������^
//@{
typedef float                   Float32;
typedef double                  Float64;
typedef float                   F32;
typedef double                  F64;
typedef Float32                 Float; ///< 32bit���������s���ɍœK�����ꂽ���������^�B
//@}

//=================================================
/// @name ������^�B
//@{
typedef const char*             ConstTmpStr; ///< �ǂݎ���p�̈ꎞ������B�ꎞ�I�Ɏg�p����֐��̈����ɓn�����Ɏg�p�B
typedef const char*             ConstStr;    ///< �ǂݎ���p�̕�����B�@
typedef const wchar_t*          ConstTmpWStr; ///< �ǂݎ���p��Unicode�ꎞ������B�ꎞ�I�Ɏg�p����֐��̈����ɓn�����Ɏg�p�B
typedef const wchar_t*          ConstWStr;    ///< �ǂݎ���p��Unicode������B�@
//@}

//============================================================
/// @name void�|�C���^�^�B
//@{
typedef const void*             ConstTmpPtr; ///< �ǂݎ���p�̈ꎞ�f�[�^�̃|�C���^�B�ꎞ�I�Ɏg�p����֐��̈����ɓn�����Ɏg�p�B
typedef const void*             ConstPtr;    ///< �ǂݎ���p�̃f�[�^�B
//@}

//=================================================
/// @name ���̑�
//@{
typedef UInt8                   Byte; ///< �o�C�g�^�B
typedef UInt32                  BinSize; ///< �o�C�i���T�C�Y�B
typedef UInt32                  BinDataOffset; ///< �f�[�^�I�t�Z�b�g�l�B����A�h���X����̃o�C�g�����̂��ƁB
//@}

//------------------------------------------------------------
// EOF
