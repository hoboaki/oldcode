using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
            mTypeNode = new TypeSymbolNode(aParent, aModuleContext, aModuleContext.ModuleDef.StaticTypeDef);
        }

        //------------------------------------------------------------
        // 自分自身のIdent。
        public Identifier GetIdentifier()
        {
            return mTypeNode.GetIdentifier();
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
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder holder, SymbolExpandCmdKind cmd)
        {
            return mTypeNode.SymbolExpand(holder, cmd);
        }

        //============================================================
        TypeSymbolNode mTypeNode;
    }
}
