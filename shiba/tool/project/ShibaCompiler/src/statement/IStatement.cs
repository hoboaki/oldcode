using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ���������C���^�[�t�F�[�X�B
    /// </summary>
    interface IStatement
    {
        //------------------------------------------------------------
        // �Ӗ���́B
        void SemanticAnalyze(SemanticAnalyzeComponent aComp);

        //------------------------------------------------------------
        // �g���[�X�B
        void Trace(Tracer aTracer);
    }
}
