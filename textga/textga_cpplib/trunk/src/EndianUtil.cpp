/**
 * @file
 * @brief EndianUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <textga/EndianUtil.hpp>

//------------------------------------------------------------
#include <textga/Endian.hpp>

//------------------------------------------------------------
namespace textga {
//------------------------------------------------------------
u16 EndianUtil::swapU16BE( const u16 aValue )
{
    if ( Endian::isLittleEndian() )
    {
        return aValue;
    }
    else
    {
        u16 val = aValue;
        {
            byte* bytePtr = reinterpret_cast< byte* >( &val );
            byte tmp = bytePtr[0];
            bytePtr[0] = bytePtr[1];
            bytePtr[1] = tmp;
        }
        return val;
    }
}

//------------------------------------------------------------
u32 EndianUtil::swapU32BE( const u32 aValue )
{
    if ( Endian::isLittleEndian() )
    {
        return aValue;
    }
    else
    {
        u32 val = aValue;
        {
            byte* bytePtr = reinterpret_cast< byte* >( &val );
            byte tmp;
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
s16 EndianUtil::swapS16BE( const s16 aValue )
{
    if ( Endian::isLittleEndian() )
    {
        return aValue;
    }
    else
    {
        s16 val = aValue;
        {
            byte* bytePtr = reinterpret_cast< byte* >( &val );
            byte tmp = bytePtr[0];
            bytePtr[0] = bytePtr[1];
            bytePtr[1] = tmp;
        }
        return val;
    }
}

//------------------------------------------------------------
s32 EndianUtil::swapS32BE( const s32 aValue )
{
    if ( Endian::isLittleEndian() )
    {
        return aValue;
    }
    else
    {
        s32 val = aValue;
        {
            byte* bytePtr = reinterpret_cast< byte* >( &val );
            byte tmp;
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
