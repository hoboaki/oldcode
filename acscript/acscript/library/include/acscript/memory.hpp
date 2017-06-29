/**
 * @file
 * @brief Memory型を記述する。
 */
#ifndef ACS_INCLUDE_MEMORY
#define ACS_INCLUDE_MEMORY

//------------------------------------------------------------
#include <acscript/allocator.hpp>

//------------------------------------------------------------
namespace acscript {

    class Memory
    {
    public:
        /**
         * @brief プログラム構築時用アロケータ。
         * オブジェクトコード、管理用コードのメモリはここからとられる。
         */
        static Allocator& buildTimeAllocator();
        
        /**
         * @brief スタック用アロケータ。
         * コンテキストのスタックメモリはここからとられる。
         */
        static Allocator& contextStackAllocator();

        /**
         * @brief 実行時用アロケータ。
         * コンテキストの実行中に確保されるオブジェクトのメモリはここからとられる。
         */
        static Allocator& runTimeAllocator();
    };

}
//------------------------------------------------------------
#endif
// EOF
