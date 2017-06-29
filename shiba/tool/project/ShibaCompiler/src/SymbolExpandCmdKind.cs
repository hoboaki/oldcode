using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// シンボル展開命令の種類。
    /// </summary>
    enum SymbolExpandCmdKind
    {
        Unknwon,
        TypeNode, ///< 型宣言。
        FunctionNodeDecl, ///< 関数宣言。
        VariableNode, ///< 変数宣言。
        FunctionNodeImpl, ///< 関数の実装。
    }
}
