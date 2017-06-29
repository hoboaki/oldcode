using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �����o�֐��̐錾�B
    /// </summary>
    class MemberFunctionDecl
    {
        public readonly Identifier Ident;
        public readonly FunctionReturnValueDecl ReturnValueDecl;
        public readonly FunctionArgumentDeclList ArgDeclList;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
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
        // Static�C���q�����Ă��邩�B
        public bool IsStatic()
        {
            return mIsStatic;
        }

        //------------------------------------------------------------
        // Abstract�C���q�����Ă��邩�B
        public bool IsAbstract()
        {
            return mIsAbstract;
        }

        //------------------------------------------------------------
        // Const�C���q�����Ă��邩�B
        public bool IsConst()
        {
            return mIsConst;
        }

        //------------------------------------------------------------
        // �֐��̒�`�Bnull�̏ꍇ������B
        public BlockStatement Statement()
        {
            return mStatement;
        }

        //------------------------------------------------------------
        // �g���[�X�B
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
