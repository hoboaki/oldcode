/**
 * @file
 * @brief MipMapUtil.hppの実装を記述する。
 */
#include <glpic/MipMapUtil.hpp>

//----------------------------------------------------------------
#include <glpic/Assert.hpp>
#include <glpic/BitUtil.hpp>

//----------------------------------------------------------------
namespace
{
    /// ２のべきじょうの値か。
    bool t_is2Bekijo( const unsigned short aSize )
    {
        GLPicAssert( 0 < aSize );
        return ::glpic::BitUtil::getMinBitU16( aSize ) 
            == ::glpic::BitUtil::getMaxBitU16( aSize )
            ? true
            : false;
    }

    /// ミップマップ的に次の値を取得する。
    unsigned short t_getNextSize( const unsigned short aSize )
    {
        GLPicAssert( 1 < aSize );
        GLPicAssert( t_is2Bekijo( aSize ) );
        return aSize >> 1;
    }
}

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
bool MipMapUtil::hasNextLevel(
    const Size2D& aSize2D
    )
{
    if ( t_is2Bekijo( aSize2D.width )
        && t_is2Bekijo( aSize2D.height )
        && 1 < aSize2D.width
        && 1 < aSize2D.height
        )
    {
        return true;
    }
    else
    {
        return false;
    }
}

//----------------------------------------------------------------
Size2D MipMapUtil::nextLevelSize2D(
    const Size2D& aSize2D
    )
{
    GLPicAssert( MipMapUtil::hasNextLevel( aSize2D ) == true );
    return Size2D::create(
        t_getNextSize( aSize2D.width )
        , t_getNextSize( aSize2D.height )
        );
}

//----------------------------------------------------------------
unsigned char MipMapUtil::calculateMaxLevel(
    const Size2D& aSize2D
    )
{
    Size2D size2D = aSize2D;
    unsigned char level = 0;
    for ( ; MipMapUtil::hasNextLevel( size2D ); ++level )
    {
        size2D = MipMapUtil::nextLevelSize2D( size2D );
    }
    return level;
}

//----------------------------------------------------------------
void MipMapUtil::unitTest()
{
    GLPicAssert( MipMapUtil::hasNextLevel( Size2D::create( 2,1 ) ) == false );
    GLPicAssert( MipMapUtil::hasNextLevel( Size2D::create( 3,2 ) ) == false );
    GLPicAssert( MipMapUtil::hasNextLevel( Size2D::create( 256,16 ) ) == true );
    GLPicAssert( MipMapUtil::hasNextLevel( Size2D::create( 0x8000,0x8000 ) ) == true );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 4,2 ) ).width == 2 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 16,4 ) ).height == 2 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 4,4 ) ).width == 2 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 4,4 ) ).height == 2 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 256,128 ) ).width == 128 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 256,128 ) ).height == 64 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 256,256 ) ).width == 128 );
    GLPicAssert( MipMapUtil::nextLevelSize2D( Size2D::create( 256,256 ) ).height == 128 );
    GLPicAssert( MipMapUtil::calculateMaxLevel( Size2D::create(43,33) ) == 0 );
    GLPicAssert( MipMapUtil::calculateMaxLevel( Size2D::create(32,16) ) == 4 );
    GLPicAssert( MipMapUtil::calculateMaxLevel( Size2D::create(32,64) ) == 5 );
    GLPicAssert( MipMapUtil::calculateMaxLevel( Size2D::create(64,64) ) == 6 );
}

//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
