using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 外部モジュール宣言。
    /// </summary>
    class ExternalModuleDecls
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ExternalModuleDecls(List<ExternalModuleDecl> aDecls)
        {
            mDecls = aDecls;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            // 名前
            aTracer.WriteName(aName);

            // 各要素
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
