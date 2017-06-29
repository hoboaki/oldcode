using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �O�����W���[���錾�B
    /// </summary>
    class ExternalModuleDecl
    {
        //------------------------------------------------------------
        // ��ށB
        public enum Kind
        {
            IMPORT, // import
            USING   // using
        };

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ExternalModuleDecl(IdentPath aPath, Kind aKind)
        {
            mIdentifierPath = aPath;
            mKind = aKind;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                mIdentifierPath.Trace(aTracer, "mIdentifierPath");
                aTracer.WriteValue("mKind", mKind.ToString());
            }
        }

        //============================================================
        private IdentPath mIdentifierPath;
        private Kind mKind;
    }
}
