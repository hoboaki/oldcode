using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        // 評価ノードを作成する。
        public IEvaluateNode CreateEvaluateNode()
        {
            // todo:
            Assert.NotReachHere();
            return null;
        }

        //------------------------------------------------------------
        // トークンを取得する。
        public Token GetToken()
        {
            // todo:
            Assert.NotReachHere();
            return null;
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
