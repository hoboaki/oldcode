/**
 * @file
 * @brief 文字列関係のユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

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
        
        /**
         * @brief バイナリデータから文字列を取得する。
         * @return 文字列が取得できればポインタをそのまま返す。取得できなければ0を返す。
         * @param data データのアドレス。
         * @param size データのサイズ。
         */
        static const char* findString( const ::textga::byte* data , size_t size );
    };

}
//------------------------------------------------------------
// EOF
