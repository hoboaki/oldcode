using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// シンボルノードのインターフェースクラス。
    /// </summary>
    interface ISymbolNode
    {
        //------------------------------------------------------------
        // 識別子の取得。
        Identifier GetIdentifier();

        //------------------------------------------------------------
        // ノードの種類を取得。
        SymbolNodeKind GetNodeKind();

        //------------------------------------------------------------
        // 親ノードを取得。
        ISymbolNode ParentNode();

        //------------------------------------------------------------
        // 指定の識別子の子ノードを探す。
        ISymbolNode FindChildNode(Identifier aIdent);

        //------------------------------------------------------------
        // シンボルを展開する。
        bool SymbolExpand( SymbolTree.ErrorInfoHolder aHolder, SymbolExpandCmdKind aKind);
    }
}
