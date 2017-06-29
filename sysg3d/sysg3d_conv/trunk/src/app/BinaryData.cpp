/** 
 * @file
 * @brief BinaryData.hppの実装を記述する。
 */
#include "app/BinaryData.hpp"

//------------------------------------------------------------
#include <sysg3d/Types.hpp>

//------------------------------------------------------------
namespace app {
//------------------------------------------------------------
BinaryData::BinaryData( const U32 aReserveBytes )
: bytes_()
, labelMap_()
, isReserved_( false )
, reservedLabel_()
{
    bytes_.reserve( aReserveBytes );
}

//------------------------------------------------------------
void BinaryData::add( const void* aAddr , const U32 aSize )
{
    if ( isReserved_ )
    {// データ位置記憶
        PJ_ASSERT( labelMap_.find( reservedLabel_ ) != labelMap_.end() );
        ::sysg3d::BinDataOffset& bdo = reinterpret_cast< ::sysg3d::BinDataOffset& >( bytes_[ labelMap_[ reservedLabel_ ] ] );
        PJ_ASSERT( bdo == 0 );
        bdo = U32( bytes_.size() );
        isReserved_ = false;
    }

    const Byte* bytes = reinterpret_cast< const Byte* >( aAddr );
    for ( U32 i = 0; i < aSize; ++i )
    {
        PJ_ASSERT( bytes_.size() < bytes_.capacity() );
        bytes_.push_back( bytes[i] );
    }
}

//------------------------------------------------------------
void BinaryData::addU32( const U32 aValue )
{
    add( &aValue , sizeof( U32 ) );
}

//------------------------------------------------------------
void BinaryData::addBDOLabel( const BDOLabel& aLabel )
{
    // mapに追加
    PJ_ASSERT( labelMap_.find( aLabel ) == labelMap_.end() );
    labelMap_[ aLabel ] = U32( bytes_.size() );

    // dataを追加
    const ::sysg3d::BinDataOffset bdo(0);
    add( &bdo , sizeof(::sysg3d::BinDataOffset) );
}

//------------------------------------------------------------
void BinaryData::reserveAddBDOEntity( const BDOLabel& aLabel )
{
    PJ_ASSERT( labelMap_.find( aLabel ) != labelMap_.end() );
    PJ_ASSERT( !isReserved_ );
    isReserved_ = true;
    reservedLabel_ = aLabel;
}

//------------------------------------------------------------
void BinaryData::addBDOEntity( 
    const BDOLabel& aReserveLabel
    , const void* aAddr
    , const U32 aSize
    )
{
    reserveAddBDOEntity( aReserveLabel );
    add( aAddr , aSize );
}

//------------------------------------------------------------
void BinaryData::addBDOEntityU32(
    const BDOLabel& aReserveLabel
    , const U32 aVal
    )
{
    reserveAddBDOEntity( aReserveLabel );
    addU32( aVal );
}

//------------------------------------------------------------
void BinaryData::addBDOEntityBDOLabel(
    const BDOLabel& aReserveLabel
    , const BDOLabel& aLabel
    )
{
    reserveAddBDOEntity( aReserveLabel );
    addBDOLabel( aLabel );
}

//------------------------------------------------------------
bool BinaryData::write( const char* aFilepath )
{
    PJ_ASSERT( bytes_.size() == bytes_.capacity() );

    FILE* fp = std::fopen( aFilepath , "wb" );
    if ( fp == 0 )
    {
        PJ_COUT( "Error: Can't open file '%s'.\n" , aFilepath );
        return false;
    }

    // write
    if ( std::fwrite( &bytes_[0]
        , bytes_.size()
        , 1
        , fp
        ) == 0 
        )
    {
        PJ_COUT( "Error: Can't write file '%s'.\n" , aFilepath );
        std::fclose( fp );
        return false;
    }

    std::fclose( fp );
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
