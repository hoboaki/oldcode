using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 子となる識別子の式。
    /// </summary>
    class ChildIdentExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ChildIdentExpression(Identifier aIdent)
        {
            mIdent = aIdent;
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
            aTracer.WriteName("ChildIdentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mIdent.Trace(aTracer, "mIdent");
            }
        }

        //============================================================
        Identifier mIdent;
    }
}
