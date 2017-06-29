/**
 * @file
 * @brief TagKindに関する関数群を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/tag/TagKind.hpp>

//------------------------------------------------------------
namespace textga {
namespace tag {

    /// TagKindに関する関数群。
    class TagKindUtil
    {
    public:
        /// 文字列から取得する。
        static TagKind fromName( const char* name );
    };
}}
//------------------------------------------------------------
// EOF
