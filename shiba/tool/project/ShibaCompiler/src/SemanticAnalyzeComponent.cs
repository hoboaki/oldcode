using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �Ӗ���͗p�R���|�[�l���g�B
    /// </summary>
    class SemanticAnalyzeComponent
    {
        //------------------------------------------------------------
        // ���J�����o�ϐ��B
        public readonly BCFunction BCFunction;
        public readonly TypeSymbolNode TypeSymbolNode;
        public readonly ISymbolNode BeginSymbolNode;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SemanticAnalyzeComponent(
            BCFunction aBCFunction
            , TypeSymbolNode aTypeSymbolNode
            , ISymbolNode aBeginSymbolNode
            )
        {
            BCFunction = aBCFunction;
            TypeSymbolNode = aTypeSymbolNode;
            BeginSymbolNode = aBeginSymbolNode;
            mPrevSymbolNode = aBeginSymbolNode;
            mFreeStackRegister = new Stack<uint>();
            mSymbolNodeStack = new Stack<ISymbolNode>();
            mEvaluateScopeStack = new Stack<EvaluateScope>();
        }

        //------------------------------------------------------------
        // �G���[��O��throw����֐��B
        public void ThrowErrorException(SymbolTree.ErrorKind aErrorKind, Token aToken)
        {
            throw new SymbolTree.ErrorException(
                new SymbolTree.ErrorInfo(aErrorKind, TypeSymbolNode.ModuleContext(), aToken)
                );
        }

        //------------------------------------------------------------
        // TypeInfo�𐶐�����B
        public TypeInfo CreateTypeInfo(TypePath aTypePath,bool aIsConst, bool aIsRef)
        {
            // todo:
            // ���͑g�ݍ��݌^�����Ή����Ă��܂���
            Assert.Check(aTypePath.BuiltInType != BuiltInType.Unknown);

            // �g�ݍ��݌^�Ƃ��č쐬����
            BuiltInType builtInType = aTypePath.BuiltInType;
            return new TypeInfo(
                new TypeInfo.TypeSymbol(aTypePath.BuiltInToken, builtInType)
                , new TypeInfo.TypeAttribute(aIsConst, aIsRef)
                );
        }

        //------------------------------------------------------------
        // ���O��SymbolNode���擾����B
        public ISymbolNode PrevSymbolNode()
        {
            return mPrevSymbolNode;
        }

        //------------------------------------------------------------
        // SymbolNode�����݂̃X�R�[�v�ɒǉ�����B
        public void AddSymbolNode(ISymbolNode aNode)
        {
            // ���ONode�̍X�V
            mPrevSymbolNode = aNode;
        }

        //------------------------------------------------------------
        // EvaluateNode�����݂̃X�R�[�v�ɒǉ�����B
        public void AddEvaluateNode(IEvaluateNode aNode)
        {
            currentScope().AddEvaluateNode(aNode);
        }

        //------------------------------------------------------------
        // �]���ς݃m�[�h�Ƃ��ăV���{���m�[�h��o�^����B
        public void AddEvaluatedSymbolNode(EvaluatedSymbolNode aEvaluatedSymbolNode)
        {
            currentScope().AddEvaluatedSymbolNode(aEvaluatedSymbolNode);
        }

        //------------------------------------------------------------
        // �߂�l�p�̕]������o�^����B
        public void ReturnEvaluateInfoSet(EvaluateInfo aEvaluateInfo)
        {
            Assert.Check(mReturnEvaluateInfo == null);
            mReturnEvaluateInfo = aEvaluateInfo;
        }

        //------------------------------------------------------------
        // �߂�l�p�̕]�������擾����B�߂�l���Ȃ����null��Ԃ��B
        public EvaluateInfo ReturnEvaluateInfoGet()
        {
            return mReturnEvaluateInfo;
        }

        //------------------------------------------------------------
        // ���݂̃X�R�[�v��continue�p�̃��x����o�^����B
        public void RegisterLabelContinue(BCLabel aLabel)
        {
            currentScope().RegisterLabelContinue(aLabel);
        }

        //------------------------------------------------------------
        // ���݂̃X�R�[�v��break�p�̃��x����o�^����B
        public void RegisterLabelBreak(BCLabel aLabel)
        {
            currentScope().RegisterLabelBreak(aLabel);
        }

        //------------------------------------------------------------
        // ���݂̃X�R�[�v��return�p�̃��x����o�^����B
        public void RegisterLabelReturn(BCLabel aLabel)
        {
            currentScope().RegisterLabelReturn(aLabel);
        }

        //------------------------------------------------------------
        // �w��̖��O�̃V���{���m�[�h��T���B
        public ISymbolNode FindSymbolNode(Identifier aIdent)
        {
            for(var node = mPrevSymbolNode;
                node != null && node.GetNodeKind() != SymbolNodeKind.Root;
                node = node.ParentNode()
                )
            {
                if (node.GetIdentifier().IsSame(aIdent))
                {
                    return node;
                }

                var childNode = node.FindChildNode(aIdent);
                if (childNode != null)
                {
                    return childNode;
                }
            }
            return null;
        }

        //------------------------------------------------------------
        // �w��̃V���{���m�[�h�ɑΉ�����]���ς݃m�[�h��T���B
        public EvaluatedSymbolNode FindEvaluatedSymbolNode(ISymbolNode aNode)
        {
            foreach (var scope in mEvaluateScopeStack)
            {
                var node = scope.FindEvaluatedSymbolNode(aNode);
                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }

        //------------------------------------------------------------
        // �X�^�b�N���W�X�^���؂��B
        public StackRegister SRReserve()
        {
            // todo:
            // 256�𒴂����Ƃ��̏����B

            // �󂫂�����Ȃ炻������炤
            if (mFreeStackRegister.Count != 0)
            {
                return new StackRegister(mFreeStackRegister.Pop());
            }

            // �m�ہ@
            StackRegister sr = new StackRegister(mStackRegisterIndexNext);
            ++mStackRegisterIndexNext;

            // �s�[�N�l���L��
            mStackRegisterIndexPeak = Math.Max(mStackRegisterIndexPeak, mStackRegisterIndexNext);

            // ���ʂ�Ԃ�
            return sr;
        }

        //------------------------------------------------------------
        // �X�^�b�N���W�X�^��Ԃ��B
        public void SRRelease(StackRegister aSR)
        {
            if (mFreeStackRegister.Count == 0
                && aSR.Index() + 1 == mStackRegisterIndexNext
                )
            {// ��ԍŌ�ɓn�������W�X�^���Ԃ��ꂽ�̂�Next�l���P������B
                --mStackRegisterIndexNext;
            }
            else
            {// �󂫃X�^�b�N�ɒǉ�
                mFreeStackRegister.Push(aSR.Index());
            }
        }

        //------------------------------------------------------------
        // �m�ۃ��W�X�^�̃s�[�N�����擾����B
        public uint SRPeakCount()
        {
            return mStackRegisterIndexPeak;
        }

        //------------------------------------------------------------
        // �`�B����EvaluateInfo��ݒ肷��B
        public void TransferredEvaluateInfoSet(EvaluateInfo aEvaluateInfo)
        {
            Assert.Check(mTransferredEvaluateInfo == null);
            mTransferredEvaluateInfo = aEvaluateInfo;
        }

        //------------------------------------------------------------
        // �`�B���ꂽEvaluateInfo���擾����B�擾������`�B���ꂽEvaluateInfo�͎����I��null���ݒ肳���B
        public EvaluateInfo TransferredEvaluateInfoReceive()
        {
            EvaluateInfo result = mTransferredEvaluateInfo;
            mTransferredEvaluateInfo = null;
            return result;
        }

        //------------------------------------------------------------
        // �`�B���ꂽEvaluateInfo�����Z�b�g����B
        public void TransferredEvaluateInfoReset()
        {
            mTransferredEvaluateInfo = null;
        }

        //------------------------------------------------------------
        // �X�R�[�v�ɓ���Ƃ��̏����B
        public void ScopeEnter()
        {
            // PrevSymbolNode��Push
            mSymbolNodeStack.Push(mPrevSymbolNode);

            // �X�R�[�v��Push
            mEvaluateScopeStack.Push(new EvaluateScope());
        }

        //------------------------------------------------------------
        // �X�R�[�v����o��Ƃ��̏����B
        public void ScopeLeave()
        {
            // �X�R�[�v���E���߂�}��
            currentScope().InsertOpCodeOnScopeLeave(this);

            // �X�R�[�v���E�C�x���g�𑗐M
            currentScope().SendEventRelease(this);

            // �X�R�[�v��Pop
            mEvaluateScopeStack.Pop();

            // PrevSymbolNode��Pop
            mPrevSymbolNode = mSymbolNodeStack.Pop();
        }

        //------------------------------------------------------------
        // Break�p�̃��x����T����Break�������s����B
        public BCLabel ExecBreakStatement()
        {
            // �܂�Label�̂���X�R�[�v��T��
            EvaluateScope targetScope = null;
            foreach (var entry in mEvaluateScopeStack)
            {
                var label = entry.LabelBreak();
                if (label != null)
                {
                    targetScope = entry;
                    break;
                }
            }
            if (targetScope == null)
            {// ������܂���ł����B
                return null;
            }

            // �ڕW�̃X�R�[�v�܂ŃX�R�[�v���E���߂�}��
            foreach (var entry in mEvaluateScopeStack)
            {
                entry.InsertOpCodeOnScopeLeave(this);
                if (entry == targetScope)
                {
                    break;
                }
            }

            // ���x����Ԃ�
            return targetScope.LabelBreak();
        }

        //------------------------------------------------------------
        // Continue�p�̃��x����T����Continue�������s����B
        public BCLabel ExecContinueStatement()
        {
            // �܂�Label�̂���X�R�[�v��T��
            EvaluateScope targetScope = null;
            foreach (var entry in mEvaluateScopeStack)
            {
                var label = entry.LabelContinue();
                if (label != null)
                {
                    targetScope = entry;
                    break;
                }
            }
            if (targetScope == null)
            {// ������܂���ł����B
                return null;
            }

            // �ڕW�̃X�R�[�v�܂ŃX�R�[�v���E���߂�}��
            foreach (var entry in mEvaluateScopeStack)
            {
                entry.InsertOpCodeOnScopeLeave(this);
                if (entry == targetScope)
                {
                    break;
                }
            }

            // ���x����Ԃ�
            return targetScope.LabelContinue();
        }

        //------------------------------------------------------------
        // Return�p�̃��x����T����Return�������s����B
        public BCLabel ExecReturnStatement()
        {
            // �܂�Label�̂���X�R�[�v��T��
            EvaluateScope targetScope = null;
            foreach (var entry in mEvaluateScopeStack)
            {
                var label = entry.LabelReturn();
                if (label != null)
                {
                    targetScope = entry;
                    break;
                }
            }
            if (targetScope == null)
            {// ������܂���ł����B
                return null;
            }

            // �ڕW�̃X�R�[�v�܂ŃX�R�[�v���E���߂�}��
            foreach (var entry in mEvaluateScopeStack)
            {
                entry.InsertOpCodeOnScopeLeave(this);
                if (entry == targetScope)
                {
                    break;
                }
            }

            // ���x����Ԃ�
            return targetScope.LabelReturn();
        }

        //------------------------------------------------------------
        // �֐��Ăяo���̃^�[�Q�b�g��ݒ肵�܂��B
        public void FunctionCallTargetSet(FunctionSymbolNode aNode)
        {
            Assert.Check(mFunctionCallTarget == null);
            mFunctionCallTarget = aNode;
            mFunctionCallFRNextIndex = 0; // FR�̃C���f�b�N�X�����Z�b�g
        }

        //------------------------------------------------------------
        // �֐��Ăяo���̃^�[�Q�b�g�����Z�b�g���܂��B
        public void FunctionCallTargetReset()
        {
            mFunctionCallTarget = null;
        }

        //------------------------------------------------------------
        // �֐��Ăяo���Ŏg�p����A���̈������W�X�^�̃C���f�b�N�X���擾����B
        public byte FunctionCallFRNextIndex()
        {
            Assert.Check(mFunctionCallTarget != null);
            byte frIndex = mFunctionCallFRNextIndex;
            ++mFunctionCallFRNextIndex;
            return frIndex;
        }

        //============================================================
        private uint mStackRegisterIndexNext;
        private uint mStackRegisterIndexPeak;
        private Stack<uint> mFreeStackRegister;
        private Stack<ISymbolNode> mSymbolNodeStack;
        private Stack<EvaluateScope> mEvaluateScopeStack;
        private ISymbolNode mPrevSymbolNode;
        private EvaluateInfo mTransferredEvaluateInfo;
        private EvaluateInfo mReturnEvaluateInfo;
        private FunctionSymbolNode mFunctionCallTarget;
        private byte mFunctionCallFRNextIndex;

        //------------------------------------------------------------
        // ��ԍŌ��Push�����X�R�[�v���擾����B
        EvaluateScope currentScope()
        {
            return mEvaluateScopeStack.Peek();
        }
    }
}
