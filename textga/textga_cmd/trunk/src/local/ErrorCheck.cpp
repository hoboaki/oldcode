/**
 * @file
 * @brief ErrorCheck.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "ErrorCheck.hpp"

//------------------------------------------------------------
#include <textga/TGADataAccessor.hpp>
#include <textga/TGAFileLoader.hpp>
#include "ErrorReport.hpp"

//------------------------------------------------------------
using namespace ::textga;

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool ErrorCheck::supportedTgaCheck( 
    const TGAFileLoader& aTgaFile 
    , const char* aFilePath 
    )
{
    if ( !aTgaFile.file().isLoaded() )
    {
        ErrorReport::fileOpenError( aFilePath );
        return false;
    }
     
    const TGADataAccessor tgaDataAccessor = aTgaFile.tgaDataAccessor();
    if ( !tgaDataAccessor.isSupportedTGA() )
    {
        ErrorReport::notSupportedTGAError( aFilePath );
        return false;
    }
    
    return true;
}

//------------------------------------------------------------
bool ErrorCheck::texTargaCheck(
    const TGAFileLoader& aTgaFile 
    , const char* aFilePath 
    )
{
    if ( !supportedTgaCheck( aTgaFile , aFilePath ) )
    {
        return false;
    }
    
    const TGADataAccessor tgaDataAccessor = aTgaFile.tgaDataAccessor();
    if ( !tgaDataAccessor.isTexTarga() )
    {
        ErrorReport::notTexTargaError( aFilePath );
        return false;
    }
    
    return true;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
