/**
 * @file
 * @brief ElementIterator.hppの実装を記述する。
 */
#include <textga/tag/ElementIterator.hpp>

//------------------------------------------------------------
#include <textga/Assert.hpp>
#include <textga/EndianUtil.hpp>
#include <textga/StringUtil.hpp>
#include <textga/tag/ELBinary.hpp>
#include <textga/tag/ELNum.hpp>
#include <textga/tag/ELSection.hpp>
#include <textga/tag/ELString.hpp>
#include <textga/tag/TagKindUtil.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {
//------------------------------------------------------------
ElementIterator::ElementIterator( const byte* aData , const size_t aSize )
: data_( aData )
, size_( aSize )
, offset_(0)
{
}

//------------------------------------------------------------
ElementIterator::~ElementIterator()
{
}

//------------------------------------------------------------
std::auto_ptr< IElement > ElementIterator::next()
{
    TEXTGA_ASSERT( hasNext() );

    std::auto_ptr< IElement > returnNextTagResult;
    const NextTagResult nextTagResult = nextTag();

    switch( nextTagResult.tagKind )
    {   
    case TagKind_SectionBegin:
        {
            std::auto_ptr< ELSection > section( new ELSection( nextTagResult.tagName ) );
            while( hasNext() )
            {
                std::auto_ptr< IElement > obj = next();
                if ( obj.get() == 0 )
                {// Nullが返ってきた。
                    if ( lastTagKind_ != TagKind_SectionEnd )
                    {
                        // SectionEndではない。
                        section.reset(); // reset
                    }
                    break;
                }
                else
                {
                    section->add( obj );
                }
            }
            returnNextTagResult.reset( section.release() );
            break;
        }

    case TagKind_NumU8:
        returnNextTagResult.reset( new ELNumU8( 
            nextTagResult.tagName 
            , *reinterpret_cast< const u8* >( nextTagResult.data )
            ) );
        break;

    case TagKind_NumU16:
        returnNextTagResult.reset( new ELNumU16( 
            nextTagResult.tagName 
            , EndianUtil::swapU16BE( *reinterpret_cast< const u16* >( nextTagResult.data ) )
            ) );
        break;

    case TagKind_NumU32:
        returnNextTagResult.reset( new ELNumU32( 
            nextTagResult.tagName 
            , EndianUtil::swapU32BE( *reinterpret_cast< const u32* >( nextTagResult.data ) )
            ) );
        break;

    case TagKind_NumS8:
        returnNextTagResult.reset( new ELNumS8( 
            nextTagResult.tagName 
            , *reinterpret_cast< const s8* >( nextTagResult.data )
            ) );
        break;

    case TagKind_NumS16:
        returnNextTagResult.reset( new ELNumS16( 
            nextTagResult.tagName 
            , EndianUtil::swapS16BE( *reinterpret_cast< const s16* >( nextTagResult.data ) )
            ) );
        break;

    case TagKind_NumS32:
        returnNextTagResult.reset( new ELNumS32( 
            nextTagResult.tagName 
            , EndianUtil::swapS32BE( *reinterpret_cast< const s32* >( nextTagResult.data ) )
            ) );
        break;

    case TagKind_String:
        returnNextTagResult.reset( new ELString( 
            nextTagResult.tagName 
            , reinterpret_cast< const char* >( nextTagResult.data )
            ) );
        break;
        
    case TagKind_Binary:
        returnNextTagResult.reset( new ELBinary( 
            nextTagResult.tagName 
            , EndianUtil::swapU32BE( *reinterpret_cast< const u32* >( nextTagResult.data ) )
            , nextTagResult.data+4
            ) );
        break;

    case TagKind_SectionEnd:
    case TagKind_Unknown:
    default:
        break;
    }
    
    lastTagKind_ = nextTagResult.tagKind;
    return returnNextTagResult;
}

//------------------------------------------------------------
bool ElementIterator::hasNext()const
{
    return offset_ < size_;
}

//------------------------------------------------------------
size_t ElementIterator::calculateRestSize()const
{
    TEXTGA_ASSERT( offset_ <= size_ );
    return size_ - offset_;
}

//------------------------------------------------------------
ElementIterator::NextTagResult ElementIterator::nextTag()
{
    NextTagResult nextTagResult;
    nextTagResult.tagKind = TagKind_Unknown;
   
    if ( !hasNext() )
    {// 次がはない。
        return nextTagResult;
    }

    // タグ名
    nextTagResult.tagName = StringUtil::findString( &data_[offset_] , calculateRestSize() );
    if ( nextTagResult.tagName == 0 )
    {// タグ名見つからず。
        return nextTagResult;
    }
    offset_ += std::strlen( nextTagResult.tagName )+1;

    // データタイプ名
    const char* dataTypeName = StringUtil::findString( &data_[offset_] , calculateRestSize() );
    if ( dataTypeName == 0 )
    {// データタイプ名見つからず。
        return nextTagResult;
    }
    offset_ += std::strlen( dataTypeName )+1;

    // タグの種類識別。
    nextTagResult.data = &data_[offset_];
    const TagKind tagKind = TagKindUtil::fromName( dataTypeName );

    size_t dataSize = 0;
    switch( tagKind )
    {
    case TagKind_SectionBegin:
    case TagKind_SectionEnd:
        break;

    case TagKind_NumU8:
    case TagKind_NumS8:
        dataSize = 1;
        break;

    case TagKind_NumU16:
    case TagKind_NumS16:
        dataSize = 2;
        break;

    case TagKind_NumU32:
    case TagKind_NumS32:
        dataSize = 4;
        break;

    case TagKind_String:
        {
            const char* tmp = StringUtil::findString( &data_[offset_] , calculateRestSize() );
            if ( tmp == 0 )
            {
                return nextTagResult;
            }
            dataSize = std::strlen( tmp )+1;
            break;
        }

    case TagKind_Binary:
        {
            if ( calculateRestSize() < 4 )
            {
                return nextTagResult;
            }
            const u32 binaryDataSize = EndianUtil::swapU32BE( *reinterpret_cast< const u32* >(data_[offset_]) );
            dataSize = 4 + binaryDataSize;
            break;
        }

    case TagKind_Unknown:
    default:
        return nextTagResult;
    }

    if ( calculateRestSize() < dataSize )
    {
        return nextTagResult;
    }

    nextTagResult.tagKind = tagKind;
    offset_ += dataSize;
    return nextTagResult;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
