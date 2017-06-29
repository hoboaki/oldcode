/**
 * @file
 * @brief BitUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <glpic/BitUtil.hpp>

//----------------------------------------------------------------
#include <glpic/Assert.hpp>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
const signed char BitUtil::NO_BIT = -1;
//----------------------------------------------------------------
signed char BitUtil::getMaxBitU16( 
    const unsigned short aValue 
    )
{
    if ( aValue == 0 )
    {
        return NO_BIT;
    }
    else
    {
        unsigned short mask = 0x8000;
        for ( signed char i = 15; i >= 0; --i ,mask >>=  1 )
        {
            if ( ( aValue & mask ) != 0 )
            {
                return i;
            }
        }
        GLPicAssert( 0 );
        return NO_BIT;
    } 
}

//----------------------------------------------------------------
signed char BitUtil::getMinBitU16(
    const unsigned short aValue
    )
{
    if ( aValue == 0 )
    {
        return NO_BIT;
    }
    else
    {
        unsigned short mask = 0x0001;
        for ( signed char i = 0; i < 16; ++i ,mask <<= 1 )
        {
            if ( ( aValue & mask ) != 0 )
            {
                return i;
            }
        }
        GLPicAssert( 0 );
        return NO_BIT;
    } 
}

//----------------------------------------------------------------
unsigned char BitUtil::pack4b4bTo8b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    )
{
    return ( aComponent1 & 0xF0 ) 
        | ( ( aComponent2 & 0xF0 ) >> 4 );
}

//----------------------------------------------------------------
unsigned char BitUtil::pack6b2bTo8b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    )
{
    return ( aComponent1 & 0xFC )
        | ( ( aComponent2 & 0xE0 ) >> 6 );
}

//----------------------------------------------------------------
unsigned char BitUtil::pack3b3b2bTo8b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    , const unsigned char aComponent3
    )
{
    return ( aComponent1 & 0xE0 )
        | ( ( aComponent2 & 0xE0 ) >> 3 )
        | ( ( aComponent3 & 0xC0 ) >> 6 );
}

//----------------------------------------------------------------
unsigned short BitUtil::pack5b5b5bTo16b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    , const unsigned char aComponent3
    )
{
    const unsigned short comp1 = aComponent1;
    const unsigned short comp2 = aComponent2;
    const unsigned short comp3 = aComponent3;
    return ( ( comp1 & 0xF8 ) << 8 )
        | ( ( comp2 & 0xF8 ) << 3 )
        | ( ( comp3 & 0xF8 ) >> 2 );
}

//----------------------------------------------------------------
unsigned long BitUtil::pack10b10b10bTo32b(
    const unsigned short aComponent1
    , const unsigned short aComponent2
    , const unsigned short aComponent3
    )
{
    const unsigned long comp1 = aComponent1;
    const unsigned long comp2 = aComponent2;
    const unsigned long comp3 = aComponent3;
    return ( ( comp1 & 0xFFC0 ) << 16 )
        | ( ( comp2 & 0xFFC0 ) << 6 )
        | ( ( comp3 & 0xFFC0 ) >> 4 );
}

//----------------------------------------------------------------
unsigned char BitUtil::pack2b2b2b2bTo8b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    , const unsigned char aComponent3
    , const unsigned char aComponent4
    )
{
    return ( aComponent1 & 0xC0 )
        | ( ( aComponent2 & 0xC0 ) >> 2 )
        | ( ( aComponent3 & 0xC0 ) >> 4 )
        | ( ( aComponent4 & 0xC0 ) >> 6 );
}

//----------------------------------------------------------------
unsigned short BitUtil::pack4b4b4b4bTo16b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    , const unsigned char aComponent3
    , const unsigned char aComponent4
    )
{
    const unsigned short comp1 = aComponent1;
    const unsigned short comp2 = aComponent2;
    const unsigned short comp3 = aComponent3;
    const unsigned short comp4 = aComponent4;
    return ( ( comp1 & 0xF0 ) << 8 )
        | ( ( comp2 & 0xF0 ) << 4 )
        | ( ( comp3 & 0xF0 ) )
        | ( ( comp4 & 0xF0 ) >> 4 );
}

//----------------------------------------------------------------
unsigned short BitUtil::pack5b5b5b1bTo16b(
    const unsigned char aComponent1
    , const unsigned char aComponent2
    , const unsigned char aComponent3
    , const unsigned char aComponent4
    )
{
    const unsigned short comp1 = aComponent1;
    const unsigned short comp2 = aComponent2;
    const unsigned short comp3 = aComponent3;
    const unsigned short comp4 = aComponent4;
    return ( ( comp1 & 0xF8 ) << 8 )
        | ( ( comp2 & 0xF8 ) << 3 )
        | ( ( comp3 & 0xF8 ) >> 2 )
        | ( ( comp4 & 0x80 ) >> 7 );
}

//----------------------------------------------------------------
unsigned long BitUtil::pack10b10b10b2bTo32b(
    const unsigned short aComponent1
    , const unsigned short aComponent2
    , const unsigned short aComponent3
    , const unsigned char aComponent4
    )
{
    const unsigned long comp1 = aComponent1;
    const unsigned long comp2 = aComponent2;
    const unsigned long comp3 = aComponent3;
    const unsigned long comp4 = aComponent4;
    return ( ( comp1 & 0xFFC0 ) << 16 )
        | ( ( comp2 & 0xFFC0 ) << 6 )
        | ( ( comp3 & 0xFFC0 ) >> 4 )
        | ( ( comp4 & 0x00C0 ) >> 6 );
}

//----------------------------------------------------------------
void BitUtil::unitTest()
{
    GLPicAssert( BitUtil::getMaxBitU16( 0xFFFF ) == 15 );
    GLPicAssert( BitUtil::getMinBitU16( 0xFFFF ) == 0 );
    GLPicAssert( BitUtil::pack4b4bTo8b( 0x80,0x80 ) == 0x88 );
    GLPicAssert( BitUtil::pack6b2bTo8b( 0x80,0x80 ) == 0x82 );
    GLPicAssert( BitUtil::pack3b3b2bTo8b( 0x80,0x80,0x80 ) == 0x92 );
    GLPicAssert( BitUtil::pack5b5b5bTo16b( 0x80,0x80,0x80 ) == 0x8420 );
    GLPicAssert( BitUtil::pack10b10b10bTo32b( 0x8000,0x8000,0x8000 ) == 0x80200800 );
    GLPicAssert( BitUtil::pack2b2b2b2bTo8b( 0x80,0x80,0x80,0x80 ) == 0xAA );
    GLPicAssert( BitUtil::pack4b4b4b4bTo16b( 0x80,0x80,0x80,0x80 ) == 0x8888 );
    GLPicAssert( BitUtil::pack5b5b5b1bTo16b( 0x80,0x80,0x80,0x80 ) == 0x8421 );
    GLPicAssert( BitUtil::pack10b10b10b2bTo32b( 0x8000,0x8000,0x8000,0x80 ) == 0x80200802 );
}

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
