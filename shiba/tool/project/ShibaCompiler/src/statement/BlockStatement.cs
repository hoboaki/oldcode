using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// BlockStatement。
    /// </summary>
    class BlockStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BlockStatement()
        {
            mStatements = new List<IStatement>();
        }

        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // スコープに入る
            aComp.ScopeEnter();

            // 意味解析
            foreach (IStatement st in mStatements)
            {
                st.SemanticAnalyze(aComp);
            }

            // スコープから出る
            aComp.ScopeLeave();
        }

        //------------------------------------------------------------
        // Statementの追加。
        public void Add(IStatement statement)
        {
            mStatements.Add(statement);
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("BlockStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                foreach (var entry in mStatements)
                {
                    entry.Trace(aTracer);
                }
            }
        }

        //============================================================
        List<IStatement> mStatements;
    }
}
