/**
 * @file
 * @brief EndianUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "EndianUtil.hpp"

//------------------------------------------------------------
#include "Endian.hpp"

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
short EndianUtil::swapS16BE( const short aValue )
{
    if ( Endian::isLittleEndian() )
    {
        return aValue;
    }
    else
    {
        short val = aValue;
        {
            unsigned char* bytePtr = reinterpret_cast< unsigned char* >( &val );
            unsigned char tmp = bytePtr[0];
            bytePtr[0] = bytePtr[1];
            bytePtr[1] = tmp;
        }
        return val;
    }
}

//------------------------------------------------------------
int EndianUtil::swapS32BE( const int aValue )
{
    if ( Endian::isLittleEndian() )
    {
        return aValue;
    }
    else
    {
        int val = aValue;
        {
            unsigned char* bytePtr = reinterpret_cast< unsigned char* >( &val );
            unsigned char tmp;
            tmp = bytePtr[0];
            bytePtr[0] = bytePtr[3];
            bytePtr[3] = tmp;
            tmp = bytePtr[1];
            bytePtr[1] = bytePtr[2];
            bytePtr[2] = tmp;
        }
        return val;
    }
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
