/**
 * @file
 * @brief PixelFormat�Ɋւ��郆�[�e�B���e�B�֐����L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <textga/Pixel.hpp>
#include <textga/PixelFormat.hpp>

//------------------------------------------------------------
namespace textga {

    /// PixelFormat�Ɋւ��郆�[�e�B���e�B�֐��Q�B
    class PixelFormatUtil
    {
    public:
        /// PixelFormat���������O�̕�����ɕϊ�����B
        static const char* toName( ::textga::PixelFormat );
        
        /// �����񂩂�PixelFormat�ɕϊ�����B
        static ::textga::PixelFormat fromName( const char* );
        
        /// �L����PixelFormat���B
        static bool isValid( ::textga::PixelFormat );
        
        /// �w��̃t�H�[�}�b�g�̒l�ɕϊ�����B
        static ::textga::Pixel convert( const ::textga::Pixel& , ::textga::PixelFormat );
        
        /// �A���t�@�`�����l�������邩�B
        static bool hasAlpha( ::textga::PixelFormat );
        
        /// DXTC���k�t�H�[�}�b�g���B
        static bool isDXTC( ::textga::PixelFormat );
        
        /// �R�[�h�e�X�g�B
        static void codeTest();
    };

}
//------------------------------------------------------------
// EOF
