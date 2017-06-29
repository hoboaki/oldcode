/** 
 * @file
 * @brief ByteVector.hppの実装を記述する。
 */
#include "app/ByteVector.hpp"

//------------------------------------------------------------
#include <cstdarg>

//------------------------------------------------------------
namespace
{
    const U32 T_RESERVE_SIZE = 2 * 1024 * 1024; // 最初にReserveするサイズ
}

//------------------------------------------------------------
namespace app {
//------------------------------------------------------------
ByteVector::ByteVector()
: bytes_()
, size_(0)
, currentIndent_(0)
{
    bytes_.resize( T_RESERVE_SIZE );
}

//------------------------------------------------------------
void ByteVector::print( const char* aFormat , ... )
{
    va_list arg;
    va_start(arg, aFormat);
    vprintf( aFormat , arg );
    va_end(arg);
}

//------------------------------------------------------------
void ByteVector::printIndentSpace( const U32 aNum )
{
    for ( U32 i = 0; i < aNum; ++i  )
    {
        print( " " );
    }
}

//------------------------------------------------------------
void ByteVector::indentEnter()
{
    ++currentIndent_;
}

//------------------------------------------------------------
void ByteVector::indentReturn()
{
    PJ_ASSERT( 0 < currentIndent_ );
    --currentIndent_;
}

//------------------------------------------------------------
void ByteVector::printCurrentIndent()
{
    printIndentSpace( currentIndent_ * 2 );
}

//------------------------------------------------------------
void ByteVector::printLineEnter()
{
    print( "\n" );
}

//------------------------------------------------------------
void ByteVector::printComment( const char* aFormat , ... )
{
    print( "<!-- " );

    va_list arg;
    va_start(arg, aFormat);
    vprintf( aFormat , arg );
    va_end(arg);

    
    print( "-->" );
}

//------------------------------------------------------------
void ByteVector::printTagLabel( const char* aLabelName )
{
    printTagLabelF( "%s" , aLabelName );
}

//------------------------------------------------------------
void ByteVector::printTagLabelF( const char* aFormat , ... )
{
    print( "<label name=\"" );
    
    va_list arg;
    va_start(arg, aFormat);
    vprintf( aFormat , arg );
    va_end(arg);

    print( "\" />" );
}

//------------------------------------------------------------
void ByteVector::printTagReference( const char* aLabelName )
{
    printTagReferenceF( "%s" , aLabelName );
}

//------------------------------------------------------------
void ByteVector::printTagReferenceF( const char* aFormat , ... )
{
    print( "<reference label_name=\"" );
    
    va_list arg;
    va_start(arg, aFormat);
    vprintf( aFormat , arg );
    va_end(arg);

    print( "\" />" );
}

//------------------------------------------------------------
void ByteVector::printTagU8Array( const U8* aPtr , const U32 aCount )
{
    print( "<uint8_array count=\"%lu\" >" , aCount );
    for ( U32 i = 0; i < aCount; ++i )
    {
        print( "%hhu " , aPtr[i] );
    }
    print( "</uint8_array>" );
}

//------------------------------------------------------------
void ByteVector::printTagU16( const U16 aValue )
{
    print( "<uint16 value=\"%hu\" />" , aValue );
}

//------------------------------------------------------------
void ByteVector::printTagU16Array( const U16* aPtr , const U32 aCount )
{
    print( "<uint16_array count=\"%lu\" >" , aCount );
    for ( U32 i = 0; i < aCount; ++i )
    {
        print( "%hu " , aPtr[i] );
    }
    print( "</uint16_array>" );
}

//------------------------------------------------------------
void ByteVector::printTagU32( const U32 aValue )
{
    print( "<uint32 value=\"%lu\" />" , aValue );
}

//------------------------------------------------------------
void ByteVector::printTagU32Array( const U32* aPtr , const U32 aCount )
{
    print( "<uint32_array count=\"%lu\" >" , aCount );
    for ( U32 i = 0; i < aCount; ++i )
    {
        print( "%lu " , aPtr[i] );
    }
    print( "</uint32_array>" );
}

//------------------------------------------------------------
void ByteVector::printTagF32Array( const F32* aPtr , const U32 aCount )
{
    print( "<float32_array count=\"%lu\" >" , aCount );
    for ( U32 i = 0; i < aCount; ++i )
    {
        print( "%f " , aPtr[i] );
    }
    print( "</float32_array>" );
}

//------------------------------------------------------------
void ByteVector::printTagString( const char* aStr )
{
    struct ReplaceRecipe
    {
        const char* src;
        const char* dest;
    };
    const ReplaceRecipe T_REPLACE_RECIPES[]=
    {
        {"<","&lt;"}
        ,{">","&gt;"}
        ,{"\"","&amp;"}
        ,{"'","&quot;"}
        ,{"&","&apos;"}
    };
    const U32 T_REPLACE_RECIPES_COUNT = PJ_ARRAY_LENGTH( T_REPLACE_RECIPES );
    std::string str = aStr;
    for ( U32 i = 0; i < T_REPLACE_RECIPES_COUNT; ++i )
    {
        const std::string srcStr( T_REPLACE_RECIPES[i].src );
        const std::string destStr( T_REPLACE_RECIPES[i].dest );
        std::string::size_type pos = 0; 
        do
        {
            pos = str.find( srcStr , pos );
            if ( pos == std::string::npos )
            {
                break;
            }
            str.replace( pos , srcStr.length() , destStr );
            pos += destStr.length();
        }while(1);
    }
    print( "<string value=\"%s\" />" , str.c_str() );
}

//------------------------------------------------------------
bool ByteVector::writeToFile( const char* aPath )
{
    FILE* fp = std::fopen( aPath , "w" );
    if ( fp == 0 )
    {
        PJ_COUT( "Error: Can't open file '%s'.\n" , aPath );
        return false;
    }

    // write
    if ( std::fwrite( &bytes_[0]
        , size_
        , 1
        , fp
        ) == 0 
        )
    {
        PJ_COUT( "Error: Can't write file '%s'.\n" , aPath );
        std::fclose( fp );
        return false;
    }

    std::fclose( fp );
    return true;
}

//------------------------------------------------------------
void ByteVector::vprintf( const char* aFormat , va_list aArg )
{
    while(1)
    {
        const size_t restSize = bytes_.size() - size_;
        const int writeSize = vsnprintf(
            &bytes_[size_] 
            , restSize
            , aFormat
            , aArg
            );
        if ( writeSize < 0
            || writeSize == restSize 
            )
        {// バッファが足りなかった可能性があるのでもう一度書く
            bytes_.resize( bytes_.size() + T_RESERVE_SIZE );
            continue;
        }
        size_ += writeSize;
        PJ_ASSERT(size_ <= bytes_.size());
        break;
    }
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
