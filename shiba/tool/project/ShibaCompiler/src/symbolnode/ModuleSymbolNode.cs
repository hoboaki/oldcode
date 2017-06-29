using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Moduleノード。
    /// </summary>
    class ModuleSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ModuleSymbolNode(ISymbolNode aParent, ModuleContext aModuleContext)
        {
            mModuleContext = aModuleContext;
            mBCModule = new BCModule(this);
            mTypeNode = new TypeSymbolNode(aParent, mBCModule, aModuleContext.ModuleDef.StaticTypeDef);
        }

        //------------------------------------------------------------
        // ModuleContextを取得する。
        public ModuleContext GetModuleContext()
        {
            return mModuleContext;
        }

        //------------------------------------------------------------
        // 自分自身のIdent。
        public Identifier GetIdentifier()
        {
            return mTypeNode.GetIdentifier();
        }

        //------------------------------------------------------------
        // ユニークなフルパス。
        public string GetUniqueFullPath()
        {
            return SymbolNodeUtil.FullPath(this);
        }

        //------------------------------------------------------------
        // ノードの種類。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Module;
        }

        //------------------------------------------------------------
        // 親ノード。
        public ISymbolNode ParentNode()
        {
            return mTypeNode.ParentNode();
        }

        //------------------------------------------------------------
        // 指定のIdentのノードを探す。
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return mTypeNode.FindChildNode(aIdent);
        }

        //------------------------------------------------------------
        // 展開命令
        public void SymbolExpand(SymbolExpandCmdKind cmd)
        {
            mTypeNode.SymbolExpand(cmd);
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            mTypeNode.Trace(aTracer);
        }

        //------------------------------------------------------------
        // XDataのダンプ。
        public void XDataDump()
        {
            System.Console.Write(mBCModule.ToXDataXml());
        }

        //------------------------------------------------------------
        // XMLファイルに書き込む。
        public void WriteToXML(string aOutputDirPath)
        {
            // ファイル名
            string fullPath = SymbolNodeUtil.FullPath(this);
            string fileName = fullPath.Replace(".", "__");

            // 書き込む
            System.IO.File.WriteAllText(aOutputDirPath + "/" + fileName + ".xml", mBCModule.ToXDataXml());
        }

        //------------------------------------------------------------
        // 出力準備。
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
