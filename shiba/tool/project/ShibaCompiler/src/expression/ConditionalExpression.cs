using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �Q�l���Z���B
    /// </summary>
    class ConditionalExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ConditionalExpression(IExpression aFirst, IExpression aSecond, IExpression aThird)
        {
            mFirst = aFirst;
            mSecond = aSecond;
            mThird = aThird;
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
            aTracer.WriteName("ConditionalExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mFirst.Trace(aTracer);
                mSecond.Trace(aTracer);
                mThird.Trace(aTracer);
            }
        }

        //============================================================
        IExpression mFirst;
        IExpression mSecond;
        IExpression mThird;
    }
}
