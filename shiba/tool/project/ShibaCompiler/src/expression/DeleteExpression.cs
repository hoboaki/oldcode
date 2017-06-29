using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// delete���B
    /// </summary>
    class DeleteExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public DeleteExpression(IExpression expr)
        {
            mExpr = expr;
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
            aTracer.WriteName("DeleteExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mExpr.Trace(aTracer);
            }
        }

        //============================================================
        IExpression mExpr;
    }
}
