using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 字句解析。
    /// ソースコードを与えるとトークンの集合を返す。
    /// </summary>
    class Lexer
    {
        //------------------------------------------------------------
        /// <summary>
        /// エラーの種類。
        /// </summary>
        public enum ErrorKind
        {
            None,
            UntermBlockComment,   // unterminated /* */ comment
            UntermCharConstant,   // unterminated character constant
            UntermNestComment,    // unterminated /+ +/ comment
            UntermStringConstant, // unterminated string constant
            UndefinedEscapeSequence, // undefined escape seqeuence
            UnsupportedChar, // unsupported char
        };

        //------------------------------------------------------------
        // エラー情報。
        public class ErrorInfo
        {
            public ErrorKind ErrorKind; // エラーの種類。
            public ushort Line; // エラーが起きた行数。
            public ushort Column; // エラーが起きた列数。
        };

        //------------------------------------------------------------
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="aText">ソースコード</param>
        public Lexer(string aText)
        {
            // キーワードペアリストを作成
            staticInit();

            // メンバの初期化
            mText = aText;
            mEnd = mText.Length;

            // 解析開始
            {
                Token prevToken = null;
                while (!IsError())
                {
                    Token t = new Token();
                    scan(t);
                    if (t.Value == Token.Kind.Comment)
                    {// コメントはパス
                        continue;
                    }
                    if (mFirstToken == null)
                    {
                        mFirstToken = t;
                        prevToken = t;
                    }
                    else
                    {
                        prevToken.Next = t;
                        prevToken = t;
                    }
                    if (t.Value == Token.Kind.EOF)
                    {
                        break;
                    }
                }
            }
        }

        //------------------------------------------------------------
        // エラーが起きたか。
        public bool IsError()
        {
            return mErrorKind != ErrorKind.None;
        }

        //------------------------------------------------------------
        // エラー情報を取得する。
        public ErrorInfo GetErrorInfo()
        {
            if (!IsError())
            {
                throw new Exception("Can't call function named 'ErrorInfo'");
            }
            ErrorInfo info = new ErrorInfo();
            info.ErrorKind = mErrorKind;
            info.Line = mLineNum;
            info.Column = mColumnNum;
            return info;
        }

        //------------------------------------------------------------
        // トークンのリストの先頭を取得。エラーが起きたときは取得できない。
        public Token FirstToken()
        {
            if (IsError())
            {
                throw new Exception("Can't call function named 'Tokens'");
            }
            return mFirstToken;
        }

        //------------------------------------------------------------
        // キーワードペアクラス。
        class KeywordPair
        {
            public string Str;
            public Token.Kind TokenKind;

            //------------------------------------------------------------
            // コンストラクタ。
            public KeywordPair(string aStr, Token.Kind aKind)
            {
                Str = aStr;
                TokenKind = aKind;
            }
        };

        //------------------------------------------------------------
        // Unicodeの幅を示すクラス。
        class UnicodeRange
        {
            public char Begin;
            public char End;

            //------------------------------------------------------------
            // コンストラクタ。
            public UnicodeRange(char aBegin, char aEnd)
            {
                Begin = aBegin;
                End = aEnd;
            }
        };

        //============================================================
        static List<KeywordPair> sKeywordPairList = null;
        static List<UnicodeRange> sUnicodeRangeList = null;

        const char CHAR_NUL = (char)0;
        const char CHAR_SUB = (char)0x1A;
        const char CHAR_LS = (char)0x2028;
        const char CHAR_PS = (char)0x2029;

        string mText;
        int mPos = 0;
        int mEnd = 0;
        ushort mLineNum = 0;
        ushort mColumnNum = 0;
        ErrorKind mErrorKind = ErrorKind.None;
        Token mFirstToken = null;

        //------------------------------------------------------------
        // 10進数数値を示すascii文字か。
        static bool isDigitChar(char aC)
        {
            return '0' <= aC && aC <= '9';
        }

        //------------------------------------------------------------
        // アルファベットを示すascii文字か。
        static bool isAlphabetChar(char aC)
        {
            return ('a' <= aC && aC <= 'z')
                || ('A' <= aC && aC <= 'Z');
        }

        //------------------------------------------------------------
        // identifierの2文字目以降に使える文字か。
        static bool isIDChar(char aC)
        {
            return isDigitChar(aC) || isAlphabetChar(aC) || aC == '_' || isUniversalAlphabet(aC);
        }

        //------------------------------------------------------------
        // 世界的なアルファベット文字か。
        static bool isUniversalAlphabet(char aC)
        {
            foreach (UnicodeRange range in sUnicodeRangeList)
            {
                if (range.Begin <= aC && aC <= range.End)
                {
                    return true;
                }
            }
            return false;
        }

        //------------------------------------------------------------
        // 改行を示す文字か。
        static bool isLineEnterChar(char aC)
        {
            return aC == '\n'
                || aC == CHAR_PS
                || aC == CHAR_LS;
        }


        //------------------------------------------------------------
        // キーワードペアリストを作成。
        static void staticInit()
        {
            if (sKeywordPairList != null)
            {// 既に作成済み
                return;
            }
            createKeywordPairList();
            createUnicodeRangeList();
        }
        //------------------------------------------------------------
        // キーワードペアリストを作成する関数本体。
        static void createKeywordPairList()
        {
            sKeywordPairList = new List<KeywordPair>();
            List<KeywordPair> k = sKeywordPairList;

            k.Add(new KeywordPair("abstract", Token.Kind.KeyAbstract));
            k.Add(new KeywordPair("alias", Token.Kind.KeyAlias));
            k.Add(new KeywordPair("assert", Token.Kind.KeyAssert));
            k.Add(new KeywordPair("auto", Token.Kind.KeyAuto));

            k.Add(new KeywordPair("bool", Token.Kind.KeyBool));
            k.Add(new KeywordPair("break", Token.Kind.KeyBreak));
            k.Add(new KeywordPair("byte", Token.Kind.KeyUInt8));

            k.Add(new KeywordPair("case", Token.Kind.KeyCase));
            k.Add(new KeywordPair("catch", Token.Kind.KeyCatch));
            k.Add(new KeywordPair("char", Token.Kind.KeyChar));
            k.Add(new KeywordPair("class", Token.Kind.KeyClass));
            k.Add(new KeywordPair("const", Token.Kind.KeyConst));
            k.Add(new KeywordPair("continue", Token.Kind.KeyContinue));

            k.Add(new KeywordPair("dchar", Token.Kind.KeyDChar));
            k.Add(new KeywordPair("default", Token.Kind.KeyDefault));
            k.Add(new KeywordPair("delete", Token.Kind.KeyDelete));
            k.Add(new KeywordPair("double", Token.Kind.KeyFloat64));
            k.Add(new KeywordPair("do", Token.Kind.KeyDo));

            k.Add(new KeywordPair("else", Token.Kind.KeyElse));
            k.Add(new KeywordPair("enum", Token.Kind.KeyEnum));

            k.Add(new KeywordPair("false", Token.Kind.KeyFalse));
            k.Add(new KeywordPair("finaly", Token.Kind.KeyFinaly));
            k.Add(new KeywordPair("float32", Token.Kind.KeyFloat32));
            k.Add(new KeywordPair("float64", Token.Kind.KeyFloat64));
            k.Add(new KeywordPair("float", Token.Kind.KeyFloat32));
            k.Add(new KeywordPair("foreach_reverse", Token.Kind.KeyForeachReverse));
            k.Add(new KeywordPair("foreach", Token.Kind.KeyForeach));
            k.Add(new KeywordPair("for", Token.Kind.KeyFor));
            k.Add(new KeywordPair("f32", Token.Kind.KeyFloat32));
            k.Add(new KeywordPair("f64", Token.Kind.KeyFloat64));

            k.Add(new KeywordPair("if", Token.Kind.KeyIf));
            k.Add(new KeywordPair("import", Token.Kind.KeyImport));
            k.Add(new KeywordPair("immutable", Token.Kind.KeyImmutable));
            k.Add(new KeywordPair("inout", Token.Kind.KeyInOut));
            k.Add(new KeywordPair("interface", Token.Kind.KeyInterface));
            k.Add(new KeywordPair("int", Token.Kind.KeySInt32));
            k.Add(new KeywordPair("invariant", Token.Kind.KeyInvariant));
            k.Add(new KeywordPair("in", Token.Kind.KeyIn));
            k.Add(new KeywordPair("is", Token.Kind.KeyIs));

            k.Add(new KeywordPair("long", Token.Kind.KeySInt64));

            k.Add(new KeywordPair("module", Token.Kind.KeyModule));

            k.Add(new KeywordPair("new", Token.Kind.KeyNew));
            k.Add(new KeywordPair("null", Token.Kind.KeyNull));

            k.Add(new KeywordPair("object", Token.Kind.KeyObject));
            k.Add(new KeywordPair("out", Token.Kind.KeyOut));
            k.Add(new KeywordPair("override", Token.Kind.KeyOverride));

            k.Add(new KeywordPair("pod", Token.Kind.KeyPod));
            k.Add(new KeywordPair("private", Token.Kind.KeyPrivate));
            k.Add(new KeywordPair("prototype", Token.Kind.KeyPrototype));
            k.Add(new KeywordPair("public", Token.Kind.KeyPublic));

            k.Add(new KeywordPair("readonly", Token.Kind.KeyReadonly));
            k.Add(new KeywordPair("ref", Token.Kind.KeyRef));
            k.Add(new KeywordPair("return", Token.Kind.KeyReturn));

            k.Add(new KeywordPair("sbyte", Token.Kind.KeySInt8));
            k.Add(new KeywordPair("scope", Token.Kind.KeyScope));
            k.Add(new KeywordPair("short", Token.Kind.KeySInt16));
            k.Add(new KeywordPair("sint16", Token.Kind.KeySInt16));
            k.Add(new KeywordPair("sint32", Token.Kind.KeySInt32));
            k.Add(new KeywordPair("sint64", Token.Kind.KeySInt64));
            k.Add(new KeywordPair("sint8", Token.Kind.KeySInt8));
            k.Add(new KeywordPair("sint", Token.Kind.KeySInt32));
            k.Add(new KeywordPair("slong", Token.Kind.KeySInt64));
            k.Add(new KeywordPair("static", Token.Kind.KeyStatic));
            k.Add(new KeywordPair("struct", Token.Kind.KeyStruct));
            k.Add(new KeywordPair("sshort", Token.Kind.KeySInt16));
            k.Add(new KeywordPair("switch", Token.Kind.KeySwitch));
            k.Add(new KeywordPair("s16", Token.Kind.KeySInt16));
            k.Add(new KeywordPair("s32", Token.Kind.KeySInt32));
            k.Add(new KeywordPair("s64", Token.Kind.KeySInt64));
            k.Add(new KeywordPair("s8", Token.Kind.KeySInt8));

            k.Add(new KeywordPair("this", Token.Kind.KeyThis));
            k.Add(new KeywordPair("throw", Token.Kind.KeyThrow));
            k.Add(new KeywordPair("true", Token.Kind.KeyTrue));
            k.Add(new KeywordPair("try", Token.Kind.KeyTry));
            k.Add(new KeywordPair("typedef", Token.Kind.KeyTypedef));

            k.Add(new KeywordPair("ubyte", Token.Kind.KeyUInt8));
            k.Add(new KeywordPair("uint16", Token.Kind.KeyUInt16));
            k.Add(new KeywordPair("uint32", Token.Kind.KeyUInt32));
            k.Add(new KeywordPair("uint64", Token.Kind.KeyUInt64));
            k.Add(new KeywordPair("uint8", Token.Kind.KeyUInt8));
            k.Add(new KeywordPair("uint", Token.Kind.KeyUInt32));
            k.Add(new KeywordPair("ulong", Token.Kind.KeyUInt64));
            k.Add(new KeywordPair("ushort", Token.Kind.KeyUInt16));
            k.Add(new KeywordPair("u16", Token.Kind.KeyUInt16));
            k.Add(new KeywordPair("u32", Token.Kind.KeyUInt32));
            k.Add(new KeywordPair("u64", Token.Kind.KeyUInt64));
            k.Add(new KeywordPair("u8", Token.Kind.KeyUInt8));
            k.Add(new KeywordPair("using", Token.Kind.KeyUsing));
            k.Add(new KeywordPair("utility", Token.Kind.KeyUtility));

            k.Add(new KeywordPair("void", Token.Kind.KeyVoid));

            k.Add(new KeywordPair("wchar", Token.Kind.KeyWChar));
            k.Add(new KeywordPair("while", Token.Kind.KeyWhile));
        }
        //------------------------------------------------------------
        // UnicodeRangeリストを作成する関数。
        static void createUnicodeRangeList()
        {
            sUnicodeRangeList = new List<UnicodeRange>();
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00AA, (char)0x00AA));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00B5, (char)0x00B5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00B7, (char)0x00B7));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00BA, (char)0x00BA));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00C0, (char)0x00D6));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00D8, (char)0x00F6));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x00F8, (char)0x01F5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x01FA, (char)0x0217));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0250, (char)0x02A8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x02B0, (char)0x02B8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x02BB, (char)0x02BB));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x02BD, (char)0x02C1));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x02D0, (char)0x02D1));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x02E0, (char)0x02E4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x037A, (char)0x037A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0386, (char)0x0386));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0388, (char)0x038A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x038C, (char)0x038C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x038E, (char)0x03A1));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03A3, (char)0x03CE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03D0, (char)0x03D6));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03DA, (char)0x03DA));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03DC, (char)0x03DC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03DE, (char)0x03DE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03E0, (char)0x03E0));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x03E2, (char)0x03F3));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0401, (char)0x040C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x040E, (char)0x044F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0451, (char)0x045C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x045E, (char)0x0481));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0490, (char)0x04C4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x04C7, (char)0x04C8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x04CB, (char)0x04CC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x04D0, (char)0x04EB));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x04EE, (char)0x04F5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x04F8, (char)0x04F9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0531, (char)0x0556));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0559, (char)0x0559));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0561, (char)0x0587));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x05B0, (char)0x05B9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x05BB, (char)0x05BD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x05BF, (char)0x05BF));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x05C1, (char)0x05C2));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x05D0, (char)0x05EA));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x05F0, (char)0x05F2));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0621, (char)0x063A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0640, (char)0x0652));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0660, (char)0x0669));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0670, (char)0x06B7));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x06BA, (char)0x06BE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x06C0, (char)0x06CE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x06D0, (char)0x06DC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x06E5, (char)0x06E8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x06EA, (char)0x06ED));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x06F0, (char)0x06F9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0901, (char)0x0903));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0905, (char)0x0939));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x093D, (char)0x093D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x093E, (char)0x094D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0950, (char)0x0952));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0958, (char)0x0963));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0966, (char)0x096F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0981, (char)0x0983));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0985, (char)0x098C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x098F, (char)0x0990));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0993, (char)0x09A8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09AA, (char)0x09B0));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09B2, (char)0x09B2));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09B6, (char)0x09B9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09BE, (char)0x09C4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09C7, (char)0x09C8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09CB, (char)0x09CD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09DC, (char)0x09DD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09DF, (char)0x09E3));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09E6, (char)0x09EF));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x09F0, (char)0x09F1));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A02, (char)0x0A02));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A05, (char)0x0A0A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A0F, (char)0x0A10));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A13, (char)0x0A28));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A2A, (char)0x0A30));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A32, (char)0x0A33));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A35, (char)0x0A36));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A38, (char)0x0A39));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A3E, (char)0x0A42));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A47, (char)0x0A48));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A4B, (char)0x0A4D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A59, (char)0x0A5C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A5E, (char)0x0A5E));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A66, (char)0x0A6F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A74, (char)0x0A74));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A81, (char)0x0A83));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A85, (char)0x0A8B));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A8D, (char)0x0A8D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A8F, (char)0x0A91));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0A93, (char)0x0AA8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AAA, (char)0x0AB0));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AB2, (char)0x0AB3));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AB5, (char)0x0AB9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0ABD, (char)0x0AC5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AC7, (char)0x0AC9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0ACB, (char)0x0ACD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AD0, (char)0x0AD0));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AE0, (char)0x0AE0));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0AE6, (char)0x0AEF));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B01, (char)0x0B03));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B05, (char)0x0B0C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B0F, (char)0x0B10));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B13, (char)0x0B28));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B2A, (char)0x0B30));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B32, (char)0x0B33));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B36, (char)0x0B39));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B3D, (char)0x0B3D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B3E, (char)0x0B43));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B47, (char)0x0B48));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B4B, (char)0x0B4D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B5C, (char)0x0B5D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B5F, (char)0x0B61));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B66, (char)0x0B6F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B82, (char)0x0B83));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B85, (char)0x0B8A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B8E, (char)0x0B90));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B92, (char)0x0B95));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B99, (char)0x0B9A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B9C, (char)0x0B9C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0B9E, (char)0x0B9F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BA3, (char)0x0BA4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BA8, (char)0x0BAA));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BAE, (char)0x0BB5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BB7, (char)0x0BB9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BBE, (char)0x0BC2));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BC6, (char)0x0BC8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BCA, (char)0x0BCD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0BE7, (char)0x0BEF));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C01, (char)0x0C03));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C05, (char)0x0C0C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C0E, (char)0x0C10));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C12, (char)0x0C28));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C2A, (char)0x0C33));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C35, (char)0x0C39));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C3E, (char)0x0C44));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C46, (char)0x0C48));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C4A, (char)0x0C4D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C60, (char)0x0C61));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C66, (char)0x0C6F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C82, (char)0x0C83));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C85, (char)0x0C8C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C8E, (char)0x0C90));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0C92, (char)0x0CA8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CAA, (char)0x0CB3));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CB5, (char)0x0CB9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CBE, (char)0x0CC4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CC6, (char)0x0CC8));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CCA, (char)0x0CCD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CDE, (char)0x0CDE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CE0, (char)0x0CE1));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0CE6, (char)0x0CEF));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D02, (char)0x0D03));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D05, (char)0x0D0C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D0E, (char)0x0D10));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D12, (char)0x0D28));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D2A, (char)0x0D39));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D3E, (char)0x0D43));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D46, (char)0x0D48));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D4A, (char)0x0D4D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D60, (char)0x0D61));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0D66, (char)0x0D6F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E01, (char)0x0E3A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E40, (char)0x0E5B));
            //unicodeRangeList.Add(new UnicodeRange( (char)0x0E50, (char)0x0E59 ));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E81, (char)0x0E82));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E84, (char)0x0E84));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E87, (char)0x0E88));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E8A, (char)0x0E8A));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E8D, (char)0x0E8D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E94, (char)0x0E97));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0E99, (char)0x0E9F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EA1, (char)0x0EA3));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EA5, (char)0x0EA5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EA7, (char)0x0EA7));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EAA, (char)0x0EAB));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EAD, (char)0x0EAE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EB0, (char)0x0EB9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EBB, (char)0x0EBD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EC0, (char)0x0EC4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EC6, (char)0x0EC6));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EC8, (char)0x0ECD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0ED0, (char)0x0ED9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0EDC, (char)0x0EDD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F00, (char)0x0F00));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F18, (char)0x0F19));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F20, (char)0x0F33));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F35, (char)0x0F35));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F37, (char)0x0F37));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F39, (char)0x0F39));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F3E, (char)0x0F47));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F49, (char)0x0F69));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F71, (char)0x0F84));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F86, (char)0x0F8B));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F90, (char)0x0F95));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F97, (char)0x0F97));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0F99, (char)0x0FAD));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0FB1, (char)0x0FB7));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x0FB9, (char)0x0FB9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x10A0, (char)0x10C5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x10D0, (char)0x10F6));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1E00, (char)0x1E9B));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1EA0, (char)0x1EF9));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F00, (char)0x1F15));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F18, (char)0x1F1D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F20, (char)0x1F45));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F48, (char)0x1F4D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F50, (char)0x1F57));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F59, (char)0x1F59));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F5B, (char)0x1F5B));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F5D, (char)0x1F5D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F5F, (char)0x1F7D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1F80, (char)0x1FB4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FB6, (char)0x1FBC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FBE, (char)0x1FBE));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FC2, (char)0x1FC4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FC6, (char)0x1FCC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FD0, (char)0x1FD3));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FD6, (char)0x1FDB));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FE0, (char)0x1FEC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FF2, (char)0x1FF4));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x1FF6, (char)0x1FFC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x203F, (char)0x2040));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x207F, (char)0x207F));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2102, (char)0x2102));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2107, (char)0x2107));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x210A, (char)0x2113));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2115, (char)0x2115));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2118, (char)0x211D));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2124, (char)0x2124));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2126, (char)0x2126));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2128, (char)0x2128));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x212A, (char)0x2131));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2133, (char)0x2138));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x2160, (char)0x2182));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x3005, (char)0x3007));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x3021, (char)0x3029));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x3041, (char)0x3093));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x309B, (char)0x309C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x30A1, (char)0x30F6));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x30FB, (char)0x30FC));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x3105, (char)0x312C));
            sUnicodeRangeList.Add(new UnicodeRange((char)0x4E00, (char)0x9FA5));
            sUnicodeRangeList.Add(new UnicodeRange((char)0xAC00, (char)0xD7A3));
        }

        //------------------------------------------------------------
        // 文字を取得する。範囲外ならNULを返す。
        char getChar(int p)
        {
            if (mEnd <= p)
            {
                return CHAR_NUL;
            }
            return mText[p];
        }

        //------------------------------------------------------------
        // テキストの位置を進める関数。
        void advancePos()
        {
            ++mPos;
            ++mColumnNum;
        }

        //------------------------------------------------------------
        // 改行する関数。
        void enterLine()
        {
            ++mLineNum;
            mColumnNum = 0;
        }

        //------------------------------------------------------------
        // １つのTokenを解析する
        void scan(Token t)
        {
            while ( true )
            {
                // トークンの位置を設定
                t.pos = mPos;
                t.posLine = mLineNum;
                t.posColumn = mColumnNum;

                switch (getChar(t.pos))
                {
                    // end of file
                    case CHAR_NUL:
                    case CHAR_SUB:
                        t.Value= Token.Kind.EOF;
                        return;

                    // white space
                    case ' ':
                    case '\t':
                    case '\v':
                    case '\f':
                        advancePos();
                        continue;

                    // end of line
                    case '\r':
                        advancePos();
                        if ( !isLineEnterChar( getChar(mPos) ) )
                        {
                            enterLine();
                        }
                        continue;
                    case '\n':
                    case CHAR_LS:
                    case CHAR_PS:
                        advancePos();
                        enterLine();
                        continue;

                    // number
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        t.Value= number(t);
                        return;

                    // string or char constant
                    case '\'':
                        t.Value= charConstant(t);
                        return;
                    case 'r':
                        if (getChar(mPos + 1) != '"')
                        {// identifierっぽい
                            scanIdentifier(t);
                        }
                        else
                        {
                            advancePos();
                            advancePos();
                            t.Value = wysiwygStringConstant(t);
                        }
                        return;
                    case '"':
                        advancePos();
                        t.Value= escapeStringConstant(t);
                        return;

                    // ident
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':  	
                    case 'q':
                    /*case 'r':*/
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w': 
                    case 'x':
                    case 'y':
                    case 'z':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        {
                            scanIdentifier(t);
                            return;
                        }

                    case '/':
                        if (scanDiv(t))
                        {
                            return;
                        }
                        continue;

                    case '&':
                        t.Value = scanOperatorSingleDoubleSAssign('&', Token.Kind.OpAnd, Token.Kind.OpAndAnd, Token.Kind.OpAndAssign);
                        return;

                    case '|':
                        t.Value = scanOperatorSingleDoubleSAssign('|', Token.Kind.OpOr, Token.Kind.OpOrOr, Token.Kind.OpOrAssign);
                        return;

                    case '-':
                        t.Value = scanOperatorSingleDoubleSAssign('-', Token.Kind.OpMinus, Token.Kind.OpMinusMinus, Token.Kind.OpMinusAssign);
                        return;

                    case '+':
                        t.Value = scanOperatorSingleDoubleSAssign('+', Token.Kind.OpPlus, Token.Kind.OpPlusPlus, Token.Kind.OpPlusAssign);
                        return;

                    case '<':
                        t.Value = scanOperatorSingleSAssignDoubleDAssign('<', Token.Kind.OpLess, Token.Kind.OpLessEqual, Token.Kind.OpLShift, Token.Kind.OpLShiftAssign);
                        return;

                    case '>':
                        t.Value = scanOperatorSingleSAssignDoubleDAssign('>', Token.Kind.OpGreater, Token.Kind.OpGreaterEqual, Token.Kind.OpRShift, Token.Kind.OpRShiftAssign);
                        return;

                    case '!':
                        t.Value = scanOperatorSingleSAssign(Token.Kind.OpNot, Token.Kind.OpNotEqual);
                        return;

                    case '=':
                        t.Value = scanOperatorSingleSAssign(Token.Kind.OpAssign, Token.Kind.OpEqual);
                        return;

                    case '*':
                        t.Value = scanOperatorSingleSAssign(Token.Kind.OpMul, Token.Kind.OpMulAssign);
                        return;

                    case '%':
                        t.Value = scanOperatorSingleSAssign(Token.Kind.OpMod, Token.Kind.OpModAssign);
                        return;

                    case '^':
                        t.Value = scanOperatorSingleSAssign(Token.Kind.OpXor, Token.Kind.OpXorAssign);
                        return;

                    case '(':
                        advancePos();
                        t.Value = Token.Kind.OpLParen;
                        return;

                    case ')':
                        advancePos();
                        t.Value = Token.Kind.OpRParen;
                        return;

                    case '[':
                        advancePos();
                        t.Value = Token.Kind.OpLBracket;
                        return;

                    case ']':
                        advancePos();
                        t.Value = Token.Kind.OpRBracket;
                        return;

                    case '{':
                        advancePos();
                        t.Value = Token.Kind.OpLCurly;
                        return;

                    case '}':
                        advancePos();
                        t.Value = Token.Kind.OpRCurly;
                        return;

                    case '?':
                        advancePos();
                        t.Value = Token.Kind.OpQuestion;
                        return;

                    case ',':
                        advancePos();
                        t.Value = Token.Kind.OpComma;
                        return;

                    case '.':
                        advancePos();
                        t.Value = Token.Kind.OpDot;
                        return;

                    case ';':
                        advancePos();
                        t.Value = Token.Kind.OpSemicolon;
                        return;

                    case ':':
                        advancePos();
                        t.Value = Token.Kind.OpColon;
                        return;

                    case '~':
                        advancePos();
                        t.Value = Token.Kind.OpTilde;
                        return;

                    case '@':
                        advancePos();
                        t.Value = Token.Kind.OpAt;
                        return;

                    default:
                        if (isIDChar(getChar(mPos)))
                        {
                            scanIdentifier(t);
                            return;
                        }
                        error(t,ErrorKind.UnsupportedChar);
                        t.Value = Token.Kind.EOF;
                        return;
                }
            }
        }

        //------------------------------------------------------------
        // オペラータ解析関数。&,&&,&=のパターン用。
        // 0文字目にposがある状態で呼ぶこと。
        Token.Kind scanOperatorSingleDoubleSAssign(char aOpChar,Token.Kind aOpSingle, Token.Kind aOpDouble, Token.Kind aOpAssign)
        {
            advancePos();
            if (getChar(mPos) == '=')
            {
                advancePos();
                return aOpAssign;
            }
            else if (getChar(mPos) == aOpChar)
            {
                advancePos();
                return aOpDouble;
            }
            else
            {
                return aOpSingle;
            }
        }

        //------------------------------------------------------------
        // オペレータ解析関数。<,<=,<<,<<=用。
        // 0文字目にposがある状態で呼ぶこと。
        Token.Kind scanOperatorSingleSAssignDoubleDAssign(char aOpChar, Token.Kind aOpSingle, Token.Kind aOpSAssign, Token.Kind aOpDouble, Token.Kind aOpDAssign)
        {
            advancePos();
            if (getChar(mPos) == aOpChar)
            {
                advancePos();
                if (getChar(mPos) == '=')
                {
                    advancePos();
                    return aOpDAssign;
                }
                else
                {
                    return aOpDouble;
                }
            }
            else if (getChar(mPos) == '=')
            {
                advancePos();
                return aOpSAssign;
            }
            else
            {
                return aOpSingle;
            }
        }

        //------------------------------------------------------------
        // オペレータ解析関数。!,!=用。
        // 0文字目にposがある状態で呼ぶこと。
        Token.Kind scanOperatorSingleSAssign(Token.Kind aOpSingle, Token.Kind aOpSAssign)
        {
            advancePos();
            if (getChar(mPos) == '=')
            {
                advancePos();
                return aOpSAssign;
            }
            return aOpSingle;
        }

        //------------------------------------------------------------
        // エラー時に呼ぶべき関数。
        void error(Token aT,ErrorKind aErrorKind)
        {
            aT.Value = Token.Kind.EOF;
            mPos = aT.pos;
            mLineNum = aT.posLine;
            mColumnNum = aT.posColumn;
            mErrorKind = aErrorKind;
        }

        //------------------------------------------------------------
        // 数値の解析。
        Token.Kind number(Token aT)
        {
            // とりあえず今は10進数しか対応しない

            UInt64 result = 0;
            UInt64 rate = 1;
            while(true)
            {
                char charNum = getChar( mPos );
                if ( charNum < '0' || '9' < charNum )
                {
                    break;
                }
                result += rate * (ulong)(charNum - '0');
                rate *= 10;
                advancePos();
            }
            aT.UInt64Value = result;

            return Token.Kind.NumUInt64;
        }

        //------------------------------------------------------------
        // シングルコーテーションで囲まれているCharConstantを解析。
        // 位置は最初のシングルコーテーションに合っている必要あり。
        Token.Kind charConstant(Token aT)
        {
            Token.Kind tk = Token.Kind.NumChar8;
            advancePos();
            char c = getChar(mPos);
            advancePos();
            switch (c)
            {
                case '\\':
                    c = escapeSequence(aT);
                    if (IsError())
                    {
                        return Token.Kind.EOF;
                    }
                    break;

                case '\n':
                case '\r':
                case CHAR_LS:
                case CHAR_PS:
                case CHAR_NUL:
                case CHAR_SUB:
                    error( aT,ErrorKind.UntermCharConstant );
                    return Token.Kind.EOF;

                default:
                    break;
            }
            aT.UInt64Value = c;

            if (getChar(mPos) != '\'')
            {
                error(aT,ErrorKind.UntermCharConstant);
                return Token.Kind.EOF;
            }
            advancePos();
            return tk;
        }

        //------------------------------------------------------------
        // エスケープシーケンス文字の解析。posは\の次の文字に進めてある必要がある。
        char escapeSequence(Token aT)
        {
            char c = getChar(mPos);
            switch (c)
            {
                case '\'':
                case '"':
                case '?':
                case '\\':
                    break;

                case 'a':
                    c = (char)0x07;
                    break;

                case 'b':
                    c = (char)0x08;
                    break;

                case 'f':
                    c = (char)0x0C;
                    break;

                case 'n':
                    c = (char)0x0A;
                    break;

                case 'r':
                    c = (char)0x0D;
                    break;

                case 't':
                    c = (char)0x09;
                    break;

                case 'v':
                    c = (char)0x0B;
                    break;

                case CHAR_NUL:
                case CHAR_SUB:
                    c = CHAR_NUL;
                    break;

                default:
                    error(aT,ErrorKind.UndefinedEscapeSequence);
                    return CHAR_NUL;
            }
            advancePos();
            return c;
        }

        //------------------------------------------------------------
        // エスケープシーケンスを展開しない文字列解釈。
        // 位置は"の後ろになっている必要がある。
        Token.Kind wysiwygStringConstant(Token aT)
        {
            return stringConstant(aT, false);
        }

        //------------------------------------------------------------
        // エスケープシーケンスを展開する文字列解釈。
        // 位置は"の後ろになっている必要がある。
        Token.Kind escapeStringConstant(Token aT)
        {
            return stringConstant(aT, true);
        }

        //------------------------------------------------------------
        // 文字列解釈
        // 位置は"の後ろになっている必要がある。
        Token.Kind stringConstant(Token aT, bool aDoEscape)
        {
            string str = "";
            while (true)
            {
                char c = getChar(mPos);
                advancePos();
                switch (c)
                {
                    case '\\':
                        if (aDoEscape)
                        {
                            c = escapeSequence(aT);
                        }
                        break;

                    case '\n':
                    case '\r':
                    case CHAR_PS:
                    case CHAR_LS:
                    case CHAR_NUL:
                    case CHAR_SUB:
                        error(aT, ErrorKind.UntermStringConstant);
                        return Token.Kind.EOF;

                    default:
                        break;
                }
                if (c == '"')
                {
                    break;
                }
                str += c;
            }
            aT.UString = str;
            {
                char c = getChar(mPos);
                if (c == 'c')
                {
                    advancePos();
                    return Token.Kind.StringChar8;
                }
                else if (c == 'w')
                {
                    advancePos();
                    return Token.Kind.StringChar16;
                }
                else if (c == 'd')
                {
                    advancePos();
                    return Token.Kind.StringChar32;
                }
                else
                {
                    return Token.Kind.StringChar8;
                }
            }
        }

        //------------------------------------------------------------
        // identifierの解釈。
        void scanIdentifier(Token aT)
        {
            char c;
            do
            {
                advancePos();
                c = getChar(mPos);
            }
            while (isIDChar(c));
            aT.Value = Token.Kind.Identifier;
            aT.Identifier = mText.Substring(aT.pos, mPos - aT.pos);
            scanKeyword(aT); // キーワード検索にトライ
            return;
        }

        //------------------------------------------------------------
        // keywordの解釈
        void scanKeyword(Token aT)
        {
            foreach (KeywordPair pair in sKeywordPairList)
            {
                if (aT.Identifier == pair.Str)
                {
                    aT.Value = pair.TokenKind;
                    return;
                }
            }
        }

        //------------------------------------------------------------
        // '/' から始まるscan。Tokenが特定されたらtrueが返る。
        bool scanDiv(Token aT)
        {
            advancePos();
            switch( getChar( mPos ) )
            {
                case '=':
                    advancePos();
                    aT.Value= Token.Kind.OpDivAssign;
                    return true;

                case '*':
                    advancePos();
                    while(true)
                    {// '/*' section
                        while(true)
                        {
                            char c = getChar(mPos);
                            switch( c )
                            {
                                case '/':
                                    break;

                                case CHAR_PS:
                                case CHAR_LS:
                                case '\n':
                                    advancePos();
                                    enterLine();
                                    continue;

                                case '\r':
                                    advancePos();
                                    if ( !isLineEnterChar(getChar(mPos)) )
                                    {
                                        enterLine();
                                    }
                                    continue;

                                case CHAR_NUL:
                                case CHAR_SUB:
                                    error( aT, ErrorKind.UntermBlockComment );
                                    mPos = mEnd;
                                    aT.Value= Token.Kind.EOF;
                                    return true;

                                default :
                                    advancePos();
                                    continue;
                            }
                            break;
                        }
                        advancePos();
                        if ( getChar(mPos-2) == '*' && mPos-3 != aT.pos )
                        {
                            aT.Value = Token.Kind.Comment;
                            return true;
                        }
                        continue;
                    }

                case '/': // '//' section
                    while(true)
                    {
                        advancePos();
                        char c = getChar(mPos);
                        switch(c)
                        {
                            case CHAR_PS:
                            case CHAR_LS:
                            case '\n':
                                break;

                            case '\r':
                                if ( isLineEnterChar(getChar(mPos+1)) )
                                {
                                    advancePos();
                                }
                                break;

                            case CHAR_NUL:
                            case CHAR_SUB:
                                aT.Value= Token.Kind.Comment;
                                return true;

                            default:
                                continue;
                        }
                        break;
                    }
                    advancePos();
                    enterLine();
                    aT.Value= Token.Kind.Comment;
                    return true;

                case '+':
                    {// '/+' section
                        int nest = 1;
                        advancePos();
                        while(true)
                        {
                            char c = getChar(mPos);
                            switch(c)
                            {
                                case '/':
                                    advancePos();
                                    if ( getChar(mPos) =='*')
                                    {
                                        advancePos();
                                        ++nest;
                                    }
                                    continue;

                                case '+':
                                    advancePos();
                                    if ( getChar(mPos) == '/')
                                    {
                                        advancePos();
                                        --nest;
                                        if ( nest == 0 )
                                        {
                                            break;
                                        }
                                    }
                                    continue;

                                case '\r':
                                    advancePos();
                                    if ( !isLineEnterChar(getChar(mPos)) )
                                    {
                                        enterLine();
                                    }
                                    continue;

                                case CHAR_PS:
                                case CHAR_LS:
                                case '\n':
                                    advancePos();
                                    enterLine();
                                    continue;

                                case CHAR_NUL:
                                case CHAR_SUB:
                                    error( aT , ErrorKind.UntermNestComment );
                                    aT.Value= Token.Kind.EOF;
                                    return true;

                                default:
                                    advancePos();
                                    continue;
                            }
                            break;
                        }
                    }
                    aT.Value= Token.Kind.Comment;
                    return true;
                
                default:
                    aT.Value= Token.Kind.OpDiv;
                    return true;
            }
        }
    }
}
