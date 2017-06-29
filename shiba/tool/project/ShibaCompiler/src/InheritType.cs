using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �p�������^�B
    /// </summary>
    class InheritType
    {
        public IdentPath IdentPath;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public InheritType(IdentPath aTypeIdentPath)
        {
            IdentPath = aTypeIdentPath;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                IdentPath.Trace(aTracer, "IdentPath");
            }
        }
    }
}
