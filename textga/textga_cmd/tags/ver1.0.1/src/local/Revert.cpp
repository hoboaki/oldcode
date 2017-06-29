/**
 * @file
 * @brief Revert.hpp�̎������L�q����B
 */
#include "Revert.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <textga/File.hpp>
#include <textga/PixelFormatUtil.hpp>
#include <textga/TGAFileLoader.hpp>
#include <textga/TGAHeader.hpp>
#include "ArgumentDataIterator.hpp"
#include "ErrorCheck.hpp"
#include "ErrorReport.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Revert::execute( const ArgumentDataIterator& aItr )
{
    // �����`�F�b�N
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
    const char* outputFilePath = itr.next();
    if ( itr.hasNext() )
    {
        ErrorReport::argumentTooMuchError();
    }
    
    // �t�@�C�����[�h���G���[�`�F�b�N
    textga::TGAFileLoader tgaFile( inputFilePath );
    if ( !ErrorCheck::texTargaCheck( tgaFile , inputFilePath ) )
    {
        return false;
    }
    const textga::TGADataAccessor tgaDataAccessor = tgaFile.tgaDataAccessor();
    if ( !tgaDataAccessor.isExistSrcData() )
    {
        std::fprintf( stderr, "Error: Not exist src data at file %s\n" , inputFilePath );
        return false;
    }
    
    // �������݊J�n
    ::textga::File outputFile( outputFilePath , "wb" );
    if ( outputFile.fp() == NULL )
    {
        ErrorReport::fileOpenError( outputFilePath );
        return false;
    }
    
    // TGAHeader
    if ( std::fwrite( tgaFile.file().data() , ::textga::TGAHeader::SIZE , 1 , outputFile.fp() ) < 1 )
    {
        ErrorReport::fileWriteError();
        return false;
    }
    
    // PixelData
    if ( std::fwrite( tgaDataAccessor.srcData() , tgaDataAccessor.pixelDataSize() , 1 , outputFile.fp() ) < 1 )
    {
        ErrorReport::fileWriteError();
        return false;
    }

    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
