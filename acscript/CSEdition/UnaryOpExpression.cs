using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 単項演算式。
    /// </summary>
    class UnaryOpExpression : IExpression
    {
        /// <summary>
        /// 演算の種類。
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
        // コンストラクタ。
        public UnaryOpExpression(OpKind aOpKind, IExpression aExpr)
        {
            mOpKind = aOpKind;
            mExpr = aExpr;
        }

        //------------------------------------------------------------
        // トレース。
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
