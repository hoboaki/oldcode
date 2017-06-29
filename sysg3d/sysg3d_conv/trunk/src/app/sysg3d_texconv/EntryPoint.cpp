/** 
 * @file
 * @brief EntryPoint.hppの実装を記述する。
 */
#include "app/sysg3d_texconv/EntryPoint.hpp"

//------------------------------------------------------------
#include <memory>
#include <textga/textga.h>
#include "app/Argument.hpp"
#include "app/ByteVector.hpp"
#include "app/CommonHeaderWriter.hpp"
#include "app/sysg3d_texconv/Constant.hpp"
#include "app/sysg3d_texconv/ConvertRecipe.hpp"

//------------------------------------------------------------
namespace
{
    /// PixelFormat名。
    const char* T_PIXELFORMAT_NAMES[]=
    {
        "Unknown"
        ,"A8"
        ,"L8"
        ,"LA8"
        ,"I8"
        ,"RGB332"
        ,"RGB565"
        ,"RGB8"
        ,"RGB5A1"
        ,"RGBA4"
        ,"RGBA8"
        ,"RGBA_S3TC_DXT1"
        ,"RGBA_S3TC_DXT3"
        ,"RGBA_S3TC_DXT5"
    };
    PJ_ARRAY_LENGTH_CHECK( T_PIXELFORMAT_NAMES , ::sysg3d::BinTexturePixelFormat_Terminate );

    // ピクセルフォーマットの取得
    const ::sysg3d::BinTexturePixelFormat t_pixelFormat( const ::textga::TGADataAccessor& tgaDataAccessor )
    {
        ::sysg3d::BinTexturePixelFormat pixelFormat = sysg3d::BinTexturePixelFormat();
        if ( !tgaDataAccessor.isTexTarga() )
        {// TexTarga形式ではない
            pixelFormat = tgaDataAccessor.bitsPerPixel() == 24
                ? ::sysg3d::BinTexturePixelFormat_RGB8
                : ::sysg3d::BinTexturePixelFormat_RGBA8;
        }
        else
        {
            switch( tgaDataAccessor.pixelFormat() )
            {        
            case ::textga::PixelFormat_A1     :
            case ::textga::PixelFormat_A2     :
            case ::textga::PixelFormat_A4     :
            case ::textga::PixelFormat_A8     :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_A8;
                break;

            case ::textga::PixelFormat_L1     :
            case ::textga::PixelFormat_L2     :
            case ::textga::PixelFormat_L4     :
            case ::textga::PixelFormat_L8     :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_L8;
                break;

            case ::textga::PixelFormat_LA1    :
            case ::textga::PixelFormat_LA2    :
            case ::textga::PixelFormat_LA4    :
            case ::textga::PixelFormat_LA8    :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_LA8;
                break;

            case ::textga::PixelFormat_I1     :
            case ::textga::PixelFormat_I2     :
            case ::textga::PixelFormat_I4     :
            case ::textga::PixelFormat_I8     :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_I8;
                break;

            case ::textga::PixelFormat_RGB232 :
            case ::textga::PixelFormat_RGB332 :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_RGB332;
                break;

            case ::textga::PixelFormat_RGB565 :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_RGB565;
                break;

            case ::textga::PixelFormat_RGB8   :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_RGB8;
                break;

            case ::textga::PixelFormat_RGB5A1 :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_RGB5A1;
                break;

            case ::textga::PixelFormat_RGBA1  :
            case ::textga::PixelFormat_RGBA2  :
            case ::textga::PixelFormat_RGBA4  :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_RGBA4;
                break;

            case ::textga::PixelFormat_RGBA6  :
            case ::textga::PixelFormat_RGBA8  :
                pixelFormat = ::sysg3d::BinTexturePixelFormat_RGBA8;
                break;

            default:
                PJ_COUT( "Error: Not supported pixel format '%s'\n" , ::textga::PixelFormatUtil::toName(tgaDataAccessor.pixelFormat())  );
                return ::sysg3d::BinTexturePixelFormat_Terminate;
            }
        }

        return pixelFormat;
    }

    U8 t_getLuminance( const ::textga::Pixel& aPix )
    {
        return static_cast<U8>( (aPix.r + aPix.g + aPix.b) / 3 );
    }
    
    U8 t_getIntensity( const ::textga::Pixel& aPix )
    {
        return static_cast<U8>( (aPix.r + aPix.g + aPix.b + aPix.a) / 4 );
    }

    void t_addU16( std::vector< U8 >& aDest , const U16 aVal )
    {
        const U8* ptr = reinterpret_cast< const U8* >( &aVal );
        aDest.push_back( ptr[0] );
        aDest.push_back( ptr[1] );
    }

    U8 t_pack4b4bTo8b(
        const U8 aComponent1
        , const U8 aComponent2
        )
    {
        return ( aComponent1 & 0xF0 ) 
            | ( ( aComponent2 & 0xF0 ) >> 4 );
    }

    U8 t_pack6b2bTo8b(
        const U8 aComponent1
        , const U8 aComponent2
        )
    {
        return ( aComponent1 & 0xFC )
            | ( ( aComponent2 & 0xE0 ) >> 6 );
    }

    U8 t_pack3b3b2bTo8b(
        const U8 aComponent1
        , const U8 aComponent2
        , const U8 aComponent3
        )
    {
        return ( aComponent1 & 0xE0 )
            | ( ( aComponent2 & 0xE0 ) >> 3 )
            | ( ( aComponent3 & 0xC0 ) >> 6 );
    }

    U8 t_pack5b5b5bTo16b(
        const U8 aComponent1
        , const U8 aComponent2
        , const U8 aComponent3
        )
    {
        const U16 comp1 = aComponent1;
        const U16 comp2 = aComponent2;
        const U16 comp3 = aComponent3;
        return ( ( comp1 & 0xF8 ) << 8 )
            | ( ( comp2 & 0xF8 ) << 3 )
            | ( ( comp3 & 0xF8 ) >> 2 );
    }

    U16 t_pack5b6b5bTo16b(
        const U8 aComponent1
        , const U8 aComponent2
        , const U8 aComponent3
        )
    {
        const U16 comp1 = aComponent1;
        const U16 comp2 = aComponent2;
        const U16 comp3 = aComponent3;
        return ( ( comp1 & 0xF8 ) << 8 )
            | ( ( comp2 & 0xFC ) << 3 )
            | ( ( comp3 & 0xF8 ) >> 3 );
    }

    U32 t_pack10b10b10bTo32b(
        const U16 aComponent1
        , const U16 aComponent2
        , const U16 aComponent3
        )
    {
        const U32 comp1 = aComponent1;
        const U32 comp2 = aComponent2;
        const U32 comp3 = aComponent3;
        return ( ( comp1 & 0xFFC0 ) << 16 )
            | ( ( comp2 & 0xFFC0 ) << 6 )
            | ( ( comp3 & 0xFFC0 ) >> 4 );
    }

    U8 t_pack2b2b2b2bTo8b(
        const U8 aComponent1
        , const U8 aComponent2
        , const U8 aComponent3
        , const U8 aComponent4
        )
    {
        return ( aComponent1 & 0xC0 )
            | ( ( aComponent2 & 0xC0 ) >> 2 )
            | ( ( aComponent3 & 0xC0 ) >> 4 )
            | ( ( aComponent4 & 0xC0 ) >> 6 );
    }

    U16 t_pack4b4b4b4bTo16b(
        const U8 aComponent1
        , const U8 aComponent2
        , const U8 aComponent3
        , const U8 aComponent4
        )
    {
        const U16 comp1 = aComponent1;
        const U16 comp2 = aComponent2;
        const U16 comp3 = aComponent3;
        const U16 comp4 = aComponent4;
        return ( ( comp1 & 0xF0 ) << 8 )
            | ( ( comp2 & 0xF0 ) << 4 )
            | ( ( comp3 & 0xF0 ) )
            | ( ( comp4 & 0xF0 ) >> 4 );
    }

    U16 t_pack5b5b5b1bTo16b(
        const U8 aComponent1
        , const U8 aComponent2
        , const U8 aComponent3
        , const U8 aComponent4
        )
    {
        const U16 comp1 = aComponent1;
        const U16 comp2 = aComponent2;
        const U16 comp3 = aComponent3;
        const U16 comp4 = aComponent4;
        return ( ( comp1 & 0xF8 ) << 8 )
            | ( ( comp2 & 0xF8 ) << 3 )
            | ( ( comp3 & 0xF8 ) >> 2 )
            | ( ( comp4 & 0x80 ) >> 7 );
    }

    U32 t_pack10b10b10b2bTo32b(
        const U16 aComponent1
        , const U16 aComponent2
        , const U16 aComponent3
        , const U8 aComponent4
        )
    {
        const U32 comp1 = aComponent1;
        const U32 comp2 = aComponent2;
        const U32 comp3 = aComponent3;
        const U32 comp4 = aComponent4;
        return ( ( comp1 & 0xFFC0 ) << 16 )
            | ( ( comp2 & 0xFFC0 ) << 6 )
            | ( ( comp3 & 0xFFC0 ) >> 4 )
            | ( ( comp4 & 0x00C0 ) >> 6 );
    }

    /// イメージデータの書き込み
    void t_printImageData( ::app::ByteVector& bv 
        , const ::textga::TGADataAccessor& tgaDataAccessor 
        , const ::sysg3d::BinTexturePixelFormat pixelFormat
        )
    {
        std::vector< U8 > inputData;
        const U16 width = tgaDataAccessor.width();
        const U16 height = tgaDataAccessor.height();
        inputData.clear();
        inputData.reserve( width * height * sizeof(U32) );
        
        // inputDataの生成
        for ( size_t y = 0; y < height; ++y )
        {
            for ( size_t x = 0; x < width; ++x )
            {
                const size_t pixelIndex = width * y + x;
                const ::textga::Pixel srcPixel = tgaDataAccessor.pixelAtIndex(pixelIndex);

                switch( pixelFormat )
                {
                case ::sysg3d::BinTexturePixelFormat_A8:
                    inputData.push_back( srcPixel.a );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_L8:
                    inputData.push_back( t_getLuminance( srcPixel ) );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_LA8:
                    inputData.push_back( t_getLuminance( srcPixel ) );
                    inputData.push_back( srcPixel.a );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_I8:
                    inputData.push_back( t_getIntensity( srcPixel ) );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_RGB332:
                    inputData.push_back( t_pack3b3b2bTo8b( srcPixel.r , srcPixel.g , srcPixel.b ) );
                    break;

                case ::sysg3d::BinTexturePixelFormat_RGB565:
                    t_addU16( inputData , t_pack5b6b5bTo16b( srcPixel.r , srcPixel.g , srcPixel.b ) );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_RGB8:
                    inputData.push_back( srcPixel.r );
                    inputData.push_back( srcPixel.g );
                    inputData.push_back( srcPixel.b );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_RGB5A1:
                    t_addU16( inputData , t_pack5b5b5b1bTo16b( srcPixel.r , srcPixel.g , srcPixel.b , srcPixel.a ) );
                    break;

                case ::sysg3d::BinTexturePixelFormat_RGBA4:
                    t_addU16( inputData , t_pack4b4b4b4bTo16b( srcPixel.r , srcPixel.g , srcPixel.b , srcPixel.a ) );
                    break;
                    
                case ::sysg3d::BinTexturePixelFormat_RGBA8:
                    inputData.push_back( srcPixel.r );
                    inputData.push_back( srcPixel.g );
                    inputData.push_back( srcPixel.b );
                    inputData.push_back( srcPixel.a );
                    break;

                default:
                    PJ_ASSERT(0);
                    return; 
                }
            }
        }

        // bvに書き込む
        U32 pixelDataSize = U32();
        switch( pixelFormat )
        {
        case ::sysg3d::BinTexturePixelFormat_A8:            
        case ::sysg3d::BinTexturePixelFormat_L8:
        case ::sysg3d::BinTexturePixelFormat_LA8:
        case ::sysg3d::BinTexturePixelFormat_I8:
        case ::sysg3d::BinTexturePixelFormat_RGB332:
        case ::sysg3d::BinTexturePixelFormat_RGB8:
        case ::sysg3d::BinTexturePixelFormat_RGBA8:
            pixelDataSize = 1;
            break;

        case ::sysg3d::BinTexturePixelFormat_RGB565:
        case ::sysg3d::BinTexturePixelFormat_RGB5A1:
        case ::sysg3d::BinTexturePixelFormat_RGBA4:
            pixelDataSize = 2;
            break;
            
        default:
            PJ_ASSERT(0);
            return; 
        }
        switch( pixelDataSize )
        {
        case 1:
            bv.printTagU8Array( 
                reinterpret_cast< const U8* >( &inputData[0] ) 
                , static_cast<U32>( inputData.size() )
                );
            break;
        case 2:
            bv.printTagU16Array( 
                reinterpret_cast< const U16* >( &inputData[0] ) 
                , static_cast<U32>( inputData.size() / sizeof(U16) )
                );
            break;
        case 4:
            bv.printTagU32Array( 
                reinterpret_cast< const U32* >( &inputData[0] ) 
                , static_cast<U32>( inputData.size() / sizeof(U32) )
                );
            break;
        default:
            PJ_ASSERT(0);
            return;
        }
    }

    std::string t_getImageNameString( const char* filepath )
    {
        // スラッシュを探す
        const std::string str( filepath );
        std::string::size_type startIndex = str.find_last_of("/");
        if ( startIndex == std::string::npos )
        {
            startIndex = 0;
        }
        else
        {
            startIndex += 1;
        }

        // dotを探す
        std::string::size_type dotIndex = str.find_first_of("." , startIndex);
        if ( dotIndex == std::string::npos )
        {
            dotIndex = str.length();
        }

        // 抜き出す
        return str.substr( startIndex , dotIndex - startIndex );
    }

}

//------------------------------------------------------------
namespace app {
namespace sysg3d_texconv {
//------------------------------------------------------------
int EntryPoint::run( const Argument& aArg )
{
    // バナー
    PJ_COUT( "[SysG3d Texture Converter] Program Version '%u.%u' Format Version '%u.%u'\n" 
        , Constant::MAJOR_VERSION
        , Constant::MINOR_VERSION
        , ::sysg3d::BinConstant::VERSION_MAJOR
        , ::sysg3d::BinConstant::VERSION_MINOR
        );

    if ( !ConvertRecipe::isValidArgument( aArg ) )
    {// 無効な引数
        ConvertRecipe::printUseage( aArg );
        return -1;
    }
    const ConvertRecipe convertRecipe( aArg );

    // TGAファイルロード
    ::textga::TGAFileLoader loader( convertRecipe.tgaFilePath() );
    if ( !loader.isLoaded() )
    {
        PJ_COUT( "Error: file load error '%s'\n" , convertRecipe.tgaFilePath() );
        return -1;
    }  
    const ::textga::TGADataAccessor tgaDataAccessor = loader.tgaDataAccessor();
    if ( !tgaDataAccessor.isSupportedTGA() )
    {
        PJ_COUT( "Error: not supported tga file '%s'\n" , convertRecipe.tgaFilePath() );
        return -1;
    }  

    // データの生成
    ByteVector bv;
    {
        // 共通ヘッダ
        ::app::CommonHeaderWriter::writeXMLBegin( bv );
        ::app::CommonHeaderWriter::write(
            bv
            , ::sysg3d::BinKind_Texture
            );

        const char* LABEL_NAME = "texture_name";
        const char* LABEL_IMAGEBDO = "texture_image_bdo_array";
        const char* LABEL_IMAGE = "texture_image";

        const ::sysg3d::BinTexturePixelFormat pixelFormat = t_pixelFormat( tgaDataAccessor );
        if ( pixelFormat == ::sysg3d::BinTexturePixelFormat_Terminate )
        {
            return -1;
        }

        // Textureヘッダ
        bv.printCurrentIndent();
        bv.printComment( "begin sysg3d::BinTextureHeader" );
        bv.printLineEnter();
        {
            bv.indentEnter();

            bv.printCurrentIndent();
            bv.printComment( "bdoName" );
            bv.printTagReference( LABEL_NAME );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "pixelFormat(%s)" , T_PIXELFORMAT_NAMES[ pixelFormat ] );
            bv.printTagU32( pixelFormat );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "width" );
            bv.printTagU16( tgaDataAccessor.width() );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "height" );
            bv.printTagU16( tgaDataAccessor.height() );
            bv.printLineEnter();

            bv.printCurrentIndent();
            bv.printComment( "imageCount" );
            bv.printTagU32( 1 ); // mip map 未対応
            bv.printLineEnter();

            // ImageBDOArray
            bv.printCurrentIndent();
            bv.printComment( "imageBDOArray" );
            bv.printTagReference( LABEL_IMAGEBDO );
            bv.printLineEnter();

            bv.indentReturn();
        }
        bv.printCurrentIndent();
        bv.printComment( "end sysg3d::BinTextureHeader" );
        bv.printLineEnter();

        // name
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_NAME );
        bv.printLineEnter();
        bv.printCurrentIndent();
        bv.printTagString( t_getImageNameString( convertRecipe.tgaFilePath() ).c_str() );
        bv.printLineEnter();
            
        bv.printCurrentIndent();
        bv.printTagLabel( LABEL_IMAGEBDO );
        bv.printLineEnter();
        // BDO
        for ( U32 i = 0; i < 1; ++i )
        {
            bv.printCurrentIndent();
            bv.printTagReferenceF( "%s_%lu" , LABEL_IMAGE, i );
            bv.printLineEnter();
        }
        // Entity
        for ( U32 i = 0; i < 1; ++i )
        {
            bv.printCurrentIndent();
            bv.printTagLabelF( "%s_%lu" , LABEL_IMAGE , i );
            bv.printLineEnter();
            bv.printCurrentIndent();
            t_printImageData( bv , tgaDataAccessor , pixelFormat );
            bv.printLineEnter();
        }        

        // end
        ::app::CommonHeaderWriter::writeXMLEnd( bv );
    }

    // データの出力
    if ( !bv.writeToFile( convertRecipe.outputFilePath() ) )
    {
        return -1;
    }

    return 0;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
