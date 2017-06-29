using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �L���X�g���B
    /// </summary>
    class CastExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public CastExpression(TypePath aTypePath, IExpression aExpr)
        {
            mTypePath = aTypePath;
            mExpr = aExpr;
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
            aTracer.WriteName("CastExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mTypePath.Trace(aTracer, "mTypePath");
                mExpr.Trace(aTracer);
            }
        }

        //============================================================
        TypePath mTypePath;
        IExpression mExpr;
    }
}
