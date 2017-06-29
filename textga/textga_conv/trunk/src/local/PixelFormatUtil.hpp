/**
 * @file
 * @brief PixelFormat�Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <local/PixelFormat.hpp>

//------------------------------------------------------------
namespace local {

    struct Pixel;
    
    /// PixelFormat�Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class PixelFormatUtil
    {
    public:
        /// PixelFormat���������O�̕�����ɕϊ�����B
        static const char* toName( PixelFormat );
        
        /// �L����PixelFormat���B
        static bool isValid( PixelFormat );
        
        /// �w��̃t�H�[�}�b�g�̒l�ɕϊ�����B
        static Pixel convert( const Pixel& , PixelFormat );
        
        /// �A���t�@�`�����l�������邩�B
        static bool hasAlpha( PixelFormat );
        
        /// DXTC���k�t�H�[�}�b�g���B
        static bool isDXTC( PixelFormat );
        
        /// squish�̈��k�t���O���擾����B
        static int dxtcCompressFlag( PixelFormat );
        
        /// �R�[�h�e�X�g�B
        static void codeTest();
    };

}
//------------------------------------------------------------
// EOF
