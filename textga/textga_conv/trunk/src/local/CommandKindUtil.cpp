/**
 * @file
 * @brief CommandKintUtil.hpp�̎������L�q����B
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
    {// �t�@�C���p�X�����Ȃ��B
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
