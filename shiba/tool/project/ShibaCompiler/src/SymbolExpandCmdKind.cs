using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �V���{���W�J���߂̎�ށB
    /// </summary>
    enum SymbolExpandCmdKind
    {
        Unknwon,
        TypeNode, ///< �^�錾�B
        FunctionNodeDecl, ///< �֐��錾�B
        VariableNode, ///< �ϐ��錾�B
        FunctionNodeImpl, ///< �֐��̎����B
    }
}
