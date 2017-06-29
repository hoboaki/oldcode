/** 
 * @file
 * @brief EntryPoint.hppの実装を記述する。
 */
#include "app/sysg3d_conv/EntryPoint.hpp"

//------------------------------------------------------------
#include <memory>
#include "app/Argument.hpp"
#include "app/sysg3d_conv/Constant.hpp"
#include "app/sysg3d_conv/ConvertRecipe.hpp"
#include "app/sysg3d_conv/MeshConv.hpp"

//------------------------------------------------------------
namespace app {
namespace sysg3d_conv {
//------------------------------------------------------------
int EntryPoint::run( const Argument& aArg )
{
    // バナー
    PJ_COUT( "[SysG3d Converter] Program Version '%u.%u' Format Version '%u.%u'\n" 
        , Constant::MAJOR_VERSION
        , Constant::MINOR_VERSION
        , ::sysg3d::BinConstant::VERSION_MAJOR
        , ::sysg3d::BinConstant::VERSION_MINOR
        );

    if ( !ConvertRecipe::isValidArgument( aArg ) )
    {// 無効な引数
        ConvertRecipe::printUseage( aArg );
        return -1;
    }
    const ConvertRecipe convertRecipe( aArg );

    // Open
    std::auto_ptr< DAE > dae( new DAE( ) );
    if ( dae->load( convertRecipe.daeFilePath() ) != DAE_OK )
    {
        PJ_COUT( "Error: file load error '%s'\n" , convertRecipe.daeFilePath() );
        return -1;
    }
    PJ_COUT( " Input     : %s\n" , convertRecipe.daeFilePath() );
    PJ_COUT( " OutputDir : %s\n" , convertRecipe.outputDir() );

    // ジオメトリ
    if ( MeshConv::run( convertRecipe ,  *dae ) != 0 )
    {
        PJ_COUT( "Error: mesh conv.\n" );
        return -1;
    }

    return 0;
}

//------------------------------------------------------------
}}
//------------------------------------------------------------
// EOF
