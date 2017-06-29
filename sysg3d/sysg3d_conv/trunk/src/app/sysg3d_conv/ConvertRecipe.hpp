/**
 * @file
 * @brief ConvertRecipe�^���L�q����B
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
        static bool isValidArgument( const Argument& ); ///< �L���Ȉ������B
        static void printUseage( const Argument& ); ///< Useage�̕\���B

        ConvertRecipe( const Argument& argRef );

        ConstStr daeFilePath()const; ///< Dae�t�@�C���̃p�X�B
        ConstStr outputDir()const; ///< �o�̓f�B���N�g���B

    private:
        const Argument& argument_;
    };

}}
//------------------------------------------------------------
// EOF
