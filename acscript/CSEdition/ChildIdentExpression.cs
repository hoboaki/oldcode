using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
