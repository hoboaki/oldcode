using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// シンボルの定義。
    /// </summary>
    class SymbolDef
    {
        //------------------------------------------------------------
        // シンボルの種類。
        public enum Kind
        {
            StaticTypeDef,
            MemberVariableDecl,
            MemberFunctionDecl,
        };

        //------------------------------------------------------------
        // 公開メンバ変数。
        public readonly Kind SymbolKind;
        public readonly StaticTypeDef StaticTypeDef;
        public readonly MemberVariableDecl MemberVariableDecl;
        public readonly MemberFunctionDecl MemberFunctionDecl;

        //------------------------------------------------------------
        // 型の定義としてコンストラクトする。
        public SymbolDef(StaticTypeDef aST)
        {
            SymbolKind = Kind.StaticTypeDef;
            StaticTypeDef = aST;
        }

        //------------------------------------------------------------
        // メンバ変数の宣言としてコンストラクトする。
        public SymbolDef(MemberVariableDecl aMV)
        {
            SymbolKind = Kind.MemberVariableDecl;
            MemberVariableDecl = aMV;
        }

        //------------------------------------------------------------
        // メンバ関数の宣言としてコンストラクトする。
        public SymbolDef(MemberFunctionDecl aMF)
        {
            SymbolKind = Kind.MemberFunctionDecl;
            MemberFunctionDecl = aMF;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer, string aName)
        {
            aTracer.WriteName(aName);
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("SymbolKind", SymbolKind.ToString());
                if (StaticTypeDef != null) { StaticTypeDef.Trace(aTracer, "StaticTypeDef"); }
                if (MemberVariableDecl != null) { MemberVariableDecl.Trace(aTracer, "MemberVariableDecl"); }
                if (MemberFunctionDecl != null) { MemberFunctionDecl.Trace(aTracer, "MemberFunctionDecl"); }
            }
        }
    }
}
