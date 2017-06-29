using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 代入式。
    /// </summary>
    class AssignmentExpression : IExpression
    {
        //------------------------------------------------------------
        // 演算の種類。
        public enum OpKind
        {
            Unknown,
            Assign,
            AddAssign,
            SubAssign,
            MulAssign,
            DivAssign,
            ModAssign,
            RShiftAssign,
            LShiftAssign,
            AndAssign,
            OrAssign,
            XorAssign,
        };

        //------------------------------------------------------------
        // コンストラクタ
        public AssignmentExpression(OpKind aOpKind, IExpression aLeftExpr, IExpression aRightExpr)
        {
            mOpKind = aOpKind;
            mLeftExpr = aLeftExpr;
            mRightExpr = aRightExpr;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("AssignmentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mLeftExpr.Trace(aTracer);
                mRightExpr.Trace(aTracer);
            }
        }

        //============================================================
        OpKind mOpKind;
        IExpression mLeftExpr;
        IExpression mRightExpr;
    }
}
