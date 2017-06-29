using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// DoWhileStatement。
    /// </summary>
    class DoWhileStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public DoWhileStatement(IExpression aExpr, IStatement aStatement)
        {
            mExpression = aExpr;
            mStatement = aStatement;
        }

        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // ラベル作成
            BCLabel labelBegin = aComp.BCFunction.LabelCreate();
            BCLabel labelContinue = aComp.BCFunction.LabelCreate();
            BCLabel labelBreak = aComp.BCFunction.LabelCreate();

            // beginラベル挿入
            aComp.BCFunction.LabelInsert(labelBegin);

            // スコープに入る
            aComp.ScopeEnter();

            // ラベルの登録
            aComp.RegisterLabelContinue(labelContinue);
            aComp.RegisterLabelBreak(labelBreak);

            // 文の解析
            mStatement.SemanticAnalyze(aComp);

            // スコープから出る
            aComp.ScopeLeave();

            // continueラベル挿入
            aComp.BCFunction.LabelInsert(labelContinue);

            // 式の意味解析
            var exprEvaluateNode = mExpression.CreateEvaluateNode();
            exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

            // boolチェック
            StatementUtil.CheckBoolExpression(aComp, exprEvaluateNode, mExpression);

            // 式の評価
            exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
            exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

            // 分岐
            aComp.BCFunction.AddOPCode_SReg_Label(
                BCOpCode.OpType.JMPPOS
                , exprEvaluateNode.GetEvaluateInfo().SR
                , labelBegin
                );

            // breakラベル挿入
            aComp.BCFunction.LabelInsert(labelBreak);
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("DoWhileStatement");
        }

        //============================================================
        IExpression mExpression;
        IStatement mStatement;
    }
}
