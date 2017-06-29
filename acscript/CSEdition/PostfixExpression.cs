using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ��u���Z���B
    /// </summary>
    class PostfixExpression : IExpression
    {
        //------------------------------------------------------------
        // ���Z�̎�ށB
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public PostfixExpression(OpKind opKind, IExpression expr)
        {
            mOpKind = opKind;
            mFirstExpr = expr;
        }

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public PostfixExpression(IExpression firstExpr, IExpression secondExpr)
        {
            mFirstExpr = firstExpr;
            mSecondExpr = secondExpr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
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
