/**
 * @file
 * @brief �ϊ��̃��V�s�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <textga/PixelFormat.hpp>
#include "CommandKind.hpp"

//------------------------------------------------------------
namespace local {

    class ArgumentDataIterator;

    /// �ϊ��̃��V�s�N���X�B
    class ConvertRecipe
    {
    public:
        /// ���V�s������������B����������true��Ԃ��B�@
        bool initialize( const ArgumentDataIterator& );
        
        bool appendSrcData;
        bool forceMode;
        bool isPixelFormatValid;
        textga::PixelFormat pixelFormat;
        std::string inputFilePath;
        std::string outputFilePath;
    };

}
//------------------------------------------------------------
// EOF
