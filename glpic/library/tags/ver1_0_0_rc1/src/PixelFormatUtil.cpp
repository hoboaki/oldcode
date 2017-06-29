/**
 * @file
 * @brief PixelFormatUtil.hpp�̎������L�q����B
 */
#include <glpic/PixelFormatUtil.hpp>

//----------------------------------------------------------------
#include <glpic/Assert.hpp>
#include <glpic/MipMapUtil.hpp>

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
bool PixelFormatUtil::isValid( const PixelFormat aFormat )
{
    return PixelFormat_Unknown < aFormat
        && aFormat < PixelFormat_Terminate;
}

//----------------------------------------------------------------
unsigned char PixelFormatUtil::getBitPerPixel(
    const PixelFormat aFormat
    )
{
    GLPicAssert( isValid( aFormat ) );
    switch( aFormat )
    {
    case PixelFormat_A8:     return 8;
        
    case PixelFormat_L8:     return 8;
        
    case PixelFormat_LA8:    return 16;
        
    case PixelFormat_I8:     return 8;

    case PixelFormat_RGB565: return 16;
    case PixelFormat_RGB8:   return 24;
        
    case PixelFormat_RGB5A1:   return 16;
    case PixelFormat_RGBA8:    return 32;

    case PixelFormat_RGB_S3TC_DXT1:  return 4;
    case PixelFormat_RGBA_S3TC_DXT3: return 8;
    case PixelFormat_RGBA_S3TC_DXT5: return 8;

    default:
        GLPicAssert(0);
        return 0;
    }
}

//----------------------------------------------------------------
unsigned char PixelFormatUtil::getPixelCountPerBlock(
    const PixelFormat aFormat
    )
{
    GLPicAssert( isValid( aFormat ) );
    switch( aFormat )
    {
    case PixelFormat_A8:     return 1;
        
    case PixelFormat_L8:     return 1;
        
    case PixelFormat_LA8:    return 1;
        
    case PixelFormat_I8:     return 1;

    case PixelFormat_RGB565: return 1;
    case PixelFormat_RGB8:   return 1;
        
    case PixelFormat_RGB5A1:   return 1;
    case PixelFormat_RGBA8:    return 1;

    case PixelFormat_RGB_S3TC_DXT1:  return 4;
    case PixelFormat_RGBA_S3TC_DXT3: return 4;
    case PixelFormat_RGBA_S3TC_DXT5: return 4;

    default:
        GLPicAssert(0);
        return 0;
    }
}

//----------------------------------------------------------------
bool PixelFormatUtil::isValidSizeForPixelFormat(
    const PixelFormat aPixelFormat
    , const Size2D& aTextureSize
    )
{
    return ((aTextureSize.width * aTextureSize.height)
        % getPixelCountPerBlock( aPixelFormat )) == 0;
}

//----------------------------------------------------------------
unsigned long PixelFormatUtil::calculatePixelDataSize(
    const PixelFormat aPixelFormat
  , const Size2D& aTextureSize
  )
{
    unsigned long bitSize;
    if ( !isValid( aPixelFormat ) )
    {// �s�N�Z���t�H�[�}�b�g���s���B
        return 0;
    }

    if ( !isValidSizeForPixelFormat( aPixelFormat , aTextureSize ) )
    {// �e�N�X�`���̃T�C�Y���t�H�[�}�b�g�ɑ΂��ĕs���B
        return 0;
    }

    // �r�b�g���̌v�Z�B
    bitSize = 
        getBitPerPixel( aPixelFormat )
        * aTextureSize.width
        * aTextureSize.height;
    GLPicAssert( bitSize != 0 );

    return ( bitSize >> 3 ) // 8�Ŋ���
        + ( ( ( bitSize & 7 ) != 0 )
            ? 1 // �]��𑫂��B
            : 0); 
}

//----------------------------------------------------------------
unsigned long PixelFormatUtil::calculateMipMapPixelDataSize(
    const PixelFormat aPixelFormat
    , const Size2D& aTextureSize
    , const unsigned char aMipMapMaxLevel
    )
{
    unsigned char i;
    unsigned long dataSize;
    Size2D size2D = aTextureSize;

    if ( !isValid( aPixelFormat ) )
    {// �s�N�Z���t�H�[�}�b�g�B
        return 0;
    }

    if ( MipMapUtil::calculateMaxLevel( aTextureSize ) < aMipMapMaxLevel )
    {// �~�b�v�}�b�v�̒l���s���B
        return 0;
    }

    for ( i = 0 , dataSize = 0; i <= aMipMapMaxLevel; ++i )
    {
        if ( i != 0 )
        {
            if ( !MipMapUtil::hasNextLevel( size2D ) )
            {// �~�b�v�}�b�v�A���̃��x�����Ȃ��͕̂s��
                return 0;
            }
            size2D = MipMapUtil::nextLevelSize2D( size2D );
        }

        if ( !isValidSizeForPixelFormat( aPixelFormat , size2D ) )
        {// �e�N�X�`���̃T�C�Y���t�H�[�}�b�g�ɑ΂��ĕs���B
            return 0;
        }

        dataSize += PixelFormatUtil::calculatePixelDataSize( 
            aPixelFormat
            , size2D
            );
    }
    return dataSize;
}

//----------------------------------------------------------------
void PixelFormatUtil::unitTest()
{
    GLPicAssert( PixelFormatUtil::calculatePixelDataSize(
        PixelFormat_A8 , Size2D::create(1,1)
        ) == 1 );
    GLPicAssert( PixelFormatUtil::calculatePixelDataSize(
        PixelFormat_LA8 , Size2D::create(1,1)
        ) == 2 );
    GLPicAssert( PixelFormatUtil::calculatePixelDataSize(
        PixelFormat_RGB565 , Size2D::create(2,1)
        ) == 4 );
    GLPicAssert( PixelFormatUtil::calculatePixelDataSize(
        PixelFormat_RGB5A1 , Size2D::create(3,1)
        ) == 6 );
    GLPicAssert( PixelFormatUtil::calculatePixelDataSize(
        PixelFormat_RGBA8 , Size2D::create(2,2)
        ) == 16 );
}


//----------------------------------------------------------------
}
//----------------------------------------------------------------
// EOF
