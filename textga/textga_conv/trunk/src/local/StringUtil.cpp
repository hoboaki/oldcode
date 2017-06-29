/**
 * @file
 * @brief StringUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "StringUtil.hpp"

//------------------------------------------------------------
#include <cstring>

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
bool StringUtil::equals( 
    const char* aText1
    , const char* aText2
    )
{
    return std::strcmp( aText1 , aText2 ) == 0;
}

//------------------------------------------------------------
bool StringUtil::caseEquals(
    const char* aText1
    , const char* aText2
    )
{
#if defined(_MSC_VER)
    return strcmpi( aText1 , aText2 ) == 0;
#else
    return strcasecmp( aText1 , aText2 ) == 0;
#endif
}

//------------------------------------------------------------
std::string StringUtil::baseName( const char* aFilePath )
{
    const std::string filepath( aFilePath );
    const std::string::size_type dotPos = filepath.rfind( "." );
    const std::string::size_type slashPos = filepath.rfind( "/" );
    const std::string::size_type startPos =
        slashPos == std::string::npos ? 0 : slashPos+1;
    return filepath.substr( startPos
        , dotPos == std::string::npos
            ? std::string::npos
            : dotPos - startPos
            );
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
