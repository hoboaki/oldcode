/**
 * @file
 * @brief CommandKindに関するユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include "CommandKind.hpp"

//------------------------------------------------------------
namespace local {

    struct ArgumentData;

    /// CommandKindに関するユーティリティ関数。
    class CommandKindUtil
    {
    public:
        /// 引数からコマンドを取得する。
        static CommandKind getCommandWithArgumentData( const ArgumentData& );
        /// コマンドを示す文字列からコマンドを取得する。
        static CommandKind getCommand( const char* );
        /// コマンドを出力する。
        static void printCommand( const char* commandName );
    };

}
//------------------------------------------------------------
// EOF
