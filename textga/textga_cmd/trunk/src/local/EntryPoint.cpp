/**
 * @file
 * @brief EntryPoint.hppの実装を記述する。
 */
#include "EntryPoint.hpp"
 
//------------------------------------------------------------
#include <cstdio>
#include "ArgumentData.hpp"
#include "ArgumentDataIterator.hpp"
#include "Clear.hpp"
#include "CommandKindUtil.hpp"
#include "Convert.hpp"
#include "Information.hpp"
#include "Merge.hpp"
#include "Revert.hpp"
#include "Useage.hpp"

//------------------------------------------------------------
using namespace ::textga;

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
    // 1. コマンドのの解釈
    const CommandKind commandKind = CommandKindUtil::getCommandWithArgumentData( aArgument );
    
    // 2. コマンドの実行
    // イテレータの準備
    ArgumentDataIterator argItr( aArgument );
    argItr.next();
    if ( argItr.hasNext() )
    {
        argItr.next();
    }
    // コマンドごとの処理
    switch ( commandKind )
    {
    case CommandKind_Help:
        Useage::print( aArgument );
        return ErrorCode_ErrorNone;
        
    case CommandKind_Clear:
        if ( !Clear::execute( argItr ) )
        {
            return ErrorCode_Other;
        }
        return ErrorCode_ErrorNone;
        
    case CommandKind_Convert:
        if ( !Convert::execute( argItr ) )
        {
            return ErrorCode_Other;
        }
        return ErrorCode_ErrorNone;
        
    case CommandKind_Information:
        if ( !Information::execute( argItr ) )
        {
            return ErrorCode_Other;
        }
        return ErrorCode_ErrorNone;
        
    case CommandKind_Merge:
        if ( !Merge::execute( argItr ) )
        {
            return ErrorCode_Other;
        }
        return ErrorCode_ErrorNone;
        
    case CommandKind_Revert:
        if ( !Revert::execute( argItr ) )
        {
            return ErrorCode_Other;
        }
        return ErrorCode_ErrorNone;
        
    case CommandKind_Invalid:
    default:
        std::fprintf( stderr, "Type '%s help' for useage.\n" , aArgument.getTextAtIndex(0) );
        return ErrorCode_Other;
    }
    
    // Finish
    return ErrorCode_ErrorNone;
}

//------------------------------------------------------------
}
//------------------------------------------------------------
// EOF
