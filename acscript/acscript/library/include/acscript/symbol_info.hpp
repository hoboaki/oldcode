/**
 * @file
 * @brief SymbolInfo型を記述する。
 */
#ifndef ACS_INCLUDE_SYMBOLINFO
#define ACS_INCLUDE_SYMBOLINFO

//------------------------------------------------------------
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /// シンボル情報。
    struct SymbolInfo
    {
        union
        {
            SymbolId id; ///< シンボルID。最上位bitが0ならinternal symbol。
            struct 
            {
                LabelId labelId; ///< ラベルID。
                ByteCodeId byteCodeId; ///< シンボルが所属するByteCodeのId。
            } internal;
            struct 
            {
                ExtSymbolId extSymbolId; ///< 外部シンボルId。
            } external;
        };

        /// 有効なシンボル情報か。
        bool isValidInfo()const
        {
            return id != INVALID_SYMBOL_ID;
        }

        /// 内部シンボルか。
        bool isInternalSymbol()const
        {
            return ( 0x80000000 & id ) == 0;
        }
        /// @name 内部シンボル用関数群。
        //@{
        LabelId labelId()const
        {
            return internal.labelId;
        }
        ByteCodeId byteCodeId()const
        {
            return internal.byteCodeId;
        }
        //@}

        /// 外部シンボルか。
        bool isExternalSymbol()const
        {
            return !isInternalSymbol();
        }
        /// @name 外部シンボル用関数群。
        //@{
        ExtSymbolId extSymbolId()const
        {
            return external.extSymbolId;
        }
        //@}
    };

}
//------------------------------------------------------------
#endif
// EOF
