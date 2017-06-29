/**
 * @file
 * @brief 変換のレシピクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <textga/PixelFormat.hpp>
#include "CommandKind.hpp"

//------------------------------------------------------------
namespace local {

    class ArgumentDataIterator;

    /// 変換のレシピクラス。
    class ConvertRecipe
    {
    public:
        /// レシピを初期化する。成功したらtrueを返す。　
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
