using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �֐��̖߂�l�̐錾�B
    /// </summary>
    class FunctionReturnValueDecl
    {
        public readonly TypePath TypePath;
        public readonly bool IsConst;
        public readonly bool IsRef;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionReturnValueDecl(
            TypePath aTypePath
            , bool aIsConst
            , bool aIsRef
            )
        {
            TypePath = aTypePath;
            IsConst = aIsConst;
            IsRef = aIsRef;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                TypePath.Trace(aTracer, "TypePath");
                aTracer.WriteValue("IsConst", IsConst.ToString());
                aTracer.WriteValue("IsRef", IsRef.ToString());
            }
        }
    }
}
