/**
 * @file
 * @brief ピクセルフォーマットの関数群を記述する。
 */
#if defined(GLPIC_INCLUDED_PIXELFORMATUTIL_HPP)
#else
#define GLPIC_INCLUDED_PIXELFORMATUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/PixelFormat.hpp>
#include <glpic/Noncreatable.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    class PixelFormatUtil : public Noncreatable
    {
    public:
        /// ピクセルフォーマットの識別子として正しい値か。
        static bool isValid( PixelFormat );

        /// 1ピクセルあたりのビット数を取得する。
        static unsigned char getBitPerPixel( PixelFormat );

        /**
         * @brief 1ブロックのピクセル数を取得する。
         *
         * ピクセルデータはブロックの整数倍である必要がある。@n
         * 例えば1ブロック4ピクセルなフォーマットの場合、@n
         * ピクセルの総数は4の倍数である必要がある。
         */
        static unsigned char getPixelCountPerBlock( PixelFormat );

        /**
         * @brief テクスチャのサイズがフォーマットに対して適切か。
         * @return 問題がなければtrue。
         * @param pixelFormat ピクセルフォーマット。
         * @param textureSize テクスチャサイズ。
         */
        static bool isValidSizeForPixelFormat( 
            PixelFormat pixelFormat
            , const Size2D& textureSize
            );

        /**
        * @brief ピクセルデータのデータ長を計算する。
        * @return データ長。引数が不正な場合、0を返す。
        * @param pixelFormat ピクセルフォーマット。
        * @param textureSize テクスチャサイズ。
        */
        static unsigned long calculatePixelDataSize(
            PixelFormat pixelFormat
            , const Size2D& textureSize
            );

        /**
        * @brief ピクセルデータのデータ長を計算する。
        * @return データ長。引数が不正な場合、0を返す。
        * @param pixelFormat ピクセルフォーマット。
        * @param textureSize テクスチャサイズ。
        * @param mipMapMaxLevel ミップマップの最大レベル。
        */
        static unsigned long calculateMipMapPixelDataSize(
            PixelFormat pixelFormat
            , const Size2D& textureSize
            , unsigned char mipMapMaxLevel
            );

        /// ピクセルフォーマットを文字列に変換する。
        static const char* toString( PixelFormat piexlFormat );

        /// PixelFormatに関するユニットテスト。
        static void unitTest();
    };

}
//----------------------------------------------------------------
#endif
// EOF
