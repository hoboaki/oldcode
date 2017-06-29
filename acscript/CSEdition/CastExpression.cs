using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
