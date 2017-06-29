/**
 * @file
 * @brief PixelFormatに関するユーティリティ関数を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <textga/Pixel.hpp>
#include <textga/PixelFormat.hpp>

//------------------------------------------------------------
namespace textga {

    /// PixelFormatに関するユーティリティ関数群。
    class PixelFormatUtil
    {
    public:
        /// PixelFormatを示す名前の文字列に変換する。
        static const char* toName( ::textga::PixelFormat );
        
        /// 文字列からPixelFormatに変換する。
        static ::textga::PixelFormat fromName( const char* );
        
        /// 有効なPixelFormatか。
        static bool isValid( ::textga::PixelFormat );
        
        /// 指定のフォーマットの値に変換する。
        static ::textga::Pixel convert( const ::textga::Pixel& , ::textga::PixelFormat );
        
        /// アルファチャンネルがあるか。
        static bool hasAlpha( ::textga::PixelFormat );
        
        /// DXTC圧縮フォーマットか。
        static bool isDXTC( ::textga::PixelFormat );
        
        /// コードテスト。
        static void codeTest();
    };

}
//------------------------------------------------------------
// EOF
