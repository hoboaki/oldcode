using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �q�ƂȂ鎯�ʎq�̎��B
    /// </summary>
    class ChildIdentExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ChildIdentExpression(Identifier aIdent)
        {
            mIdent = aIdent;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {            
            aTracer.WriteName("ChildIdentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mIdent.Trace(aTracer, "mIdent");
            }
        }

        //============================================================
        Identifier mIdent;
    }
}
