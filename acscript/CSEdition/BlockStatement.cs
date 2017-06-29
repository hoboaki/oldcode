using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
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
        public bool SemanticAnalyze(SemanticAnalyzeComponent comp)
        {
            // バックアップ
            ISymbolNode backupPrevSymbolNode = comp.PrevSymbolNode;

            // 意味解析
            foreach (IStatement st in mStatements)
            {
                if (!st.SemanticAnalyze(comp))
                {
                    return false;
                }
            }

            // バックアップを元に戻す
            comp.PrevSymbolNode = backupPrevSymbolNode;

            return true;
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
