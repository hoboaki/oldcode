/**
 * @file
 * @brief EntryPoint.hppの実装を記述する。
 */
#include "EntryPoint.hpp"

//------------------------------------------------------------
#include <cassert>
#include <vector>
#include <textga/Endian.hpp>

//------------------------------------------------------------
#define ASSERT assert

//------------------------------------------------------------
namespace
{
    const int T_RESULT_ERROR = -1;
    typedef unsigned char u8;
    typedef unsigned short u16;

    u8 t_getLuminance( const ::textga::Pixel& aPix )
    {
        return static_cast<u8>( (aPix.r + aPix.g + aPix.b) / 3 );
    }
    
    u8 t_getIntensity( const ::textga::Pixel& aPix )
    {
        return static_cast<u8>( (aPix.r + aPix.g + aPix.b + aPix.a) / 4 );
    }

    void t_addU16( std::vector< u8 >& aDest , const u16 aVal )
    {
        const u8* ptr = reinterpret_cast< const u8* >( &aVal );
        if ( textga::Endian::isBigEndian() )
        {
            aDest.push_back( ptr[1] );
            aDest.push_back( ptr[0] );
        }
        else
        {
            aDest.push_back( ptr[0] );
            aDest.push_back( ptr[1] );
        }
    }

}

//------------------------------------------------------------
namespace pj {
//------------------------------------------------------------
int EntryPoint::run( const int aARGC , const char* aARGV[] )
{
    if ( aARGC != 3 )
    {// 引数チェック
        std::fprintf( stderr , "Useage: glpic_conv input_textga_filepath output_glpic_filepath\n" );
        return T_RESULT_ERROR;
    }
    const char* inputTextgaFilepath = aARGV[1];
    const char* outputGlpicFilepath = aARGV[2];

    // Load TGAFile
    ::textga::TGAFileLoader loader( inputTextgaFilepath );
    if ( !loader.isLoaded() )
    {
        // Load Failed
        std::fprintf( stderr , "Error: Load failed '%s'\n" , inputTextgaFilepath );
        return T_RESULT_ERROR;
    }
    
    // Standard TGA Check
    const ::textga::TGADataAccessor tgaDataAccessor = loader.tgaDataAccessor();
    if ( !tgaDataAccessor.isSupportedTGA() )
    {
        // Not supported tga file
        std::fprintf( stderr , "Error: Not supported tgafile '%s'\n" , inputTextgaFilepath );
        return T_RESULT_ERROR;
    }

    // ピクセルフォーマットの取得
    glpic::PixelFormat pixelFormat = glpic::PixelFormat();
    if ( !tgaDataAccessor.isTexTarga() )
    {// TexTarga形式ではない
        pixelFormat = tgaDataAccessor.bitsPerPixel() == 24
            ? glpic::PixelFormat_RGB8
            : glpic::PixelFormat_RGBA8;
    }
    else
    {
        switch( tgaDataAccessor.pixelFormat() )
        {        
        case ::textga::PixelFormat_A1     :
        case ::textga::PixelFormat_A2     :
        case ::textga::PixelFormat_A4     :
        case ::textga::PixelFormat_A8     :
            pixelFormat = glpic::PixelFormat_A8;
            break;

        case ::textga::PixelFormat_L1     :
        case ::textga::PixelFormat_L2     :
        case ::textga::PixelFormat_L4     :
        case ::textga::PixelFormat_L8     :
            pixelFormat = glpic::PixelFormat_L8;
            break;

        case ::textga::PixelFormat_LA1    :
        case ::textga::PixelFormat_LA2    :
        case ::textga::PixelFormat_LA4    :
        case ::textga::PixelFormat_LA8    :
            pixelFormat = glpic::PixelFormat_LA8;
            break;

        case ::textga::PixelFormat_I1     :
        case ::textga::PixelFormat_I2     :
        case ::textga::PixelFormat_I4     :
        case ::textga::PixelFormat_I8     :
            pixelFormat = glpic::PixelFormat_I8;
            break;

        case ::textga::PixelFormat_RGB232 :
        case ::textga::PixelFormat_RGB332 :
            pixelFormat = glpic::PixelFormat_RGB332;
            break;

        case ::textga::PixelFormat_RGB565 :
            pixelFormat = glpic::PixelFormat_RGB565;
            break;

        case ::textga::PixelFormat_RGB8   :
            pixelFormat = glpic::PixelFormat_RGB8;
            break;

        case ::textga::PixelFormat_RGB5A1 :
            pixelFormat = glpic::PixelFormat_RGB5A1;
            break;

        case ::textga::PixelFormat_RGBA1  :
        case ::textga::PixelFormat_RGBA2  :
        case ::textga::PixelFormat_RGBA4  :
            pixelFormat = glpic::PixelFormat_RGBA4;
            break;

        case ::textga::PixelFormat_RGBA6  :
        case ::textga::PixelFormat_RGBA8  :
            pixelFormat = glpic::PixelFormat_RGBA8;
            break;

        default:
            std::fprintf( stderr , "Error: Not supported pixel format '%s'\n" , ::textga::PixelFormatUtil::toName(tgaDataAccessor.pixelFormat())  );
            return T_RESULT_ERROR;
        }
    }

    // ピクセルデータ列の作成
    std::vector< u8 > inputData;
    {
        const u16 width = tgaDataAccessor.width();
        const u16 height = tgaDataAccessor.height();
        inputData.resize( ::glpic::PixelFormatUtil::calculatePixelDataSize(
            pixelFormat
            , ::glpic::Size2D::create( width , height )
            ));
        inputData.clear();
        for ( size_t y = 0; y < height; ++y )
        {
            for ( size_t x = 0; x < width; ++x )
            {
                const size_t pixelIndex = width * y + x;
                const ::textga::Pixel srcPixel = tgaDataAccessor.pixelAtIndex(pixelIndex);

                switch( pixelFormat )
                {
                case ::glpic::PixelFormat_A8:
                    inputData.push_back( srcPixel.a );
                    break;
                    
                case ::glpic::PixelFormat_L8:
                    inputData.push_back( t_getLuminance( srcPixel ) );
                    break;
                    
                case ::glpic::PixelFormat_LA8:
                    inputData.push_back( t_getLuminance( srcPixel ) );
                    inputData.push_back( srcPixel.a );
                    break;
                    
                case ::glpic::PixelFormat_I8:
                    inputData.push_back( t_getIntensity( srcPixel ) );
                    break;
                    
                case ::glpic::PixelFormat_RGB332:
                    inputData.push_back( ::glpic::BitUtil::pack3b3b2bTo8b( srcPixel.r , srcPixel.g , srcPixel.b ) );
                    break;

                case ::glpic::PixelFormat_RGB565:
                    t_addU16( inputData , ::glpic::BitUtil::pack5b6b5bTo16b( srcPixel.r , srcPixel.g , srcPixel.b ) );
                    break;
                    
                case ::glpic::PixelFormat_RGB8:
                    inputData.push_back( srcPixel.r );
                    inputData.push_back( srcPixel.g );
                    inputData.push_back( srcPixel.b );
                    break;
                    
                case ::glpic::PixelFormat_RGB5A1:
                    t_addU16( inputData , ::glpic::BitUtil::pack5b5b5b1bTo16b( srcPixel.r , srcPixel.g , srcPixel.b , srcPixel.a ) );
                    break;

                case ::glpic::PixelFormat_RGBA4:
                    t_addU16( inputData , ::glpic::BitUtil::pack4b4b4b4bTo16b( srcPixel.r , srcPixel.g , srcPixel.b , srcPixel.a ) );
                    break;
                    
                case ::glpic::PixelFormat_RGBA8:
                    inputData.push_back( srcPixel.r );
                    inputData.push_back( srcPixel.g );
                    inputData.push_back( srcPixel.b );
                    inputData.push_back( srcPixel.a );
                    break;

                default:
                    ASSERT(0);
                    return T_RESULT_ERROR; 
                }
            }
        }
    }

    // GLPic作成準備
    const glpic::Size2D texSize( ::glpic::Size2D::create( 
        tgaDataAccessor.width()
        , tgaDataAccessor.height()
        ));
    const glpic::DataCreator::Recipe recipe = glpic::DataCreator::Recipe::create(
        pixelFormat
        , false
        , 0
        , texSize
        , texSize
        );
    const glpic::BinaryData srcBinData = glpic::BinaryData::create(
        &inputData[0] 
        , static_cast< unsigned long >( inputData.size() )
        );
    std::vector< u8 > outputData;
    outputData.resize( glpic::DataCreator::calculateDataSize( recipe ) );
    ASSERT( outputData.size() != 0 );
    glpic::BinaryData outBinData = ::glpic::BinaryData::create(
        &outputData[0] 
        , static_cast< unsigned long >( outputData.size() )
        );

    // 作成
    const ::glpic::DataCreator::Result result = glpic::DataCreator::createData(
        recipe
        , &srcBinData
        , outBinData 
        );
    if ( result.errorKind != ::glpic::DataCreator::ErrorKind_None )
    {
        std::fprintf( stderr , "Error: GLPic createData Failed %d\n" , result.errorKind );
        return T_RESULT_ERROR;
    }

    // ファイル出力
    {
        FILE* fp = std::fopen( outputGlpicFilepath , "wb" );
        if ( fp == 0 )
        {
            std::fprintf( stderr , "Error: open failed '%s'\n" , outputGlpicFilepath );
            return T_RESULT_ERROR;
        }
        std::fwrite( outBinData.address , outBinData.size , 1 , fp );
        std::fclose( fp );
    }

    // Log出力
    std::printf( "<GLPic Convertor>\n" );
    std::printf( "Texture Size (w,h) : (%u,%u)\n" , texSize.width , texSize.height );
    std::printf( "Pixel Format : %s\n" , glpic::PixelFormatUtil::toString( recipe.pixelFormat ) );
    std::printf( "-> '%s'(%lubytes)\n" , outputGlpicFilepath , outBinData.size );
    std::printf( "\n" );

    return 0;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
