using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// SymbolTreeのテストクラス。
    /// </summary>
    class SymbolTreeTest
    {
        //------------------------------------------------------------
        // 実行関数。
        static public void Execute()
        {
            // テストケースを集める
            List< TestRecipe > testCaseList = new List<TestRecipe>();
            testCaseList.Add( testAdd() );

            // テストを実行
            foreach(var entry in testCaseList)
            {
                // シンボル作成
                SymbolTree symbolTree = new SymbolTree();

                // モジュールを追加
                foreach(var code in entry.CodeRepos)
                {
                    // Lexer
                    var lexer = new Lexer(code);
                    if (lexer.IsError())
                    {
                        System.Console.Error.Write(
                              "###########################################################" + System.Environment.NewLine
                            + "# LexerError:" + lexer.GetErrorInfo().ErrorKind.ToString() + System.Environment.NewLine
                            + "#   line: " + lexer.GetErrorInfo().Line + System.Environment.NewLine
                            + "#    col: " + lexer.GetErrorInfo().Column + System.Environment.NewLine
                            + "###########################################################" + System.Environment.NewLine
                            );
                        Assert.NotReachHere();
                    }

                    // Parser
                    var parser = new Parser(lexer);
                    if (parser.GetErrorKind() != Parser.ErrorKind.NONE)
                    {
                        System.Console.Error.Write(
                              "###########################################################" + System.Environment.NewLine
                            + "# ParserError:" + parser.GetErrorKind().ToString() + System.Environment.NewLine
                            + "#   line: " + parser.GetErrorToken().posLine + System.Environment.NewLine
                            + "#    col: " + parser.GetErrorToken().posColumn + System.Environment.NewLine
                            + "###########################################################" + System.Environment.NewLine
                            );
                        Assert.NotReachHere();
                    }
                    
                    // Add
                    if (!symbolTree.Add(parser.ModuleContext))
                    {
                        dumpSymbolTreeErrorInfo(symbolTree);
                    }
                }

                // 展開
                if (!symbolTree.Expand())
                {
                    dumpSymbolTreeErrorInfo(symbolTree);
                }
                Assert.Check(symbolTree.GetErrorInfo().Kind == SymbolTree.ErrorKind.NONE);

                // 各モジュールをダンプ
                symbolTree.XDataDump();
            }
        }
        
        //============================================================

        //------------------------------------------------------------
        // シンボルツリーのエラーを表示する。
        static void dumpSymbolTreeErrorInfo(SymbolTree aSymbolTree)
        {
            System.Console.Error.Write(
              "###########################################################" + System.Environment.NewLine
            + "# SymbolTreeError:" + aSymbolTree.GetErrorInfo().Kind.ToString() + System.Environment.NewLine
            + "#   line: " + aSymbolTree.GetErrorInfo().Token.posLine + System.Environment.NewLine
            + "#    col: " + aSymbolTree.GetErrorInfo().Token.posColumn + System.Environment.NewLine
            + "###########################################################" + System.Environment.NewLine
                            );
            Assert.NotReachHere();
        }

        //------------------------------------------------------------
        // 加算だけのシンプルなテスト。
        static TestRecipe testAdd()
        {
            var recipe = new TestRecipe();

            recipe.CodeRepos.Add(
@"module App.EntryPoint;
utility EntryPoint
{
    static void Arithmetic(const int aA , const int aB)
    {
        int add = aA + aB;
        int sub = aA - aB;
        int mul = aA * aB;
        int div = aA / aB;
        int mod = aA % aB;
    }

    static void BitOp(const int aA , const int aB)
    {
        int and = aA & aB;
        int or = aA | aB;
        int xor = aA ^ aB;
    }

    static void ShiftOp(const int aA , const int aB)
    {
        int shiftL = aA << aB;
        int shiftR = aA >> aB;
    }

    static void AssignExpr(const int aA)
    {
        int val = aA;
        val += aA;
        val -= aA;
        val *= aA;
        val /= aA;
        val %= aA;
        val &= aA;
        val |= aA;
        val ^= aA;
        val <<= aA;
        val >>= aA;
    }

    static void PostfixIncDec()
    {
        int a = 1;
        int inc = a++;
        int dec = a--;
    }

    static void UnaryOp()
    {
        int a = 1;
        int inc = ++a;
        int dec = --a;
        int positive = +a;
        int negative = -a;
        int bitWiseNot = ~a;
        bool logicalNot = !(a == 0);
    }

    static void DeclBoolVar()
    {
        bool boolValue;
        bool boolValueTrue = true;
        bool boolValueTrue = false;
    }

    static void RelationalOp()
    {
        bool lessTest = 1 < 2;
        bool lessEqualTest = 3 <= 4;
        bool greaterTest = 5 > 6;
        bool greaterEqualTest = 7 >= 8;
    }

    static void EqualityOp()
    {
        bool intEqual = 1 == 2;
        bool intNotEqual = 2 != 3;
        bool boolEqual = intEqual == intNotEqual;
        bool boolNotEqual = intNotEqual != intEqual;
    }

    static void LogicalOp( const int aArg )
    {   
        bool logicalAnd = 0 <= aArg && aArg < 10;
        bool logicalOr = 0 < aArg || 10 <= aArg;
    }

    static void While()
    {
        int i = 0;

        // normal
        while ( i < 10 )
        {
            ++i;
        }

        // continue test
        while ( i < 10 )
        {
            i += 1;
            continue;
        }

        // break test
        while ( i < 10 )
        {
            break;
        }
    }

    static void If( const int aA , const int aB )
    {
        int result;
        if ( aA == aB )
        {
            result = 0;
        }
        else if ( aA < aB )
        {
            result = -1;
        }
        else if ( aB < aA )
        {
            result = 1;
        }
    }

    static void DoWhile( const int aLoopCount )
    {
        int i = 1;
        do
        {
            i += i;
            if ( i == 5 )
            {// break test
                break;
            }
            if ( i == 6 )
            {// continue test
                continue;
            }
        }
        while( i < aLoopCount );
    }
 
    static void For()
    {
        for ( int i = 0; i < 10; i++ )
        {
            for ( int k = 0; k < 10; ++k )
            {// break test
                break;
            }

            // continue test
            continue;
        }
    }

    static void ReturnNoType()
    {
        return;
    }
    
    static bool ReturnBool( const int aArg )
    {
        if ( ( aArg % 2 ) == 0 )
        {
            return true;
        }
        return false;
    }

    static void FuncVoidNoArg()
    {
    }

    static void CallFuncVoidNoArg()
    {
        FuncVoidNoArg();
    }

    static int FuncIntNoArg()
    {
        return 1;
    }

    static void CallFuncIntNoArg()
    {
        int result = FuncIntNoArg();
    }

    static int FuncIntWithArg( const int aArg )
    {
        return aArg + 1;
    }

    static void CallFuncIntWithArg()
    {
        int result = FuncIntWithArg( 1 );
    }
};
"
            );
            return recipe;
        }

        //------------------------------------------------------------
        // １つのテストを示すレシピ。
        class TestRecipe
        {
            public readonly List< string > CodeRepos;

            //------------------------------------------------------------
            // コンストラクタ。
            public TestRecipe()
            {
                CodeRepos = new List<string>();
            }
        };
    }
}
