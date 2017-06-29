using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// SequenceExpression。
    /// </summary>
    class SequenceExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public SequenceExpression(IExpression aTermExpr)
        {
            mTermExpr = aTermExpr;
        }

        //------------------------------------------------------------
        // コンストラクタ。
        public SequenceExpression(IExpression aTermExpr, SequenceExpression aNextExpr)
            : this(aTermExpr)
        {
            mNextExpr = aNextExpr;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteName("mTermExpr");
                using (new Tracer.IndentScope(aTracer))
                {
                    mTermExpr.Trace(aTracer);
                }
                if (mNextExpr == null)
                {
                    aTracer.WriteValue("mNextExpr", "null");
                }
                else
                {
                    mNextExpr.Trace(aTracer);
                }
            }
        }

        //============================================================
        IExpression mTermExpr;
        SequenceExpression mNextExpr;
    }
}
