/**
 * @file
 * @brief Information.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "Information.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <textga/PixelFormatUtil.hpp>
#include <textga/TGAFileLoader.hpp>
#include "ArgumentDataIterator.hpp"
#include "ErrorCheck.hpp"
#include "ErrorReport.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Information::execute( const ArgumentDataIterator& aItr )
{
    ArgumentDataIterator itr = aItr;
    if ( !itr.hasNext() )
    {
        ErrorReport::argumentNotEnoughError();
        return false;
    }
    
    while ( itr.hasNext() )
    {
        const char* fileName = itr.next();
        textga::TGAFileLoader tgaFile( fileName );
        if ( !ErrorCheck::supportedTgaCheck( tgaFile , fileName ) )
        {
            return false;
        }
        
        const textga::TGADataAccessor tgaDataAccessor = tgaFile.tgaDataAccessor();
        std::printf( "Path   : %s\n" , fileName );
        std::printf( "Width  : %d\n" , tgaDataAccessor.width() );
        std::printf( "Height : %d\n" , tgaDataAccessor.height() );
        std::printf( "BPP    : %d\n" , tgaDataAccessor.bitsPerPixel() );
        if ( tgaDataAccessor.isTexTarga() )
        {
            std::printf( "Is Tex-Targa : Yes\n" );
            std::printf( "Pixel-Format : %s\n" , textga::PixelFormatUtil::toName( tgaDataAccessor.pixelFormat() ) );
            std::printf( "Has Src-Data : %s\n" , tgaDataAccessor.isExistSrcData() ? "Yes" : "No" );
        }
        else
        {
            std::printf( "Is Tex-Targa : No\n" );
        }
        std::printf( "\n" );
    }
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
