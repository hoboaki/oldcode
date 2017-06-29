using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
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
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // 評価ノードを作成
            var evaluateNode = mExpression.CreateEvaluateNode();

            // 評価準備
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

            // 評価実行
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

            // 親の評価終了 
            evaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

            // Scope終了イベントは送る必要がない
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
