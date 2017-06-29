/**
 * @file
 * @brief Version.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/Version.hpp>

//------------------------------------------------------------
#include <cstdio>
#include <textga/CFunctionAlias.hpp>

//------------------------------------------------------------
namespace textga {
//------------------------------------------------------------
const u8   Version::FORMAT_VERSION = 1;
const uint Version::LIBRARY_VERSION = 0;
//------------------------------------------------------------
std::string Version::asString()
{
    const size_t buffSize = 8;
    char strBuff[ buffSize ];
    TEXTGA_C_SPRINTF( strBuff , "%u.%u" , FORMAT_VERSION , LIBRARY_VERSION );
    return strBuff;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
