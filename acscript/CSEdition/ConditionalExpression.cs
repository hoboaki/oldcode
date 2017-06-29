using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 参考演算式。
    /// </summary>
    class ConditionalExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ConditionalExpression(IExpression aFirst, IExpression aSecond, IExpression aThird)
        {
            mFirst = aFirst;
            mSecond = aSecond;
            mThird = aThird;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ConditionalExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mFirst.Trace(aTracer);
                mSecond.Trace(aTracer);
                mThird.Trace(aTracer);
            }
        }

        //============================================================
        IExpression mFirst;
        IExpression mSecond;
        IExpression mThird;
    }
}
