using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 二項演算式。
    /// </summary>
    class BinaryOpExpression : IExpression
    {
        //------------------------------------------------------------
        // 演算の種類。
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
        // コンストラクタ
        public BinaryOpExpression(OpKind aOpKind, IExpression aFirst,IExpression aSecond)
        {
            mOpKind = aOpKind;
            mFirst = aFirst;
            mSecond = aSecond;
        }

        //------------------------------------------------------------
        // トレース。
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
