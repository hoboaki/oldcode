/**
 * @file
 * @brief ByteVector型を記述する。
 */
#pragma once

//------------------------------------------------------------
#include <vector>

//------------------------------------------------------------
namespace app {

    /// バイト配列。
    class ByteVector
    {
    public:
        ByteVector();

        /// 書き込む。
        void print( const char* , ... );
        /// インデントスペースを書き込む。
        void printIndentSpace( U32 num );
        /// インデントを増やす。
        void indentEnter();
        /// インデントを減らす。
        void indentReturn();
        /// 現在のインデントを書き込む。
        void printCurrentIndent();
        /// 改行を書き込む。
        void printLineEnter();
        /// コメントを書き込む。
        void printComment( const char* , ... );
        /// Labelタグを書き込む。
        void printTagLabel( const char* labelName );
        /// Labelタグを書き込む。(format版)
        void printTagLabelF( const char* , ... );
        /// Referenceタグを書き込む。
        void printTagReference( const char* labelName );
        /// Referenceタグを書き込む。(format版)
        void printTagReferenceF( const char* , ... );
        /// U8Arrayタグを書き込む。
        void printTagU8Array( const U8* ptr , U32 count );
        /// U16タグを書き込む。
        void printTagU16( U16 val );
        /// U16Arrayタグを書き込む。
        void printTagU16Array( const U16* ptr , U32 count );
        /// U32タグを書き込む。
        void printTagU32( U32 val );
        /// U32Arrayタグを書き込む。
        void printTagU32Array( const U32* ptr , U32 count );
        /// F32Arrayタグを書き込む。
        void printTagF32Array( const F32* ptr , U32 count );
        /// Stringタグを書き込む。
        void printTagString( const char* str );

        /// ファイルに書き出す
        bool writeToFile( const char* path );

    private:
        std::vector< char > bytes_;
        U32 size_;
        U32 currentIndent_;
        //------------------------------------------------------------
        void vprintf( const char* , va_list );
    };

}
//------------------------------------------------------------
// EOF
