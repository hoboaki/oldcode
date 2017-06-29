using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// モジュールの定義クラス。
    /// </summary>
    class ModuleDef
    {
        public readonly StaticTypeDef StaticTypeDef;

        //------------------------------------------------------------
        // コンストラクタ。
        public ModuleDef(StaticTypeDef aStaticTypeDef)
        {
            StaticTypeDef = aStaticTypeDef;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using(new Tracer.IndentScope(aTracer))
            {
                StaticTypeDef.Trace(aTracer, aName);
            }
        }
    }
}
