using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// SequenceExpression�B
    /// </summary>
    class SequenceExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SequenceExpression(IExpression aTermExpr)
        {
            mTermExpr = aTermExpr;
        }

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SequenceExpression(IExpression aTermExpr, SequenceExpression aNextExpr)
            : this(aTermExpr)
        {
            mNextExpr = aNextExpr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteName("mTermExpr");
                using (new Tracer.IndentScope(aTracer))
                {
                    mTermExpr.Trace(aTracer);
                }
                if (mNextExpr == null)
                {
                    aTracer.WriteValue("mNextExpr", "null");
                }
                else
                {
                    mNextExpr.Trace(aTracer);
                }
            }
        }

        //============================================================
        IExpression mTermExpr;
        SequenceExpression mNextExpr;
    }
}
