/**
 * @file
 * @brief 文字列関係のユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>

//------------------------------------------------------------
namespace local {

    /// 文字列関係のユーティリティ関数。
    class StringUtil
    {
    public:
        /// 等しいか。
        static bool equals( const char* str1 , const char* str2 );
        
        /// 大文字、小文字関係なく等しいか。
        static bool caseEquals( const char* str1 , const char* str2 );
        
        /// ファイルパスから拡張子を外したファイル名を取得する。
        static std::string baseName( const char* filepath );
    };

}
//------------------------------------------------------------
// EOF
