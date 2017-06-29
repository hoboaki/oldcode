using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 型の定義。
    /// </summary>
    class StaticTypeDef
    {
        //------------------------------------------------------------
        /// <summary>
        /// 型の種類。
        /// </summary>
        public enum Kind
        {
            Unknown,
            Class,
            Enum,
            Interface,
            Pod,
            Struct,
            Typedef,
            Utility,
        };

        //------------------------------------------------------------
        // 公開メンバ変数。
        public readonly Kind TypeKind;
        public readonly Protection TypeProtection;
        public Identifier Ident;
        public List<InheritType> InheritTypeList;
        public List<SymbolDef> SymbolDefList;

        //------------------------------------------------------------
        // コンストラクタ。
        public StaticTypeDef(Kind aKind,Protection aTypeProtection)
        {
            Assert.Check(aKind != Kind.Unknown);
            Assert.Check(aTypeProtection != Protection.DEFAULT);
            TypeKind = aKind;
            TypeProtection = aTypeProtection;

            InheritTypeList = new List<InheritType>();
            SymbolDefList = new List<SymbolDef>();
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("TypeKind", TypeKind.ToString());
                aTracer.WriteValue("TypeProtection", TypeProtection.ToString());
                Ident.Trace(aTracer, "Ident");

                // InheritTypeList
                aTracer.WriteNameWithCount("InheritTypeList", InheritTypeList.Count);
                using (new Tracer.IndentScope(aTracer))
                {
                    uint idx = 0;
                    foreach (var entry in InheritTypeList)
                    {
                        entry.Trace(aTracer, "[" + idx + "]");
                        ++idx;
                    }
                }

                // SymbolDefList
                aTracer.WriteNameWithCount("SymbolDefList", SymbolDefList.Count);
                using (new Tracer.IndentScope(aTracer))
                {
                    uint idx = 0;
                    foreach (var entry in SymbolDefList)
                    {
                        entry.Trace(aTracer, "[" + idx + "]");
                        ++idx;
                    }
                }
            }
        }
    }
}
