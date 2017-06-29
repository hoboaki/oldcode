using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
            NOT_DECLARATION_IDENT,
            NOT_SUPPORTED_EXPRESSION,
            NOT_SUPPORTED_TYPENAME,
            CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB,
            INVALID_BREAK,
            INVALID_CONTINUE,
            SEMICOLON_EXPECTED,
            EXPRESSION_EXPECTED,
            FUNCTION_SYMBOL_EXPECTED,
        }

        //------------------------------------------------------------
        // �G���[���B
        public struct  ErrorInfo
        {
            public readonly ErrorKind Kind;
            public readonly ModuleContext ModuleContext;
            public readonly Token Token;
            public readonly TypeInfo TypeInfoA;
            public readonly TypeInfo TypeInfoB;

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public ErrorInfo(ErrorKind aErrorKind, ModuleContext aModuleContext, Token aErrorToken)
                : this(aErrorKind, aModuleContext, aErrorToken, null, null)
            {
            }

            //------------------------------------------------------------
            //  �^���Q����R���X�g���N�^�B
            public ErrorInfo(ErrorKind aErrorKind, ModuleContext aModuleContext, Token aErrorToken, TypeInfo aTypeInfoA, TypeInfo aTypeInfoB)
            {
                Kind = aErrorKind;
                ModuleContext = aModuleContext;
                Token = aErrorToken;
                TypeInfoA = aTypeInfoA;
                TypeInfoB = aTypeInfoB;
            }
        };
        
        //------------------------------------------------------------
        // �G���[���ݒ肳�ꂽ�Ƃ��ɍ�����O�B
        public class ErrorException : Exception
        {
            public ErrorInfo Info;

            //------------------------------------------------------------
            // �R���X�g���N�^�B
            public ErrorException(ErrorInfo aErrorInfo)
                : base()
            {
                Info = aErrorInfo;
            }
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public SymbolTree()
        {
            mRoot = new RootSymbolNode();
            mModuleSymbolNodeList = new List<ModuleSymbolNode>();
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            mRoot.Trace(aTracer);
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

                if (nextNode == null)
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
            Assert.Check(node.GetNodeKind() == SymbolNodeKind.NameSpace);

            // Module�m�[�h��ǉ�
            var moduleSymbolNode = new ModuleSymbolNode(node, aModuleContext);
            ((NamespaceSymbolNode)node).AddNode(moduleSymbolNode);
            mModuleSymbolNodeList.Add(moduleSymbolNode);

            return true;
        }

        //------------------------------------------------------------
        // �SModule��W�J����B
        public bool Expand()
        {
            try
            {
                // �^
                mRoot.SymbolExpand(SymbolExpandCmdKind.TypeNode);

                // �ϐ�
                mRoot.SymbolExpand(SymbolExpandCmdKind.VariableNode);

                // �֐��̐錾
                mRoot.SymbolExpand(SymbolExpandCmdKind.FunctionNodeDecl);

                // �֐��̎���
                mRoot.SymbolExpand(SymbolExpandCmdKind.FunctionNodeImpl);

                // �o�͏���
                foreach (var entry in mModuleSymbolNodeList)
                {
                    entry.ReadyToOutput();
                }
            }
            catch (ErrorException aException)
            {// �G���[����
                mErrorInfo = aException.Info;
                return false;
            }

            // �W�J����
            return true;
        }

        //------------------------------------------------------------
        // �SModule���t�@�C���ɏo�͂���B
        public void WriteToXML(string aOutputDirPath)
        {
            foreach (var entry in mModuleSymbolNodeList)
            {
                entry.WriteToXML(aOutputDirPath);
            }
        }

        //------------------------------------------------------------
        // �SModule��XData�����ă_���v����B
        public void XDataDump()
        {
            foreach (var entry in mModuleSymbolNodeList)
            {
                entry.XDataDump();
            }
        }

        //------------------------------------------------------------
        // �G���[�����擾����B
        public ErrorInfo GetErrorInfo()
        {
            return mErrorInfo;
        }
        
        //============================================================
        RootSymbolNode mRoot;
        ErrorInfo mErrorInfo;
        List<ModuleSymbolNode> mModuleSymbolNodeList;

        //------------------------------------------------------------
        // �G���[������ݒ肷��B
        void setErrorInfo(ModuleContext aModuleContext, ErrorKind aErrorKind, Token aErrorToken)
        {
            mErrorInfo = new ErrorInfo(aErrorKind, aModuleContext, aErrorToken);
        }
    }
}
