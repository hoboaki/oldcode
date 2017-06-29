using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �֐��̈����̐錾�B
    /// </summary>
    class FunctionArgumentDecl
    {
        public readonly TypePath TypePath;
        public readonly Identifier Ident;
        public readonly bool IsConst;
        public readonly bool IsRef;
        public readonly bool IsIn;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public FunctionArgumentDecl(
            TypePath aTypePath
            , Identifier aIdent
            , bool aIsConst
            , bool aIsRef
            , bool aIsIn
            )
        {
            TypePath = aTypePath;
            Ident = aIdent;
            IsConst = aIsConst;
            IsRef = aIsRef;
            IsIn = aIsIn;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                TypePath.Trace(aTracer, aName);
                Ident.Trace(aTracer, "Ident");
                aTracer.WriteValue("IsConst", IsConst.ToString());
                aTracer.WriteValue("IsRef", IsRef.ToString());
                aTracer.WriteValue("IsIn", IsIn.ToString());
            }
        }
    }
}
