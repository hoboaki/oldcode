using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �֐��Ăяo�����B
    /// </summary>
    class FunctionCallExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionCallExpression()
        {
        }

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionCallExpression(SequenceExpression aSequenceExpr)
        {
            mSequenceExpr = aSequenceExpr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("FunctionCallExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mSequenceExpr.Trace(aTracer);
            }
        }

        //============================================================
        SequenceExpression mSequenceExpr = null;
    }
}
