/**
 * @file
 * @brief 要素の種類を示す列挙型を記述する。　
 */
#pragma once

//------------------------------------------------------------
namespace textga { 
namespace tag {

    /// 要素を示す列挙型。
    enum ElementKind
    {
        ElementKind_Unknown  ///< 不明。
        ,ElementKind_Section ///< セクション。
        ,ElementKind_NumU8   ///< U8。
        ,ElementKind_NumU16  ///< U16。
        ,ElementKind_NumU32  ///< U32。
        ,ElementKind_NumS8   ///< S8。
        ,ElementKind_NumS16  ///< S16。
        ,ElementKind_NumS32  ///< S32。
        ,ElementKind_String  ///< 文字列。
        ,ElementKind_Binary  ///< バイナリデータ。
    };

}}
//------------------------------------------------------------
// EOF
