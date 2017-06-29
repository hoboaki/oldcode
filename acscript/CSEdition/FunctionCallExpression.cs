using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 関数呼び出し式。
    /// </summary>
    class FunctionCallExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionCallExpression()
        {
        }

        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionCallExpression(SequenceExpression aSequenceExpr)
        {
            mSequenceExpr = aSequenceExpr;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("FunctionCallExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mSequenceExpr.Trace(aTracer);
            }
        }

        //============================================================
        SequenceExpression mSequenceExpr = null;
    }
}
