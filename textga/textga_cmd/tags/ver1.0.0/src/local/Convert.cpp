/**
 * @file
 * @brief Convert.hppの実装を記述する。
 */
 #include "Convert.hpp"

//------------------------------------------------------------
#include <squish.h>
#include <textga/Constant.hpp>
#include <textga/EndianUtil.hpp>
#include <textga/File.hpp>
#include <textga/Pixel.hpp>
#include <textga/PixelFormatUtil.hpp>
#include <textga/StringUtil.hpp>
#include <textga/TGAHeader.hpp>
#include <textga/TGAFileLoader.hpp>
#include <textga/Version.hpp>
#include "Assert.hpp"
#include "ArgumentData.hpp"
#include "ArgumentDataIterator.hpp"
#include "ErrorCheck.hpp"
#include "ErrorReport.hpp"
#include "CommandKindUtil.hpp"
#include "ConvertRecipe.hpp"

//------------------------------------------------------------
using namespace ::textga;

//------------------------------------------------------------
namespace
{
    /// 文字列の書き込み処理。
    bool t_writeString( const char* aStr , FILE* fp )
    {        
        if ( std::fwrite( aStr , std::strlen( aStr )+1 , 1 , fp ) < 1 )
        {
            ::local::ErrorReport::fileWriteError();
            return false;
        }
        return true;
    }
    
    /// squishの圧縮フラグを取得する。
    int t_dxtcCompressFlag( PixelFormat aPixelFormat )
    {    
        PJ_ASSERT( PixelFormatUtil::isDXTC( aPixelFormat ) );
        switch( aPixelFormat )
        {
        case PixelFormat_RGBA_S3TC_DXT1:
            return squish::kDxt1;
            
        case PixelFormat_RGBA_S3TC_DXT3:
            return squish::kDxt3;
            
        case PixelFormat_RGBA_S3TC_DXT5:
            return squish::kDxt5;
        
        default:
            PJ_ASSERT(0);
            return 0;
        }   
    }
}

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Convert::execute( const ArgumentDataIterator& aArgumentDataIterator )
{
    ConvertRecipe convertRecipe;
    if ( !convertRecipe.initialize( aArgumentDataIterator ) )
    {// レシピ変換失敗。
        return false;
    }
    
    // TGAロード
    TGAFileLoader inputTGAFile( convertRecipe.inputFilePath.c_str() );
    if ( !ErrorCheck::supportedTgaCheck( inputTGAFile , convertRecipe.inputFilePath.c_str() ) )
    {
        return false;
    }
    
    // ピクセルフォーマットの自動選別
    const TGADataAccessor inputTGA = inputTGAFile.tgaDataAccessor();
    if ( !convertRecipe.isPixelFormatValid )
    {
        const u8 bitsPerPixel = inputTGA.bitsPerPixel();
        if ( bitsPerPixel == 24 )
        {
            convertRecipe.pixelFormat = PixelFormat_RGB8;
        }
        else if ( bitsPerPixel == 32 )
        {
            convertRecipe.pixelFormat = PixelFormat_RGBA8;
        }
        else
        {
            PJ_ASSERT(0);
            return false;
        }
    }
    
    // 出力先の自動判別
    if ( convertRecipe.outputFilePath.length() == 0 )
    {
        convertRecipe.outputFilePath = StringUtil::baseName( convertRecipe.inputFilePath.c_str() ) + ".tex.tga";
    }
    
    // 変換開始
    if ( !impl( convertRecipe , inputTGA ) )
    {
        return false;
    }
    
    // 成功
    return true;
}

//------------------------------------------------------------
bool Convert::impl( 
    const ConvertRecipe& aRecipe 
    , const TGADataAccessor& aInputTGA
    )
{
    PixelFormatUtil::codeTest();
    CommandKindUtil::printCommand( "Convert" );
    std::printf( "input  : %s\n" , aRecipe.inputFilePath.c_str() );
    std::printf( "output : %s\n" , aRecipe.outputFilePath.c_str() );
    std::printf( "format : %s\n" , PixelFormatUtil::toName( aRecipe.pixelFormat ) );
    
    File file( aRecipe.outputFilePath.c_str() , "wb" );
    if ( file.fp() == NULL )
    {
        ErrorReport::fileOpenError( aRecipe.outputFilePath.c_str() );
        return false;
    }
    
    // TGAHeader
    {
        const bool hasAlpha = PixelFormatUtil::hasAlpha( aRecipe.pixelFormat );
        TGAHeader header;
        std::memset( &header , 0 , sizeof( TGAHeader ) );
        header.dataTypeCode = 2;
        header.width = EndianUtil::swapU16BE( aInputTGA.width() );
        header.height = EndianUtil::swapU16BE( aInputTGA.height() );
        header.bitsPerPixel = hasAlpha ? 32 : 24;
        header.imageDescriptor = hasAlpha ? 8 : 0;
        if ( std::fwrite( &header , TGAHeader::SIZE , 1 , file.fp() ) < 1 )
        {
            ErrorReport::fileWriteError();
            return false;
        }
    }
    
    // pixels
    if ( !convertPixels( aRecipe , aInputTGA , file ) )
    {
        return false;
    }
    
    // extra area
    {
        // logo and version
        if ( !t_writeString( textga::Constant::TAG_LOGO , file.fp() ) )
        {
            return false;
        }
        if ( std::fwrite( &textga::Version::FORMAT_VERSION , 1 , 1 , file.fp() ) < 1 )
        {
            ErrorReport::fileWriteError();
            return false;
        }
        
        // format
        if ( !t_writeString( textga::Constant::TAG_PIXEL_FORMAT , file.fp() ) )
        {
            return false;
        }
        if ( !t_writeString( PixelFormatUtil::toName( aRecipe.pixelFormat ) , file.fp() ) )
        {
            return false;
        }
        
        // src_data
        if ( aRecipe.appendSrcData )
        {
            const size_t pixelDataSize = PixelFormatUtil::hasAlpha( aRecipe.pixelFormat ) ? 4 : 3;
            std::vector< byte > bytes;
            bytes.resize( aInputTGA.pixelCount() * pixelDataSize );
            size_t offset = 0;
            for ( size_t i = 0; i < aInputTGA.pixelCount(); ++i )
            {
                const Pixel pix = aInputTGA.pixelAtIndex(i);
                bytes[offset] = pix.b;
                ++offset;
                bytes[offset] = pix.g;
                ++offset;
                bytes[offset] = pix.r;
                ++offset;
                if ( pixelDataSize == 4 )
                {
                    bytes[offset] = pix.a;
                    ++offset;
                }
            }
            
            if ( !t_writeString( textga::Constant::TAG_SRC_DATA , file.fp() ) )
            {
                return false;
            }
            if ( std::fwrite( &bytes[0] , bytes.size() , 1 , file.fp() ) < 1 )
            {
                ErrorReport::fileWriteError();
                return false;
            }
        }
         
        // 終了タグ
        if ( !t_writeString( textga::Constant::TAG_TERMINATE , file.fp() ) )
        {
            return false;
        }
    }
    
    std::printf( "Convert Success.\n" );
    return true;
}

//------------------------------------------------------------
bool Convert::convertPixels(
    const ConvertRecipe& aRecipe
    , const TGADataAccessor& aInputTGA
    , File& aOutputFile
    )
{
    std::vector< byte > bytes;
    const size_t pixelDataSize = PixelFormatUtil::hasAlpha( aRecipe.pixelFormat ) ? 4 : 3;
    bytes.resize( aInputTGA.pixelCount() * pixelDataSize );
        
    if ( PixelFormatUtil::isDXTC( aRecipe.pixelFormat ) )
    {// DXTC圧縮
        const size_t PER = 16;
        // 16の倍数か。
        if ( aInputTGA.pixelCount()%PER != 0 )
        {
            std::fprintf( stderr, "Error: DXTC required that pixel count is multiple of %u.\n" , PER );
            return false;
        }
        
        const int compressFlag = t_dxtcCompressFlag( aRecipe.pixelFormat );
        squish::u8 pixels[PER*4];
        squish::u8 block[16];
        
        size_t offset = 0;
        for ( size_t i = 0; i < aInputTGA.pixelCount(); i+=PER )
        {
            // コピー
            for ( size_t p = 0; p < PER; ++p )
            {
                const Pixel pix = aInputTGA.pixelAtIndex( std::min( i+p , aInputTGA.pixelCount()-1 ) );
                const size_t pixelsOffset = p * 4;
                pixels[ pixelsOffset  ] = pix.r;
                pixels[ pixelsOffset+1] = pix.g;
                pixels[ pixelsOffset+2] = pix.b;
                pixels[ pixelsOffset+3] = pix.a;
            }
            
            // エンコード
            squish::Compress( pixels , block , compressFlag );
            
            // デコード
            squish::Decompress( pixels , block , compressFlag );
            
            // コピー
            for ( size_t p = 0; p < PER; ++p )
            {
                PJ_ASSERT( offset < bytes.size() );
                const size_t pixelsOffset = p * 4;
                bytes[offset] = pixels[ pixelsOffset + 2 ];
                ++offset;
                bytes[offset] = pixels[ pixelsOffset + 1 ];
                ++offset;
                bytes[offset] = pixels[ pixelsOffset + 0 ];
                ++offset;
                bytes[offset] = pixels[ pixelsOffset + 3 ];
                ++offset;
            }
        }
        PJ_ASSERT( offset == bytes.size() );
    }
    else
    {// 非圧縮
        size_t offset = 0;
        for ( size_t i = 0; i < aInputTGA.pixelCount(); ++i )
        {
            const Pixel pix = PixelFormatUtil::convert(
                aInputTGA.pixelAtIndex(i) , aRecipe.pixelFormat
                );
            bytes[offset] = pix.b;
            ++offset;
            bytes[offset] = pix.g;
            ++offset;
            bytes[offset] = pix.r;
            ++offset;
            if ( pixelDataSize == 4 )
            {
                bytes[offset] = pix.a;
                ++offset;
            }
        }
        PJ_ASSERT( offset == bytes.size() );
    }
        
    // 書き出し
    if ( std::fwrite( &bytes[0] , bytes.size() , 1 , aOutputFile.fp() ) < 1 )
    {
        ErrorReport::fileWriteError();
        return false;
    }
    
    // 成功
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
