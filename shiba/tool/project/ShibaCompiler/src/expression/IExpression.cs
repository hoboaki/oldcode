using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ���������C���^�[�t�F�[�X�B
    /// </summary>
    interface IExpression
    {
        //------------------------------------------------------------
        // �]���m�[�h���쐬����B
        IEvaluateNode CreateEvaluateNode();

        //------------------------------------------------------------
        // �g�[�N�����擾����B
        Token GetToken();

        //------------------------------------------------------------
        // �g���[�X�B
        void Trace(Tracer aTracer);
    }
}
