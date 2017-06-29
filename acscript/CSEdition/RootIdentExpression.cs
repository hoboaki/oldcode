using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Rootとなる識別子式。
    /// </summary>
    class RootIdentExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public RootIdentExpression(Identifier aIdent, bool aIsNamespaceRoot)
        {
            mIdent = aIdent;
            mIsNamespaceRoot = aIsNamespaceRoot;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("RootIdentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mIdent.Trace(aTracer, "mIdent");
                aTracer.WriteValue("mIsNamespaceRoot", mIsNamespaceRoot.ToString());
            }
        }

        //============================================================
        Identifier mIdent;
        bool mIsNamespaceRoot;
    }
}
