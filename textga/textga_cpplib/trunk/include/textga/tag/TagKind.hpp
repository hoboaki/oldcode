/**
 * @file
 * @brief タグの種類を示す列挙型を記述する。　
 */
#pragma once

//------------------------------------------------------------
namespace textga { 
namespace tag {

    /// タグを示す列挙型。
    enum TagKind
    {
        TagKind_Unknown  ///< 不明。
        ,TagKind_SectionBegin ///< セクション開始。
        ,TagKind_SectionEnd   ///< セクション終了。
        ,TagKind_NumU8      ///< U8。
        ,TagKind_NumU16     ///< U16。
        ,TagKind_NumU32     ///< U32。
        ,TagKind_NumS8      ///< S8。
        ,TagKind_NumS16     ///< S16。
        ,TagKind_NumS32     ///< S32。
        ,TagKind_String  ///< 文字列。
        ,TagKind_Binary  ///< バイナリデータ。
    };

}}
//------------------------------------------------------------
// EOF
