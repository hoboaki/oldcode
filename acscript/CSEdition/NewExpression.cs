using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
