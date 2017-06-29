using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ��u���Z���B
    /// </summary>
    class PostfixExpression : IExpression
    {
        //------------------------------------------------------------
        // ���Z�̎�ށB
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
        };

        //------------------------------------------------------------
        // ���̎�ށB
        public enum ExprKind
        {
            IncDec // ++,--
            , FunctionCallExpr // FunctionCallExpression
            , IndexExpr // IndexExpression
            , ChildIdentExpr // ChildIdentExpression
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public PostfixExpression(Token aOpToken, OpKind aOpKind, IExpression aExpr)
        {
            mExprKind = ExprKind.IncDec;
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mFirstExpr = aExpr;
        }

        //------------------------------------------------------------
        // FunctionCall�̃R���X�g���N�^�B
        public PostfixExpression(IExpression aFirstExpr, FunctionCallExpression aFuncCallExpr)
        {
            mExprKind = ExprKind.FunctionCallExpr;
            mFirstExpr = aFirstExpr;
            mFunctionCallExpr = aFuncCallExpr;
        }

        //------------------------------------------------------------
        // Index�̃R���X�g���N�^�B
        public PostfixExpression(IExpression aFirstExpr, IndexExpression aIndexExpr)
        {
            mExprKind = ExprKind.IndexExpr;
            mFirstExpr = aFirstExpr;
            mIndexExpr = aIndexExpr;
        }

        //------------------------------------------------------------
        // ChildIdent�̃R���X�g���N�^�B
        public PostfixExpression(IExpression aFirstExpr, ChildIdentExpression aChildIdentExpr)
        {
            mExprKind = ExprKind.ChildIdentExpr;
            mFirstExpr = aFirstExpr;
            mChildIdentExpr = aChildIdentExpr;
        }

        //------------------------------------------------------------
        // �]���m�[�h���쐬����B
        public IEvaluateNode CreateEvaluateNode()
        {
            switch (mExprKind)
            {
                case ExprKind.IncDec: return new IncDecEvaluateNode(this);
                case ExprKind.FunctionCallExpr: return mFunctionCallExpr.CreateEvaluateNodeWithFirstExpr(mFirstExpr);
                default:
                    Assert.NotReachHere();
                    return null;
            }
        }

        //------------------------------------------------------------
        // �g�[�N�����擾����B
        public Token GetToken()
        {
            return mOpToken != null ? mOpToken : mFirstExpr.GetToken();
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTrace)
        {
            bool isIndent = mOpKind != OpKind.Unknown;
            if (isIndent)
            {
                aTrace.WriteValue("PostfixExpression",mOpKind.ToString());
                aTrace.indentInc();
            }
            if (isIndent)
            {
                aTrace.indentDec();
            }
        }

        //============================================================

        //------------------------------------------------------------
        // IncDec�p�]���m�[�h�B
        class IncDecEvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public IncDecEvaluateNode(PostfixExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mFirstExpr.CreateEvaluateNode();
            }

            //------------------------------------------------------------
            // EvaluateInfo���擾����B
            public EvaluateInfo GetEvaluateInfo()
            {
                return mFirstNode.GetEvaluateInfo();
            }

            //------------------------------------------------------------
            // �]���C�x���g�𑗐M����B
            public void SendEvent(SemanticAnalyzeComponent aComp, EvaluateNodeEventKind aEventKind)
            {
                switch (aEventKind)
                {
                    case EvaluateNodeEventKind.Analyze: eventAnalyze(aComp); break;
                    case EvaluateNodeEventKind.Evaluate: eventEvaluate(aComp); break;
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            PostfixExpression mExpr;
            IEvaluateNode mFirstNode;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // �Öق̕ϊ��̑Ή�
                // ����int�����Ή����Ȃ�
                if (mFirstNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mFirstNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // ���߂̑I��
                BCOpCode.OpType opType = BCOpCode.OpType.NOP;
                switch(mExpr.mOpKind)
                {
                case OpKind.Inc: opType = BCOpCode.OpType.INCI32; break;
                case OpKind.Dec: opType = BCOpCode.OpType.DECI32; break;
                default:
                    Assert.NotReachHere();
                    break;
                }

                // ���߂�ǉ�
                aComp.BCFunction.AddOPCode_SReg(
                    opType
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // OnParentEvaluateEnd
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ������B
        ExprKind mExprKind;
        Token mOpToken;
        OpKind mOpKind = OpKind.Unknown;
        IExpression mFirstExpr;
        FunctionCallExpression mFunctionCallExpr;
        IndexExpression mIndexExpr;
        ChildIdentExpression mChildIdentExpr;
    }
}
