using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// new���B
    /// </summary>
    class NewExpression : IExpression
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public NewExpression(TypePath aTypePath, FunctionCallExpression aFuncCallExpr)
        {
            mTypePath = aTypePath;
            mFuncCallExpr = aFuncCallExpr;
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
            aTracer.WriteName("NewExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mTypePath.Trace(aTracer,"mTypePath");
                mFuncCallExpr.Trace(aTracer);
            }
        }

        //============================================================
        TypePath mTypePath;
        FunctionCallExpression mFuncCallExpr;
    }
}
