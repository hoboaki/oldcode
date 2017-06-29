/**
 * @file
 * @brief GLPicHeader.hppの実装を記述する。
 */
#include <glpic/GLPicHeader.hpp>

//----------------------------------------------------------------
#include <cstring>
#include <glpic/Assert.hpp>
#include <glpic/ByteOrderUtil.hpp>
#include <glpic/PixelFormatUtil.hpp>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
PixelFormat GLPicHeader::getPixelFormat()const
{
    GLPicAssert( isValid() );
    return static_cast<PixelFormat>( this->pixelFormat );
}

//----------------------------------------------------------------
bool GLPicHeader::getImageScaled()const
{
    GLPicAssert( isValid() );
    return ( this->flagAndMipMapLevel & 0x80 ) != 0;
}

//----------------------------------------------------------------
unsigned char GLPicHeader::getMipMapMaxLevel()const
{
    GLPicAssert( isValid() );
    return ( this->flagAndMipMapLevel & 0x7F );
}

//----------------------------------------------------------------
Size2D GLPicHeader::getTextureSize2D()const
{
    GLPicAssert( isValid() );
    return Size2D::create( this->textureWidth , this->textureHeight );
}

//----------------------------------------------------------------
Size2D GLPicHeader::getImageSize2D()const
{
    GLPicAssert( isValid() );
    return Size2D::create( this->imageWidth , this->imageHeight );
}

//----------------------------------------------------------------
unsigned long GLPicHeader::getPixelDataSize()const
{
    GLPicAssert( isValid() );
    return this->pixelDataSize;
}

//----------------------------------------------------------------
unsigned long GLPicHeader::getUserDataSize()const
{
    GLPicAssert( isValid() );
    return this->userDataSize;
}

//----------------------------------------------------------------
void* GLPicHeader::getPixelData()
{
    GLPicAssert( isValid() );
    return reinterpret_cast<char*>(this) + this->pixelDataOffset;
}

//----------------------------------------------------------------
const void* GLPicHeader::getPixelData()const
{
    GLPicAssert( isValid() );
    return reinterpret_cast<const char*>(this) + this->pixelDataOffset;
}

//----------------------------------------------------------------
void* GLPicHeader::getUserData()
{
    GLPicAssert( isValid() );
    return reinterpret_cast<char*>(this) + this->userDataOffset;
}

//----------------------------------------------------------------
const void* GLPicHeader::getUserData()const
{
    GLPicAssert( isValid() );
    return reinterpret_cast<const char*>(this) + this->userDataOffset;
}

//----------------------------------------------------------------
bool GLPicHeader::isValid()const
{
    // シグネチャ
    if ( std::strncmp( this->signature 
          , SIGNATURE
          , sizeof( this->signature )
          ) != 0
        )
    {
        return false;
    }

    // バージョン
    if ( this->version != VERSION )
    {
        return false;
    }

    // エンディアン
    if ( this->endianCheck != ENDIAN_CHECK_VALUE )
    {
        return false;
    }

    // ピクセルフォーマット
    if ( !PixelFormatUtil::isValid( static_cast<PixelFormat>(this->pixelFormat) ) )
    {
        return false;
    }

    // テクスチャのサイズ
    if ( this->textureWidth == 0 // 1以上であるべき
        || this->textureHeight == 0
        )
    {
        return false;
    }

    // 元イメージサイズ
    if ( this->imageWidth == 0 // 1以上であるべき
        || this->imageHeight == 0
        || this->textureWidth < this->imageWidth // テクスチャのサイズ以下である
        || this->textureHeight < this->imageHeight 
        )
    {
        return false;
    }

    // ピクセルデータ
    if ( this->pixelDataOffset < sizeof( GLPicHeader ) // ヘッダより後にあるべき
        || this->pixelDataSize == 0 // ピクセルデータは必須
        )
    {
        return false;
    }

    // ユーザーデータ
    if ( this->userDataOffset == 0 
        || this->userDataSize == 0
        )
    {// ユーザーデータがないとき
        if ( ( this->userDataOffset == 0 && this->userDataSize != 0 )
            || ( this->userDataOffset != 0 && this->userDataSize == 0 )
            )
        {
            return false;
        }
    }
    else
    {// ユーザーデータがあるとき
        if ( this->userDataOffset < sizeof( GLPicHeader ) // ヘッダより後にあるべき
            || this->userDataOffset < this->pixelDataOffset+this->pixelDataSize // ピクセルデータより後にあるべき
            )
        {
            return false;
        }
    }

    // 正常です。
    return true;
}

//----------------------------------------------------------------
bool GLPicHeader::isByteOrderInversed()const
{
    return this->endianCheck == INVERSED_ENDIAN_CHECK_VALUE;
}

//----------------------------------------------------------------
void GLPicHeader::inverseByteOrder()
{
    this->endianCheck       = ByteOrderUtil::inverseU16( this->endianCheck );
    this->textureWidth      = ByteOrderUtil::inverseU16( this->textureWidth );
    this->textureHeight     = ByteOrderUtil::inverseU16( this->textureHeight );
    this->imageWidth        = ByteOrderUtil::inverseU16( this->imageWidth );
    this->imageHeight       = ByteOrderUtil::inverseU16( this->imageHeight );
    this->pixelDataOffset   = ByteOrderUtil::inverseU32( this->pixelDataOffset );
    this->pixelDataSize     = ByteOrderUtil::inverseU32( this->pixelDataSize );
    this->userDataOffset    = ByteOrderUtil::inverseU32( this->userDataOffset );
    this->userDataSize      = ByteOrderUtil::inverseU32( this->userDataSize );
}

//----------------------------------------------------------------
const char* const GLPicHeader::SIGNATURE = "GPF";
const unsigned char GLPicHeader::VERSION = 0x02;
const unsigned short GLPicHeader::ENDIAN_CHECK_VALUE = 0x1234;
const unsigned short GLPicHeader::INVERSED_ENDIAN_CHECK_VALUE = 0x3412;

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
