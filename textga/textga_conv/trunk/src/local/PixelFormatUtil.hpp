/**
 * @file
 * @brief PixelFormatに関するユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <local/PixelFormat.hpp>

//------------------------------------------------------------
namespace local {

    struct Pixel;
    
    /// PixelFormatに関するユーティリティ関数群。
    class PixelFormatUtil
    {
    public:
        /// PixelFormatを示す名前の文字列に変換する。
        static const char* toName( PixelFormat );
        
        /// 有効なPixelFormatか。
        static bool isValid( PixelFormat );
        
        /// 指定のフォーマットの値に変換する。
        static Pixel convert( const Pixel& , PixelFormat );
        
        /// アルファチャンネルがあるか。
        static bool hasAlpha( PixelFormat );
        
        /// DXTC圧縮フォーマットか。
        static bool isDXTC( PixelFormat );
        
        /// squishの圧縮フラグを取得する。
        static int dxtcCompressFlag( PixelFormat );
        
        /// コードテスト。
        static void codeTest();
    };

}
//------------------------------------------------------------
// EOF
