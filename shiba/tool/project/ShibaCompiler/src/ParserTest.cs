using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Parser�̃e�X�g�B
    /// </summary>
    class ParserTest
    {
        //------------------------------------------------------------
        // ���s�֐��B
        static public void Execute()
        {
            List<TestRecipe> recipes = new List<TestRecipe>();
            //------------------------------------------------------------
            // ������module�B
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
            // ������prototype module�B
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
            // prototype module�Ɏ����������Ă��܂����P�[�X�B
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
        // �P�̃e�X�g���������V�s�B
        class TestRecipe
        {
            public readonly string Code;
            public readonly Parser.ErrorKind ErrorKind;

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public TestRecipe(string aCode, Parser.ErrorKind aErrorKind)
            {
                Code = aCode;
                ErrorKind = aErrorKind;
            }
        };
    }
}
