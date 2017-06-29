using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// new式。
    /// </summary>
    class NewExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public NewExpression(TypePath aTypePath, FunctionCallExpression aFuncCallExpr)
        {
            mTypePath = aTypePath;
            mFuncCallExpr = aFuncCallExpr;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("NewExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mTypePath.Trace(aTracer,"mTypePath");
                mFuncCallExpr.Trace(aTracer);
            }
        }

        //============================================================
        TypePath mTypePath;
        FunctionCallExpression mFuncCallExpr;
    }
}
