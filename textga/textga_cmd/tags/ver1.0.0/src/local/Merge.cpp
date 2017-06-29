/**
 * @file
 * @brief Merge.hppの実装を記述する。
 */
#include "Merge.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <textga/File.hpp>
#include <textga/Pixel.hpp>
#include <textga/PixelFormatUtil.hpp>
#include <textga/StringUtil.hpp>
#include <textga/TGAFileLoader.hpp>
#include <textga/TGAHeader.hpp>
#include "Assert.hpp"
#include "ArgumentDataIterator.hpp"
#include "ErrorCheck.hpp"
#include "ErrorReport.hpp"

//------------------------------------------------------------
using namespace ::textga;

//------------------------------------------------------------
namespace
{
    enum ChannelKind
    {
        ChannelKind_Average
        ,ChannelKind_Red
        ,ChannelKind_Green
        ,ChannelKind_Blue
        ,ChannelKind_Alpha
        
        ,ChanndlKind_Terminate
        ,ChannelKind_Unknown
    };
    
    ChannelKind t_strToChannelKind( const char* aKind )
    {
        if ( StringUtil::equals( aKind , "ave" ) )
        {
            return ChannelKind_Average;
        }
        else if ( StringUtil::equals( aKind , "r" ) )
        {
            return ChannelKind_Red;
        }
        else if ( StringUtil::equals( aKind , "g" ) )
        {
            return ChannelKind_Green;
        }
        else if ( StringUtil::equals( aKind , "b" ) )
        {
            return ChannelKind_Blue;
        }
        else if ( StringUtil::equals( aKind , "a" ) )
        {
            return ChannelKind_Alpha;
        }
        else
        {
            return ChannelKind_Unknown;
        }
    }
    
    u8 t_getAlpha( const Pixel& aPixel , const ChannelKind aKind )
    {
        switch( aKind )
        {
        case ChannelKind_Average:
            return static_cast< u8 >( (aPixel.r + aPixel.g + aPixel.b) / 8 );
            
        case ChannelKind_Red:
            return aPixel.r;
            
        case ChannelKind_Green:
            return aPixel.g;
            
        case ChannelKind_Blue:
            return aPixel.b;
            
        case ChannelKind_Alpha:
            return aPixel.a;
            
        default:
            PJ_ASSERT(0);
            return 0;
        }
    }
}

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Merge::execute( const ArgumentDataIterator& aItr )
{
    // 引数チェック
    ArgumentDataIterator itr = aItr;
    if ( !itr.hasNext() )
    {
        ErrorReport::argumentNotEnoughError();
        return false;
    }
    const char* inputFilePath = itr.next();
    if ( !itr.hasNext() )
    {
        ErrorReport::argumentNotEnoughError();
        return false;
    }
    const char* alphaFilePath = itr.next();
    if ( !itr.hasNext() )
    {
        ErrorReport::argumentNotEnoughError();
        return false;
    }
    const char* outputFilePath = itr.next();
    ChannelKind channelKind = ChannelKind_Average;
    if ( itr.hasNext() )
    {
        const char* optionName = itr.next();
        if ( !StringUtil::equals( optionName , "-c" ) )
        {
            ErrorReport::argumentUnknownOptionError( optionName );
            return false;
        }
        if ( !itr.hasNext() )
        {
            ErrorReport::argumentNotEnoughError();
            return false;
        }
        const char* channelName = itr.next();
        channelKind = t_strToChannelKind( channelName );
        if ( channelKind == ChannelKind_Unknown )
        {
            std::fprintf( stderr, "Error: Invalid channel name '%s'.\n" , channelName );
            return false;
        }
        if ( itr.hasNext() )
        {
            ErrorReport::argumentTooMuchError();
            return false;
        }
    }
    
    // 元ファイルロード＆エラーチェック
    textga::TGAFileLoader srcTgaFile( inputFilePath );
    if ( !ErrorCheck::supportedTgaCheck( srcTgaFile , inputFilePath ) )
    {
        return false;
    }
    const textga::TGADataAccessor srcTgaDataAccessor = srcTgaFile.tgaDataAccessor();
    
    // アルファファイルロード
    textga::TGAFileLoader alphaTgaFile( alphaFilePath );
    if ( !ErrorCheck::supportedTgaCheck( alphaTgaFile , alphaFilePath ) )
    {
        return false;
    }
    const textga::TGADataAccessor alphaTgaDataAccessor = alphaTgaFile.tgaDataAccessor();
    if ( srcTgaDataAccessor.width() != alphaTgaDataAccessor.width()
        || srcTgaDataAccessor.height() != alphaTgaDataAccessor.height()
        )
    {   
        std::fprintf( stderr, "Error: Alpha image size is not equals to src image size.\n" );
        return false;
    }
    if ( channelKind == ChannelKind_Alpha
        && alphaTgaDataAccessor.bitsPerPixel() != 32
        )
    {
        std::fprintf( stderr, "Error: Not exist alpha channel in %s.\n" , alphaFilePath );
        return false;
    }
    
    // 書き込み開始
    ::textga::File outputFile( outputFilePath , "wb" );
    if ( outputFile.fp() == NULL )
    {
        ErrorReport::fileOpenError( outputFilePath );
        return false;
    }
    
    // TGAHeader
    {
        TGAHeader header = *reinterpret_cast< const TGAHeader* >( srcTgaFile.file().data() );
        header.bitsPerPixel = 32;
        if ( std::fwrite( &header , ::textga::TGAHeader::SIZE , 1 , outputFile.fp() ) < 1 )
        {
            ErrorReport::fileWriteError();
            return false;
        }
    }
    
    // PixelData
    {
        const size_t elementNum = 4;
        std::vector< ::textga::byte > bytes( elementNum * srcTgaDataAccessor.pixelCount() );
        for ( size_t i = 0; i < srcTgaDataAccessor.pixelCount() ; ++i )
        {
            const size_t offset = elementNum * i;
            Pixel pix = srcTgaDataAccessor.pixelAtIndex(i);
            pix.a = t_getAlpha( alphaTgaDataAccessor.pixelAtIndex(i) , channelKind );
            bytes[offset] = pix.b;
            bytes[offset+1] = pix.g;
            bytes[offset+2] = pix.r;
            bytes[offset+3] = pix.a;
        }
        if ( std::fwrite( &bytes[0] , bytes.size() , 1 , outputFile.fp() ) < 1 )
        {
            ErrorReport::fileWriteError();
            return false;
        }
    }
    
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
