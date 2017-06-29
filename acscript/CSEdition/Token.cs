using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �g�[�N���B
    /// </summary>
    class Token
    {
        //------------------------------------------------------------
        // �����G���R�[�h�w��B
        public enum CharEncode
        {
            UTF8,
            UTF16,
            UTF32,
        };

        //------------------------------------------------------------
        // �g�[�N���̎�ށB
        public enum Kind
        {
            Reserved,
            // Operator
            OpDiv,           // /
            OpDivAssign,     // /=
            OpDot,           // .
            OpAnd,           // &
            OpAndAssign,     // &=
            OpAndAnd,        // &&
            OpOr,            // |
            OpOrAssign,      // |=
            OpOrOr,          // ||
            OpMinus,         // -
            OpMinusAssign,   // -=
            OpMinusMinus,    // --
            OpPlus,          // +
            OpPlusAssign,    // +=
            OpPlusPlus,      // ++
            OpLess,          // <
            OpLessEqual,     // <=
            OpLShift,        // <<
            OpLShiftAssign,  // <<=
            OpGreater,       // >
            OpGreaterEqual,  // >=
            OpRShift,        // >>
            OpRShiftAssign,  // >>=
            OpNot,           // !
            OpNotEqual,      // !=
            OpLParen,        // (
            OpRParen,        // )
            OpLBracket,      // [
            OpRBracket,      // ]
            OpLCurly,        // {
            OpRCurly,        // }
            OpQuestion,      // ?
            OpComma,         // ,
            OpSemicolon,     // ;
            OpColon,         // :
            OpAssign,        // =
            OpEqual,         // ==
            OpMul,           // *
            OpMulAssign,     // *=
            OpMod,           // %
            OpModAssign,     // %=KEY?_
            OpXor,           // ^
            OpXorAssign,     // ^=
            OpTilde,         // ~
            OpAt,            // @

            // Numeric literals
            NumSInt32,
            NumUInt32,
            NumSInt64,
            NumUInt64,
            NumFloat32,
            NumFloat64,

            // Char constants
            NumChar8,
            NumChar16,
            NumChar32,

            // String constants
            StringChar8,
            StringChar16,
            StringChar32,

            // Keyword
            // - a
            KeyAbstract,       // abstract
            KeyAlias,          // alias
            KeyAssert,         // assert
            // - b
            KeyBool,           // bool
            KeyBreak,          // break
            // - c
            KeyCase,           // case
            KeyCast,           // cast
            KeyChar,           // char
            KeyClass,          // class
            KeyConst,          // const
            KeyContinue,       // continue
            // - d
            KeyDChar,          // dchar
            KeyDefault,        // default
            KeyDelete,         // delete
            KeyDo,             // do
            // - e
            KeyElse,           // else
            KeyEnum,           // enum
            // - f
            KeyFalse,          // false
            KeyFloat32,        // float,float32,f32
            KeyFloat64,        // double,float64,f64
            KeyFor,            // for
            // - i
            KeyIf,             // if
            KeyImmutable,      // immutable
            KeyImport,         // import
            KeyIn,             // in
            KeyInterface,      // interface
            KeyInvariant,      // invariant
            KeyIs,             // is
            // - m
            KeyModule,         // module
            // - n
            KeyNew,            // new
            KeyNull,           // null
            // - o
            KeyObject,         // object
            KeyOverride,       // override
            // - p
            KeyPod,            // pod
            KeyPrivate,        // private
            KeyPrototype,        // prototype
            KeyPublic,         // public     
            // - r
            KeyReadonly,       // readonly
            KeyRef,            // ref
            KeyReturn,         // return
            // - s
            KeySInt16,         // short,sshort,sint16,s16
            KeySInt32,         // int,sint,sint32,s32
            KeySInt64,         // long,slong,sint64,s64
            KeySInt8,          // sbyte,sint8,s8
            KeyStatic,         // static
            KeyStruct,         // struct
            KeySwitch,         // switch
            // - t
            KeyThis,           // this
            KeyTrue,           // true
            KeyTypedef,        // typedef
            // - u
            KeyUInt16,         // ushort,uint16,u16
            KeyUInt32,         // uint,uint32,u32
            KeyUInt64,         // ulong,uint64,u64
            KeyUInt8,          // byte,ubyte,uint8,u8
            KeyUsing,          // using
            KeyUtility,        // utility
            // - v
            KeyVoid,           // void
            // -w
            KeyWChar,          // wchar
            KeyWhile,          // while

            // Reserved Keyword
            KeyAuto,           // auto
            KeyCatch,          // catch
            KeyFinaly,         // finaly
            KeyForeach,        // foreach
            KeyForeachReverse, // foreach_reverse
            KeyInOut,          // inout
            KeyOut,            // out
            KeyScope,          // scope
            KeyThrow,          // throw
            KeyTry,            // try

            // Other
            Identifier,
            Comment,
            EOF,
        };

        public int pos = 0; // �e�L�X�g�̈ʒu�B�擪����̕������B
        public ushort posLine = 0; // �g�[�N���J�n�ʒu�̃e�L�X�g�̍s�ԍ��B
        public ushort posColumn = 0; // �g�[�N���J�n�ʒu�̃e�L�X�g�̗�ԍ��B
        public Kind Value = Kind.Reserved; // �g�[�N���̎��
        public Token Next = null; // ���̃g�[�N���B
        // �ȉ��̃����o�[��C++�ɏ����N�����Ƃ���Union�ŕ\���B
        public Int64 Int64Value = 0;
        public UInt64 UInt64Value = 0;
        public Single Float32Value = 0;
        public Double Float64Value = 0;
        public string UString = null;
        public CharEncode UStringEncode = CharEncode.UTF16;
        public string Identifier = null;
    }
}
