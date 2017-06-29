using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 継承したインターフェース。
    /// </summary>
    class InheritInterface
    {
        public IdentPath TypeIdentPath;
        public IdentPath ImplIdentPath;

        //------------------------------------------------------------
        // コンストラクタ。
        public InheritInterface(IdentPath aTypeIdentPath, IdentPath aImplIdentPath)
        {
            TypeIdentPath = aTypeIdentPath;
            ImplIdentPath = aImplIdentPath;
        }

        //------------------------------------------------------------
        // トレース。
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
