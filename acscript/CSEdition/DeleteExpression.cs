using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// delete���B
    /// </summary>
    class DeleteExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public DeleteExpression(IExpression expr)
        {
            mExpr = expr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("DeleteExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mExpr.Trace(aTracer);
            }
        }

        //============================================================
        IExpression mExpr;
    }
}
