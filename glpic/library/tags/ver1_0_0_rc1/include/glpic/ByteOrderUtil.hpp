/**
 * @file
 * @brief バイトオーダーに関する関数を記述する。
 */
#if defined(GLPIC_INCLUDED_BYTEORDERUTIL_HPP)
#else
#define GLPIC_INCLUDED_BYTEORDERUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/Noncreatable.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// バイトオーダーに関する関数群。
    class ByteOrderUtil : public ::glpic::Noncreatable
    {
    public:
        /// U16のバイトオーダーを逆転する。
        static unsigned short inverseU16( unsigned short );

        /// U32のバイトオーダーを逆転する。
        static unsigned long inverseU32( unsigned long );
    };

}
//----------------------------------------------------------------
#endif
// EOF
