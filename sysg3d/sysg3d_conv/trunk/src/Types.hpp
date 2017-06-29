/**
 * @file
 * @brief 基本型を定義する。
 */
#pragma once

//------------------------------------------------------------

//=================================================
/// @name 符号無し型
//@{
typedef unsigned char           UInt8;
typedef unsigned short int      UInt16;
typedef unsigned int            UInt32;
typedef unsigned long long int  UInt64;
typedef unsigned char           U8;
typedef unsigned short int      U16;
typedef unsigned int            U32;
typedef unsigned long long int  U64;
typedef UInt32                  UInt; ///< 32bitが扱え実行環境に最適化された符号なしの型。
//@}

//=================================================
/// @name 符号有り型
//@{
typedef signed char             SInt8;
typedef signed short int        SInt16;
typedef signed int              SInt32;
typedef signed long long int    SInt64;
typedef signed char             S8;
typedef signed short int        S16;
typedef signed int              S32;
typedef signed long long int    S64;
typedef SInt32                  SInt; ///< 32bitが扱え実行環境に最適化された符号ありの型。
//@}

//=================================================
/// @name 浮動小数型
//@{
typedef float                   Float32;
typedef double                  Float64;
typedef float                   F32;
typedef double                  F64;
typedef Float32                 Float; ///< 32bitが扱え実行環境に最適化された浮動小数型。
//@}

//=================================================
/// @name 文字列型。
//@{
typedef const char*             ConstTmpStr; ///< 読み取り専用の一時文字列。一時的に使用する関数の引数に渡す時に使用。
typedef const char*             ConstStr;    ///< 読み取り専用の文字列。　
typedef const wchar_t*          ConstTmpWStr; ///< 読み取り専用のUnicode一時文字列。一時的に使用する関数の引数に渡す時に使用。
typedef const wchar_t*          ConstWStr;    ///< 読み取り専用のUnicode文字列。　
//@}

//============================================================
/// @name voidポインタ型。
//@{
typedef const void*             ConstTmpPtr; ///< 読み取り専用の一時データのポインタ。一時的に使用する関数の引数に渡す時に使用。
typedef const void*             ConstPtr;    ///< 読み取り専用のデータ。
//@}

//=================================================
/// @name その他
//@{
typedef UInt8                   Byte; ///< バイト型。
typedef UInt32                  BinSize; ///< バイナリサイズ。
typedef UInt32                  BinDataOffset; ///< データオフセット値。あるアドレスからのバイト距離のこと。
//@}

//------------------------------------------------------------
// EOF
