using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// Variableノード。
    /// </summary>
    class VariableSymbolNode : ISymbolNode
    {
        //------------------------------------------------------------
        // コンストラクタ。
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
        // 識別子の取得。
        public Identifier GetIdentifier()
        {
            return mIdent;
        }

        //------------------------------------------------------------
        // ノードの種類の取得。
        public SymbolNodeKind GetNodeKind()
        {
            return SymbolNodeKind.Variable;
        }

        //------------------------------------------------------------
        // 親ノードの取得。
        public ISymbolNode ParentNode()
        {
            return mParent;
        }

        //------------------------------------------------------------
        // 子ノードを探す。
        public ISymbolNode FindChildNode(Identifier aIdent)
        {
            return null; // 子ノードはない。
        }

        //------------------------------------------------------------
        // シンボルを展開する。
        public bool SymbolExpand(SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aCmdKind)
        {
            // todo:impl
            return true;
        }

        //============================================================
        ISymbolNode mParent;
        Identifier mIdent;
        TypeInfo mTypeInfo;
    }
}
