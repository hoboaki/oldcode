using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �m�[�h�̎�ށB
    /// </summary>
    public enum SymbolNodeKind
    {
        Unknown,
        Root, // ���[�g�B
        NameSpace, // ���O��ԁB
        Module, // ���W���[���B
        Type, // �^�B
        Variable, // �ϐ��B
        Function, // �֐��B�@
    };

}
