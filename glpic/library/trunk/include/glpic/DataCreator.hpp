/**
 * @file
 * @brief GLPicデータを作る関数を記述する。
 */
#if defined(GLPIC_INCLUDED_DATACREATOR_HPP)
#else
#define GLPIC_INCLUDED_DATACREATOR_HPP

//----------------------------------------------------------------
// include
#include <glpic/BinaryData.hpp>
#include <glpic/PixelFormat.hpp>
#include <glpic/Noncreatable.hpp>
#include <glpic/Size2D.hpp>

//----------------------------------------------------------------
// public
namespace glpic {

    /// GLPicデータを作る関数群。
    class DataCreator : public Noncreatable
    {
    public:
        /// エラー情報。
        enum ErrorKind
        {
            ErrorKind_None ///< エラー無し。
            ,ErrorKind_NotEnoughDataSize ///< 渡されたデータサイズが足らない。
            ,ErrorKind_InvalidArgument ///< レシピおよびオフセット値が不正。
            ,ErrorKind_InvalidPixelDataSize ///< ピクセルデータのデータサイズが不正。
        };

        /// GLPicデータを作る際のレシピ構造体。
        struct Recipe
        {
            PixelFormat     pixelFormat; ///< ピクセルフォーマット。
            bool            imageScaled; ///< イメージは引き延ばされているか。
            unsigned char   mipMapMaxLevel; ///< MipMapの最大レベル。MipMapを使わないときは0。
            Size2D          textureSize; ///< テクスチャサイズ。
            Size2D          imageSize; ///< イメージサイズ。

            //----------------------------------------------------------------
            /// GLPicデータのレシピを作成。
            static Recipe create(
                PixelFormat pixelFormat
                , bool imageScaled
                , unsigned char mipMapMaxLevel
                , const Size2D& textureSize
                , const Size2D& imageSize
                );

            /// レシピが有効か。
            bool isValid()const;
        };

        /// 作成結果。
        struct Result
        {
            ErrorKind errorKind; ///< エラー情報。
            unsigned long dataSize; ///< エラーがなければ、作成されたデータのサイズ。

            //----------------------------------------------------------------
            /// 作成関数。
            static Result create( ErrorKind errorKind , unsigned long dataSize );
        };

        /**
        * @brief 必要なデータサイズを計算する。
        * @return データサイズ。引数が不正な場合、0を返す。
        * @param recipe レシピ。
        */
        static unsigned long calculateDataSize(
            const Recipe& recipe
            );

        /**
        * @brief 必要なデータサイズを計算する。
        * @return データサイズ。引数が不正な場合、0を返す。
        * @param recipe レシピ。
        * @param pixelDataOffset ピクセルデータのオフセット位置。
        * @param userDataOffset ユーザーデータのオフセット位置。
        * @param userDataSize ユーザーデータのサイズ。
        */
        static unsigned long calculateDataSizeCustum(
            const Recipe& recipe
            , const unsigned long pixelDataOffset
            , const unsigned long userDataOffset
            , const unsigned long userDataSize
            );

        /**
        * @brief シンプルなデータを作成する。
        * @return エラー情報、データのサイズなど。
        * @param recipe レシピ。
        * @param pixelDataArray ピクセルデータの配列。配列のサイズはmipMapMaxLevel+1であること。
        * @param allocedData 作成したデータを格納する。確保済みのメモリを代入しておくこと。
        */
        static Result createData(
            const Recipe& recipe
            , const BinaryData* const pixelData
            , const BinaryData& allocedData
            );

        /**
        * @brief オプションを全て指定してデータを作成する。
        * @return エラー情報、データのサイズなど。
        * @param recipe レシピ。
        * @param pixelDataOffset ピクセルデータの開始位置。データの先頭からのバイトサイズ。
        * @param pixelDataArray ピクセルデータの配列。配列のサイズはmipMapMaxLevel+1であること。
        * @param userDataOffset ユーザーデータの開始位置。データの先頭からのバイトサイズ。使わない場合は0。
        * @param userDataPointer ユーザーデータへのポインタ。いらない場合は0。
        * @param allocedData 作成したデータを格納する。確保済みのメモリを代入しておくこと。
        */
        static Result createDataCustum(
            const Recipe& recipe
            , const unsigned long pixelDataOffset
            , const BinaryData* const pixelDataArray
            , const unsigned long userDataOffset
            , const BinaryData* const userDataPointer
            , const BinaryData& allocedData
            );

    };

}
//----------------------------------------------------------------
#endif
//----------------------------------------------------------------
// EOF
