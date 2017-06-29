/**
 * @file
 * @brief ミップマップに関するユーティリティ関数を記述する。
 */
#if defined(GLPIC_INCLUDED_MIPMAPUTIL_HPP)
#else
#define GLPIC_INCLUDED_MIPMAPUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/Noncreatable.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// ミップマップに関するユーティリティ関数群。
    class MipMapUtil : public Noncreatable
    {
    public:
        /**
         * @brief 次のMipMapレベルがあるか。
         * 横幅、縦幅いずれかが２のべき上でない場合、
         * 横幅、縦幅いずれかが１である場合、偽を返す。
         */
        static bool hasNextLevel( const Size2D& );

        /**
        * @brief 次のMipMapレベルのサイズを取得する。
        * GLPicMipMapUtil_HasNextLevelがGLPicBoolを
        * 返すようなサイズを渡した場合の動作は不定。
        */
        static Size2D nextLevelSize2D( const Size2D& );

        /**
        * @brief MipMapの最大レベル数を取得する。
        * @return 0以上。
        */
        static unsigned char calculateMaxLevel( const Size2D& );

        /// MipMapUtilのユニットテスト。
        static void unitTest();

    };

}
//----------------------------------------------------------------
#endif
// EOF
