/**
 * @file
 * @brief SymbolInfo�^���L�q����B
 */
#ifndef ACS_INCLUDE_SYMBOLINFO
#define ACS_INCLUDE_SYMBOLINFO

//------------------------------------------------------------
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /// �V���{�����B
    struct SymbolInfo
    {
        union
        {
            SymbolId id; ///< �V���{��ID�B�ŏ��bit��0�Ȃ�internal symbol�B
            struct 
            {
                LabelId labelId; ///< ���x��ID�B
                ByteCodeId byteCodeId; ///< �V���{������������ByteCode��Id�B
            } internal;
            struct 
            {
                ExtSymbolId extSymbolId; ///< �O���V���{��Id�B
            } external;
        };

        /// �L���ȃV���{����񂩁B
        bool isValidInfo()const
        {
            return id != INVALID_SYMBOL_ID;
        }

        /// �����V���{�����B
        bool isInternalSymbol()const
        {
            return ( 0x80000000 & id ) == 0;
        }
        /// @name �����V���{���p�֐��Q�B
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

        /// �O���V���{�����B
        bool isExternalSymbol()const
        {
            return !isInternalSymbol();
        }
        /// @name �O���V���{���p�֐��Q�B
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
