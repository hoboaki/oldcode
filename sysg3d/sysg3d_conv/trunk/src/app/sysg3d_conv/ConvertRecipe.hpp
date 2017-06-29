/**
 * @file
 * @brief ConvertRecipe型を記述する。
 */
#pragma once

//------------------------------------------------------------
#include "app/Argument.hpp"

//------------------------------------------------------------
namespace app {
namespace sysg3d_conv {

    class ConvertRecipe
    {
    public:
        static bool isValidArgument( const Argument& ); ///< 有効な引数か。
        static void printUseage( const Argument& ); ///< Useageの表示。

        ConvertRecipe( const Argument& argRef );

        ConstStr daeFilePath()const; ///< Daeファイルのパス。
        ConstStr outputDir()const; ///< 出力ディレクトリ。

    private:
        const Argument& argument_;
    };

}}
//------------------------------------------------------------
// EOF
