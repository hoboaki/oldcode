/**
 * @file 
 * @brief GLPIC�̃w�b�_�ƃf�[�^�A�N�Z�X�֐����L�q����B
 */
#if defined(GLPIC_INCLUDED_GLPICHEADER_HPP)
#else
#define GLPIC_INCLUDED_GLPICHEADER_HPP

//----------------------------------------------------------------
// include
#include <glpic/PixelFormat.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// GLPic�̃w�b�_�\���́B
    struct GLPicHeader
    {
    // public
        
        /**
        * @brief �s�N�Z���t�H�[�}�b�g���擾����B
        * @return �s�N�Z���t�H�[�}�b�g�̎�ށB
        * �f�[�^���s���ȂƂ��̖߂�l�͕s��B
        */
        PixelFormat getPixelFormat()const;

        /**
        * @brief �C���[�W���������΂���Ă��邩�擾����B
        * @return �������΂���Ă���悤�ł����true�B
        * ���̊֐���true��Ԃ��ꍇ�A�C���[�W��
        * �e�N�X�`���S�̂Ɉ������΂���Ă���B
        */
        bool getImageScaled()const;

        /**
        * @brief �~�b�v�}�b�v�̍ő僌�x�������擾����B
        * @return �g�p���Ă��Ȃ��ꍇ��0�B
        * �f�[�^���s���ȂƂ��̖߂�l�͕s��B
        */
        unsigned char getMipMapMaxLevel()const;

        /**
        * @brief �e�N�X�`���̃s�N�Z�������擾����B
        * @return �s�N�Z�����B
        * �f�[�^���s���ȂƂ��̖߂�l�͕s��B
        */
        Size2D getTextureSize2D()const;

        /**
        * @brief ���̃C���[�W�̃s�N�Z�������擾����B
        * @return �s�N�Z�����B
        * �f�[�^���s���ȂƂ��̖߂�l�͕s��B
        */
        Size2D getImageSize2D()const;

        /**
        * @brief �s�N�Z���f�[�^�̃f�[�^�����擾����B
        * @return �f�[�^���B
        * �f�[�^���s���ȂƂ��̖߂�l�͕s��B
        */
        unsigned long getPixelDataSize()const;

        /**
        * @brief ���[�U�[�f�[�^�̃f�[�^�����擾����B
        * @return �f�[�^���B
        * �f�[�^���s���ȂƂ��̖߂�l�͕s��B
        */
        unsigned long getUserDataSize()const;

        /**
        * @brief �s�N�Z���f�[�^���擾����B
        * @return �s�N�Z���f�[�^�ւ̃|�C���^�B
        * �f�[�^���s���ȂƂ��A�߂�l�͕s��B
        */
        void* getPixelData();

        /**
        * @brief �s�N�Z���f�[�^���擾����B
        * @return �s�N�Z���f�[�^�ւ̃|�C���^�B
        * �f�[�^���s���ȂƂ��A�߂�l�͕s��B
        */
        const void* getPixelData()const;

        /**
        * @brief ���[�U�[�f�[�^���擾����B
        * @return ���[�U�[�f�[�^�ւ̃|�C���^�B
        * �f�[�^���s���ȂƂ��A���[�U�[�f�[�^����0�̂Ƃ��A�߂�l�͕s��B
        */
        void* getUserData();

        /**
        * @brief ���[�U�[�f�[�^���擾����B
        * @return ���[�U�[�f�[�^�ւ̃|�C���^�B
        * �f�[�^���s���ȂƂ��A���[�U�[�f�[�^����0�̂Ƃ��A�߂�l�͕s��B
        */
        const void* getUserData()const;

        /**
        * @brief �f�[�^�����������B
        * �V�O�l�`�����s���ȂƂ��A
        * �o�C�g�I�[�_�[���t�]���Ă���Ƃ��Ȃǂ�GLPicBool_False��Ԃ��B
        * ���̊֐����g���ăf�[�^�����������Ƃ��m�F���Ă���
        * Get*�֐����g�p���邱�Ƃ𐄏�����B
        */
        bool isValid()const;

        /**
        * @brief �o�C�g�I�[�_�[���t�]���Ă��邩�B
        * �o�C�g�I�[�_�[���t�]���Ă��邱�Ƃ������Œ�l�������GXPicBool_True��Ԃ��B
        * �f�[�^�����Ă�����A�o�C�g�I�[�_�[���������ꍇ��GXPicBool_False��Ԃ��B
        */
        bool isByteOrderInversed()const;

        /**
        * @brief �o�C�g�I�[�_�[�𔽓]����B
        * �o�C�g�I�[�_�[���������Ă����]����̂Œ��ӁB
        */
        void inverseByteOrder();

    // private
        //----------------------------------------------------------------
        // �����o�B���ڃA�N�Z�X�����A�A�N�Z�T���g�p���邱�ƁB
        // 0
        char             signature[3];     ///< �V�O�l�`���B�Œ蕶���񂪓���B
        unsigned char    version;          ///< �t�@�C���t�H�[�}�b�g�̃o�[�W�������B
        unsigned short   endianCheck;      ///< �G���f�B�A���`�F�b�N�B�Œ�l������B
        unsigned char    pixelFormat;      ///< �s�N�Z���t�H�[�}�b�g�B
        unsigned char    flagAndMipMapLevel; ///< �ŏ�ʃr�b�g:�C���[�W�͈������΂���Ă��邩�B����ȊO�F�~�b�v�}�b�v�̍ő僌�x���B
        // 8
        unsigned short   textureWidth;     ///< �e�N�X�`���̉����B
        unsigned short   textureHeight;    ///< �e�N�X�`���̏c���B
        unsigned short   imageWidth;       ///< ���̃C���[�W�̉����B
        unsigned short   imageHeight;      ///< ���̃C���[�W�̏c���B
        // 16
        unsigned long    pixelDataOffset;  ///< �w�b�_�̐擪����pixelData�ւ̃I�t�Z�b�g�l
        unsigned long    pixelDataSize;    ///< �s�N�Z���̃f�[�^�T�C�Y�B
        // 24
        unsigned long    userDataOffset;   ///< �w�b�_�̐擪����userData�ւ̃I�t�Z�b�g�l�B
        unsigned long    userDataSize;     ///< ���[�U�[�f�[�^�̃T�C�Y�B

        //----------------------------------------------------------------        
        /// �w�b�_�̐擪�ɂ���V�O�l�`��������B
        static const char* const SIGNATURE;

        /// �t�@�C���t�H�[�}�b�g�o�[�W�����B
        static const unsigned char VERSION;

        /// ����ȃo�C�g�I�[�_�[�������l�B
        static const unsigned short ENDIAN_CHECK_VALUE;

        /// �t�]���Ă���o�C�g�I�[�_�[�������l�B
        static const unsigned short INVERSED_ENDIAN_CHECK_VALUE;

    };

}
//----------------------------------------------------------------
#endif
// EOF
