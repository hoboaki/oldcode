using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �V���{���̃c���[�B
    /// </summary>
    class SymbolTree
    {
        //------------------------------------------------------------
        // �G���[�̎�ށB
        public enum ErrorKind
        {
            NONE,
            NODE_NAME_IS_ALREADY_EXIST_AS_NOT_NAMESPACE,
            NODE_NAME_IS_ALREADY_EXIST,
        }

        //------------------------------------------------------------
        // �G���[���B
        public class  ErrorInfo
        {
            public readonly ErrorKind Kind;
            public readonly ModuleContext ModuleContext;
            public readonly Token Token;

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public ErrorInfo(ErrorKind aErrorKind, ModuleContext aModuleContext, Token aErrorToken)
            {
                Kind = aErrorKind;
                ModuleContext = aModuleContext;
                Token = aErrorToken;
            }
        };

        //------------------------------------------------------------
        // �G���[�����i�[����N���X�B
        public class ErrorInfoHolder
        {
            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public ErrorInfoHolder()
            {
            }

            //------------------------------------------------------------
            // �ݒ�֐��B
            public void Set( ErrorInfo aInfo )
            {
                mInfo = aInfo;
            }
            
            //------------------------------------------------------------
            // �擾�֐��B
            public ErrorInfo Get()
            {
                return mInfo;
            }

            //============================================================
            private ErrorInfo mInfo;
        }

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SymbolTree()
        {
            mRoot = new RootSymbolNode();
        }
        
        //============================================================
        RootSymbolNode mRoot;
        ErrorInfo mErrorInfo;

        //------------------------------------------------------------
        // �G���[������ݒ肷��B
        void setErrorInfo(ModuleContext aModuleContext, ErrorKind aErrorKind, Token aErrorToken)
        {
            mErrorInfo = new ErrorInfo(aErrorKind, aModuleContext, aErrorToken);
        }

        //------------------------------------------------------------
        // ���W���[����ǉ�����B
        public bool Add(ModuleContext aModuleContext)
        {
            // �����p�X�̂��̂����ɒǉ�����Ă��Ȃ����`�F�b�N���ǉ�����            
            ISymbolNode node = mRoot;
            IdentPath identPath = aModuleContext.ModuleDecl.IdentifierPath;
            for (int i = 0; i < identPath.Count; ++i)
            {
                ISymbolNode nextNode = node.FindChildNode(identPath.At(i));
                if (i + 1 == identPath.Count)
                {// �Ō�
                    if (nextNode != null)
                    {// �Ō�Ȃ̂ɑ��݂��Ă���
                        setErrorInfo(aModuleContext, ErrorKind.NODE_NAME_IS_ALREADY_EXIST, node.GetIdentifier().Token);
                        return false;
                    }
                    break;
                }

                if ( nextNode == null)
                {// ���݂��Ȃ��悤�Ȃ̂Œǉ�
                    NamespaceSymbolNode addNode = new NamespaceSymbolNode(node, identPath.At(i));
                    if (node.GetNodeKind() == SymbolNodeKind.Root)
                    {
                        ((RootSymbolNode)node).AddNode(addNode);
                    }
                    else
                    {
                        ((NamespaceSymbolNode)node).AddNode(addNode);
                    }
                    node = addNode;
                }                
                else
                {// ���ɑ��݂��Ă���
                    node = nextNode;
                    if (node.GetNodeKind() != SymbolNodeKind.NameSpace)
                    {// ���O��Ԃ���Ȃ����̂Ƃ��đ��݂���
                        setErrorInfo(aModuleContext, ErrorKind.NODE_NAME_IS_ALREADY_EXIST_AS_NOT_NAMESPACE, node.GetIdentifier().Token);
                        return false;
                    }
                }
            }
            System.Diagnostics.Debug.Assert(node.GetNodeKind() == SymbolNodeKind.NameSpace);

            // Module�m�[�h��ǉ�
            ((NamespaceSymbolNode)node).AddNode(new ModuleSymbolNode(node, aModuleContext));

            return true;
        }

        //------------------------------------------------------------
        // �SModule��W�J����B
        bool Expand()
        {
            ErrorInfoHolder holder = new ErrorInfoHolder();

            // �^
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.TypeNode))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // �ϐ�
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.VariableNode))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // �֐��̐錾
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.FunctionNodeDecl))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // �֐��̎���
            if (!mRoot.SymbolExpand(holder, SymbolExpandCmdKind.FunctionNodeImpl))
            {
                mErrorInfo = holder.Get();
                return false;
            }

            // �W�J����
            return true;
        }
    }
}
