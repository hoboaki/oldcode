/**
 * @file
 * @brief Version.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "Version.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <textga/CFunctionAlias.hpp>
#include <textga/Version.hpp>

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
const ::textga::uint Version::BUGFIX_VERSION = 0;

//------------------------------------------------------------
std::string Version::asString()
{
    const size_t buffSize = 12;
    char strBuff[ buffSize ];
    TEXTGA_C_SPRINTF( strBuff , "%u.%u.%u" 
        , ::textga::Version::FORMAT_VERSION
        , ::textga::Version::LIBRARY_VERSION
        , BUGFIX_VERSION
        );
    return strBuff;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
