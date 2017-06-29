using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
