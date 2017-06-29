using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        // �]���m�[�h���쐬����B
        public IEvaluateNode CreateEvaluateNode()
        {
            // todo:
            Assert.NotReachHere();
            return null;
        }

        //------------------------------------------------------------
        // �g�[�N�����擾����B
        public Token GetToken()
        {
            // todo:
            Assert.NotReachHere();
            return null;
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
