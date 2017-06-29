using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// delete文。
    /// </summary>
    class DeleteExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public DeleteExpression(IExpression expr)
        {
            mExpr = expr;
        }

        //------------------------------------------------------------
        // トレース。
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
