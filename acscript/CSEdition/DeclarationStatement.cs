using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �錾���B
    /// </summary>
    class DeclarationStatement : IStatement
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public DeclarationStatement(VariableDecl aVariableDecl, bool aIsConst)
        {
            mVariableDecl = aVariableDecl;
            mIsConst = aIsConst;
        }

        //------------------------------------------------------------
        // �Ӗ���́B
        public bool SemanticAnalyze(SemanticAnalyzeComponent comp)
        {
            TypeInfo typeInfo = comp.CreateTypeInfo(mVariableDecl.TypePath);

            VariableSymbolNode newNode = new VariableSymbolNode(
                comp.PrevSymbolNode
                , mVariableDecl.Ident
                , typeInfo
                );
            if (!comp.OnSymbolNodeCreateMethod(newNode))
            {
                return false;
            }
            comp.PrevSymbolNode = newNode;

            return true;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("DeclarationStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                mVariableDecl.Trace(aTracer, "mVariableDecl");
                aTracer.WriteValue("mIsConst", mIsConst.ToString());
            }
        }

        //============================================================
        VariableDecl mVariableDecl;
        bool mIsConst;
    }
}
