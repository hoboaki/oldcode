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
        CommandKind_Invalid         ///< 不正な引数。
        
        ,CommandKind_Convert        ///< 変換。
        ,CommandKind_Clear          ///< 拡張情報の削除。
        ,CommandKind_Help           ///< ヘルプの表示。
        ,CommandKind_Information    ///< 情報。
        ,CommandKind_Merge          ///< アルファチャンネルのマージ。
        ,CommandKind_Revert         ///< 元に戻す。
        
        ,CommandKind_Terminate
    };

}
//------------------------------------------------------------
// EOF
