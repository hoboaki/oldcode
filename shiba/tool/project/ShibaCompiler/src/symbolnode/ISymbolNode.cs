using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        // ユニークなフルパスを返す。（関数なら引数の型まで含まれたもの）
        string GetUniqueFullPath();

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
        void SymbolExpand(SymbolExpandCmdKind aKind);

        //------------------------------------------------------------
        // トレースする。
        void Trace(Tracer aTracer);
    }
}
