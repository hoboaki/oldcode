using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �ϐ��̐錾�B
    /// </summary>
    class VariableDecl
    {
        public readonly TypePath TypePath;
        public readonly Identifier Ident;
        public readonly IExpression Expr;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public VariableDecl(TypePath aTypePath, Identifier aIdent, IExpression aExpr)
        {
            TypePath = aTypePath;
            Ident = aIdent;
            Expr = aExpr;
        }

        //------------------------------------------------------------
        // ���������inull�̏ꍇ������j�B
        public IExpression Expression()
        {
            return Expr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, String aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                TypePath.Trace(aTracer, "TypePath");
                Ident.Trace(aTracer, "Ident");
                if (Expr == null)
                {
                    aTracer.WriteValue("Expr", "null");
                }
                else
                {
                    aTracer.WriteName("Expr");
                    using (new Tracer.IndentScope(aTracer))
                    {
                        Expr.Trace(aTracer);
                    }
                }
            }
        }
    }
}
