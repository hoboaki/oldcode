using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// キャスト式。
    /// </summary>
    class CastExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public CastExpression(TypePath aTypePath, IExpression aExpr)
        {
            mTypePath = aTypePath;
            mExpr = aExpr;
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
