using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �����B
    /// </summary>
    class ExpressionStatement : IStatement
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ExpressionStatement(IExpression aExpression)
        {
            mExpression = aExpression;
        }

        //------------------------------------------------------------
        // �Ӗ���́B
        public bool SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            return false;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ExpressionStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                mExpression.Trace(aTracer);
            }
        }

        //============================================================
        IExpression mExpression;
    }
}
