/**
 * @file
 * @brief ByteCode�^���L�q����B
 */
#ifndef ACS_INCLUDE_BYTECODE
#define ACS_INCLUDE_BYTECODE

//------------------------------------------------------------
#include <acscript/container.hpp>
#include <acscript/macro.hpp>
#include <acscript/symbol_info.hpp>
#include <acscript/types.hpp>

//------------------------------------------------------------
namespace acscript {

    /**
     * @brief �P�̃o�C�g�R�[�h�B
     *
     * �o�C�g�R�[�h�̃��C�A�E�g�B
     * |------------------------------------------------
     * | Header           
     * | localSymbolOffsetTableSection - U32�̔z��BHeader�̐擪����̃I�t�Z�b�g�l�̔z��B
     * | ...
     * | globalSymbolInfoTableSection  - SymbolInfo�̔z��B
     * | ...
     * | dataSection - �f�[�^�Z�N�V�����B
     * | ...
     * | textSection - �e�L�X�g�Z�N�V�����B
     * | ...
     * | End Of Data
     * |------------------------------------------------
     *
     * dataSection�͈�J���ɂ܂Ƃ߂��ق����L���b�V���ɏ��₷���Ȃ邩���B�v�����B
     */
    struct ByteCode
    {
        U32 byteCodeSize; ///< Byte code size.
        U32 globalSymbolTableSectionOffset; ///< Global symbol table section offset.
        U32 dataSectionOffset; ///< Data section offset.
        U32 textSectionOffset; ///< Text section offset.

        /// LocalSymbolTableSectionOffset
        U32 localSymbolTableSectionOffset()const 
        {
            return sizeof(ByteCode);
        }

        /// Label valid check.
        bool isLabelValid( const LabelId label )const
        {
            return label < totalLabelCount();
        }

        /// Total label count.
        U32 totalLabelCount()const
        {
            return ( dataSectionOffset - localSymbolTableSectionOffset() ) >> 2; // div sizeof(U32)
        }

        /// Local symbol count.
        U32 localSymbolCount()const
        {
            return ( globalSymbolTableSectionOffset - localSymbolTableSectionOffset() ) >> 2; // div sizeof(U32)
        }

        // Global symbol count.
        U32 globalSymbolCount()const
        {
            return ( dataSectionOffset - globalSymbolTableSectionOffset ) >> 2; // div sizeof(U32)
        }

        /// Local symbol check.
        Bool isLocalSymbolLabel( const LabelId label )const
        {
            return label < localSymbolCount();
        }

        /// Label id to local symbol offset.
        U32 labelIdToLocalSymbolOffset( const LabelId label )const
        {
            ACS_ASSERT( isLocalSymbolLabel( label ) );
            return reinterpret_cast<const U32* >( &reinterpret_cast<const U8*>(this)[ localSymbolTableSectionOffset() ] )[ label ];
        }
        
        /// Global symbol check.
        Bool isGlobalSymbolLabel( const LabelId label )const
        {
            return localSymbolCount() <= label && label < totalLabelCount();
        }

        /// Label id to global symbol info.
        SymbolInfo labelIdToGlobalSymbolInfo( const LabelId label )const
        {
            ACS_ASSERT( isGlobalSymbolLabel( label ) );
            return reinterpret_cast<const SymbolInfo* >( &reinterpret_cast<const U8*>(this)[ localSymbolTableSectionOffset() ] )[ label ];
        }

        /// Valid offset check.
        Bool isOffsetValid( const U32 offset )const
        {
            return dataSectionOffset <= offset && offset < byteCodeSize;
        }

        /// Offset to readonly address.
        const void* offsetToAddress( const U32 offset )const
        {
            ACS_ASSERT( isOffsetValid( offset ) );
            return &reinterpret_cast< const U8* >(this)[ offset ];
        }
        
        /// Offset to address.
        void* offsetToAddress( const U32 offset )
        {
            ACS_ASSERT( isOffsetValid( offset ) );
            return &reinterpret_cast< U8* >(this)[ offset ];
        }
    };

}
//------------------------------------------------------------
#endif
// EOF
