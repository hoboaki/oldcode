using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 関数の戻り値の宣言。
    /// </summary>
    class FunctionReturnValueDecl
    {
        public readonly TypePath TypePath;
        public readonly bool IsConst;
        public readonly bool IsRef;

        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionReturnValueDecl(
            TypePath aTypePath
            , bool aIsConst
            , bool aIsRef
            )
        {
            TypePath = aTypePath;
            IsConst = aIsConst;
            IsRef = aIsRef;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                TypePath.Trace(aTracer, "TypePath");
                aTracer.WriteValue("IsConst", IsConst.ToString());
                aTracer.WriteValue("IsRef", IsRef.ToString());
            }
        }
    }
}
