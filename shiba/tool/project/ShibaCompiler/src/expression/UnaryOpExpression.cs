using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �P�����Z���B
    /// </summary>
    class UnaryOpExpression : IExpression
    {
        /// <summary>
        /// ���Z�̎�ށB
        /// </summary>
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
            , Positive // +
            , Negative // -
            , LogicalNot // !
            , BitwiseNot // ~ 
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public UnaryOpExpression(Token aOpToken, OpKind aOpKind, IExpression aExpr)
        {
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mExpr = aExpr;
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
            return mOpToken;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("UnaryOpExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mExpr.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(UnaryOpExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mExpr.CreateEvaluateNode();
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
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            UnaryOpExpression mExpr;
            IEvaluateNode mFirstNode;
            EvaluateInfo mEvaluateInfo;
            TransferredEIHolder mTransferredEIHolder;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // EvaluateInfo�̗p��
                switch (mExpr.mOpKind)
                {
                    case OpKind.Inc:
                    case OpKind.Dec:
                    case OpKind.Positive:
                        // ���̂��̂����̂܂�
                        mEvaluateInfo = mFirstNode.GetEvaluateInfo();
                        break;

                    case OpKind.Negative:
                    case OpKind.BitwiseNot:
                        // �^�C�v�͈ꏏ�����Ǖʂ̃��W�X�^���g���\��������̂�EvaluateInfo���쐬
                        mEvaluateInfo = EvaluateInfo.CreateAsValue(mFirstNode.GetEvaluateInfo().TypeInfo);
                        break;

                    case OpKind.LogicalNot:
                        // bool��
                        mEvaluateInfo = EvaluateInfo.CreateAsValue(new TypeInfo(new TypeInfo.TypeSymbol(mExpr.mOpToken, BuiltInType.Bool), new TypeInfo.TypeAttribute(true, false)));
                        break;

                    default:
                        Assert.NotReachHere();
                        break;
                }

                // Holder�쐬
                mTransferredEIHolder = new TransferredEIHolder(mEvaluateInfo);
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                switch (mExpr.mOpKind)
                {
                    case OpKind.Inc: evaluateIncDec(aComp, true); break;
                    case OpKind.Dec: evaluateIncDec(aComp, false); break;
                    case OpKind.Positive: evaluatePositive(aComp); break;
                    case OpKind.Negative: evaluateNegativeBitwiseNot(aComp,true); break;
                    case OpKind.BitwiseNot: evaluateNegativeBitwiseNot(aComp, false); break;
                    case OpKind.LogicalNot: evaluateLogicalNot(aComp); break;
                    default:
                        Assert.NotReachHere();
                        break;
                }
            }

            //------------------------------------------------------------
            // Inc,Dec�p�]���֐��B
            void evaluateIncDec(SemanticAnalyzeComponent aComp, bool aIsInc)
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

                // ���߂�ǉ�
                aComp.BCFunction.AddOPCode_SReg(
                    aIsInc ? BCOpCode.OpType.INCI32 : BCOpCode.OpType.DECI32 
                    , mEvaluateInfo.SR
                    );
            }

            //------------------------------------------------------------
            // Positive�p�]���֐��B
            void evaluatePositive(SemanticAnalyzeComponent aComp)
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
            }

            //------------------------------------------------------------
            // Negative,BitwiseNot�p�]���֐��B
            void evaluateNegativeBitwiseNot(SemanticAnalyzeComponent aComp,bool aIsNegative)
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

                // SR�ݒ�
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // �`�B�ł���Ȃ�`�B����
                mTransferredEIHolder.TransferIfPossible(aComp);

                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B��񃊃Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // ���ߒǉ�
                aComp.BCFunction.AddOPCode_SReg1_SReg2(
                    aIsNegative ? BCOpCode.OpType.NEGS32 : BCOpCode.OpType.NTI32
                    , mEvaluateInfo.SR
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // �C�x���g���M
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // LogicalNot�p�]���֐��B
            void evaluateLogicalNot(SemanticAnalyzeComponent aComp)
            {
                // bool�����Ή����Ȃ�
                StatementUtil.CheckBoolExpression(aComp, mFirstNode, mExpr.mExpr);

                // SR�ݒ�
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // �`�B�ł���Ȃ�`�B����
                mTransferredEIHolder.TransferIfPossible(aComp);

                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B��񃊃Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // ���ߒǉ�
                aComp.BCFunction.AddOPCode_SReg1_SReg2(
                    BCOpCode.OpType.NTBOOL
                    , mEvaluateInfo.SR
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // �C�x���g���M
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                switch (mExpr.mOpKind)
                {
                    case OpKind.Inc:
                    case OpKind.Dec:
                    case OpKind.Positive:
                        // ���̃^�C�~���O�ŃC�x���g���M
                        mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                        break;

                    case OpKind.Negative:
                    case OpKind.BitwiseNot:
                    case OpKind.LogicalNot:
                        // ���W�X�^�ԋp
                        mTransferredEIHolder.ReleaseIfNeccesary(aComp);
                        break;

                    default:
                        break;
                }
            }
        };

        //------------------------------------------------------------
        Token mOpToken;
        OpKind mOpKind;
        IExpression mExpr;
    }
}
