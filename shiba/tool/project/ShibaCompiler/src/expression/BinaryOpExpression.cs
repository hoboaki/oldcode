using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �񍀉��Z���B
    /// </summary>
    class BinaryOpExpression : IExpression
    {
        //------------------------------------------------------------
        // ���Z�̎�ށB
        public enum OpKind
        {
            Unknnown,
            AdditiveAdd,
            AdditiveSub,
            BitwiseAnd,
            BitwiseOr,
            BitwiseXor,
            EqualityEqual,
            EqualityNotEqual,
            IdentityEqual,
            IdentityNotEqual,
            LogicalAnd,
            LogicalOr,
            MultiplicativeDiv,
            MultiplicativeMod,
            MultiplicativeMul,
            RelationalGreater,
            RelationalGreaterEqual,
            RelationalLess,
            RelationalLessEqual,
            ShiftLeft,
            ShiftRight,
        };

        //------------------------------------------------------------
        // �R���X�g���N�^
        public BinaryOpExpression(Token aOpToken, OpKind aOpKind, IExpression aFirst,IExpression aSecond)
        {
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mFirst = aFirst;
            mSecond = aSecond;
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
            aTracer.WriteName("BinaryOpExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mFirst.Trace(aTracer);
                mSecond.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(BinaryOpExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mFirst.CreateEvaluateNode();
                mSecondNode = mExpr.mSecond.CreateEvaluateNode();
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
            BinaryOpExpression mExpr;
            IEvaluateNode mFirstNode;
            IEvaluateNode mSecondNode;
            EvaluateInfo mEvaluateInfo;
            TransferredEIHolder mTransferredEIHolder;
            
            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // �P��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // �Q��
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                
                // �]�����̍쐬
                TypeInfo ti;
                switch (mExpr.mOpKind)
                {
                    case OpKind.RelationalGreater:
                    case OpKind.RelationalGreaterEqual:
                    case OpKind.RelationalLess:
                    case OpKind.RelationalLessEqual:
                    case OpKind.EqualityEqual:
                    case OpKind.EqualityNotEqual:
                    case OpKind.IdentityEqual:
                    case OpKind.IdentityNotEqual:
                    case OpKind.LogicalAnd:
                    case OpKind.LogicalOr:
                        // bool�̏���Ԃ��@
                        ti = new TypeInfo(new TypeInfo.TypeSymbol(mExpr.mOpToken, BuiltInType.Bool), new TypeInfo.TypeAttribute(true, false));
                        break;

                    default:
                        // ���ӂ̏������̂܂ܕԂ�
                        // todo: �Öق̕ϊ��Ή�
                        ti = mFirstNode.GetEvaluateInfo().TypeInfo;
                        break;
                }
                mEvaluateInfo = EvaluateInfo.CreateAsValue(ti);

                // Holder�쐬
                mTransferredEIHolder = new TransferredEIHolder(mEvaluateInfo);
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {                
                // �e���Z���Ƃ̏���
                switch (mExpr.mOpKind)
                {
                    case OpKind.AdditiveAdd: evaluateNumberType(aComp, BCOpCode.OpType.ADDI32, false); break;
                    case OpKind.AdditiveSub: evaluateNumberType(aComp, BCOpCode.OpType.SUBI32, false); break;
                    case OpKind.BitwiseAnd: evaluateNumberType(aComp, BCOpCode.OpType.ANDI32, false); break;
                    case OpKind.BitwiseOr: evaluateNumberType(aComp, BCOpCode.OpType.ORI32, false); break;
                    case OpKind.BitwiseXor: evaluateNumberType(aComp, BCOpCode.OpType.XORI32, false); break;
                    case OpKind.EqualityEqual: evaluateEquality(aComp, false); break;
                    case OpKind.EqualityNotEqual: evaluateEquality(aComp, true); break;
                    case OpKind.LogicalAnd: evaluateLogicalOp(aComp, true); break;
                    case OpKind.LogicalOr: evaluateLogicalOp(aComp, false); break;
                    case OpKind.MultiplicativeMul: evaluateNumberType(aComp, BCOpCode.OpType.MULS32, false); break;
                    case OpKind.MultiplicativeDiv: evaluateNumberType(aComp, BCOpCode.OpType.DIVS32, false); break;
                    case OpKind.MultiplicativeMod: evaluateNumberType(aComp, BCOpCode.OpType.MODS32, false); break;
                    case OpKind.RelationalLess: evaluateNumberType(aComp, BCOpCode.OpType.LTS32, false); break;
                    case OpKind.RelationalLessEqual: evaluateNumberType(aComp, BCOpCode.OpType.LES32, false); break;
                    case OpKind.RelationalGreater: evaluateNumberType(aComp, BCOpCode.OpType.LTS32, true); break;
                    case OpKind.RelationalGreaterEqual: evaluateNumberType(aComp, BCOpCode.OpType.LES32, true); break;
                    case OpKind.ShiftLeft: evaluateNumberType(aComp, BCOpCode.OpType.SLLI32, false); break;
                    case OpKind.ShiftRight: evaluateNumberType(aComp, BCOpCode.OpType.SLRI32, false); break;
                    default:
                        {
                            // todo:
                            // �����ȉ��Z�̑Ή�
                            aComp.ThrowErrorException(
                                SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION
                                , mExpr.mOpToken
                                );
                        }
                        break;
                }
            }

            //------------------------------------------------------------
            // �������l�̉��Z���������Ȃ��^�C�v�̉��Z�B
            void evaluateNumberType(
                SemanticAnalyzeComponent aComp
                , BCOpCode.OpType aOpType
                , bool aSwapLR // true�Ȃ獶�ӂƉE�ӂ��t�]������B
                )
            {
                // ���W�X�^�ݒ�
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // �`�B�ݒ�
                mTransferredEIHolder.TransferIfPossible(aComp);

                // �P��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // �\�Ȃ�Q�߂ɓ`�B����
                if (!mFirstNode.GetEvaluateInfo().SR.IsSame(mEvaluateInfo.SR))
                {
                    mTransferredEIHolder.TransferIfPossible(aComp);
                }

                // �Q��
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // todo:
                // �Öق̕ϊ��̑Ή�
                // ����int�����Ή����Ȃ�
                if (mFirstNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mFirstNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32
                    || mSecondNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mSecondNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // ���Z
                StackRegister leftSR = mFirstNode.GetEvaluateInfo().SR;
                StackRegister rightSR = mSecondNode.GetEvaluateInfo().SR;
                aComp.BCFunction.AddOPCode_SReg1_SReg2_SReg3(
                    aOpType
                    , mEvaluateInfo.SR
                    , aSwapLR ? rightSR : leftSR
                    , aSwapLR ? leftSR : rightSR
                    );

                // �ʒm
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
            
            //------------------------------------------------------------
            // '==','!='�̉��Z�B
            void evaluateEquality(
                SemanticAnalyzeComponent aComp
                , bool aIsNotEqual
                )
            {
                // ���W�X�^�ݒ�
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // �`�B�ݒ�
                mTransferredEIHolder.TransferIfPossible(aComp);

                // �P��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // �\�Ȃ�Q�߂ɓ`�B����
                if ( !mFirstNode.GetEvaluateInfo().SR.IsSame( mEvaluateInfo.SR ) )
                {
                    mTransferredEIHolder.TransferIfPossible(aComp);
                }

                // �Q��
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // todo:
                // �Öق̕ϊ��̑Ή�
                // ����int,bool�����Ή����Ȃ�
                var firstEI = mFirstNode.GetEvaluateInfo();
                var secondEI = mSecondNode.GetEvaluateInfo();
                if (firstEI.Kind != EvaluateInfo.InfoKind.Value
                    || (firstEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && firstEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    || secondEI.Kind != EvaluateInfo.InfoKind.Value
                    || (secondEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && secondEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // ���߂̑I��
                BCOpCode.OpType opType = BCOpCode.OpType.NOP;
                switch (firstEI.TypeInfo.Symbol.GetBuiltInType())
                {
                    case BuiltInType.SInt32:
                    case BuiltInType.UInt32:
                        opType = aIsNotEqual ? BCOpCode.OpType.NEI32 : BCOpCode.OpType.EQI32;
                        break;

                    case BuiltInType.Bool:
                        opType = aIsNotEqual ? BCOpCode.OpType.NEBOOL : BCOpCode.OpType.EQBOOL;
                        break;

                    default:
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                        break;
                }

                // �ǉ�
                aComp.BCFunction.AddOPCode_SReg1_SReg2_SReg3(opType, mEvaluateInfo.SR, firstEI.SR, secondEI.SR);

                // �ʒm
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // '&&','||'�̉��Z�B
            void evaluateLogicalOp(
                SemanticAnalyzeComponent aComp
                , bool aIsAnd
                )
            {
                // bool�����Ή����Ȃ�
                var firstEI = mFirstNode.GetEvaluateInfo();
                var secondEI = mSecondNode.GetEvaluateInfo();
                if (firstEI.Kind != EvaluateInfo.InfoKind.Value
                    || firstEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool
                    )
                {
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mOpToken
                        , firstEI.TypeInfo
                        , mEvaluateInfo.TypeInfo
                        ));
                }
                if (secondEI.Kind != EvaluateInfo.InfoKind.Value
                    || secondEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool
                    )
                {
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mOpToken
                        , secondEI.TypeInfo
                        , mEvaluateInfo.TypeInfo
                        ));
                }

                // ���W�X�^�ݒ�
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // �`�B�ݒ�
                mTransferredEIHolder.TransferIfPossible(aComp);

                // ���x���m��
                BCLabel labelEnd = aComp.BCFunction.LabelCreate(); // End�p

                // �P��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // �P�߂̌��ʂ��󂯂��Z���]��
                // �����������Ă�����Q�ڂ̎����΂�
                aComp.BCFunction.AddOPCode_SReg_Label(
                    aIsAnd ? BCOpCode.OpType.JMPNEG : BCOpCode.OpType.JMPPOS
                    , mEvaluateInfo.SR // �`�B���Ă���̂Ŏ�����SR�Ɍ��ʂ��i�[����Ă���͂��@
                    , labelEnd
                    );

                // �`�B�ݒ�
                mTransferredEIHolder.TransferIfPossible(aComp);

                // �Q��
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // �Q�߂̌��ʂ���
                if (!mEvaluateInfo.SR.IsSame(secondEI.SR))
                {
                    aComp.BCFunction.AddOPCode_SReg1_SReg2(
                        BCOpCode.OpType.LDSRSR
                        , mEvaluateInfo.SR
                        , secondEI.SR
                        );
                }
                
                // End���x���}��
                aComp.BCFunction.LabelInsert(labelEnd);
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^�ԋp
                mTransferredEIHolder.ReleaseIfNeccesary(aComp);
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ������B
        Token mOpToken;
        OpKind mOpKind;
        IExpression mFirst;
        IExpression mSecond;
    }
}
