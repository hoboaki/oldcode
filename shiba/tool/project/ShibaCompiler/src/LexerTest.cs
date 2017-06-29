using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Lexerのテスト。
    /// </summary>
    class LexerTest
    {
        //------------------------------------------------------------
        // １つめのトークンをテストするテストのレシピ。
        class FirstTokenTestRecipe
        {
            public string Code;
            public Token.Kind TokenKind;

            //------------------------------------------------------------
            // コンストラクタ。
            public FirstTokenTestRecipe(string aCode, Token.Kind aTokenKind)
            {
                Code = aCode;
                TokenKind = aTokenKind;
            }
        };

        //------------------------------------------------------------
        // エラーがおきることをテストするレシピ。
        class ErrorTestRecipe
        {
            public string Code;
            public Lexer.ErrorKind ErrorKind;

            //------------------------------------------------------------
            // コンストラクタ。
            public ErrorTestRecipe(string aCode, Lexer.ErrorKind aErrorKind)
            {
                Code = aCode;
                ErrorKind = aErrorKind;
            }
        };

        //------------------------------------------------------------
        // テスト実行。
        static public void Execute()
        {
            Lexer lex;
            {// 最初に希望のTokenがくるだろうテスト
                List<FirstTokenTestRecipe> testCaseList = new List<FirstTokenTestRecipe>();
                // 適当に
                testCaseList.Add(new FirstTokenTestRecipe("void function();", Token.Kind.KeyVoid));
                testCaseList.Add(new FirstTokenTestRecipe("  in", Token.Kind.KeyIn));
                //testCaseList.Add(new FirstTokenTestRecipe(" //", Token.Kind.COMMENT));
                //testCaseList.Add(new FirstTokenTestRecipe("/**/", Token.Kind.COMMENT));
                //testCaseList.Add(new FirstTokenTestRecipe("/+ /+ +/ int +/", Token.Kind.COMMENT));
                testCaseList.Add(new FirstTokenTestRecipe("\"hoge\"", Token.Kind.StringChar8));
                testCaseList.Add(new FirstTokenTestRecipe("r\"hoge\"", Token.Kind.StringChar8));
                testCaseList.Add(new FirstTokenTestRecipe("\"hoge\"c", Token.Kind.StringChar8));
                testCaseList.Add(new FirstTokenTestRecipe("\"hoge\"w", Token.Kind.StringChar16));
                testCaseList.Add(new FirstTokenTestRecipe("\"hoge\"d", Token.Kind.StringChar32));
                testCaseList.Add(new FirstTokenTestRecipe("こんにちは", Token.Kind.Identifier));
                testCaseList.Add(new FirstTokenTestRecipe("inoutda", Token.Kind.Identifier));
                // 全網羅
                testCaseList.Add(new FirstTokenTestRecipe("/", Token.Kind.OpDiv));
                testCaseList.Add(new FirstTokenTestRecipe("/=", Token.Kind.OpDivAssign));
                testCaseList.Add(new FirstTokenTestRecipe(".", Token.Kind.OpDot));
                testCaseList.Add(new FirstTokenTestRecipe("&", Token.Kind.OpAnd));
                testCaseList.Add(new FirstTokenTestRecipe("&=", Token.Kind.OpAndAssign));
                testCaseList.Add(new FirstTokenTestRecipe("&&", Token.Kind.OpAndAnd));
                testCaseList.Add(new FirstTokenTestRecipe("|", Token.Kind.OpOr));
                testCaseList.Add(new FirstTokenTestRecipe("|=", Token.Kind.OpOrAssign));
                testCaseList.Add(new FirstTokenTestRecipe("||", Token.Kind.OpOrOr));
                testCaseList.Add(new FirstTokenTestRecipe("-", Token.Kind.OpMinus));
                testCaseList.Add(new FirstTokenTestRecipe("-=", Token.Kind.OpMinusAssign));
                testCaseList.Add(new FirstTokenTestRecipe("--", Token.Kind.OpMinusMinus));
                testCaseList.Add(new FirstTokenTestRecipe("+", Token.Kind.OpPlus));
                testCaseList.Add(new FirstTokenTestRecipe("+=", Token.Kind.OpPlusAssign));
                testCaseList.Add(new FirstTokenTestRecipe("++", Token.Kind.OpPlusPlus));
                testCaseList.Add(new FirstTokenTestRecipe("<", Token.Kind.OpLess));
                testCaseList.Add(new FirstTokenTestRecipe("<=", Token.Kind.OpLessEqual));
                testCaseList.Add(new FirstTokenTestRecipe("<<", Token.Kind.OpLShift));
                testCaseList.Add(new FirstTokenTestRecipe("<<=", Token.Kind.OpLShiftAssign));
                testCaseList.Add(new FirstTokenTestRecipe(">", Token.Kind.OpGreater));
                testCaseList.Add(new FirstTokenTestRecipe(">=", Token.Kind.OpGreaterEqual));
                testCaseList.Add(new FirstTokenTestRecipe(">>", Token.Kind.OpRShift));
                testCaseList.Add(new FirstTokenTestRecipe(">>=", Token.Kind.OpRShiftAssign));
                testCaseList.Add(new FirstTokenTestRecipe("!", Token.Kind.OpNot));
                testCaseList.Add(new FirstTokenTestRecipe("!=", Token.Kind.OpNotEqual));
                testCaseList.Add(new FirstTokenTestRecipe("(", Token.Kind.OpLParen));
                testCaseList.Add(new FirstTokenTestRecipe(")", Token.Kind.OpRParen));
                testCaseList.Add(new FirstTokenTestRecipe("[", Token.Kind.OpLBracket));
                testCaseList.Add(new FirstTokenTestRecipe("]", Token.Kind.OpRBracket));
                testCaseList.Add(new FirstTokenTestRecipe("{", Token.Kind.OpLCurly));
                testCaseList.Add(new FirstTokenTestRecipe("}", Token.Kind.OpRCurly));
                testCaseList.Add(new FirstTokenTestRecipe("?", Token.Kind.OpQuestion));
                testCaseList.Add(new FirstTokenTestRecipe(",", Token.Kind.OpComma));
                testCaseList.Add(new FirstTokenTestRecipe(";", Token.Kind.OpSemicolon));
                testCaseList.Add(new FirstTokenTestRecipe(":", Token.Kind.OpColon));
                testCaseList.Add(new FirstTokenTestRecipe("=", Token.Kind.OpAssign));
                testCaseList.Add(new FirstTokenTestRecipe("==", Token.Kind.OpEqual));
                testCaseList.Add(new FirstTokenTestRecipe("*", Token.Kind.OpMul));
                testCaseList.Add(new FirstTokenTestRecipe("*=", Token.Kind.OpMulAssign));
                testCaseList.Add(new FirstTokenTestRecipe("%", Token.Kind.OpMod));
                testCaseList.Add(new FirstTokenTestRecipe("%=", Token.Kind.OpModAssign));
                testCaseList.Add(new FirstTokenTestRecipe("^", Token.Kind.OpXor));
                testCaseList.Add(new FirstTokenTestRecipe("^=", Token.Kind.OpXorAssign));
                testCaseList.Add(new FirstTokenTestRecipe("~", Token.Kind.OpTilde));
                testCaseList.Add(new FirstTokenTestRecipe("@", Token.Kind.OpAt));
                testCaseList.Add(new FirstTokenTestRecipe("abstract", Token.Kind.KeyAbstract));
                testCaseList.Add(new FirstTokenTestRecipe("alias", Token.Kind.KeyAlias));
                testCaseList.Add(new FirstTokenTestRecipe("assert", Token.Kind.KeyAssert));
                testCaseList.Add(new FirstTokenTestRecipe("auto", Token.Kind.KeyAuto));
                testCaseList.Add(new FirstTokenTestRecipe("bool", Token.Kind.KeyBool));
                testCaseList.Add(new FirstTokenTestRecipe("break", Token.Kind.KeyBreak));
                testCaseList.Add(new FirstTokenTestRecipe("case", Token.Kind.KeyCase));
                testCaseList.Add(new FirstTokenTestRecipe("catch", Token.Kind.KeyCatch));
                testCaseList.Add(new FirstTokenTestRecipe("char", Token.Kind.KeyChar));
                testCaseList.Add(new FirstTokenTestRecipe("class", Token.Kind.KeyClass));
                testCaseList.Add(new FirstTokenTestRecipe("const", Token.Kind.KeyConst));
                testCaseList.Add(new FirstTokenTestRecipe("continue", Token.Kind.KeyContinue));
                testCaseList.Add(new FirstTokenTestRecipe("dchar", Token.Kind.KeyDChar));
                testCaseList.Add(new FirstTokenTestRecipe("default", Token.Kind.KeyDefault));
                testCaseList.Add(new FirstTokenTestRecipe("delete", Token.Kind.KeyDelete));
                testCaseList.Add(new FirstTokenTestRecipe("do", Token.Kind.KeyDo));
                testCaseList.Add(new FirstTokenTestRecipe("else", Token.Kind.KeyElse));
                testCaseList.Add(new FirstTokenTestRecipe("enum", Token.Kind.KeyEnum));
                testCaseList.Add(new FirstTokenTestRecipe("false", Token.Kind.KeyFalse));
                testCaseList.Add(new FirstTokenTestRecipe("finaly", Token.Kind.KeyFinaly));
                testCaseList.Add(new FirstTokenTestRecipe("float", Token.Kind.KeyFloat32));
                testCaseList.Add(new FirstTokenTestRecipe("float32", Token.Kind.KeyFloat32));
                testCaseList.Add(new FirstTokenTestRecipe("f32", Token.Kind.KeyFloat32));
                testCaseList.Add(new FirstTokenTestRecipe("double", Token.Kind.KeyFloat64));
                testCaseList.Add(new FirstTokenTestRecipe("float64", Token.Kind.KeyFloat64));
                testCaseList.Add(new FirstTokenTestRecipe("f64", Token.Kind.KeyFloat64));
                testCaseList.Add(new FirstTokenTestRecipe("for", Token.Kind.KeyFor));
                testCaseList.Add(new FirstTokenTestRecipe("foreach", Token.Kind.KeyForeach));
                testCaseList.Add(new FirstTokenTestRecipe("foreach_reverse", Token.Kind.KeyForeachReverse));
                testCaseList.Add(new FirstTokenTestRecipe("if", Token.Kind.KeyIf));
                testCaseList.Add(new FirstTokenTestRecipe("import", Token.Kind.KeyImport));
                testCaseList.Add(new FirstTokenTestRecipe("immutable", Token.Kind.KeyImmutable));
                testCaseList.Add(new FirstTokenTestRecipe("in", Token.Kind.KeyIn));
                testCaseList.Add(new FirstTokenTestRecipe("inout", Token.Kind.KeyInOut));
                testCaseList.Add(new FirstTokenTestRecipe("interface", Token.Kind.KeyInterface));
                testCaseList.Add(new FirstTokenTestRecipe("invariant", Token.Kind.KeyInvariant));
                testCaseList.Add(new FirstTokenTestRecipe("is", Token.Kind.KeyIs));
                testCaseList.Add(new FirstTokenTestRecipe("module", Token.Kind.KeyModule));
                testCaseList.Add(new FirstTokenTestRecipe("new", Token.Kind.KeyNew));
                testCaseList.Add(new FirstTokenTestRecipe("null", Token.Kind.KeyNull));
                testCaseList.Add(new FirstTokenTestRecipe("object", Token.Kind.KeyObject));
                testCaseList.Add(new FirstTokenTestRecipe("out", Token.Kind.KeyOut));
                testCaseList.Add(new FirstTokenTestRecipe("override", Token.Kind.KeyOverride));
                testCaseList.Add(new FirstTokenTestRecipe("pod", Token.Kind.KeyPod));
                testCaseList.Add(new FirstTokenTestRecipe("private", Token.Kind.KeyPrivate));
                testCaseList.Add(new FirstTokenTestRecipe("prototype", Token.Kind.KeyPrototype));
                testCaseList.Add(new FirstTokenTestRecipe("public", Token.Kind.KeyPublic));
                testCaseList.Add(new FirstTokenTestRecipe("readonly", Token.Kind.KeyReadonly));
                testCaseList.Add(new FirstTokenTestRecipe("ref", Token.Kind.KeyRef));
                testCaseList.Add(new FirstTokenTestRecipe("return", Token.Kind.KeyReturn));
                testCaseList.Add(new FirstTokenTestRecipe("scope", Token.Kind.KeyScope));
                testCaseList.Add(new FirstTokenTestRecipe("short", Token.Kind.KeySInt16));
                testCaseList.Add(new FirstTokenTestRecipe("sshort", Token.Kind.KeySInt16));
                testCaseList.Add(new FirstTokenTestRecipe("sint16", Token.Kind.KeySInt16));
                testCaseList.Add(new FirstTokenTestRecipe("s16", Token.Kind.KeySInt16));
                testCaseList.Add(new FirstTokenTestRecipe("int", Token.Kind.KeySInt32));
                testCaseList.Add(new FirstTokenTestRecipe("sint", Token.Kind.KeySInt32));
                testCaseList.Add(new FirstTokenTestRecipe("sint32", Token.Kind.KeySInt32));
                testCaseList.Add(new FirstTokenTestRecipe("s32", Token.Kind.KeySInt32));
                testCaseList.Add(new FirstTokenTestRecipe("long", Token.Kind.KeySInt64));
                testCaseList.Add(new FirstTokenTestRecipe("slong", Token.Kind.KeySInt64));
                testCaseList.Add(new FirstTokenTestRecipe("sint64", Token.Kind.KeySInt64));
                testCaseList.Add(new FirstTokenTestRecipe("s64", Token.Kind.KeySInt64));
                testCaseList.Add(new FirstTokenTestRecipe("sbyte", Token.Kind.KeySInt8));
                testCaseList.Add(new FirstTokenTestRecipe("sint8", Token.Kind.KeySInt8));
                testCaseList.Add(new FirstTokenTestRecipe("s8", Token.Kind.KeySInt8));
                testCaseList.Add(new FirstTokenTestRecipe("static", Token.Kind.KeyStatic));
                testCaseList.Add(new FirstTokenTestRecipe("struct", Token.Kind.KeyStruct));
                testCaseList.Add(new FirstTokenTestRecipe("switch", Token.Kind.KeySwitch));
                testCaseList.Add(new FirstTokenTestRecipe("this", Token.Kind.KeyThis));
                testCaseList.Add(new FirstTokenTestRecipe("throw", Token.Kind.KeyThrow));
                testCaseList.Add(new FirstTokenTestRecipe("true", Token.Kind.KeyTrue));
                testCaseList.Add(new FirstTokenTestRecipe("try", Token.Kind.KeyTry));
                testCaseList.Add(new FirstTokenTestRecipe("typedef", Token.Kind.KeyTypedef));
                testCaseList.Add(new FirstTokenTestRecipe("ushort", Token.Kind.KeyUInt16));
                testCaseList.Add(new FirstTokenTestRecipe("uint16", Token.Kind.KeyUInt16));
                testCaseList.Add(new FirstTokenTestRecipe("u16", Token.Kind.KeyUInt16));
                testCaseList.Add(new FirstTokenTestRecipe("uint", Token.Kind.KeyUInt32));
                testCaseList.Add(new FirstTokenTestRecipe("uint32", Token.Kind.KeyUInt32));
                testCaseList.Add(new FirstTokenTestRecipe("u32", Token.Kind.KeyUInt32));
                testCaseList.Add(new FirstTokenTestRecipe("ulong", Token.Kind.KeyUInt64));
                testCaseList.Add(new FirstTokenTestRecipe("uint64", Token.Kind.KeyUInt64));
                testCaseList.Add(new FirstTokenTestRecipe("u64", Token.Kind.KeyUInt64));
                testCaseList.Add(new FirstTokenTestRecipe("byte", Token.Kind.KeyUInt8));
                testCaseList.Add(new FirstTokenTestRecipe("ubyte", Token.Kind.KeyUInt8));
                testCaseList.Add(new FirstTokenTestRecipe("uint8", Token.Kind.KeyUInt8));
                testCaseList.Add(new FirstTokenTestRecipe("u8", Token.Kind.KeyUInt8));
                testCaseList.Add(new FirstTokenTestRecipe("using", Token.Kind.KeyUsing));
                testCaseList.Add(new FirstTokenTestRecipe("utility", Token.Kind.KeyUtility));
                testCaseList.Add(new FirstTokenTestRecipe("void", Token.Kind.KeyVoid));
                testCaseList.Add(new FirstTokenTestRecipe("wchar", Token.Kind.KeyWChar));
                testCaseList.Add(new FirstTokenTestRecipe("while", Token.Kind.KeyWhile));
                // 実行
                foreach (FirstTokenTestRecipe recipe in testCaseList)
                {
                    lex = new Lexer(recipe.Code);
                    Assert.Check(lex.FirstToken().Value == recipe.TokenKind);
                }
            }
            {// エラーパターンテスト
                List<ErrorTestRecipe> testCaseList = new List<ErrorTestRecipe>();
                // 一通り網羅
                testCaseList.Add(new ErrorTestRecipe("/* hoge *", Lexer.ErrorKind.UntermBlockComment));
                testCaseList.Add(new ErrorTestRecipe(" '\\", Lexer.ErrorKind.UntermCharConstant));
                testCaseList.Add(new ErrorTestRecipe(" /+ /* /+ // +/ +", Lexer.ErrorKind.UntermNestComment));
                testCaseList.Add(new ErrorTestRecipe(" \"asdas", Lexer.ErrorKind.UntermStringConstant));
                testCaseList.Add(new ErrorTestRecipe("'\\o'", Lexer.ErrorKind.UndefinedEscapeSequence));
                testCaseList.Add(new ErrorTestRecipe("#", Lexer.ErrorKind.UnsupportedChar));
                // 実行
                foreach (ErrorTestRecipe recipe in testCaseList)
                {
                    lex = new Lexer(recipe.Code);
                    Assert.Check(lex.GetErrorInfo().ErrorKind == recipe.ErrorKind);
                }
            }
        }
    }
}
