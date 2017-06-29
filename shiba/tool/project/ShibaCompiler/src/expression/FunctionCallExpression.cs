using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �֐��Ăяo�����B
    /// </summary>
    class FunctionCallExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionCallExpression(Token aOpToken)
        {
            mOpToken = aOpToken;
        }

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionCallExpression(Token aOpToken, SequenceExpression aSequenceExpr)
        {
            mOpToken = aOpToken;
            mSequenceExpr = aSequenceExpr;
        }

        //------------------------------------------------------------
        // �]���m�[�h���쐬����B
        public IEvaluateNode CreateEvaluateNode()
        {
            // ���̊֐��ō쐬���邱�Ƃ����̎��_�ł͋֎~����
            //�i�֎~���Ȃ��Ă悢���@���v�������炻�̂Ƃ��ɑΏ�����j
            Assert.NotReachHere();
            return null;
        }

        //------------------------------------------------------------
        // �]���m�[�h���쐬����B
        public IEvaluateNode CreateEvaluateNodeWithFirstExpr(IExpression aFirstExpr)
        {
            return new EvaluateNode(aFirstExpr, this);
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
            aTracer.WriteName("FunctionCallExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mSequenceExpr.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(IExpression aFirstExpr, FunctionCallExpression aFuncExpr)
            {
                mFirstExpr = aFirstExpr;
                mFirstNode = mFirstExpr.CreateEvaluateNode();
                mFuncExpr = aFuncExpr;
                if (mFuncExpr.mSequenceExpr != null)
                {
                    mSeqNode = mFuncExpr.mSequenceExpr.CreateEvaluateNode();
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
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            IExpression mFirstExpr;
            IEvaluateNode mFirstNode;
            IEvaluateNode mSeqNode;
            FunctionCallExpression mFuncExpr;
            EvaluateInfo mEvaluateInfo;
            TransferredEIHolder mTransferredEI;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // �֐��V���{�����ǂ����̃`�F�b�N
                if (mFirstNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.StaticSymbol
                    || mFirstNode.GetEvaluateInfo().Symbol.GetNodeKind() != SymbolNodeKind.Function
                    )
                {// �G���[
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.FUNCTION_SYMBOL_EXPECTED, mFirstExpr.GetToken());
                }

                // ��������̑Ή�
                if (mSeqNode != null)
                {
                    mSeqNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                }

                // �V���{���̎擾
                FunctionSymbolNode funcSymbol = (FunctionSymbolNode)mFirstNode.GetEvaluateInfo().Symbol;

                // �]�������쐬
                mEvaluateInfo = EvaluateInfo.CreateAsValue(funcSymbol.ReturnTypeInfo());

                // �`�����z���_�[�̍쐬
                mTransferredEI = new TransferredEIHolder(mEvaluateInfo);
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^�̊m��
                if (mEvaluateInfo.TypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn // �g�ݍ��݌^����Ȃ�
                    || mEvaluateInfo.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Void // void����Ȃ�
                    )
                {// ���W�X�^���K�v
                    // �`�������擾���Ă݂�
                    mTransferredEI.ReceiveAndSetSR(aComp);
                }

                // �P�߂�]��
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // �V���{���̎擾
                FunctionSymbolNode funcSymbol = (FunctionSymbolNode)mFirstNode.GetEvaluateInfo().Symbol;

                // �����̑Ή�
                if (mSeqNode != null)
                {
                    // �܂��]���ݒ�
                    mSeqNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                    // �֐����ݒ�
                    aComp.FunctionCallTargetSet(funcSymbol);

                    // �߂�l�p��FR���m��
                    if (mEvaluateInfo.SR.IsValid)
                    {
                        aComp.FunctionCallFRNextIndex();
                    }

                    // ������FR���m��
                    mSeqNode.SendEvent(aComp, EvaluateNodeEventKind.SetupFR);

                    // �֐���񃊃Z�b�g
                    aComp.FunctionCallTargetReset();
                }

                // �֐��R�[������
                aComp.BCFunction.AddOPCode_SymbolTableIndex(
                    BCOpCode.OpType.CALL
                    , funcSymbol
                    );

                // �֐��̌��ʂ��󂯎��
                if (mEvaluateInfo.SR.IsValid)
                {
                    aComp.BCFunction.AddOPCode_SReg(
                        BCOpCode.OpType.LDSRFZ
                        , mEvaluateInfo.SR
                        );
                }

                // �]���I���C�x���g���M
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^���
                mTransferredEI.ReleaseIfNeccesary(aComp);
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ������B
        Token mOpToken;
        SequenceExpression mSequenceExpr = null;
    }
}
