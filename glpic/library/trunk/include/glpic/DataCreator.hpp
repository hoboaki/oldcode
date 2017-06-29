/**
 * @file
 * @brief GLPic�f�[�^�����֐����L�q����B
 */
#if defined(GLPIC_INCLUDED_DATACREATOR_HPP)
#else
#define GLPIC_INCLUDED_DATACREATOR_HPP

//----------------------------------------------------------------
// include
#include <glpic/BinaryData.hpp>
#include <glpic/PixelFormat.hpp>
#include <glpic/Noncreatable.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// GLPic�f�[�^�����֐��Q�B
    class DataCreator : public Noncreatable
    {
    public:
        /// �G���[���B
        enum ErrorKind
        {
            ErrorKind_None ///< �G���[�����B
            ,ErrorKind_NotEnoughDataSize ///< �n���ꂽ�f�[�^�T�C�Y������Ȃ��B
            ,ErrorKind_InvalidArgument ///< ���V�s����уI�t�Z�b�g�l���s���B
            ,ErrorKind_InvalidPixelDataSize ///< �s�N�Z���f�[�^�̃f�[�^�T�C�Y���s���B
        };

        /// GLPic�f�[�^�����ۂ̃��V�s�\���́B
        struct Recipe
        {
            PixelFormat     pixelFormat; ///< �s�N�Z���t�H�[�}�b�g�B
            bool            imageScaled; ///< �C���[�W�͈������΂���Ă��邩�B
            unsigned char   mipMapMaxLevel; ///< MipMap�̍ő僌�x���BMipMap���g��Ȃ��Ƃ���0�B
            Size2D          textureSize; ///< �e�N�X�`���T�C�Y�B
            Size2D          imageSize; ///< �C���[�W�T�C�Y�B

            //----------------------------------------------------------------
            /// GLPic�f�[�^�̃��V�s���쐬�B
            static Recipe create(
                PixelFormat pixelFormat
                , bool imageScaled
                , unsigned char mipMapMaxLevel
                , const Size2D& textureSize
                , const Size2D& imageSize
                );

            /// ���V�s���L�����B
            bool isValid()const;
        };

        /// �쐬���ʁB
        struct Result
        {
            ErrorKind errorKind; ///< �G���[���B
            unsigned long dataSize; ///< �G���[���Ȃ���΁A�쐬���ꂽ�f�[�^�̃T�C�Y�B

            //----------------------------------------------------------------
            /// �쐬�֐��B
            static Result create( ErrorKind errorKind , unsigned long dataSize );
        };

        /**
        * @brief �K�v�ȃf�[�^�T�C�Y���v�Z����B
        * @return �f�[�^�T�C�Y�B�������s���ȏꍇ�A0��Ԃ��B
        * @param recipe ���V�s�B
        */
        static unsigned long calculateDataSize(
            const Recipe& recipe
            );

        /**
        * @brief �K�v�ȃf�[�^�T�C�Y���v�Z����B
        * @return �f�[�^�T�C�Y�B�������s���ȏꍇ�A0��Ԃ��B
        * @param recipe ���V�s�B
        * @param pixelDataOffset �s�N�Z���f�[�^�̃I�t�Z�b�g�ʒu�B
        * @param userDataOffset ���[�U�[�f�[�^�̃I�t�Z�b�g�ʒu�B
        * @param userDataSize ���[�U�[�f�[�^�̃T�C�Y�B
        */
        static unsigned long calculateDataSizeCustum(
            const Recipe& recipe
            , const unsigned long pixelDataOffset
            , const unsigned long userDataOffset
            , const unsigned long userDataSize
            );

        /**
        * @brief �V���v���ȃf�[�^���쐬����B
        * @return �G���[���A�f�[�^�̃T�C�Y�ȂǁB
        * @param recipe ���V�s�B
        * @param pixelDataArray �s�N�Z���f�[�^�̔z��B�z��̃T�C�Y��mipMapMaxLevel+1�ł��邱�ƁB
        * @param allocedData �쐬�����f�[�^���i�[����B�m�ۍς݂̃������������Ă������ƁB
        */
        static Result createData(
            const Recipe& recipe
            , const BinaryData* const pixelData
            , const BinaryData& allocedData
            );

        /**
        * @brief �I�v�V������S�Ďw�肵�ăf�[�^���쐬����B
        * @return �G���[���A�f�[�^�̃T�C�Y�ȂǁB
        * @param recipe ���V�s�B
        * @param pixelDataOffset �s�N�Z���f�[�^�̊J�n�ʒu�B�f�[�^�̐擪����̃o�C�g�T�C�Y�B
        * @param pixelDataArray �s�N�Z���f�[�^�̔z��B�z��̃T�C�Y��mipMapMaxLevel+1�ł��邱�ƁB
        * @param userDataOffset ���[�U�[�f�[�^�̊J�n�ʒu�B�f�[�^�̐擪����̃o�C�g�T�C�Y�B�g��Ȃ��ꍇ��0�B
        * @param userDataPointer ���[�U�[�f�[�^�ւ̃|�C���^�B����Ȃ��ꍇ��0�B
        * @param allocedData �쐬�����f�[�^���i�[����B�m�ۍς݂̃������������Ă������ƁB
        */
        static Result createDataCustum(
            const Recipe& recipe
            , const unsigned long pixelDataOffset
            , const BinaryData* const pixelDataArray
            , const unsigned long userDataOffset
            , const BinaryData* const userDataPointer
            , const BinaryData& allocedData
            );

    };

}
//----------------------------------------------------------------
#endif
//----------------------------------------------------------------
// EOF
