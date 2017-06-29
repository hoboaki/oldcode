using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
