using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
