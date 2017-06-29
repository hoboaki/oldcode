/**
 * @file
 * @brief ByteOrderUtil.hpp‚ÌÀ‘•‚ğ‹Lq‚·‚éB
 */
#include <glpic/ByteOrderUtil.hpp>

//----------------------------------------------------------------
#include <glpic/Assert.hpp>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
unsigned short ByteOrderUtil::inverseU16(
    const unsigned short aValue
    )
{
    unsigned short value = aValue;
    unsigned char* byteArray = reinterpret_cast<unsigned char*>(&value);
    unsigned char tmp = byteArray[0];
    byteArray[0] = byteArray[1];
    byteArray[1] = tmp;
    return value;
}

//----------------------------------------------------------------
unsigned long ByteOrderUtil::inverseU32(
    const unsigned long aValue
    )
{
    unsigned long value = aValue;
    unsigned char* byteArray = reinterpret_cast<unsigned char*>(&value);
    unsigned char tmp = byteArray[0];
    byteArray[0] = byteArray[3];
    byteArray[3] = tmp;
    tmp = byteArray[1];
    byteArray[1] = byteArray[2];
    byteArray[2] = tmp;
    return value;
}

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
