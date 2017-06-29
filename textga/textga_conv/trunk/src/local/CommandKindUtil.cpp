/**
 * @file
 * @brief CommandKintUtil.hppの実装を記述する。
 */
#include "CommandKindUtil.hpp"

//------------------------------------------------------------
#include "ArgumentData.hpp"
#include "StringUtil.hpp"

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
    
    if ( StringUtil::equals( aArgData.getTextAtIndex( 1 ) , "--help" ) 
        || StringUtil::equals( aArgData.getTextAtIndex( 1 ) , "-h" ) 
        ||  StringUtil::equals( aArgData.getTextAtIndex( 1 ) , "help" ) 
        )
    {// Help
        return CommandKind_PrintUseage;
    }
    
    return CommandKind_Convert;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
