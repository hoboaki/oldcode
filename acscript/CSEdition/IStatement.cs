using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ���������C���^�[�t�F�[�X�B
    /// </summary>
    interface IStatement
    {
        //------------------------------------------------------------
        // �Ӗ���́B
        bool SemanticAnalyze(SemanticAnalyzeComponent aComp);

        //------------------------------------------------------------
        // �g���[�X�B
        void Trace(Tracer aTracer);
    }
}
