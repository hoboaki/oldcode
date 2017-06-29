/** 
 * @file
 * @brief ConvertRecipe.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "app/sysg3d_conv/ConvertRecipe.hpp"

//------------------------------------------------------------
namespace app {
namespace sysg3d_conv {
//------------------------------------------------------------
bool ConvertRecipe::isValidArgument( const Argument& aArg )
{
    return  aArg.count() == 3;
}

//------------------------------------------------------------
void ConvertRecipe::printUseage( const Argument& aArg )
{
    PJ_COUT( "Useage : %s daeFilePath outputDirPath\n"
        , aArg.argAtIndex(0)
        );
}

//------------------------------------------------------------
ConvertRecipe::ConvertRecipe( const Argument& aArgRef )
: argument_( aArgRef )
{
}

//------------------------------------------------------------
ConstStr ConvertRecipe::daeFilePath()const
{
    return argument_.argAtIndex(1);
}

//------------------------------------------------------------
ConstStr ConvertRecipe::outputDir()const
{
    return argument_.argAtIndex(2);
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
