using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
            mModuleContext = aModuleContext;
            mBCModule = new BCModule(this);
            mTypeNode = new TypeSymbolNode(aParent, mBCModule, aModuleContext.ModuleDef.StaticTypeDef);
        }

        //------------------------------------------------------------
        // ModuleContext���擾����B
        public ModuleContext GetModuleContext()
        {
            return mModuleContext;
        }

        //------------------------------------------------------------
        // �������g��Ident�B
        public Identifier GetIdentifier()
        {
            return mTypeNode.GetIdentifier();
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
        public void SymbolExpand(SymbolExpandCmdKind cmd)
        {
            mTypeNode.SymbolExpand(cmd);
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            mTypeNode.Trace(aTracer);
        }

        //------------------------------------------------------------
        // XData�̃_���v�B
        public void XDataDump()
        {
            System.Console.Write(mBCModule.ToXDataXml());
        }

        //------------------------------------------------------------
        // XML�t�@�C���ɏ������ށB
        public void WriteToXML(string aOutputDirPath)
        {
            // �t�@�C����
            string fullPath = SymbolNodeUtil.FullPath(this);
            string fileName = fullPath.Replace(".", "__");

            // ��������
            System.IO.File.WriteAllText(aOutputDirPath + "/" + fileName + ".xml", mBCModule.ToXDataXml());
        }

        //------------------------------------------------------------
        // �o�͏����B
        public void ReadyToOutput()
        {
            mBCModule.ReadyToOutput();
        }

        //============================================================
        ModuleContext mModuleContext;
        BCModule mBCModule;
        TypeSymbolNode mTypeNode;
    }
}
