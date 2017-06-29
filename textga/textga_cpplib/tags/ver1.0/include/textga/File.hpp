/**
 * @file
 * @brief Fileのラッパークラスを記述する。
 */
#pragma once

//------------------------------------------------------------
#include <cstdio>

//------------------------------------------------------------
namespace textga {

    /**
     * @brief Fileのラッパークラス。
     * コンストラクタでfopen、デストラクタでfcloseをする。
     */
    class File
    {
    public:
        File( const char* filePath , const char* option );
        ~File();
        
        FILE* fp();
        
    private:
        File( const File& ); // noncopyable
        
        FILE* fp_;
    };

}
//------------------------------------------------------------
// EOF
