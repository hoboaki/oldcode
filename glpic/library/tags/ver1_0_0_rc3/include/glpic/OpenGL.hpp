/**
 * *@file
 * @brief OpenGLのヘッダをインクルードする。
 * GLPIC_NO_OPENGLが定義されていたらインクルードしない。
 */
#if defined(GLPIC_INCLUDED_OPENGL_HPP)
#else
#define GLPIC_INCLUDED_OPENGL_HPP

//----------------------------------------------------------------
// public
#if defined(GLPIC_NO_OPENGL)
#else

#if defined(WIN32)
#include <windows.h>
#include <GL/gl.h>
#endif

#if defined(__APPLE__)
#include <OpenGL/gl.h>
#endif

#endif
//----------------------------------------------------------------
#endif
// EOF
