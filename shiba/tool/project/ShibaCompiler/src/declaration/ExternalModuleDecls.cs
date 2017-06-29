using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// �O�����W���[���錾�B
    /// </summary>
    class ExternalModuleDecls
    {
        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public ExternalModuleDecls(List<ExternalModuleDecl> aDecls)
        {
            mDecls = aDecls;
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            // ���O
            aTracer.WriteName(aName);

            // �e�v�f
            using (new Tracer.IndentScope(aTracer))
            {
                string name = "mDecls";
                aTracer.WriteNameWithCount(name, mDecls.Count);
                uint idx = 0;
                using (new Tracer.IndentScope(aTracer))
                {
                    foreach (var decl in mDecls)
                    {
                        decl.Trace(aTracer, "[" + idx + "]");
                        ++idx;
                    }
                }
            }
        }

        //============================================================
        List<ExternalModuleDecl> mDecls;
    }
}
