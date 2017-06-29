using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// SequenceExpression�B
    /// </summary>
    class SequenceExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SequenceExpression(IExpression aFirstExpr)
        {
            mFirstExpr = aFirstExpr;
        }

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SequenceExpression(IExpression aFirstExpr, SequenceExpression aNextExpr)
            : this(aFirstExpr)
        {
            mNextExpr = aNextExpr;
        }

        //------------------------------------------------------------
        // �]���m�[�h���쐬����B
        public IEvaluateNode CreateEvaluateNode()
        {
            return new EvaluateNode(this);
        }

        //------------------------------------------------------------
        // �g�[�N�����擾����B
        public Token GetToken()
        {
            return mFirstExpr.GetToken();
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
                    mFirstExpr.Trace(aTracer);
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

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(SequenceExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mFirstExpr.CreateEvaluateNode();
                if (mExpr.mNextExpr != null)
                {
                    mNextNode = mExpr.mNextExpr.CreateEvaluateNode();
                }
            }

            //------------------------------------------------------------
            // EvaluateInfo���擾����B
            public EvaluateInfo GetEvaluateInfo()
            {
                return mEvaluateInfo;
            }

            //------------------------------------------------------------
            // �]���C�x���g�𑗐M����B
            public void SendEvent(SemanticAnalyzeComponent aComp, EvaluateNodeEventKind aEventKind)
            {
                switch (aEventKind)
                {
                    case EvaluateNodeEventKind.Analyze: eventAnalyze(aComp); break;
                    case EvaluateNodeEventKind.Evaluate: eventEvaluate(aComp); break;
                    case EvaluateNodeEventKind.SetupFR: eventSetupFR(aComp); break;
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            SequenceExpression mExpr;
            IEvaluateNode mFirstNode;
            IEvaluateNode mNextNode;
            EvaluateInfo mEvaluateInfo;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // �Q�߂�����΂Q�߂�]��
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                }

                // �]�������쐬
                mEvaluateInfo = mFirstNode.GetEvaluateInfo();
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �Q�߂�����΂Q�߂�]��
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                }
            }

            //------------------------------------------------------------�@
            // FR���蓖�āB�@
            void eventSetupFR(SemanticAnalyzeComponent aComp)
            {
                // �P��
                byte frIndex = aComp.FunctionCallFRNextIndex();
                aComp.BCFunction.AddOPCode_CU1_SR(
                    BCOpCode.OpType.LDFRSR
                    , frIndex
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // �Q�߂�����΂Q�߂�
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.SetupFR);
                }
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // �]���I���C�x���g���M
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                }
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ������B
        IExpression mFirstExpr;
        SequenceExpression mNextExpr;
    }
}
