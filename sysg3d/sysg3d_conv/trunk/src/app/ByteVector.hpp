/**
 * @file
 * @brief ByteVector�^���L�q����B
 */
#pragma once

//------------------------------------------------------------
#include <vector>

//------------------------------------------------------------
namespace app {

    /// �o�C�g�z��B
    class ByteVector
    {
    public:
        ByteVector();

        /// �������ށB
        void print( const char* , ... );
        /// �C���f���g�X�y�[�X���������ށB
        void printIndentSpace( U32 num );
        /// �C���f���g�𑝂₷�B
        void indentEnter();
        /// �C���f���g�����炷�B
        void indentReturn();
        /// ���݂̃C���f���g���������ށB
        void printCurrentIndent();
        /// ���s���������ށB
        void printLineEnter();
        /// �R�����g���������ށB
        void printComment( const char* , ... );
        /// Label�^�O���������ށB
        void printTagLabel( const char* labelName );
        /// Label�^�O���������ށB(format��)
        void printTagLabelF( const char* , ... );
        /// Reference�^�O���������ށB
        void printTagReference( const char* labelName );
        /// Reference�^�O���������ށB(format��)
        void printTagReferenceF( const char* , ... );
        /// U8Array�^�O���������ށB
        void printTagU8Array( const U8* ptr , U32 count );
        /// U16�^�O���������ށB
        void printTagU16( U16 val );
        /// U16Array�^�O���������ށB
        void printTagU16Array( const U16* ptr , U32 count );
        /// U32�^�O���������ށB
        void printTagU32( U32 val );
        /// U32Array�^�O���������ށB
        void printTagU32Array( const U32* ptr , U32 count );
        /// F32Array�^�O���������ށB
        void printTagF32Array( const F32* ptr , U32 count );
        /// String�^�O���������ށB
        void printTagString( const char* str );

        /// �t�@�C���ɏ����o��
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
