/**
 * @file
 * @brief PixelFormatに関するユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/PixelFormat.hpp>

//------------------------------------------------------------
namespace textga {

    struct Pixel;
    
    /// PixelFormatに関するユーティリティ関数群。
    class PixelFormatUtil
    {
    public:
        /// PixelFormatを示す名前の文字列に変換する。
        static const char* toName( PixelFormat );
        
        /// 文字列からPixelFormatに変換する。
        static PixelFormat fromName( const char* );
        
        /// 有効なPixelFormatか。
        static bool isValid( PixelFormat );
        
        /// 指定のフォーマットの値に変換する。
        static Pixel convert( const Pixel& , PixelFormat );
        
        /// アルファチャンネルがあるか。
        static bool hasAlpha( PixelFormat );
        
        /// DXTC圧縮フォーマットか。
        static bool isDXTC( PixelFormat );
        
        /// コードテスト。
        static void codeTest();
    };

}
//------------------------------------------------------------
// EOF
