using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// ������B
    /// </summary>
    class AssignmentExpression : IExpression
    {
        //------------------------------------------------------------
        // ���Z�̎�ށB
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
        // �R���X�g���N�^
        public AssignmentExpression(OpKind aOpKind, IExpression aLeftExpr, IExpression aRightExpr)
        {
            mOpKind = aOpKind;
            mLeftExpr = aLeftExpr;
            mRightExpr = aRightExpr;
        }

        //------------------------------------------------------------
        // �g���[�X�B
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
