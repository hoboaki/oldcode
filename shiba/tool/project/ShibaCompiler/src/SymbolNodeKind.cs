using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ノードの種類。
    /// </summary>
    public enum SymbolNodeKind
    {
        Unknown,
        Root, // ルート。
        NameSpace, // 名前空間。
        Module, // モジュール。
        Type, // 型。
        Variable, // 変数。
        Function, // 関数。　
    };

}
