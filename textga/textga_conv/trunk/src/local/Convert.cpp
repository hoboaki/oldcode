/**
 * @file
 * @brief Convert.hppの実装を記述する。
 */
 #include "Convert.hpp"

//------------------------------------------------------------
#include <squish.h>
#include "Assert.hpp"
#include "ArgumentData.hpp"
#include "ConvertRecipe.hpp"
#include "EndianUtil.hpp"
#include "File.hpp"
#include "Pixel.hpp"
#include "PixelFormatUtil.hpp"
#include "StringUtil.hpp"
#include "TGAHeader.hpp"
#include "TGAFileLoader.hpp"

//------------------------------------------------------------
namespace
{
    const char* ERROR_MSG_WRITE_FAILED = "Error: Writing Failed.\n";
    
    /// 文字列の書き込み処理。
    bool t_writeString( const char* aStr , FILE* fp )
    {        
        if ( std::fwrite( aStr , std::strlen( aStr )+1 , 1 , fp ) < 1 )
        {
            std::fprintf( stderr, ERROR_MSG_WRITE_FAILED );
            return false;
        }
        return true;
    }
    
}

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Convert::execute( const ArgumentData& aArgumentData )
{
    ConvertRecipe convertRecipe;
    if ( !convertRecipe.initialize( aArgumentData ) )
    {// レシピ変換失敗。
        return false;
    }
    
    // TGAロード
    TGAFileLoader inputTGA( convertRecipe.inputFilePath );
    if ( !inputTGA.isOpened() )
    {
        std::fprintf( stderr, "Error: Can't open '%s'\n" , convertRecipe.inputFilePath.c_str() );
        return false;
    }
    if ( !inputTGA.isSupportedFile() )
    {
        std::fprintf( stderr, "Error: Not Support TGA File '%s'\n" , convertRecipe.inputFilePath.c_str() );
        return false;
    }
    
    // ピクセルフォーマットの自動選別
    if ( !convertRecipe.isPixelFormatValid )
    {
        const char bitsPerPixel = inputTGA.bitsPerPixel();
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
    , const TGAFileLoader& aInputTGA
    )
{
    PixelFormatUtil::codeTest();
    std::printf( "[TexTarga Convert]\n" );
    std::printf( "input  : %s\n" , aRecipe.inputFilePath.c_str() );
    std::printf( "output : %s\n" , aRecipe.outputFilePath.c_str() );
    std::printf( "format : %s\n" , PixelFormatUtil::toName( aRecipe.pixelFormat ) );
    
    File file( aRecipe.outputFilePath.c_str() , "wb" );
    if ( file.fp() == NULL )
    {
        std::fprintf( stderr, "Error: Can't open '%s'.\n" , aRecipe.outputFilePath.c_str() );
        return false;
    }
    
    // TGAHeader
    {
        const bool hasAlpha = PixelFormatUtil::hasAlpha( aRecipe.pixelFormat );
        TGAHeader header;
        std::memset( &header , 0 , sizeof( TGAHeader ) );
        header.dataTypeCode = 2;
        header.width = EndianUtil::swapS16BE( aInputTGA.width() );
        header.height = EndianUtil::swapS16BE( aInputTGA.height() );
        header.bitsPerPixel = hasAlpha ? 32 : 24;
        header.imageDescriptor = hasAlpha ? 8 : 0;
        if ( std::fwrite( &header , TGAHeader::SIZE , 1 , file.fp() ) < 1 )
        {
            std::fprintf( stderr, ERROR_MSG_WRITE_FAILED );
            return false;
        }
    }
    
    // ピクセル
    if ( !convertPixels( aRecipe , aInputTGA , file ) )
    {
        return false;
    }
    
    // footer
    {
        // logo
        if ( !t_writeString( "TEX_TARGA" , file.fp() ) )
        {
            return false;
        }
        
        // format
        if ( !t_writeString( "PIXEL_FORMAT" , file.fp() ) )
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
            std::vector< unsigned char > bytes;
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
            
            if ( !t_writeString( "SRC_DATA" , file.fp() ) )
            {
                return false;
            }
            if ( std::fwrite( &bytes[0] , bytes.size() , 1 , file.fp() ) < 1 )
            {
                std::fprintf( stderr, ERROR_MSG_WRITE_FAILED );
                return false;
            }
        }
    }
    
    std::printf( "Convert Success.\n" );
    return true;
}

//------------------------------------------------------------
bool Convert::convertPixels(
    const ConvertRecipe& aRecipe
    , const TGAFileLoader& aInputTGA
    , File& aOutputFile
    )
{
    std::vector< unsigned char > bytes;
    const size_t pixelDataSize = PixelFormatUtil::hasAlpha( aRecipe.pixelFormat ) ? 4 : 3;
    bytes.resize( aInputTGA.pixelCount() * pixelDataSize );
        
    if ( PixelFormatUtil::isDXTC( aRecipe.pixelFormat ) )
    {// DXTC圧縮
        const size_t PER = 16;
        // 16の倍数か。
        if ( aInputTGA.pixelCount()%PER != 0 )
        {
            std::fprintf( stderr, "Error: DXTC Required that Pixel Count is Multiple of %u.\n" , PER );
            return false;
        }
        
        const int compressFlag = PixelFormatUtil::dxtcCompressFlag( aRecipe.pixelFormat );
        squish::u8 pixels[PER*4];
        squish::u8 block[16];
        
        size_t offset = 0;
        for ( size_t i = 0; i < aInputTGA.pixelCount(); i+=PER )
        {
            // コピー
            for ( size_t p = 0; p < PER; ++p )
            {
                const Pixel pix = aInputTGA.pixelAtIndex(i+p);
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
        std::fprintf( stderr, ERROR_MSG_WRITE_FAILED );
        return false;
    }
    
    // 成功
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
