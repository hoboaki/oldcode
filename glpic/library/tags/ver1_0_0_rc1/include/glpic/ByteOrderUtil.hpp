/**
 * @file
 * @brief �o�C�g�I�[�_�[�Ɋւ���֐����L�q����B
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

    /// �o�C�g�I�[�_�[�Ɋւ���֐��Q�B
    class ByteOrderUtil : public ::glpic::Noncreatable
    {
    public:
        /// U16�̃o�C�g�I�[�_�[���t�]����B
        static unsigned short inverseU16( unsigned short );

        /// U32�̃o�C�g�I�[�_�[���t�]����B
        static unsigned long inverseU32( unsigned long );
    };

}
//----------------------------------------------------------------
#endif
// EOF
