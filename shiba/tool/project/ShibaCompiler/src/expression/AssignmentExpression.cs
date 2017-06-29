using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ������B
    /// </summary>
    class AssignmentExpression : IExpression
    {
        //------------------------------------------------------------
        // ���Z�̎�ށB
        public enum OpKind
        {
            Unknown,
            Assign,
            AddAssign,
            SubAssign,
            MulAssign,
            DivAssign,
            ModAssign,
            RShiftAssign,
            LShiftAssign,
            AndAssign,
            OrAssign,
            XorAssign,
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public AssignmentExpression(Token aOpToken, OpKind aOpKind, IExpression aLeftExpr, IExpression aRightExpr)
        {
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mLeftExpr = aLeftExpr;
            mRightExpr = aRightExpr;
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
            aTracer.WriteName("AssignmentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mLeftExpr.Trace(aTracer);
                mRightExpr.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(AssignmentExpression aExpr)
            {
                mExpr = aExpr;
                mLeftNode = mExpr.mLeftExpr.CreateEvaluateNode();
                mRightNode = mExpr.mRightExpr.CreateEvaluateNode();
            }

            //------------------------------------------------------------
            // EvaluateInfo���擾����B
            public EvaluateInfo GetEvaluateInfo()
            {
                return mLeftNode.GetEvaluateInfo();
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
            AssignmentExpression mExpr;
            IEvaluateNode mLeftNode;
            IEvaluateNode mRightNode;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // ����]��
                mLeftNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // �E��]��
                mRightNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // �Öق̕ϊ��̑Ή�
                // ����int,bool�����Ή����Ȃ�
                if (mLeftNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mRightNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || (mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != mRightNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType())
                    || (mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    || (mRightNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && mRightNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // ����]��
                mLeftNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // ����EvaluateInfo��`�B�ݒ�
                if (mExpr.mOpKind == OpKind.Assign)
                {// Assign�̂Ƃ�����
                    // �ė��p�֎~�ݒ�
                    var ei = mLeftNode.GetEvaluateInfo();
                    ei.DisableReuseSR();

                    // �`�B
                    aComp.TransferredEvaluateInfoSet(ei);
                }

                // �E��]��
                mRightNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �`�B���Z�b�g
                aComp.TransferredEvaluateInfoReset();

                // Assign�Ȃ炱���ŏI��
                if (mExpr.mOpKind == OpKind.Assign)
                {
                    if (!mLeftNode.GetEvaluateInfo().SR.IsSame(mRightNode.GetEvaluateInfo().SR))
                    {// ���W�X�^���قȂ�΃��[�h���߂�ǉ�����
                        aComp.BCFunction.AddOPCode_SReg1_SReg2(
                            BCOpCode.OpType.LDSRSR
                            , mLeftNode.GetEvaluateInfo().SR
                            , mRightNode.GetEvaluateInfo().SR
                            );
                    }
                    return;
                }

                // bool�Ȃ牉�Z���ł��Ȃ��̂ŃG���[����
                if (mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() == BuiltInType.Bool)
                {
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mOpToken
                        , mLeftNode.GetEvaluateInfo().TypeInfo
                        , new TypeInfo(new TypeInfo.TypeSymbol(null, BuiltInType.SInt32), new TypeInfo.TypeAttribute(true,false))
                        ));
                }

                // ���Z�q�ɑΉ����閽�߂�I��
                BCOpCode.OpType opType = BCOpCode.OpType.NOP;
                switch (mExpr.mOpKind)
                {
                    case OpKind.AddAssign: opType = BCOpCode.OpType.ADDI32; break;
                    case OpKind.SubAssign: opType = BCOpCode.OpType.SUBI32; break;
                    case OpKind.MulAssign: opType = BCOpCode.OpType.MULS32; break;
                    case OpKind.DivAssign: opType = BCOpCode.OpType.DIVS32; break;
                    case OpKind.ModAssign: opType = BCOpCode.OpType.MODS32; break;
                    case OpKind.AndAssign: opType = BCOpCode.OpType.ANDI32; break;
                    case OpKind.OrAssign: opType = BCOpCode.OpType.ORI32; break;
                    case OpKind.XorAssign: opType = BCOpCode.OpType.XORI32; break;
                    case OpKind.LShiftAssign: opType = BCOpCode.OpType.SLLI32; break;
                    case OpKind.RShiftAssign: opType = BCOpCode.OpType.SLRI32; break;
                    default:
                        Assert.NotReachHere();
                        break;
                }

                // ���ߒǉ�
                aComp.BCFunction.AddOPCode_SReg1_SReg2_SReg3(
                    opType
                    , mLeftNode.GetEvaluateInfo().SR
                    , mLeftNode.GetEvaluateInfo().SR
                    , mRightNode.GetEvaluateInfo().SR
                    );
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // OnParentEvaluateEnd
                mRightNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                mLeftNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ�����
        Token mOpToken;
        OpKind mOpKind;
        IExpression mLeftExpr;
        IExpression mRightExpr;
    }
}
