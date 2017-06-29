/**
 * @file
 * @brief TGA�f�[�^�̃A�N�Z�T�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>
#include <textga/PixelFormat.hpp>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

    struct Pixel;
    struct TGAHeader;
    
    /// TGA�f�[�^�̃A�N�Z�T�N���X�B
    class TGADataAccessor
    {
    public:
        /**
         * @brief TGA�f�[�^�̃A�h���X�ƃT�C�Y��n���B
         *
         * ���̃N���X�����ʂ܂Ń|�C���^�������f�[�^�������Ă͂����Ȃ��B
         */
        TGADataAccessor( const byte* data , size_t dataSize );
    
        /// �ϊ����T�|�[�g�ł���TGA�t�@�C�����B
        bool isSupportedTGA()const;
        
        //------------------------------------------------------------
        /**
         * @name TGA�̏��擾�B
         *
         * isSupportedTGA() == true �̂Ƃ��̂݌Ăׂ�B
         */
        //@{
        /// �������擾�B
        u16 width()const;
        /// �c�����擾�B
        u16 height()const;
        /// bps���擾�B
        u8  bitsPerPixel()const;
        /// �s�N�Z���̑������擾����B
        size_t pixelCount()const;
        /// �s�N�Z���f�[�^�̃o�C�g�����擾����B
        size_t pixelDataSize()const;
        /**
         * @brief �w���index�̃s�N�Z�����擾����B
         * @param index 0 <= val < pixelCount()
         */
        Pixel pixelAtIndex( size_t index )const;
        /// TexTarga�`�����B
        bool isTexTarga()const;
        //@}
        
        //------------------------------------------------------------
        /**
         * @name TexTarga�̏��擾�B
         *
         * isTexTarga() == true �̂Ƃ��̂݌Ăׂ�B
         */
        //@{
        /// �s�N�Z���t�H�[�}�b�g���擾����B
        PixelFormat pixelFormat()const;
        /// SrcData�����݂��邩�B
        bool isExistSrcData()const;
        /**
         * @brief SrcData�̃s�N�Z�����擾����B
         * @param index 0 <= val < pixelCount()
         */
        Pixel srcDataPixelAtIndex( size_t index )const;
        /// SrcData�̃A�h���X���擾����B
        const byte* srcData()const;
        //@}
        
    private:
        const byte* const data_;
        const size_t dataSize_;
        
        bool isSupportedTGA_;
        u8 bitsPerPixel_;
        size_t pixelCount_;
        
        bool isTexTarga_;
        PixelFormat ttPixelFormat_;
        const byte* ttSrcData_; // SrcData�̐擪�A�h���X�B���݂��Ȃ����NULL�B
        
        //------------------------------------------------------------
        const TGAHeader& header()const;
        Pixel getPixel( const byte* data , size_t index )const;
    };
    
}
//------------------------------------------------------------
// EOF
