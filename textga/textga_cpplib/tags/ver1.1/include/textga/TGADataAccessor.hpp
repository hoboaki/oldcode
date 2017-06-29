/**
 * @file
 * @brief TGAデータのアクセサクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>
#include <textga/PixelFormat.hpp>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {

    struct Pixel;
    struct TGAHeader;
    
    /// TGAデータのアクセサクラス。
    class TGADataAccessor
    {
    public:
        /**
         * @brief TGAデータのアドレスとサイズを渡す。
         *
         * このクラスが死ぬまでポインタが示すデータが失われてはいけない。
         */
        TGADataAccessor( const byte* data , size_t dataSize );
    
        /// 変換をサポートできるTGAファイルか。
        bool isSupportedTGA()const;
        
        //------------------------------------------------------------
        /**
         * @name TGAの情報取得。
         *
         * isSupportedTGA() == true のときのみ呼べる。
         */
        //@{
        /// 横幅を取得。
        u16 width()const;
        /// 縦幅を取得。
        u16 height()const;
        /// bpsを取得。
        u8  bitsPerPixel()const;
        /// ピクセルの総数を取得する。
        size_t pixelCount()const;
        /// ピクセルデータのバイト数を取得する。
        size_t pixelDataSize()const;
        /**
         * @brief 指定のindexのピクセルを取得する。
         * @param index 0 <= val < pixelCount()
         */
        Pixel pixelAtIndex( size_t index )const;
        /// TexTarga形式か。
        bool isTexTarga()const;
        //@}
        
        //------------------------------------------------------------
        /**
         * @name TexTargaの情報取得。
         *
         * isTexTarga() == true のときのみ呼べる。
         */
        //@{
        /// ピクセルフォーマットを取得する。
        PixelFormat pixelFormat()const;
        /// SrcDataが存在するか。
        bool isExistSrcData()const;
        /**
         * @brief SrcDataのピクセルを取得する。
         * @param index 0 <= val < pixelCount()
         */
        Pixel srcDataPixelAtIndex( size_t index )const;
        /// SrcDataのアドレスを取得する。
        const byte* srcData()const;
        //@}
        
    private:
        const byte* const data_;
        const size_t dataSize_;
        
        bool isSupportedTGA_;
        u8 bitsPerPixel_;
        size_t pixelCount_;
        
        bool isTexTarga_;
        PixelFormat ttPixelFormat_;
        const byte* ttSrcData_; // SrcDataの先頭アドレス。存在しなければNULL。
        
        //------------------------------------------------------------
        const TGAHeader& header()const;
        Pixel getPixel( const byte* data , size_t index )const;
    };
    
}
//------------------------------------------------------------
// EOF
