/**
 * @file
 * @brief TGAFileLoader.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/TGAFileLoader.hpp>

//------------------------------------------------------------
#include <textga/Assert.hpp>

//------------------------------------------------------------
namespace textga {
//------------------------------------------------------------
TGAFileLoader::TGAFileLoader( const char* aFilepath )
: file_( aFilepath )
{
}

//------------------------------------------------------------
bool TGAFileLoader::isLoaded()const
{
    return file_.isLoaded();
}

//------------------------------------------------------------
TGADataAccessor TGAFileLoader::tgaDataAccessor()const
{
    TEXTGA_ASSERT( isLoaded() );
    return TGADataAccessor( file_.data() , file_.dataSize() );
}

//------------------------------------------------------------
const FileLoader& TGAFileLoader::file()const
{
    return file_;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
