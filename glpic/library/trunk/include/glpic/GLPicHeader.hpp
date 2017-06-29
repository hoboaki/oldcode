/**
 * @file 
 * @brief GLPICのヘッダとデータアクセス関数を記述する。
 */
#if defined(GLPIC_INCLUDED_GLPICHEADER_HPP)
#else
#define GLPIC_INCLUDED_GLPICHEADER_HPP

//----------------------------------------------------------------
// include
#include <glpic/PixelFormat.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// GLPicのヘッダ構造体。
    struct GLPicHeader
    {
    // public
        
        /**
        * @brief ピクセルフォーマットを取得する。
        * @return ピクセルフォーマットの種類。
        * データが不正なときの戻り値は不定。
        */
        PixelFormat getPixelFormat()const;

        /**
        * @brief イメージが引き延ばされているか取得する。
        * @return 引き延ばされているようであればtrue。
        * この関数がtrueを返す場合、イメージは
        * テクスチャ全体に引き延ばされている。
        */
        bool getImageScaled()const;

        /**
        * @brief ミップマップの最大レベル数を取得する。
        * @return 使用していない場合は0。
        * データが不正なときの戻り値は不定。
        */
        unsigned char getMipMapMaxLevel()const;

        /**
        * @brief テクスチャのピクセル数を取得する。
        * @return ピクセル数。
        * データが不正なときの戻り値は不定。
        */
        Size2D getTextureSize2D()const;

        /**
        * @brief 元のイメージのピクセル数を取得する。
        * @return ピクセル数。
        * データが不正なときの戻り値は不定。
        */
        Size2D getImageSize2D()const;

        /**
        * @brief ピクセルデータのデータ長を取得する。
        * @return データ長。
        * データが不正なときの戻り値は不定。
        */
        unsigned long getPixelDataSize()const;

        /**
        * @brief ユーザーデータのデータ長を取得する。
        * @return データ長。
        * データが不正なときの戻り値は不定。
        */
        unsigned long getUserDataSize()const;

        /**
        * @brief ピクセルデータを取得する。
        * @return ピクセルデータへのポインタ。
        * データが不正なとき、戻り値は不定。
        */
        void* getPixelData();

        /**
        * @brief ピクセルデータを取得する。
        * @return ピクセルデータへのポインタ。
        * データが不正なとき、戻り値は不定。
        */
        const void* getPixelData()const;

        /**
        * @brief ユーザーデータを取得する。
        * @return ユーザーデータへのポインタ。
        * データが不正なとき、ユーザーデータ長が0のとき、戻り値は不定。
        */
        void* getUserData();

        /**
        * @brief ユーザーデータを取得する。
        * @return ユーザーデータへのポインタ。
        * データが不正なとき、ユーザーデータ長が0のとき、戻り値は不定。
        */
        const void* getUserData()const;

        /**
        * @brief データが正しいか。
        * シグネチャが不正なとき、
        * バイトオーダーが逆転しているときなどにGLPicBool_Falseを返す。
        * この関数を使ってデータが正しいことを確認してから
        * Get*関数を使用することを推奨する。
        */
        bool isValid()const;

        /**
        * @brief バイトオーダーが逆転しているか。
        * バイトオーダーが逆転していることを示す固定値があればGXPicBool_Trueを返す。
        * データが壊れていたり、バイトオーダーが正しい場合はGXPicBool_Falseを返す。
        */
        bool isByteOrderInversed()const;

        /**
        * @brief バイトオーダーを反転する。
        * バイトオーダーが正しくても反転するので注意。
        */
        void inverseByteOrder();

    // private
        //----------------------------------------------------------------
        // メンバ。直接アクセスせず、アクセサを使用すること。
        // 0
        char             signature[3];     ///< シグネチャ。固定文字列が入る。
        unsigned char    version;          ///< ファイルフォーマットのバージョン情報。
        unsigned short   endianCheck;      ///< エンディアンチェック。固定値が入る。
        unsigned char    pixelFormat;      ///< ピクセルフォーマット。
        unsigned char    flagAndMipMapLevel; ///< 最上位ビット:イメージは引き延ばされているか。それ以外：ミップマップの最大レベル。
        // 8
        unsigned short   textureWidth;     ///< テクスチャの横幅。
        unsigned short   textureHeight;    ///< テクスチャの縦幅。
        unsigned short   imageWidth;       ///< 元のイメージの横幅。
        unsigned short   imageHeight;      ///< 元のイメージの縦幅。
        // 16
        unsigned long    pixelDataOffset;  ///< ヘッダの先頭からpixelDataへのオフセット値
        unsigned long    pixelDataSize;    ///< ピクセルのデータサイズ。
        // 24
        unsigned long    userDataOffset;   ///< ヘッダの先頭からuserDataへのオフセット値。
        unsigned long    userDataSize;     ///< ユーザーデータのサイズ。

        //----------------------------------------------------------------        
        /// ヘッダの先頭にあるシグネチャ文字列。
        static const char* const SIGNATURE;

        /// ファイルフォーマットバージョン。
        static const unsigned char VERSION;

        /// 正常なバイトオーダーを示す値。
        static const unsigned short ENDIAN_CHECK_VALUE;

        /// 逆転しているバイトオーダーを示す値。
        static const unsigned short INVERSED_ENDIAN_CHECK_VALUE;

    };

}
//----------------------------------------------------------------
#endif
// EOF
