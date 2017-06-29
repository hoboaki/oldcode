/**
 * @file
 * @brief �s�N�Z���t�H�[�}�b�g�̊֐��Q���L�q����B
 */
#if defined(GLPIC_INCLUDED_PIXELFORMATUTIL_HPP)
#else
#define GLPIC_INCLUDED_PIXELFORMATUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/PixelFormat.hpp>
#include <glpic/Noncreatable.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    class PixelFormatUtil : public Noncreatable
    {
    public:
        /// �s�N�Z���t�H�[�}�b�g�̎��ʎq�Ƃ��Đ������l���B
        static bool isValid( PixelFormat );

        /// 1�s�N�Z��������̃r�b�g�����擾����B
        static unsigned char getBitPerPixel( PixelFormat );

        /**
         * @brief 1�u���b�N�̃s�N�Z�������擾����B
         *
         * �s�N�Z���f�[�^�̓u���b�N�̐����{�ł���K�v������B@n
         * �Ⴆ��1�u���b�N4�s�N�Z���ȃt�H�[�}�b�g�̏ꍇ�A@n
         * �s�N�Z���̑�����4�̔{���ł���K�v������B
         */
        static unsigned char getPixelCountPerBlock( PixelFormat );

        /**
         * @brief �e�N�X�`���̃T�C�Y���t�H�[�}�b�g�ɑ΂��ēK�؂��B
         * @return ��肪�Ȃ����true�B
         * @param pixelFormat �s�N�Z���t�H�[�}�b�g�B
         * @param textureSize �e�N�X�`���T�C�Y�B
         */
        static bool isValidSizeForPixelFormat( 
            PixelFormat pixelFormat
            , const Size2D& textureSize
            );

        /**
        * @brief �s�N�Z���f�[�^�̃f�[�^�����v�Z����B
        * @return �f�[�^���B�������s���ȏꍇ�A0��Ԃ��B
        * @param pixelFormat �s�N�Z���t�H�[�}�b�g�B
        * @param textureSize �e�N�X�`���T�C�Y�B
        */
        static unsigned long calculatePixelDataSize(
            PixelFormat pixelFormat
            , const Size2D& textureSize
            );

        /**
        * @brief �s�N�Z���f�[�^�̃f�[�^�����v�Z����B
        * @return �f�[�^���B�������s���ȏꍇ�A0��Ԃ��B
        * @param pixelFormat �s�N�Z���t�H�[�}�b�g�B
        * @param textureSize �e�N�X�`���T�C�Y�B
        * @param mipMapMaxLevel �~�b�v�}�b�v�̍ő僌�x���B
        */
        static unsigned long calculateMipMapPixelDataSize(
            PixelFormat pixelFormat
            , const Size2D& textureSize
            , unsigned char mipMapMaxLevel
            );

        /// �s�N�Z���t�H�[�}�b�g�𕶎���ɕϊ�����B
        static const char* toString( PixelFormat piexlFormat );

        /// PixelFormat�Ɋւ��郆�j�b�g�e�X�g�B
        static void unitTest();
    };

}
//----------------------------------------------------------------
#endif
// EOF
