/**
 * @file
 * @brief TGAファイルをロードするクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>
#include <textga/FileLoader.hpp>
#include <textga/TGADataAccessor.hpp>

//------------------------------------------------------------
namespace textga {
    
    /// TGAファイルをロードするクラスを記述する。
    class TGAFileLoader
    {
    public:
        TGAFileLoader( const char* filePath );
    
        /// ファイルが読み込めたか。
        bool isLoaded()const;
        
        /**
         * @brief TGADataアクセサを取得する。
         * isLoaded() == true のときのみ呼べる。
         * アクセサが生き続ける限り、このTGAFileLoaderをdeleteしてはいけない。
         */
        TGADataAccessor tgaDataAccessor()const;
        
        /// ファイルの参照を返す。。
        const FileLoader& file()const;
        
    private:
        FileLoader file_;
    };
    
}
//------------------------------------------------------------
// EOF
