using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ���O��ԂƂȂ�m�[�h�B
    /// </summary>
    class NamespaceSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public NamespaceSymbolNode(ISymbolNode aParent, Identifier aIdent)
        {
            mParent = aParent;
            mIdent = aIdent;
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // �������g��Ident�B
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
        // �m�[�h�̎�ށB
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.NameSpace;
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
            foreach (ISymbolNode node in mNodeList)
            {
                node.SymbolExpand(aCmd);
            }
        }

        //------------------------------------------------------------
        // �g���[�X����B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteValue(GetIdentifier().String(),"NamespaceSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var entry in mNodeList)
                {
                    entry.Trace(aTracer);
                }
            }
        }

        //------------------------------------------------------------
        // �m�[�h�̒ǉ��B
        public void AddNode(ISymbolNode aNode)
        {
            Assert.Check(
                aNode.GetNodeKind() == SymbolNodeKind.NameSpace
                || aNode.GetNodeKind() == SymbolNodeKind.Module
                );
            mNodeList.Add(aNode);
        }

        //============================================================
        ISymbolNode mParent;
        Identifier mIdent;
        SymbolNodeList mNodeList;
    }
}
