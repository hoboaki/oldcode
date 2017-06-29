/**
 * @file
 * @brief コマンドの種類を示す識別子を記述する。
 */
#pragma once

//------------------------------------------------------------
namespace local {

    /// コマンドの種類を示す識別子。
    enum CommandKind
    {
        CommandKind_Invalid         ///< 不正なコマンド。
        ,CommandKind_PrintUseage    ///< ヘルプの表示。
        ,CommandKind_Convert        ///< 変換。
        ,CommandKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
