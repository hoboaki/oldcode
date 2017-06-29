/**
 * @file
 * @brief ByteCode型を記述する。
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
     * @brief １つのバイトコード。
     *
     * バイトコードのレイアウト。
     * |------------------------------------------------
     * | Header           
     * | localSymbolOffsetTableSection - U32の配列。Headerの先頭からのオフセット値の配列。
     * | ...
     * | globalSymbolInfoTableSection  - SymbolInfoの配列。
     * | ...
     * | dataSection - データセクション。
     * | ...
     * | textSection - テキストセクション。
     * | ...
     * | End Of Data
     * |------------------------------------------------
     *
     * dataSectionは一カ所にまとめたほうがキャッシュに乗りやすくなるかも。要検討。
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
