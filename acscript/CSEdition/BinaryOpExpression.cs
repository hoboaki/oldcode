using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �񍀉��Z���B
    /// </summary>
    class BinaryOpExpression : IExpression
    {
        //------------------------------------------------------------
        // ���Z�̎�ށB
        public enum OpKind
        {
            Unknnown,
            AdditiveAdd,
            AdditiveSub,
            BitwiseAnd,
            BitwiseOr,
            BitwiseXor,
            EqualityEqual,
            EqualityNotEqual,
            IdentityEqual,
            IdentityNotEqual,
            LogicalAnd,
            LogicalOr,
            MultiplicativeDiv,
            MultiplicativeMod,
            MultiplicativeMul,
            RelationalGreater,
            RelationalGreaterEqual,
            RelationalLess,
            RelationalLessEqual,
            ShiftLeft,
            ShiftRight,
        };

        //------------------------------------------------------------
        // �R���X�g���N�^
        public BinaryOpExpression(OpKind aOpKind, IExpression aFirst,IExpression aSecond)
        {
            mOpKind = aOpKind;
            mFirst = aFirst;
            mSecond = aSecond;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("BinaryOpExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mFirst.Trace(aTracer);
                mSecond.Trace(aTracer);
            }
        }

        //============================================================
        OpKind mOpKind;
        IExpression mFirst;
        IExpression mSecond;
    }
}
