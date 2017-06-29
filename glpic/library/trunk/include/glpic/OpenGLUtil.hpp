/**
 * @file
 * @brief OpenGL�Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 * GLPIC_NO_OPENGL����`����Ă���Ǝg���Ȃ��B
 */
#if defined(GLPIC_INCLUDED_OPENGLUTIL_HPP)
#else
#define GLPIC_INCLUDED_OPENGLUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/Noncreatable.hpp>
#include <glpic/OpenGL.hpp>
#include <glpic/PixelFormat.hpp>

//----------------------------------------------------------------
// public
namespace glpic {
#if defined(GLPIC_NO_OPENGL)
#else

    /// OpenGL�Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class OpenGLUtil : public Noncreatable
    {
    public:
        /// glTexImage2D�̈����Ƃ��Ďg��internalFormat�ɕϊ�����B
        static GLenum toInternalFormat( PixelFormat );

        /// glTexImage2D�̈����Ƃ��Ďg��format�ɕϊ�����B
        static GLenum toFormat( PixelFormat );

        /// glTexImage2D�̈����Ƃ��Ďg��type�ɕϊ�����B
        static GLenum toType( PixelFormat );
    };

#endif
}
//----------------------------------------------------------------
#endif
// EOF
