using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �p�������C���^�[�t�F�[�X�B
    /// </summary>
    class InheritInterface
    {
        public IdentPath TypeIdentPath;
        public IdentPath ImplIdentPath;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public InheritInterface(IdentPath aTypeIdentPath, IdentPath aImplIdentPath)
        {
            TypeIdentPath = aTypeIdentPath;
            ImplIdentPath = aImplIdentPath;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                TypeIdentPath.Trace(aTracer, "TypeIdentPath");
                ImplIdentPath.Trace(aTracer, "ImplIdentPath");
            }
        }
    }
}
