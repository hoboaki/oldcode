/**
 * @file
 * @brief 変換のレシピクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include "CommandKind.hpp"
#include "PixelFormat.hpp"

//------------------------------------------------------------
namespace local {

    struct ArgumentData;

    /// 変換のレシピクラス。
    class ConvertRecipe
    {
    public:
        /// レシピを初期化する。成功したらtrueを返す。　
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
