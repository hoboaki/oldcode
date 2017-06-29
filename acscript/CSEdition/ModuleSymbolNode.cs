using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Module�m�[�h�B
    /// </summary>
    class ModuleSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ModuleSymbolNode(ISymbolNode aParent, ModuleContext aModuleContext)
        {
            mTypeNode = new TypeSymbolNode(aParent, aModuleContext, aModuleContext.ModuleDef.StaticTypeDef);
        }

        //------------------------------------------------------------
        // �������g��Ident�B
        public Identifier GetIdentifier()
        {
            return mTypeNode.GetIdentifier();
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ށB
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Module;
        }

        //------------------------------------------------------------
        // �e�m�[�h�B
        public ISymbolNode ParentNode()
        {
            return mTypeNode.ParentNode();
        }

        //------------------------------------------------------------
        // �w���Ident�̃m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return mTypeNode.FindChildNode(aIdent);
        }

        //------------------------------------------------------------
        // �W�J����
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder holder, SymbolExpandCmdKind cmd)
        {
            return mTypeNode.SymbolExpand(holder, cmd);
        }

        //============================================================
        TypeSymbolNode mTypeNode;
    }
}
