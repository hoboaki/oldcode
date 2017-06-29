/**
 * @file
 * @brief OpenGLUtil.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include <glpic/OpenGLUtil.hpp>

#if defined(GLPIC_NO_OPEN_GL)
#else
//----------------------------------------------------------------
#include <glpic/Assert.hpp>
#include <glpic/OpenGL.hpp>
#include <glpic/PixelFormatUtil.hpp>

//----------------------------------------------------------------
#ifndef GL_COMPRESSED_RGB_S3TC_DXT1_EXT
#define GL_COMPRESSED_RGB_S3TC_DXT1_EXT 0x83F0
#endif

#ifndef GL_COMPRESSED_RGBA_S3TC_DXT1_EXT
#define GL_COMPRESSED_RGBA_S3TC_DXT1_EXT 0x83F1
#endif

#ifndef GL_COMPRESSED_RGBA_S3TC_DXT3_EXT
#define GL_COMPRESSED_RGBA_S3TC_DXT3_EXT 0x83F2
#endif

#ifndef GL_COMPRESSED_RGBA_S3TC_DXT5_EXT
#define GL_COMPRESSED_RGBA_S3TC_DXT5_EXT 0x83F3
#endif

//----------------------------------------------------------------
namespace glpic {
//----------------------------------------------------------------
GLenum OpenGLUtil::toInternalFormat(
    const PixelFormat aPixelFormat
    )
{
    GLPicAssert( PixelFormatUtil::isValid( aPixelFormat ) );

    switch ( aPixelFormat )
    {
    case PixelFormat_A8:
        return GL_ALPHA8;

    case PixelFormat_L8:
        return GL_LUMINANCE8;

    case PixelFormat_LA8:
        return GL_LUMINANCE8_ALPHA8;

    case PixelFormat_I8:
        return GL_INTENSITY8;

    case PixelFormat_RGB565:
        return GL_RGB5;

    case PixelFormat_RGB8:
        return GL_RGB8;

    case PixelFormat_RGB5A1:
        return GL_RGB5_A1;

    case PixelFormat_RGBA8:
        return GL_RGBA8;

    case PixelFormat_RGBA_S3TC_DXT1:
        return GL_COMPRESSED_RGBA_S3TC_DXT1_EXT;

    case PixelFormat_RGBA_S3TC_DXT3:
        return GL_COMPRESSED_RGBA_S3TC_DXT3_EXT;

    case PixelFormat_RGBA_S3TC_DXT5:
        return GL_COMPRESSED_RGBA_S3TC_DXT5_EXT;

    default:
        GLPicAssert(0);
        return 0;
    }
}

//----------------------------------------------------------------
GLenum OpenGLUtil::toFormat(
    const PixelFormat aPixelFormat
    )
{
    GLPicAssert( PixelFormatUtil::isValid( aPixelFormat ) );

    switch ( aPixelFormat )
    {
    case PixelFormat_A8:
        return GL_ALPHA;

    case PixelFormat_L8:
        return GL_LUMINANCE;

    case PixelFormat_LA8:
        return GL_LUMINANCE_ALPHA;

    case PixelFormat_I8:
        return GL_INTENSITY;

    case PixelFormat_RGB565:
    case PixelFormat_RGB8:
        return GL_RGB;

    case PixelFormat_RGB5A1:
    case PixelFormat_RGBA8:
    case PixelFormat_RGBA_S3TC_DXT1:
    case PixelFormat_RGBA_S3TC_DXT3:
    case PixelFormat_RGBA_S3TC_DXT5:
        return GL_RGBA;

    default:
        GLPicAssert(0);
        return 0;
    }
}

}
//----------------------------------------------------------------
#endif
// EOF
