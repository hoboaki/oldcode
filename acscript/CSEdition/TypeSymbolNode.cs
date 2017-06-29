using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Type�m�[�h�B
    /// </summary>
    class TypeSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public TypeSymbolNode(ISymbolNode aParent, ModuleContext aModuleContext, StaticTypeDef aStaticTypeDef)
        {
            mParent = aParent;
            mModuleContext = aModuleContext;
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
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aErrorHolder, SymbolExpandCmdKind aCmd)
        {
            switch (aCmd)
            {
                case SymbolExpandCmdKind.TypeNode:
                    return expandTypeNode(aErrorHolder);

                case SymbolExpandCmdKind.FunctionNodeDecl:
                    return expandFunctionNodeDecl(aErrorHolder);

                default:
                    return true;
            }
        }

        //============================================================
        ISymbolNode mParent;
        ModuleContext mModuleContext;
        StaticTypeDef mStaticTypeDef;
        SymbolNodeList mNodeList;

        //------------------------------------------------------------
        // TypeNode�W�J�B
        private bool expandTypeNode(SymbolTree.ErrorInfoHolder aErrorHolder)
        {
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.StaticTypeDef != null)
                {
                    if (checkIdentDuplication(aErrorHolder, symbol.StaticTypeDef.Ident))
                    {
                        return false;
                    }
                    addNode(new TypeSymbolNode(this, mModuleContext, symbol.StaticTypeDef));
                }
            }
            return true;
        }

        //------------------------------------------------------------
        // FunctionNode�錾�W�J�B
        private bool expandFunctionNodeDecl(SymbolTree.ErrorInfoHolder aErrorHolder)
        {
            foreach (SymbolDef symbol in mStaticTypeDef.SymbolDefList)
            {
                if (symbol.MemberFunctionDecl != null)
                {
                    if (checkIdentDuplication(aErrorHolder, symbol.MemberFunctionDecl.Ident))
                    {
                        return false;
                    }
                    addNode(new FunctionSymbolNode(this, mModuleContext, symbol.MemberFunctionDecl));
                }
            }
            return true;
        }

        //------------------------------------------------------------
        // Ident�d���`�F�b�N�Btrue�Ȃ�d�����Ă���B
        private bool checkIdentDuplication(SymbolTree.ErrorInfoHolder aErrorHolder, Identifier aIdent)
        {
            if (FindChildNode(aIdent) != null)
            {
                aErrorHolder.Set(new SymbolTree.ErrorInfo(SymbolTree.ErrorKind.NODE_NAME_IS_ALREADY_EXIST, mModuleContext, aIdent.Token));
                return true;
            }
            return false;
        }

        //------------------------------------------------------------
        // �m�[�h�̒ǉ��B
        private void addNode(ISymbolNode aNode)
        {
            System.Diagnostics.Debug.Assert(
                aNode.GetNodeKind() == SymbolNodeKind.Type
                || aNode.GetNodeKind() == SymbolNodeKind.Variable
                || aNode.GetNodeKind() == SymbolNodeKind.Function
                );
            mNodeList.Add(aNode);
        }       
    }
}
