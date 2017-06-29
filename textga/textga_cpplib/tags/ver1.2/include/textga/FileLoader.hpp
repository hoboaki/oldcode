/**
 * @file
 * @brief ファイルをロードするクラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <string>
#include <vector>
#include <textga/Types.hpp>

//------------------------------------------------------------
namespace textga {
    
    /// ファイルをロードするクラスを記述する。
    class FileLoader
    {
    public:
        FileLoader( const char* filePath );
    
        /// 開けたか。
        bool isLoaded()const;
        
        /// データを取得する。
        const ::textga::byte* data()const;
        /// データサイズを取得する。
        size_t dataSize()const;
        
    private:
        bool isLoaded_;
        std::vector< ::textga::byte > bytes_;
    };
}
//------------------------------------------------------------
// EOF
