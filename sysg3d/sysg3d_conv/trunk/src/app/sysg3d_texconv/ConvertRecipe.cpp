/** 
 * @file
 * @brief ConvertRecipe.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "app/sysg3d_texconv/ConvertRecipe.hpp"

//------------------------------------------------------------
namespace app {
namespace sysg3d_texconv {
//------------------------------------------------------------
bool ConvertRecipe::isValidArgument( const Argument& aArg )
{
    return  aArg.count() == 3;
}

//------------------------------------------------------------
void ConvertRecipe::printUseage( const Argument& aArg )
{
    PJ_COUT( "Useage : %s tgaFilePath outputFilePath\n"
        , aArg.argAtIndex(0)
        );
}

//------------------------------------------------------------
ConvertRecipe::ConvertRecipe( const Argument& aArgRef )
: argument_( aArgRef )
{
}

//------------------------------------------------------------
ConstStr ConvertRecipe::tgaFilePath()const
{
    return argument_.argAtIndex(1);
}

//------------------------------------------------------------
ConstStr ConvertRecipe::outputFilePath()const
{
    return argument_.argAtIndex(2);
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
