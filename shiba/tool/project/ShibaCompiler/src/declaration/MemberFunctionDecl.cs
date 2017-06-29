using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// メンバ関数の宣言。
    /// </summary>
    class MemberFunctionDecl
    {
        public readonly Identifier Ident;
        public readonly FunctionReturnValueDecl ReturnValueDecl;
        public readonly FunctionArgumentDeclList ArgDeclList;

        //------------------------------------------------------------
        // コンストラクタ。
        public MemberFunctionDecl( 
            Identifier aIdent
            , FunctionReturnValueDecl aReturnValueDecl
            , FunctionArgumentDeclList aArgDeclList
            , BlockStatement aStatement
            , bool aIsAbstract
            , bool aIsConst
            , bool aIsOverride
            , bool aIsStatic
            )
        {
            Ident = aIdent;
            ReturnValueDecl = aReturnValueDecl;
            ArgDeclList = aArgDeclList;
            mStatement = aStatement;
            mIsAbstract = aIsAbstract;
            mIsConst = aIsConst;
            mIsOverride = aIsOverride;
            mIsStatic = aIsStatic;
        }

        //------------------------------------------------------------
        // Static修飾子がついているか。
        public bool IsStatic()
        {
            return mIsStatic;
        }

        //------------------------------------------------------------
        // Abstract修飾子がついているか。
        public bool IsAbstract()
        {
            return mIsAbstract;
        }

        //------------------------------------------------------------
        // Const修飾子がついているか。
        public bool IsConst()
        {
            return mIsConst;
        }

        //------------------------------------------------------------
        // 関数の定義。nullの場合もある。
        public BlockStatement Statement()
        {
            return mStatement;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                Ident.Trace(aTracer, "Ident");
                ReturnValueDecl.Trace(aTracer,"ReturnValueDecl");
                ArgDeclList.Trace(aTracer, "ArgDeclList");
                if (mStatement == null)
                {
                    aTracer.WriteValue("mStatement", "null");
                }
                else
                {
                    aTracer.WriteName("mStatement");
                    using(new Tracer.IndentScope(aTracer))
                    {
                        mStatement.Trace(aTracer);    
                    }
                }
                aTracer.WriteValue("mIsAbstract", mIsAbstract.ToString());
                aTracer.WriteValue("mIsConst", mIsConst.ToString());
                aTracer.WriteValue("mIsOverride", mIsOverride.ToString());
                aTracer.WriteValue("mIsStatic", mIsStatic.ToString());
            }
        }

        //============================================================
        BlockStatement mStatement;
        bool mIsAbstract;
        bool mIsConst;
        bool mIsOverride;
        bool mIsStatic;
    }
}
