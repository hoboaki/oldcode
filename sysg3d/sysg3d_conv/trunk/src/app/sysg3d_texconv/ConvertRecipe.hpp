/**
 * @file
 * @brief ConvertRecipe�^���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include "app/Argument.hpp"

//------------------------------------------------------------
namespace app {
namespace sysg3d_texconv {

    class ConvertRecipe
    {
    public:
        static bool isValidArgument( const Argument& ); ///< �L���Ȉ������B
        static void printUseage( const Argument& ); ///< Useage�̕\���B

        ConvertRecipe( const Argument& argRef );

        ConstStr tgaFilePath()const; ///< TGA�t�@�C���̃p�X�B
        ConstStr outputFilePath()const; ///< �o�̓t�@�C���̃p�X�B

    private:
        const Argument& argument_;
    };

}}
//------------------------------------------------------------
// EOF
