using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 外部モジュール宣言。
    /// </summary>
    class ExternalModuleDecl
    {
        //------------------------------------------------------------
        // 種類。
        public enum Kind
        {
            IMPORT, // import
            USING   // using
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public ExternalModuleDecl(IdentPath aPath, Kind aKind)
        {
            mIdentifierPath = aPath;
            mKind = aKind;
        }

        //------------------------------------------------------------
        // トレース。
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
