using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Moduleのコンテキストクラス。
    /// </summary>
    class ModuleContext
    {
        public ModuleDecl ModuleDecl;
        public ExternalModuleDecls ExternalModuleDecls;
        public ModuleDef ModuleDef;

        //------------------------------------------------------------
        // コンストラクタ。
        public ModuleContext()
        {
        }

        //------------------------------------------------------------
        // トレースする。
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
