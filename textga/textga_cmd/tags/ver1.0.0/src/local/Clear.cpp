/**
 * @file
 * @brief Clear.hpp�̎������L�q����B
 */
#include "Clear.hpp"

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
using namespace ::textga;

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool Clear::execute( const ArgumentDataIterator& aItr )
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
    
    // �t�@�C�����[�h
    textga::TGAFileLoader tgaFile( inputFilePath );
    if ( !ErrorCheck::texTargaCheck( tgaFile , inputFilePath ) )
    {
        return false;
    }
    
    // �������݊J�n
    ::textga::File outputFile( outputFilePath , "wb" );
    if ( outputFile.fp() == NULL )
    {
        ErrorReport::fileOpenError( outputFilePath );
        return false;
    }
    
    // TGAHeader And Pixeldata
    const TGADataAccessor tgaDataAccessor = tgaFile.tgaDataAccessor();
    if ( std::fwrite( tgaFile.file().data() , ::textga::TGAHeader::SIZE + tgaDataAccessor.pixelDataSize() , 1 , outputFile.fp() ) < 1 )
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
