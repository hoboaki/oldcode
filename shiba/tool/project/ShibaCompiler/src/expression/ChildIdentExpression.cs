using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        // �]���m�[�h���쐬����B
        public IEvaluateNode CreateEvaluateNode()
        {
            // todo:
            Assert.NotReachHere();
            return null;
        }

        //------------------------------------------------------------
        // �g�[�N�����擾����B
        public Token GetToken()
        {
            // todo:
            Assert.NotReachHere();
            return null;
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
