using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Root�ƂȂ鎯�ʎq���B
    /// </summary>
    class RootIdentExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public RootIdentExpression(Identifier aIdent, bool aIsNamespaceRoot)
        {
            mIdent = aIdent;
            mIsNamespaceRoot = aIsNamespaceRoot;
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
            return mIdent.Token;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("RootIdentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mIdent.Trace(aTracer, "mIdent");
                aTracer.WriteValue("mIsNamespaceRoot", mIsNamespaceRoot.ToString());
            }
        }

        //============================================================

        //------------------------------------------------------------
        // �]���m�[�h�B
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public EvaluateNode(RootIdentExpression aExpr)
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
                        break;
                }
            }

            //============================================================
            RootIdentExpression mExpr;
            EvaluateInfo mEvaluateInfo;

            //------------------------------------------------------------�@
            // �]�������B�@
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // todo: Root�̑Ή�
                Assert.Check(!mExpr.mIsNamespaceRoot);

                // �V���{���̌���
                ISymbolNode symbolNode = aComp.FindSymbolNode(mExpr.mIdent);
                if (symbolNode == null)
                {// �V���{����������Ȃ�
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.NOT_DECLARATION_IDENT
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mIdent.Token
                        ));
                }

                // �]�����쐬
                if (symbolNode.GetNodeKind() == SymbolNodeKind.Variable)
                {// �ϐ�
                    // �Ή�����]���ς݃m�[�h��T��
                    EvaluatedSymbolNode evaluatedSymbolNode = aComp.FindEvaluatedSymbolNode(symbolNode);
                    if (evaluatedSymbolNode == null)
                    {// todo: ���[�J���ϐ��ȊO�̑Ή�
                        Assert.NotReachHere();
                    }
                    // �쐬
                    mEvaluateInfo = evaluatedSymbolNode.EvaluateNode.GetEvaluateInfo();
                }
                else
                {// �V���{��
                    mEvaluateInfo = EvaluateInfo.CreateAsStaticSymbol(symbolNode);
                }
            }

            //------------------------------------------------------------�@
            // �]�����s�B�@
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // ���[�J���ϐ��ȊO�Ȃ炱���Ń��W�X�^���m�ۂ��邱�Ƃ�����
            }

            //------------------------------------------------------------
            // ��n���B
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // ���[�J���ϐ��ȊO�Ȃ炱���Ń��W�X�^��������邱�Ƃ�����
            }

            //------------------------------------------------------------
            // ���̏I���B
            void eventOnStatementEnd(SemanticAnalyzeComponent aComp)
            {
            }
        };

        //------------------------------------------------------------
        // �����o�ϐ������B
        Identifier mIdent;
        bool mIsNamespaceRoot;
    }
}
