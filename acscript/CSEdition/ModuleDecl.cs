using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// モジュールの宣言。
    /// </summary>
    class ModuleDecl
    {
        public readonly IdentPath IdentifierPath;
        public readonly bool IsProtoType;

        //------------------------------------------------------------
        // コンストラクタ。
        public ModuleDecl(IdentPath aPath, bool aIsProtoType)
        {
            IdentifierPath = aPath;
            IsProtoType = aIsProtoType;
        }

        //------------------------------------------------------------
        // トレースする。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                IdentifierPath.Trace(aTracer, "IdentifierPath");
                aTracer.WriteValue("IsProtoType", IsProtoType.ToString());
            }
        }
    }
}
