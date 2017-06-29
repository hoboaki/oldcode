using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Root�ƂȂ鎯�ʎq���B
    /// </summary>
    class RootIdentExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public RootIdentExpression(Identifier aIdent, bool aIsNamespaceRoot)
        {
            mIdent = aIdent;
            mIsNamespaceRoot = aIsNamespaceRoot;
        }

        //------------------------------------------------------------
        // �g���[�X�B
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
