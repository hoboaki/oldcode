using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �L���X�g���B
    /// </summary>
    class CastExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public CastExpression(TypePath aTypePath, IExpression aExpr)
        {
            mTypePath = aTypePath;
            mExpr = aExpr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("CastExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mTypePath.Trace(aTracer, "mTypePath");
                mExpr.Trace(aTracer);
            }
        }

        //============================================================
        TypePath mTypePath;
        IExpression mExpr;
    }
}
