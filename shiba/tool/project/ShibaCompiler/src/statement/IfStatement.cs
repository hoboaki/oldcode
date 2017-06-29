using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// IfStatement。
    /// </summary>
    class IfStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public IfStatement(IExpression aExpr, IStatement aThenStatement, IStatement aElseStatement)
        {
            mExpression = aExpr;
            mThenStatement = aThenStatement;
            mElseStatement = aElseStatement;
        }

        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // ラベル作成
            BCLabel labelElse = aComp.BCFunction.LabelCreate();
            BCLabel labelEnd = aComp.BCFunction.LabelCreate();

            // 式の意味解析
            var exprEvaluateNode = mExpression.CreateEvaluateNode();
            exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

            // boolチェック
            StatementUtil.CheckBoolExpression(aComp, exprEvaluateNode, mExpression);

            // 式の評価
            exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
            exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

            // 式がfalseならelseに飛ぶ
            aComp.BCFunction.AddOPCode_SReg_Label(
                BCOpCode.OpType.JMPNEG
                , exprEvaluateNode.GetEvaluateInfo().SR
                , labelElse
                );

            // Thenの解析
            {
                // スコープに入る
                aComp.ScopeEnter();

                // Thenの解析
                mThenStatement.SemanticAnalyze(aComp);

                // スコープから出る
                aComp.ScopeLeave();

                // endに飛ぶ
                if (mElseStatement != null)
                {// elseがあるときだけでよい
                    aComp.BCFunction.AddOPCode_Label(
                        BCOpCode.OpType.JMP
                        , labelEnd
                        );
                }
            }

            // elseラベル挿入
            aComp.BCFunction.LabelInsert(labelElse);

            // Elseの解析
            if (mElseStatement != null)
            {
                // スコープに入る
                aComp.ScopeEnter();

                // Thenの解析
                mElseStatement.SemanticAnalyze(aComp);

                // スコープから出る
                aComp.ScopeLeave();
            }

            // endラベル挿入
            aComp.BCFunction.LabelInsert(labelEnd);
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("IfStatement");
        }

        //============================================================
        IExpression mExpression;
        IStatement mThenStatement;
        IStatement mElseStatement;
    }
}
