using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �����o�ϐ��錾�B
    /// </summary>
    class MemberVariableDecl
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public MemberVariableDecl( VariableDecl aVariableDecl , bool aIsStatic , bool aIsConst , bool aIsReadonly )
        {
            mVariableDecl = aVariableDecl;
            mIsStatic = aIsStatic;
            mIsConst = aIsConst;
            mIsReadonly = aIsReadonly;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.Write(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                mVariableDecl.Trace(aTracer, "mVariableDecl");
                aTracer.WriteValue("mIsStatic", mIsStatic.ToString());
                aTracer.WriteValue("mIsConst", mIsConst.ToString());
                aTracer.WriteValue("mIsReadOnly", mIsReadonly.ToString());
            }
        }

        //============================================================
        VariableDecl mVariableDecl;
        bool mIsStatic;
        bool mIsConst;
        bool mIsReadonly;
    }
}
