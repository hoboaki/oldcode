using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �萔���B
    /// </summary>
    class ConstantLiteralExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ConstantLiteralExpression(Token aT)
        {
            mToken = aT;
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
            return mToken;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ConstantLiteralExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mToken.Value", mToken.Value.ToString());
                switch (mToken.Value)
                {
                    case Token.Kind.StringChar8:
                    case Token.Kind.StringChar16:
                    case Token.Kind.StringChar32:
                        aTracer.WriteValue("value", mToken.UString);
                        break;

                    case Token.Kind.NumSInt32:
                    case Token.Kind.NumSInt64:
                        aTracer.WriteValue("value", mToken.Int64Value.ToString());
                        break;

                    case Token.Kind.NumUInt32:
                    case Token.Kind.NumUInt64:
                        aTracer.WriteValue("value", mToken.UInt64Value.ToString());
                        break;

                    case Token.Kind.NumFloat32:
                        aTracer.WriteValue("value", mToken.Float32Value.ToString());
                        break;

                    case Token.Kind.NumFloat64:
                        aTracer.WriteValue("value", mToken.Float64Value.ToString());
                        break;

                    default:
                        break;
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
            public EvaluateNode(ConstantLiteralExpression aExpr)
            {
                mExpr = aExpr;
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
            ConstantLiteralExpression mExpr;
            EvaluateInfo mEvaluateInfo;
            EvaluateInfo mTransferredEI;
            
            //------------------------------------------------------------�@
            // �]�������B�@
            bool eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // BuiltInType�̑I��
                BuiltInType builtInType;
                switch (mExpr.mToken.Value)
                {
                    case Token.Kind.NumSInt32:
                        builtInType = BuiltInType.SInt32;
                        break;

                    case Token.Kind.KeyTrue:
                    case Token.Kind.KeyFalse:
                        builtInType = BuiltInType.Bool;
                        break;

                    default:
                        // todo: �����Ȍ^�̑Ή�
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mToken);
                        return false;
                }

                // EvaluateInfo�쐬
                TypePath typePath = new TypePath(mExpr.mToken, builtInType);
                mEvaluateInfo = EvaluateInfo.CreateAsValue(
                    aComp.CreateTypeInfo(typePath, true, true)
                    );

                return true;
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            bool eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // �`�B���ꂽ�]�������擾
                mTransferredEI = aComp.TransferredEvaluateInfoReceive();

                // ���W�X�^�m�ہ@
                mEvaluateInfo.SR = mTransferredEI == null
                    ? aComp.SRReserve()
                    : mTransferredEI.SR;

                // ���[�h����
                switch (mExpr.mToken.Value)
                {
                    case Token.Kind.NumSInt32:
                        aComp.BCFunction.AddOPCode_SReg_ConstantTableIndex(
                            BCOpCode.OpType.LDSRC4
                            , mEvaluateInfo.SR
                            , (int)mExpr.mToken.Int64Value
                            );
                        break;

                    case Token.Kind.KeyTrue:
                        aComp.BCFunction.AddOPCode_SReg(
                            BCOpCode.OpType.LDSRBT
                            , mEvaluateInfo.SR
                            );
                        break;

                    case Token.Kind.KeyFalse:
                        aComp.BCFunction.AddOPCode_SReg(
                            BCOpCode.OpType.LDSRZR
                            , mEvaluateInfo.SR
                            );
                        break;

                    default:
                        // todo: �����Ȍ^�̑Ή�
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mToken);
                        return false;
                }

                // ����
                return true;
            }

            //------------------------------------------------------------
            // ��n���B
            bool eventRelease(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^�ԋp
                if (mTransferredEI == null)
                {
                    aComp.SRRelease(mEvaluateInfo.SR);
                }

                return true;
            }

            //------------------------------------------------------------
            // ���̏I���B
            bool eventOnStatementEnd(SemanticAnalyzeComponent aComp)
            {
                // ������邱�Ƃ͂Ȃ��͂�
                return true;
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ������B
        Token mToken;
    }
}
