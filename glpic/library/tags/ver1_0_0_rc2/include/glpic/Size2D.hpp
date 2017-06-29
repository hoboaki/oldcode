/**
 * @file
 * @brief 2Dのサイズを示す構造体を記述する。
 */
#if defined(GLPIC_INCLUDED_SIZE2D_HPP)
#else
#define GLPIC_INCLUDED_SIZE2D_HPP

//----------------------------------------------------------------
// public
namespace glpic {

    /// 2Dのサイズを示す構造体。
    struct Size2D
    {
        unsigned short width;  ///< 横幅。
        unsigned short height; ///< 縦幅。

        //----------------------------------------------------------------
        /// 作成関数。
        static Size2D create( unsigned short , unsigned short height );
    };

}
//----------------------------------------------------------------
#endif 
// EOF
