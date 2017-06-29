/**
 * @file
 * @brief Useage.hppÇÃé¿ëïÇãLèqÇ∑ÇÈÅB
 */
#include "Useage.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <local/ArgumentData.hpp>
#include <local/PixelFormatUtil.hpp>
#include <string>

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
void Useage::print( const ArgumentData& aArgument )
{
    const char* toolName = aArgument.count != 0
        ? aArgument.texts[0]
        : "textga_conv";
    
    std::printf( "useage: %s inputTGAFile [-rs] [-f format] [-o output]\n" , toolName );
    std::printf( "  rs remove src : not append src data\n" );
    std::printf( "  o output : output file path\n" );
    std::printf( "  f format : convert pixel format name\n" );
    std::printf( "    <pixel format name list>\n" );
    
    const char* preStr = "     ";
    std::string fmtStr;
    const std::string::size_type limitSize = 30;
    for ( int i = PixelFormat_Begin; i < PixelFormat_End; ++i )
    {
        if ( fmtStr.length() == 0 )
        {
            fmtStr += preStr;
        }
        fmtStr += PixelFormatUtil::toName( static_cast<PixelFormat>(i) );
        fmtStr += " ";
        if ( limitSize <= fmtStr.length() )
        {
            std::printf( "%s\n" , fmtStr.c_str() );
            fmtStr = "";
        }
    }
    if ( fmtStr.length() != 0 )
    {
        std::printf( "%s\n" , fmtStr.c_str() );
    }
    
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
