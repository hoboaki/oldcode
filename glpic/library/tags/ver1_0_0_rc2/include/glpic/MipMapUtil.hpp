/**
 * @file
 * @brief �~�b�v�}�b�v�Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#if defined(GLPIC_INCLUDED_MIPMAPUTIL_HPP)
#else
#define GLPIC_INCLUDED_MIPMAPUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/Noncreatable.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// �~�b�v�}�b�v�Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class MipMapUtil : public Noncreatable
    {
    public:
        /**
         * @brief ����MipMap���x�������邩�B
         * �����A�c�������ꂩ���Q�ׂ̂���łȂ��ꍇ�A
         * �����A�c�������ꂩ���P�ł���ꍇ�A�U��Ԃ��B
         */
        static bool hasNextLevel( const Size2D& );

        /**
        * @brief ����MipMap���x���̃T�C�Y���擾����B
        * GLPicMipMapUtil_HasNextLevel��GLPicBool��
        * �Ԃ��悤�ȃT�C�Y��n�����ꍇ�̓���͕s��B
        */
        static Size2D nextLevelSize2D( const Size2D& );

        /**
        * @brief MipMap�̍ő僌�x�������擾����B
        * @return 0�ȏ�B
        */
        static unsigned char calculateMaxLevel( const Size2D& );

        /// MipMapUtil�̃��j�b�g�e�X�g�B
        static void unitTest();

    };

}
//----------------------------------------------------------------
#endif
// EOF
