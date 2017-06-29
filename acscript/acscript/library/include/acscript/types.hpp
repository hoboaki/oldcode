/**
 * @file
 * @brief 基本型を定義する。
 */
#ifndef ACS_INCLUDE_TYPES
#define ACS_INCLUDE_TYPES

//------------------------------------------------------------
namespace acscript {

    /// @name 基本型。
    //@{
    typedef bool    Bool;
    typedef float   F32;
    typedef double  F64;
    typedef char            S8;
    typedef short           S16;
    typedef int             S32;
    typedef long long int   S64;
    typedef unsigned char           U8;
    typedef unsigned short          U16;
    typedef unsigned int            U32;
    typedef unsigned long long int  U64;
    //@}

    /// ラベルID。最上位bitは常に0。
    typedef ::acscript::U16 LabelId;

    /// ByteCodeのID。
    typedef ::acscript::U16 ByteCodeId;

    /// 無効なByteCodeID。
    enum { INVALID_BYTE_CODE_ID = 0 };

    /// シンボルID。
    typedef ::acscript::U32 SymbolId;

    /// 無効なシンボルID。
    enum { INVALID_SYMBOL_ID = 0 };

    /// 外部シンボルID。最上位bitは常に1。
    typedef ::acscript::U32 ExtSymbolId;

}
//------------------------------------------------------------
#endif
// EOF
