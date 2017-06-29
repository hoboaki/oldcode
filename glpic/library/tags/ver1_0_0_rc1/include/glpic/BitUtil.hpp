/**
 * @file
 * @brief ビット計算に関するユーティリティ関数を記述する。
 */
#if defined(GLPIC_INCLUDED_BITUTIL_HPP)
#else
#define GLPIC_INCLUDED_BITUTIL_HPP

//----------------------------------------------------------------
// include
#include <glpic/Noncreatable.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// ビット計算に関するユーティリティ関数群。
    class BitUtil : public ::glpic::Noncreatable
    {
    public:
        /// ビットが存在しないことを示す値。
        static const signed char NO_BIT;

        /**
        * @brief 指定した値の1である最大のビット位置を取得する。
        * @return 指定した値が0のときGLPicBitUtil_NoBitを返す。
        * 例：0b01001000 = 6
        */
        static signed char getMaxBitU16( unsigned short ); 

        /**
        * @brief 指定した値の1である最小のビット位置を取得する。
        * @return 指定した値が0のときGLPicBitUtil_NoBitを返す。
        * 例：0b01110100 = 2
        */
        static signed char getMinBitU16( unsigned short );

        /**
        * @brief 2つの要素を4:4でパックする。
        * @param component1 上位4ビットを見る値。
        * @param component2 上位4ビットを見る値。
        */
        static unsigned char pack4b4bTo8b(
            unsigned char component1
            , unsigned char component2
            );

        /**
        * @brief 2つの要素を6:2でパックする。
        * @param component1 上位6ビットを見る値。
        * @param component2 上位2ビットを見る値。
        */
        static unsigned char pack6b2bTo8b(
            unsigned char component1
            , unsigned char component2
            );

        /**
        * @brief ３つの要素を3:3:2でパックする。
        * @param component1 上位3ビットを見る値。
        * @param component2 上位3ビットを見る値。
        * @param component3 上位2ビットを見る値。
        */
        static unsigned char pack3b3b2bTo8b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            );

        /**
        * @brief ３つの要素を5:5:5(N1)でパックする。
        * @param component1 上位5ビットを見る値。
        * @param component2 上位5ビットを見る値。
        * @param component3 上位5ビットを見る値。
        */
        static unsigned short pack5b5b5bTo16b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            );

        /**
        * @brief ３つの要素を10:10:10(N2)でパックする。
        * @param component1 上位10ビットを見る値。
        * @param component2 上位10ビットを見る値。
        * @param component3 上位10ビットを見る値。
        */
        static unsigned long pack10b10b10bTo32b(
            unsigned short component1
            , unsigned short component2
            , unsigned short component3
            );

        /**
        * @brief ４つの要素を2:2:2:2でパックする。
        * @param component1 上位2ビットを見る値。
        * @param component2 上位2ビットを見る値。
        * @param component3 上位2ビットを見る値。
        * @param component4 上位2ビットを見る値。
        */
        static unsigned char pack2b2b2b2bTo8b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            , unsigned char component4
            );

        /**
        * @brief ４つの要素を4:4:4:4でパックする。
        * @param component1 上位4ビットを見る値。
        * @param component2 上位4ビットを見る値。
        * @param component3 上位4ビットを見る値。
        * @param component4 上位4ビットを見る値。
        */
        static unsigned short pack4b4b4b4bTo16b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            , unsigned char component4
            );

        /**
        * @brief ４つの要素を5:5:5:1でパックする。
        * @param component1 上位5ビットを見る値。
        * @param component2 上位5ビットを見る値。
        * @param component3 上位5ビットを見る値。
        * @param component4 上位1ビットを見る値。
        */
        static unsigned short pack5b5b5b1bTo16b(
            unsigned char component1
            , unsigned char component2
            , unsigned char component3
            , unsigned char component4
            );

        /**
        * @brief ４つの要素を10:10:10:2でパックする。
        * @param component1 上位10ビットを見る値。
        * @param component2 上位10ビットを見る値。
        * @param component3 上位10ビットを見る値。
        * @param component4 上位2ビットを見る値。
        */
        static unsigned long pack10b10b10b2bTo32b(
            unsigned short component1
            , unsigned short component2
            , unsigned short component3
            , unsigned char component4
            );

        /// BitUtilのユニットテスト。
        static void unitTest();
    };

}
//----------------------------------------------------------------
#endif 
//EOF
