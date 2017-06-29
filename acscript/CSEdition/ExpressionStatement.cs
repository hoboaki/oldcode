using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 式文。
    /// </summary>
    class ExpressionStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ExpressionStatement(IExpression aExpression)
        {
            mExpression = aExpression;
        }

        //------------------------------------------------------------
        // 意味解析。
        public bool SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            return false;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ExpressionStatement");
            using (new Tracer.IndentScope(aTracer))
            {
                mExpression.Trace(aTracer);
            }
        }

        //============================================================
        IExpression mExpression;
    }
}
