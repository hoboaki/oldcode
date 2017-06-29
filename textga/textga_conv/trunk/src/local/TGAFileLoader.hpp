/**
 * @file
 * @brief TGA�t�@�C�������[�h����N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>

//------------------------------------------------------------
namespace local {

    struct Pixel;
    struct TGAHeader;
    
    /// TGA�t�@�C�������[�h����N���X���L�q����B
    class TGAFileLoader
    {
    public:
        TGAFileLoader( const std::string& filePath );
    
        /// �J�������B
        bool isOpened()const;
        /// �ϊ����T�|�[�g�ł���TGA�t�@�C�����B
        bool isSupportedFile()const;
        
        /// �������擾�B
        short width()const;
        /// �c�����擾�B
        short height()const;
        /// bps���擾�B
        char  bitsPerPixel()const;
        /// �s�N�Z���̑������擾����B
        size_t pixelCount()const;
        /// �w���index�̃s�N�Z�����擾����B
        Pixel pixelAtIndex( size_t index )const;
        
    private:
        bool isOpened_;
        char bitsPerPixel_;
        size_t pixelCount_;
        std::vector< unsigned char > bytes_;
        
        //------------------------------------------------------------
        const TGAHeader& header()const;
    };
    
}
//------------------------------------------------------------
// EOF
