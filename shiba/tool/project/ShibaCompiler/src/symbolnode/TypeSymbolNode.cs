using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Type�m�[�h�B
    /// </summary>
    class TypeSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public TypeSymbolNode(ISymbolNode aParent, BCModule aBCModule, StaticTypeDef aStaticTypeDef)
        {
            mParent = aParent;
            mBCModule = aBCModule;
            mStaticTypeDef = aStaticTypeDef;
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // �������g��Ident�B
        public Identifier GetIdentifier()
        {
            return mStaticTypeDef.Ident;
        }

        //------------------------------------------------------------
        // ���j�[�N�ȃt���p�X�B
        public string GetUniqueFullPath()
        {
            return SymbolNodeUtil.FullPath(this);
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ށB
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Type;
        }

        //------------------------------------------------------------
        // �e�m�[�h�B
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // �w���Ident�̃m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return mNodeList.FindNode(aIdent);
        }

        //------------------------------------------------------------
        // �W�J���߁B
        public void SymbolExpand(SymbolExpandCmdKind aCmd)
        {
            switch (aCmd)
            {
                case SymbolExpandCmdKind.TypeNode:
                    expandTypeNode();
                    break;

                case SymbolExpandCmdKind.FunctionNodeDecl:
                    expandFunctionNodeDecl();
                    break;

                case SymbolExpandCmdKind.VariableNode:
                    expandVariableNode();
                    break;

                case SymbolExpandCmdKind.FunctionNodeImpl:
                    expandChilds(SymbolExpandCmdKind.FunctionNodeImpl);
                    break;

                default:
                    break;
            }
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(), "TypeSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var node in mNodeList)
                {
                    node.Trace(aTracer);
                }
            }
        }

        //------------------------------------------------------------
        // ModuleContext���擾����B
        public ModuleContext ModuleContext()
        {
            return mBCModule.ModuleContext();
        }

        //============================================================
        ISymbolNode mParent;
        BCModule mBCModule;
        StaticTypeDef mStaticTypeDef;
        SymbolNodeList mNodeList;
        BCObjectType mBCObjectType;
        
        //------------------------------------------------------------
        // TypeNode�W�J�B
        private void expandTypeNode()
        {
            // �����F���̎��_�Ŏq�m�[�h��0�Ȃ̂Ŏq�m�[�h�W�J�v���͏o���Ȃ�
            Assert.Check(mNodeList.Count() == 0);

            // �o�C�g�R�[�h�I�u�W�F�N�g�쐬
            mBCObjectType = mBCModule.GenerateObjectType(this);

            // �������g
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.StaticTypeDef != null)
                {
                    // �d���`�F�b�N
                    checkIdentDuplication(symbol.StaticTypeDef.Ident);

                    // �m�[�h�쐬
                    var newNode = new TypeSymbolNode(this, mBCModule, symbol.StaticTypeDef);

                    // �X�ɓW�J
                    newNode.SymbolExpand(SymbolExpandCmdKind.TypeNode);

                    // �ǉ�
                    addNode(newNode);
                }
            }
        }

        //------------------------------------------------------------
        // FunctionNode�錾�W�J�B
        private void expandFunctionNodeDecl()
        {
            // �܂��q�m�[�h
            expandChilds(SymbolExpandCmdKind.FunctionNodeDecl);

            // �V���{������
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.MemberFunctionDecl != null)
                {
                    // �d���`�F�b�N
                    checkIdentDuplication(symbol.MemberFunctionDecl.Ident);

                    // �m�[�h�쐬
                    var newNode = new FunctionSymbolNode(this, mBCObjectType, symbol.MemberFunctionDecl);

                    // �X�ɓW�J
                    newNode.SymbolExpand(SymbolExpandCmdKind.FunctionNodeDecl);

                    // �ǉ�
                    addNode(newNode);
                }
            }
        }

        //------------------------------------------------------------
        // VariableNode�W�J�B
        private void expandVariableNode()
        {
            // �܂��q�m�[�h
            expandChilds(SymbolExpandCmdKind.VariableNode);

            // todo:
            // ������todo�Ȃ񂾂�������@�i2010/08/07)
        }

        //------------------------------------------------------------
        // �q�m�[�h�̓W�J�B
        private void expandChilds(SymbolExpandCmdKind aCmd)
        {
            foreach (var node in mNodeList)
            {
                // �W�J
                node.SymbolExpand(aCmd);
            }
        }


        //------------------------------------------------------------
        // Ident�d���`�F�b�N�Btrue�Ȃ�d�����Ă���B
        private void checkIdentDuplication(Identifier aIdent)
        {
            if (FindChildNode(aIdent) != null)
            {
                throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(SymbolTree.ErrorKind.NODE_NAME_IS_ALREADY_EXIST, mBCModule.ModuleContext(), aIdent.Token));
            }
        }

        //------------------------------------------------------------
        // �m�[�h�̒ǉ��B
        private void addNode(ISymbolNode aNode)
        {
            Assert.Check(
                aNode.GetNodeKind() == SymbolNodeKind.Type
                || aNode.GetNodeKind() == SymbolNodeKind.Variable
                || aNode.GetNodeKind() == SymbolNodeKind.Function
                );
            mNodeList.Add(aNode);
        }       
    }
}
