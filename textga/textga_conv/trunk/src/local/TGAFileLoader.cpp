/**
 * @file
 * @brief TGAFileLoader.hppの実装を記述する。
 */
 #include "TGAFileLoader.hpp"

//------------------------------------------------------------
#include <cstdio>
#include "Assert.hpp"
#include "EndianUtil.hpp"
#include "File.hpp"
#include "Pixel.hpp"
#include "TGAHeader.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
TGAFileLoader::TGAFileLoader( const std::string& aFilepath )
: isOpened_( false )
, bitsPerPixel_()
, pixelCount_()
, bytes_()
{
    File file( aFilepath.c_str() , "rb" );
    if ( file.fp() == 0 )
    {
        return;
    }
    isOpened_ = true;
    
    // ファイルサイズを調べる
    if ( std::fseek( file.fp() , 0 , SEEK_END ) != 0 )
    {
        return;
    }
    const long tellSize = std::ftell( file.fp() );
    if ( tellSize <= 0 )
    {
        return ;
    }
    if ( std::fseek( file.fp() , 0 , SEEK_SET ) != 0 )
    {
        return;
    }
    
    // ロード
    const size_t fileSize = static_cast< size_t >( tellSize );
    bytes_.resize( fileSize );
    std::fread( &bytes_.at(0) , fileSize , 1 , file.fp()  );
    
    // 各プロパティのメモ。
    if ( isSupportedFile() )
    {
        bitsPerPixel_ = header().bitsPerPixel;
        pixelCount_ = width() * height();
    }
}

//------------------------------------------------------------
bool TGAFileLoader::isOpened()const
{
    return isOpened_;
}

//------------------------------------------------------------
bool TGAFileLoader::isSupportedFile()const
{
    PJ_ASSERT( isOpened() );
    if ( bytes_.size() <= TGAHeader::SIZE )
    {
        return false;
    }

    const TGAHeader& tgaHeader = header();
    if ( tgaHeader.idLength != 0 )
    {
        return false;
    }
    if ( tgaHeader.colorMapType != 0 )
    {
        return false;
    }
    if ( tgaHeader.dataTypeCode != 2 )
    {
        return false;
    }
    if ( tgaHeader.colorMapElement1 != 0 )
    {
        return false;
    }
    if ( tgaHeader.colorMapElement2 != 0 )
    {
        return false;
    }
    if ( tgaHeader.x_origin != 0 )
    {
        return false;
    }
    if ( tgaHeader.y_origin != 0 )
    {
        return false;
    }
    if ( tgaHeader.width == 0 )
    {
        return false;
    }
    if ( tgaHeader.height == 0 )
    {
        return false;
    }
    switch( tgaHeader.bitsPerPixel )
    {
    case 24:
        if ( tgaHeader.imageDescriptor != 0 )
        {
            return false;
        }
        break;
        
    case 32:
        if ( tgaHeader.imageDescriptor != 8 )
        {
            return false;
        }
        break;
    
    default:
        return false;
    }
    
    // ファイルの長さが十分にあるか
    {
        const long needSize = sizeof( TGAHeader ) 
            + (tgaHeader.bitsPerPixel/8) 
                * EndianUtil::swapS16BE( tgaHeader.width )
                * EndianUtil::swapS16BE( tgaHeader.height );
            
        if ( bytes_.size() < needSize )
        {
            return false;
        }
    }
    
    return true;
}

//------------------------------------------------------------
short TGAFileLoader::width()const
{
    PJ_ASSERT( isSupportedFile() );
    return EndianUtil::swapS16BE( header().width );
}

//------------------------------------------------------------
short TGAFileLoader::height()const
{
    PJ_ASSERT( isSupportedFile() );
    return EndianUtil::swapS16BE( header().height );
}

//------------------------------------------------------------
char TGAFileLoader::bitsPerPixel()const
{
    PJ_ASSERT( isSupportedFile() );
    return bitsPerPixel_;
}

//------------------------------------------------------------
size_t TGAFileLoader::pixelCount()const
{
    PJ_ASSERT( isSupportedFile() );
    return pixelCount_;
}

//------------------------------------------------------------
Pixel TGAFileLoader::pixelAtIndex( const size_t aIndex )const
{
    PJ_ASSERT( aIndex < pixelCount() );
    size_t offset = TGAHeader::SIZE;
    offset += (bitsPerPixel()/8) * aIndex;
    PJ_ASSERT( offset < bytes_.size() );
    PJ_ASSERT( offset+bitsPerPixel()/8 < bytes_.size() );
    
    Pixel pix;
    pix.b = bytes_[offset];
    pix.g = bytes_[offset+1];
    pix.r = bytes_[offset+2];
    if ( bitsPerPixel() == 32 )
    {
        pix.a = bytes_[offset+3];
    }
    else
    {
        pix.a = 255;
    }
    return pix;
}

//------------------------------------------------------------
const TGAHeader& TGAFileLoader::header()const
{
    return reinterpret_cast< const TGAHeader& >(
        bytes_.at(0)
        );
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
