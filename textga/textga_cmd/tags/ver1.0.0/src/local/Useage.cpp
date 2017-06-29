/**
 * @file
 * @brief Useage.hpp‚ÌÀ‘•‚ğ‹Lq‚·‚éB
 */
#include "Useage.hpp"

//------------------------------------------------------------
#include <cstdio>
#include <string>
#include <textga/PixelFormatUtil.hpp>
#include "ArgumentData.hpp"
#include "Assert.hpp"
#include "CommandKindUtil.hpp"
#include "Version.hpp"

//------------------------------------------------------------
using namespace ::textga;

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
void Useage::print( const ArgumentData& aArgument )
{
    PJ_ASSERT( 2 <= aArgument.count );
    const char* toolName = aArgument.texts[0];
    const char* subCmdName = 3 <= aArgument.count 
        ? aArgument.texts[2]
        : 0;
    
    const CommandKind subCmdKind = subCmdName != 0
        ? CommandKindUtil::getCommand( subCmdName )
        : CommandKind_Invalid;
    const char* preStr = "     ";
        
    switch( subCmdKind )
    {
    case CommandKind_Convert:
        {
            std::printf( "convert (cv conv): Convert standard targa file to tex-targa file.\n" );
            std::printf( "useage: %s conv inputTGAFile outputTGAFile [-rs] [-f] [-p pixel-format]\n" , toolName );
            std::printf( "  -rs remove src : not append src data\n" );
            std::printf( "  -f force to convert : convert as force mode.\n" );
            std::printf( "      Ignore restriction of pixel-format as much as possible.\n" );
            std::printf( "  -p pixel-format : convert pixel-format name\n" );
            std::printf( "\n");
            std::printf( "Available pixel-formats:\n" );
            
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
        break;
        
    case CommandKind_Clear:
        std::printf( "clear (cl): Create removed extra info targa file.\n" );
        std::printf( "useage: %s remove inputFilePath outputFilePath\n" , toolName );
        break;
        
    case CommandKind_Information:
        std::printf( "information (if info): Display targa file information.\n" );
        std::printf( "useage: %s info filepath...\n" , toolName );
        break;
        
    case CommandKind_Merge:
        std::printf( "merge (mg): Merge alpha channel.\n" );
        std::printf( "useage: %s merge inputTgaFilePath mergeTgaFilePath outputFilePath [-c channel]\n" , toolName );
        std::printf( "  -c : alpha channel element name.\n" );
        std::printf( "Warning: Extra information will be removed after merging.\n" );
        std::printf( "Available channel.\n" );
        std::printf( "    ave : average of r,g,b(default)\n" );
        std::printf( "    r : red channel.\n" );
        std::printf( "    g : green channel.\n" );
        std::printf( "    b : blue channel.\n" );
        std::printf( "    a : alpha channel.\n" );
        break;
            
    case CommandKind_Revert:
        std::printf( "revert (rv): Create targa file with tex-targa src data\n" );
        std::printf( "useage: %s revert inputFilePath outputFilePath\n" , toolName );
        break;
    
    case CommandKind_Help:
    default:
        std::printf( "TexTarga command line tool.\n" );
        std::printf( "Version %s.\n" , ::local::Version::asString().c_str() );
        std::printf( "\n" );
        std::printf( "help (-h --help ? ): Display help.\n" );
        std::printf( "useage: %s help [SubCommand]\n" , toolName ); 
        std::printf( "\n");
        std::printf( "Avalilable subcommands:\n" );
        std::printf( "%sclear\n"  , preStr );
        std::printf( "%sconvert\n" , preStr );
        std::printf( "%shelp\n" , preStr );
        std::printf( "%sinfo\n"  , preStr );
        std::printf( "%smerge\n"  , preStr );
        std::printf( "%srevert\n"  , preStr );
        break;
    }
            
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
