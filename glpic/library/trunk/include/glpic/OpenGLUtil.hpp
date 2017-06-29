/**
 * @file
 * @brief OpenGLに関するユーティリティ関数を記述する。
 * GLPIC_NO_OPENGLが定義されていると使えない。
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

    /// OpenGLに関するユーティリティ関数群。
    class OpenGLUtil : public Noncreatable
    {
    public:
        /// glTexImage2Dの引数として使うinternalFormatに変換する。
        static GLenum toInternalFormat( PixelFormat );

        /// glTexImage2Dの引数として使うformatに変換する。
        static GLenum toFormat( PixelFormat );

        /// glTexImage2Dの引数として使うtypeに変換する。
        static GLenum toType( PixelFormat );
    };

#endif
}
//----------------------------------------------------------------
#endif
// EOF
