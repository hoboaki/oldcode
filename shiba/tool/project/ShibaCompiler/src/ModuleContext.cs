using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Module�̃R���e�L�X�g�N���X�B
    /// </summary>
    class ModuleContext
    {
        public ModuleDecl ModuleDecl;
        public ExternalModuleDecls ExternalModuleDecls;
        public ModuleDef ModuleDef;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ModuleContext()
        {
        }

        //------------------------------------------------------------
        // �g���[�X����B
        public void Trace( Tracer aTracer , string aName )
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                ModuleDecl.Trace(aTracer, "ModuleDecl");
                ExternalModuleDecls.Trace(aTracer, "ExternalModuleDecls");
                ModuleDef.Trace(aTracer, "ModuleDef");
            }
        }
    }
}
