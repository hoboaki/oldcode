/**
 * @file
 * @brief DataCreator.hppの実装を記述する。
 */
#include <glpic/DataCreator.hpp>

//----------------------------------------------------------------
#include <glpic/Assert.hpp>
#include <glpic/GLPicHeader.hpp>
#include <glpic/MipMapUtil.hpp>
#include <glpic/PixelFormatUtil.hpp>
#include <cstring>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
DataCreator::Recipe DataCreator::Recipe::create(
    const PixelFormat aPixelFormat 
    , const bool aImageScaled
    , const unsigned char aMipMapMaxLevel
    , const Size2D& aTexSize
    , const Size2D& aImageSize
    )
{
    Recipe recipe = Recipe();
    recipe.pixelFormat = aPixelFormat;
    recipe.imageScaled = aImageScaled;
    recipe.mipMapMaxLevel = aMipMapMaxLevel;
    recipe.textureSize = aTexSize;
    recipe.imageSize = aImageSize;
    return recipe;
}

//------------------------------------------------------------
bool DataCreator::Recipe::isValid()const
{
    return PixelFormatUtil::isValid( pixelFormat )
        && 0 < textureSize.width
        && 0 < textureSize.height
        && 0 < imageSize.width
        && 0 < imageSize.height
        ;
}

//----------------------------------------------------------------
DataCreator::Result DataCreator::Result::create(
    const DataCreator::ErrorKind aErrorKind
    , const unsigned long aDataSize
    )
{
    DataCreator::Result result;
    result.errorKind = aErrorKind;
    result.dataSize = aDataSize;
    return result;
}

//----------------------------------------------------------------
unsigned long DataCreator::calculateDataSize(
    const Recipe& aRecipe
    )
{
    return DataCreator::calculateDataSizeCustum(
        aRecipe
        , sizeof( GLPicHeader )
        , 0
        , 0
        );
}

//----------------------------------------------------------------
unsigned long DataCreator::calculateDataSizeCustum(
    const Recipe& aRecipe
    , const unsigned long aPixelDataOffset
    , const unsigned long aUserDataOffset
    , const unsigned long aUserDataSize
    )
{
    unsigned long pixelDataEndAddr;

    if ( !aRecipe.isValid() )
    {// レシピ不正。
        return 0;
    }

    if ( aPixelDataOffset < sizeof( GLPicHeader ) )
    {// ピクセルデータオフセットが不正
        return 0;
    }

    // ピクセルデータの最終アドレス
    pixelDataEndAddr = aPixelDataOffset 
        + PixelFormatUtil::calculateMipMapPixelDataSize( 
            aRecipe.pixelFormat 
            , aRecipe.textureSize 
            , aRecipe.mipMapMaxLevel
            );

    if ( aUserDataOffset != 0
        && aUserDataOffset < pixelDataEndAddr
        )
    {// ユーザーデータの位置が不正
        return 0;
    }

    if ( aUserDataOffset != 0 )
    {// ユーザーデータの最後の位置を
        return aUserDataOffset + aUserDataSize;
    }
    else
    {// ピクセルデータの最後の位置を
        return pixelDataEndAddr;
    }
}

//----------------------------------------------------------------
DataCreator::Result DataCreator::createData(
    const Recipe& aRecipe
    , const BinaryData* const aPixelDataArray
    , BinaryData& aAllocedData
    )
{
    return createDataCustum(
        aRecipe
        , sizeof( GLPicHeader )
        , aPixelDataArray
        , 0
        , 0
        , aAllocedData
        );
}

//----------------------------------------------------------------
DataCreator::Result DataCreator::createDataCustum(
    const Recipe& aRecipe
    , const unsigned long aPixelDataOffset
    , const BinaryData* const aPixelDataArray
    , const unsigned long aUserDataOffset
    , const BinaryData* const aUserDataPointer
    , BinaryData& aAllocedData
    )
{
    unsigned char i;
    unsigned long sizeMemo;
    Size2D size2D;
    GLPicHeader* header;
    const unsigned long dataSize = DataCreator::calculateDataSizeCustum(
        aRecipe
        , aPixelDataOffset
        , aUserDataOffset
        , aUserDataPointer == 0
            ? 0
            : aUserDataPointer->size
        );

    if ( dataSize == 0 )
    {// レシピが不正
        return DataCreator::Result::create(
            DataCreator::ErrorKind_InvalidArgument
            , 0
            );
    }

    if ( aAllocedData.size < dataSize
        )
    {// バッファが足らない
        return DataCreator::Result::create(
            DataCreator::ErrorKind_NotEnoughDataSize
            , 0
            );
    }

    // ピクセルデータサイズのチェック
    size2D = aRecipe.textureSize;
    for ( i = 0 ; i <= aRecipe.mipMapMaxLevel; ++i )
    {
        if ( i != 0 )
        {
            size2D = MipMapUtil::nextLevelSize2D( size2D );
        }
        if ( PixelFormatUtil::calculatePixelDataSize( aRecipe.pixelFormat , size2D )
            != aPixelDataArray[i].size
            )
        {// データサイズが不一致
            return DataCreator::Result::create(
                DataCreator::ErrorKind_InvalidArgument
                , 0
                );
        }
    }

    // ヘッダ作成。
    header = (GLPicHeader*)aAllocedData.address;
    std::strncpy( header->signature , GLPicHeader::SIGNATURE , sizeof( header->signature ) );
    header->version = GLPicHeader::VERSION;
    header->endianCheck = GLPicHeader::ENDIAN_CHECK_VALUE;
    header->pixelFormat = (unsigned char)aRecipe.pixelFormat;
    header->flagAndMipMapLevel = aRecipe.imageScaled
        ? 0x80 : 0;
    header->flagAndMipMapLevel |= aRecipe.mipMapMaxLevel;
    header->textureWidth = aRecipe.textureSize.width;
    header->textureHeight = aRecipe.textureSize.height;
    header->imageWidth = aRecipe.imageSize.width;
    header->imageHeight = aRecipe.imageSize.height;
    header->pixelDataOffset = aPixelDataOffset;
    header->pixelDataSize = PixelFormatUtil::calculateMipMapPixelDataSize(
        aRecipe.pixelFormat
        , aRecipe.textureSize
        , aRecipe.mipMapMaxLevel
        );
    header->userDataSize = aUserDataPointer != NULL 
        ? aUserDataPointer->size
        : 0;
    header->userDataOffset = aUserDataOffset;

    // ピクセルデータのコピー
    sizeMemo = aPixelDataOffset;
    for ( i = 0; i <= aRecipe.mipMapMaxLevel; ++i )
    {
        // データのコピー
        GLPicAssert( sizeMemo < aAllocedData.size );
        memcpy( (unsigned char*)(aAllocedData.address) + sizeMemo
            , aPixelDataArray[i].address
            , aPixelDataArray[i].size
            );
        sizeMemo += aPixelDataArray[i].size;
    }

    // ユーザーデータのコピー
    if ( aUserDataPointer != 0 )
    {
        GLPicAssert( aUserDataOffset + aUserDataPointer->size < aAllocedData.size );
        memcpy( (unsigned char*)(aAllocedData.address) + aUserDataOffset
            , aUserDataPointer->address
            , aUserDataPointer->size
            );
    }

    // 終了
    return DataCreator::Result::create(
        DataCreator::ErrorKind_None
        , dataSize
        );
}

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
