/**
 * @file
 * @brief バイナリデータを示す構造体を記述する。
 */
#if defined(GLPIC_INCLUDED_BINARYDATA_HPP)
#else
#define GLPIC_INCLUDED_BINARYDATA_HPP

//----------------------------------------------------------------
// public
namespace glpic {

    /// バイナリデータを示す構造体。
    struct BinaryData
    {
        void*         address;  ///< データの先頭アドレス。
        unsigned long size;     ///< データ長。

        //----------------------------------------------------------------
        /// 作成関数。
        static BinaryData create( void* address , unsigned long size );
    };

}
//----------------------------------------------------------------
#endif
// EOF
