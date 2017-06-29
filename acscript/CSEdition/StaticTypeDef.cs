using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// �^�̒�`�B
    /// </summary>
    class StaticTypeDef
    {
        //------------------------------------------------------------
        /// <summary>
        /// �^�̎�ށB
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
        // ���J�����o�ϐ��B
        public readonly Kind TypeKind;
        public readonly Protection TypeProtection;
        public Identifier Ident;
        public List<InheritInterface> InheritInterfaceList;
        public List<SymbolDef> SymbolDefList;

        //------------------------------------------------------------
        // �R���X�g���N�^�B
        public StaticTypeDef(Kind aKind,Protection aTypeProtection)
        {
            System.Diagnostics.Debug.Assert(aKind != Kind.Unknown);
            System.Diagnostics.Debug.Assert(aTypeProtection != Protection.DEFAULT);
            TypeKind = aKind;
            TypeProtection = aTypeProtection;

            InheritInterfaceList = new List<InheritInterface>();
            SymbolDefList = new List<SymbolDef>();
        }

        //------------------------------------------------------------
        // �g���[�X�B
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("TypeKind", TypeKind.ToString());
                aTracer.WriteValue("TypeProtection", TypeProtection.ToString());
                Ident.Trace(aTracer, "Ident");

                // InheritInterfaceList
                aTracer.WriteNameWithCount("InheritInterfaceList", InheritInterfaceList.Count);
                using (new Tracer.IndentScope(aTracer))
                {
                    uint idx = 0;
                    foreach (var entry in InheritInterfaceList)
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
