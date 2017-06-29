using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 宣言文。
    /// </summary>
    class DeclarationStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public DeclarationStatement(VariableDecl aVariableDecl, bool aIsConst)
        {
            mVariableDecl = aVariableDecl;
            mIsConst = aIsConst;
        }

        //------------------------------------------------------------
        // 意味解析。
        public bool SemanticAnalyze(SemanticAnalyzeComponent comp)
        {
            TypeInfo typeInfo = comp.CreateTypeInfo(mVariableDecl.TypePath);

            VariableSymbolNode newNode = new VariableSymbolNode(
                comp.PrevSymbolNode
                , mVariableDecl.Ident
                , typeInfo
                );
            if (!comp.OnSymbolNodeCreateMethod(newNode))
            {
                return false;
            }
            comp.PrevSymbolNode = newNode;

            return true;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("DeclarationStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                mVariableDecl.Trace(aTracer, "mVariableDecl");
                aTracer.WriteValue("mIsConst", mIsConst.ToString());
            }
        }

        //============================================================
        VariableDecl mVariableDecl;
        bool mIsConst;
    }
}
