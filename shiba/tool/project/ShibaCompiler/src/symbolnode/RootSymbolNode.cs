using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ���[�g�ƂȂ�m�[�h�B
    /// </summary>
    class RootSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public RootSymbolNode()
        {
            mNodeList = new SymbolNodeList();
        }

        //------------------------------------------------------------
        // �������g��Ident�BRoot�ɂ͖����B
        public Identifier GetIdentifier()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // ���j�[�N�ȃt���p�X�BRoot�ɂ͖����B
        public string GetUniqueFullPath()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // �m�[�h�̎�ށB
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Root;
        }

        //------------------------------------------------------------
        // �e�m�[�h�BRoot�ɂ͖����B
        public ISymbolNode ParentNode()
        {
            throw new Exception();
        }

        //------------------------------------------------------------
        // �w���Ident�̃m�[�h��T���B
        public ISymbolNode FindChildNode(Identifier ident)
        {
            return mNodeList.FindNode(ident);
        }

        //------------------------------------------------------------
        // �W�J����
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
            aTracer.WriteName("RootSymbolNode");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var node in mNodeList)
                {
                    node.Trace(aTracer);
                }
            }
        }

        //------------------------------------------------------------
        // �m�[�h�̒ǉ��B
        public void AddNode(ISymbolNode aNode)
        {
            // Root�ɒǉ��ł���̂�namespace��module�B
            Assert.Check(
                aNode.GetNodeKind() == SymbolNodeKind.NameSpace
                || aNode.GetNodeKind() == SymbolNodeKind.Module
                );
            mNodeList.Add(aNode);
        }

        //============================================================
        SymbolNodeList mNodeList;
    }
}
