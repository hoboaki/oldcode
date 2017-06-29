using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �C���f�N�X���B
    /// </summary>
    class IndexExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public IndexExpression(SequenceExpression aSequenceExpr)
        {
            mSequenceExpr = aSequenceExpr;
        }
        
        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("IndecExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mSequenceExpr.Trace(aTracer);
            }
        }

        //============================================================
        SequenceExpression mSequenceExpr = null;
    }
}
