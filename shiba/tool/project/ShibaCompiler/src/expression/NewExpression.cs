using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
