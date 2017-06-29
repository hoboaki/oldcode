/**
 * @file
 * @brief �ϊ��̃��V�s�N���X���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include "CommandKind.hpp"
#include "PixelFormat.hpp"

//------------------------------------------------------------
namespace local {

    struct ArgumentData;

    /// �ϊ��̃��V�s�N���X�B
    class ConvertRecipe
    {
    public:
        /// ���V�s������������B����������true��Ԃ��B�@
        bool initialize( const ArgumentData& );
        
        bool appendSrcData;
        bool isPixelFormatValid;
        PixelFormat pixelFormat;
        std::string inputFilePath;
        std::string outputFilePath;
    };

}
//------------------------------------------------------------
// EOF
