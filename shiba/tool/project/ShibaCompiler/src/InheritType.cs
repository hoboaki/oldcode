using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 継承した型。
    /// </summary>
    class InheritType
    {
        public IdentPath IdentPath;

        //------------------------------------------------------------
        // コンストラクタ。
        public InheritType(IdentPath aTypeIdentPath)
        {
            IdentPath = aTypeIdentPath;
        }

        //------------------------------------------------------------
        // トレース。
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
