/**
 * @file
 * @brief EntryPoint.hpp�̎������L�q����B
 */
#include "EntryPoint.hpp"
 
//------------------------------------------------------------
#include "ArgumentData.hpp"
#include "CommandKindUtil.hpp"
#include "Convert.hpp"
#include "Useage.hpp"

//------------------------------------------------------------
namespace
{
    enum ErrorCode
    {
        ErrorCode_ErrorNone
        , ErrorCode_Other
    };
}

//------------------------------------------------------------
namespace local {
//------------------------------------------------------------
int EntryPoint::run( const ArgumentData& aArgument )
{
    // 1. �R�}���h�̂̉���
    const CommandKind commandKind = CommandKindUtil::getCommandWithArgumentData( aArgument );
    
    // 2. �R�}���h�̎��s
    switch ( commandKind )
    {
    case CommandKind_PrintUseage:
        Useage::print( aArgument );
        return ErrorCode_ErrorNone;
        
    case CommandKind_Convert:
        if ( !Convert::execute( aArgument ) )
        {
            return ErrorCode_Other;
        }
        return ErrorCode_ErrorNone;
        
    case CommandKind_Invalid:
    default:
        Useage::print( aArgument );
        return ErrorCode_Other;
    }
    
    // Finish
    return ErrorCode_ErrorNone;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
