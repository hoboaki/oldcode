/**
 * @file
 * @brief CommandKintUtil.hppの実装を記述する。
 */
#include "CommandKindUtil.hpp"

//------------------------------------------------------------
#include <textga/StringUtil.hpp>
#include "ArgumentData.hpp"

//------------------------------------------------------------
using namespace ::textga;

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
CommandKind CommandKindUtil::getCommandWithArgumentData(
    const ArgumentData& aArgData
    )
{
    if ( aArgData.count <= 1 )
    {// ファイルパスしかない。
        return CommandKind_Invalid;
    }
    return getCommand( aArgData.getTextAtIndex(1) );
}

//------------------------------------------------------------
CommandKind CommandKindUtil::getCommand( const char* aCmd )
{
    const char* arg = aCmd;
    if ( StringUtil::equals( arg , "cl" )
        || StringUtil::equals( arg , "clear" )
        )
    {// Clear
        return CommandKind_Clear;
    }
    else if ( StringUtil::equals( arg , "cv" )
        || StringUtil::equals( arg , "conv" )
        || StringUtil::equals( arg , "convert" )
        )
    {// Convert
        return CommandKind_Convert;
    }
    else if ( StringUtil::equals( arg , "--help" ) 
        ||  StringUtil::equals( arg , "help" ) 
        || StringUtil::equals( arg , "-h" ) 
        || StringUtil::equals( arg , "?" ) 
        )
    {// Help
        return CommandKind_Help;
    }
    else if ( StringUtil::equals( arg , "if" )
        || StringUtil::equals( arg , "info" )
        || StringUtil::equals( arg , "information" )
        )
    {// Information
        return CommandKind_Information;
    }
    else if ( StringUtil::equals( arg , "mg" )
        || StringUtil::equals( arg , "merge" )
        )
    {// Merge
        return CommandKind_Merge;
    }
    else if ( StringUtil::equals( arg , "rv" )
        || StringUtil::equals( arg , "revert" )
        )
    {// Revert
        return CommandKind_Revert;
    }
    
    return CommandKind_Invalid;
}

//------------------------------------------------------------
void CommandKindUtil::printCommand( const char* aName )
{
    std::printf( "[TexTarga Command Line Tool <%s>]\n" , aName );
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
