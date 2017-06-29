using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �P�����Z���B
    /// </summary>
    class UnaryOpExpression : IExpression
    {
        /// <summary>
        /// ���Z�̎�ށB
        /// </summary>
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
            , Positive // +
            , Negative // -
            , LogicalNot // !
            , BitwiseNot // ~ 
            , ObjectHandle // @
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public UnaryOpExpression(OpKind aOpKind, IExpression aExpr)
        {
            mOpKind = aOpKind;
            mExpr = aExpr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("UnaryOpExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mExpr.Trace(aTracer);
            }
        }

        //============================================================
        OpKind mOpKind;
        IExpression mExpr;
    }
}
