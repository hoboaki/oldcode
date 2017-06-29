using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ForStatement。
    /// </summary>
    class ForStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ForStatement(
            IStatement aInitStatement
            , IExpression aCondExpr
            , IExpression aIncExpr
            , IStatement aStatement
            )
        {
            mInitStatement = aInitStatement;
            mCondExpr = aCondExpr;
            mIncExpr = aIncExpr;
            mStatement = aStatement;
        }

        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // ラベル作成
            BCLabel labelCondition = aComp.BCFunction.LabelCreate();
            BCLabel labelContinue = aComp.BCFunction.LabelCreate();
            BCLabel labelBreak = aComp.BCFunction.LabelCreate();

            {
                // スコープに入る
                aComp.ScopeEnter();

                // Initialize
                if (mInitStatement != null)
                {
                    // 文の解析
                    mInitStatement.SemanticAnalyze(aComp);
                }

                // Conditionへ飛ぶ
                aComp.BCFunction.AddOPCode_Label(BCOpCode.OpType.JMP, labelCondition);

                {
                    // スコープに入る
                    aComp.ScopeEnter();

                    // Increment
                    aComp.BCFunction.LabelInsert(labelContinue);
                    if (mIncExpr != null)
                    {
                        // 式の意味解析
                        var exprEvaluateNode = mIncExpr.CreateEvaluateNode();
                        exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                        // 式の評価
                        exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                        exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                    }

                    // Condition
                    aComp.BCFunction.LabelInsert(labelCondition);
                    if (mCondExpr != null)
                    {
                        // 式の意味解析
                        var exprEvaluateNode = mCondExpr.CreateEvaluateNode();
                        exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                        // boolチェック
                        StatementUtil.CheckBoolExpression(aComp, exprEvaluateNode, mCondExpr);

                        // 式の評価
                        exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                        exprEvaluateNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

                        // 分岐
                        aComp.BCFunction.AddOPCode_SReg_Label(
                            BCOpCode.OpType.JMPNEG
                            , exprEvaluateNode.GetEvaluateInfo().SR
                            , labelBreak
                            );
                    }

                    {// Statement
                        // スコープに入る
                        aComp.ScopeEnter();

                        // ラベルの登録
                        aComp.RegisterLabelContinue(labelContinue);
                        aComp.RegisterLabelBreak(labelBreak);

                        // 文の解析
                        mStatement.SemanticAnalyze(aComp);

                        // スコープから出る
                        aComp.ScopeLeave();

                        // continueする
                        aComp.BCFunction.AddOPCode_Label(
                            BCOpCode.OpType.JMP
                            , labelContinue
                            );
                    }

                    // スコープから出る
                    aComp.ScopeLeave();
                }

                // breakラベル挿入
                aComp.BCFunction.LabelInsert(labelBreak);

                // スコープから出る
                aComp.ScopeLeave();
            }
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ForStatement");
        }

        //============================================================
        IStatement mInitStatement;
        IExpression mCondExpr;
        IExpression mIncExpr;
        IStatement mStatement;
    }
}
