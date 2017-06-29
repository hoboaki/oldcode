/**
 * @file
 * @brief ��{�^���`����B
 */
#ifndef ACS_INCLUDE_TYPES
#define ACS_INCLUDE_TYPES

//------------------------------------------------------------
namespace acscript {

    /// @name ��{�^�B
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

    /// ���x��ID�B�ŏ��bit�͏��0�B
    typedef ::acscript::U16 LabelId;

    /// ByteCode��ID�B
    typedef ::acscript::U16 ByteCodeId;

    /// ������ByteCodeID�B
    enum { INVALID_BYTE_CODE_ID = 0 };

    /// �V���{��ID�B
    typedef ::acscript::U32 SymbolId;

    /// �����ȃV���{��ID�B
    enum { INVALID_SYMBOL_ID = 0 };

    /// �O���V���{��ID�B�ŏ��bit�͏��1�B
    typedef ::acscript::U32 ExtSymbolId;

}
//------------------------------------------------------------
#endif
// EOF
