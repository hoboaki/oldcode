using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// インデクス式。
    /// </summary>
    class IndexExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public IndexExpression(SequenceExpression aSequenceExpr)
        {
            mSequenceExpr = aSequenceExpr;
        }
        
        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("IndecExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mSequenceExpr.Trace(aTracer);
            }
        }

        //============================================================
        SequenceExpression mSequenceExpr = null;
    }
}
