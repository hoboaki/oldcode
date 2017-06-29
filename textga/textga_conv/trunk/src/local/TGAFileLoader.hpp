/**
 * @file
 * @brief TGAファイルをロードするクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>

//------------------------------------------------------------
namespace local {

    struct Pixel;
    struct TGAHeader;
    
    /// TGAファイルをロードするクラスを記述する。
    class TGAFileLoader
    {
    public:
        TGAFileLoader( const std::string& filePath );
    
        /// 開けたか。
        bool isOpened()const;
        /// 変換をサポートできるTGAファイルか。
        bool isSupportedFile()const;
        
        /// 横幅を取得。
        short width()const;
        /// 縦幅を取得。
        short height()const;
        /// bpsを取得。
        char  bitsPerPixel()const;
        /// ピクセルの総数を取得する。
        size_t pixelCount()const;
        /// 指定のindexのピクセルを取得する。
        Pixel pixelAtIndex( size_t index )const;
        
    private:
        bool isOpened_;
        char bitsPerPixel_;
        size_t pixelCount_;
        std::vector< unsigned char > bytes_;
        
        //------------------------------------------------------------
        const TGAHeader& header()const;
    };
    
}
//------------------------------------------------------------
// EOF
