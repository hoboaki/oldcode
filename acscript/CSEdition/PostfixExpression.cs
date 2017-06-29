using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 後置演算式。
    /// </summary>
    class PostfixExpression : IExpression
    {
        //------------------------------------------------------------
        // 演算の種類。
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public PostfixExpression(OpKind opKind, IExpression expr)
        {
            mOpKind = opKind;
            mFirstExpr = expr;
        }

        //------------------------------------------------------------
        // コンストラクタ。
        public PostfixExpression(IExpression firstExpr, IExpression secondExpr)
        {
            mFirstExpr = firstExpr;
            mSecondExpr = secondExpr;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTrace)
        {
            bool isIndent = mOpKind != OpKind.Unknown;
            if (isIndent)
            {
                aTrace.WriteValue("PostfixExpression",mOpKind.ToString());
                aTrace.indentInc();
            }
            if (isIndent)
            {
                aTrace.indentDec();
            }
        }

        //============================================================
        OpKind mOpKind = OpKind.Unknown;
        IExpression mFirstExpr;
        IExpression mSecondExpr;
    }
}
