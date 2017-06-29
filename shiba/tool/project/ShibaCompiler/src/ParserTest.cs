using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Parserのテスト。
    /// </summary>
    class ParserTest
    {
        //------------------------------------------------------------
        // 実行関数。
        static public void Execute()
        {
            List<TestRecipe> recipes = new List<TestRecipe>();
            //------------------------------------------------------------
            // 正しいmodule。
            recipes.Add(new TestRecipe(
@"module App.Hoge;
using BaseLib.Math.Vector3;
class Hoge
{
public:
    struct Foo
    {
        int memberVariableTest;
        int assignedMemberVariableTest = ((1 + 2) * 4) == 3 ? 5 / 10 : 8 % 2;
    };
    interface IInterfaceType
    {
        abstract const int getId()const;
        abstract const Foo getFoo(in int key)const;
        static bool staticMemberVariable;
    };
    static void staticFunction() 
    {
        int a;
        int b = 2;
        { 
            a = b * 2;
        } 
    }
private:
    Foo mFoo;
};
"
            , Parser.ErrorKind.NONE
            ));
            //------------------------------------------------------------
            // 正しいprototype module。
            recipes.Add(new TestRecipe(
@"prototype module App.Math.Vector3;
pod Vector3
{
public:
    float x;
    float y;
    float z;
    float length()const;
};
"
            , Parser.ErrorKind.NONE
            ));
            //------------------------------------------------------------
            // prototype moduleに実装を書いてしまったケース。
            recipes.Add(new TestRecipe(
@"prototype module Hoge.Foo;
class Hoge
{
    void func(){}
};
"
            , Parser.ErrorKind.MEMBER_FUNCTION_DECL_SEMICOLON_EXPECTED
            ));            
            //------------------------------------------------------------
            foreach (TestRecipe recipe in recipes)
            {
                Lexer lex = new Lexer(recipe.Code);
                Parser ps = new Parser(lex);
                Assert.Check(ps.GetErrorKind() == recipe.ErrorKind);
            }
        }

        //------------------------------------------------------------
        // １つのテストを示すレシピ。
        class TestRecipe
        {
            public readonly string Code;
            public readonly Parser.ErrorKind ErrorKind;

            //------------------------------------------------------------
            // コンストラクタ。
            public TestRecipe(string aCode, Parser.ErrorKind aErrorKind)
            {
                Code = aCode;
                ErrorKind = aErrorKind;
            }
        };
    }
}
