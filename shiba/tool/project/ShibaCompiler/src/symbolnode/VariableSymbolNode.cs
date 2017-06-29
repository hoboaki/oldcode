using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Variable�m�[�h�B
    /// </summary>
    class VariableSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public VariableSymbolNode(
            ISymbolNode aParent
            , Identifier aIdent
            , TypeInfo aTypeInfo
            )
        {
            mParent = aParent;
            mIdent = aIdent;
            mTypeInfo = aTypeInfo;
        }

        //------------------------------------------------------------
        // TypeInfo�̎擾�B
        public TypeInfo GetTypeInfo()
        {
            return mTypeInfo;
        }

        //------------------------------------------------------------
        // ���ʎq�̎擾�B
        public Identifier GetIdentifier()
        {
            return mIdent;
        }

        //------------------------------------------------------------
        // ���j�[�N�ȃt���p�X�B
        public string GetUniqueFullPath()
        {
            return SymbolNodeUtil.FullPath(this);
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ނ̎擾�B
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Variable;
        }

        //------------------------------------------------------------
        // �e�m�[�h�̎擾�B
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // �q�m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return null; // �q�m�[�h�͂Ȃ��B
        }

        //------------------------------------------------------------
        // �V���{����W�J����B
        public void SymbolExpand(SymbolExpandCmdKind aCmdKind)
        {
            // todo: impl
        }

        //------------------------------------------------------------
        // �g���[�X����B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(), "VariableSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {                
                // todo: typeinfo
            }
        }

        //============================================================
        ISymbolNode mParent;
        Identifier mIdent;
        TypeInfo mTypeInfo;
    }
}
