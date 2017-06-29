using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // �]���m�[�h���쐬
            var evaluateNode = mExpression.CreateEvaluateNode();

            // �]������
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

            // �]�����s
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

            // �e�̕]���I�� 
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

            // Scope�I���C�x���g�͑���K�v���Ȃ�
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
