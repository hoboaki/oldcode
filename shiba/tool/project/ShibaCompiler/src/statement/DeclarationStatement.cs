using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �錾���B
    /// </summary>
    class DeclarationStatement : IStatement
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public DeclarationStatement(VariableDecl aVariableDecl, bool aIsConst)
        {
            mVariableDecl = aVariableDecl;
            mIsConst = aIsConst;
        }

        //------------------------------------------------------------
        // �Ӗ���́B
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // TypeInfo�쐬
            TypeInfo typeInfo = aComp.CreateTypeInfo(mVariableDecl.TypePath, mIsConst, false);

            // �V���{���m�[�h���쐬
            VariableSymbolNode symbolNode = new VariableSymbolNode(
                aComp.PrevSymbolNode()
                , mVariableDecl.Ident
                , typeInfo
                );
            aComp.AddSymbolNode(symbolNode);

            // �]���m�[�h���쐬
            var evaluateNode = new EvaluateNode(symbolNode,mVariableDecl.Expression());

            // �]������
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

            // �]�����s
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

            // �e�̕]���I�� 
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

            // ���蓖�čς݃V���{���m�[�h�Ƃ��ēo�^
            aComp.AddEvaluatedSymbolNode(new EvaluatedSymbolNode(symbolNode, evaluateNode));

            // Scope�I���C�x���g�̂��߂ɒǉ�
            aComp.AddEvaluateNode(evaluateNode);
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("DeclarationStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                mVariableDecl.Trace(aTracer, "mVariableDecl");
                aTracer.WriteValue("mIsConst", mIsConst.ToString());
            }
        }

        //============================================================

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(VariableSymbolNode aSymbol, IExpression aExpr)
            {
                mSymbol = aSymbol;
                mExpr = aExpr;
            }

            //------------------------------------------------------------
            // TypeInfo���擾����B
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
                    case EvaluateNodeEventKind.InsertOpCodeOnScopeLeave: eventInsertOpCodeOnScopeLeave(aComp); break;
                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            VariableSymbolNode mSymbol;
            IExpression mExpr;
            IEvaluateNode mExprNode;
            EvaluateInfo mEvaluateInfo;
            
            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // �^���擾
                TypeInfo typeInfo = mSymbol.GetTypeInfo();
                
                // todo: �����Ȍ^�̑Ή�
                // - �����_��int,bool�̂ݑΉ�
                if (typeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                    || (typeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && typeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, typeInfo.Symbol.GetToken());
                }

                // �]�����쐬
                mEvaluateInfo = EvaluateInfo.CreateAsValue(typeInfo);

                // �������������邩
                if (mExpr != null)
                {// ���������̌��ʂŃ��[�h���߂�����
                    // �쐬
                    mExprNode = mExpr.CreateEvaluateNode();

                    // �]��
                    mExprNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                }
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // ���W�X�^�m�ہ@
                mEvaluateInfo.SR = aComp.SRReserve();

                // �������������邩�ۂ��̕���
                if (mExpr == null)
                {// �������q���Ȃ��̂Ŋ���l�ŏ���������

                    // todo: �g�ݍ��݌^�ȊO�̑Ή�
                    if (mEvaluateInfo.TypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn)
                    {// 
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, mEvaluateInfo.TypeInfo.Symbol.GetToken());
                    }

                    // �g�ݍ��݌^�Ȃ�[��������
                    aComp.BCFunction.AddOPCode_SReg(
                        BCOpCode.OpType.LDSRZR
                        , mEvaluateInfo.SR
                        );
                }
                else
                {// ���������̌��ʂŃ��[�h���߂�����
                    // �]������`�B
                    aComp.TransferredEvaluateInfoSet(mEvaluateInfo);

                    // �]��
                    mExprNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                    // �`�B���Z�b�g
                    aComp.TransferredEvaluateInfoReset();

                    // todo:
                    // �Öق̕ϊ��̑Ή�

                    // �^�m�F
                    Assert.Check(mExprNode.GetEvaluateInfo().Kind == EvaluateInfo.InfoKind.Value);
                    EvaluateInfo exprEI = mExprNode.GetEvaluateInfo();
                    TypeInfo exprTypeInfo = exprEI.TypeInfo;
                    if (exprTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                        || (exprTypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && exprTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                        )
                    {// todo: ����bool��int�����T�|�[�g���Ȃ�
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, exprTypeInfo.Symbol.GetToken());
                    }

                    // �^���������m�F
                    if (mEvaluateInfo.TypeInfo.Symbol.GetBuiltInType() != exprTypeInfo.Symbol.GetBuiltInType())
                    {// �Ⴄ�^
                        // �G���[���b�Z�[�W
                        throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                            SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                            , aComp.TypeSymbolNode.ModuleContext()
                            , mSymbol.GetIdentifier().Token
                            , exprTypeInfo
                            , mEvaluateInfo.TypeInfo
                            ));
                    }

                    // ���W�X�^���قȂ�΃��[�h���߂�ǉ�����
                    if (!mEvaluateInfo.SR.IsSame(exprEI.SR))
                    {
                        aComp.BCFunction.AddOPCode_SReg1_SReg2(
                            BCOpCode.OpType.LDSRSR
                            , mEvaluateInfo.SR
                            , exprEI.SR
                            );
                    }
                    // �e�̕]�����I��������Ƃ�������
                    mExprNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                }
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // struct�̃f�X�g���N�^�Ăяo��

                // ���W�X�^���
                aComp.SRRelease(mEvaluateInfo.SR);
            }
            
            //------------------------------------------------------------
            // �X�R�[�v���E���߂�}���B
            void eventInsertOpCodeOnScopeLeave(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // struct�̃f�X�g���N�^�Ăяo��
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ�����
        VariableDecl mVariableDecl;
        bool mIsConst;
    }
}
