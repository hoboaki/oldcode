using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// SequenceExpression。
    /// </summary>
    class SequenceExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public SequenceExpression(IExpression aFirstExpr)
        {
            mFirstExpr = aFirstExpr;
        }

        //------------------------------------------------------------
        // コンストラクタ。
        public SequenceExpression(IExpression aFirstExpr, SequenceExpression aNextExpr)
            : this(aFirstExpr)
        {
            mNextExpr = aNextExpr;
        }

        //------------------------------------------------------------
        // 評価ノードを作成する。
        public IEvaluateNode CreateEvaluateNode()
        {
            return new EvaluateNode(this);
        }

        //------------------------------------------------------------
        // トークンを取得する。
        public Token GetToken()
        {
            return mFirstExpr.GetToken();
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteName("mTermExpr");
                using (new Tracer.IndentScope(aTracer))
                {
                    mFirstExpr.Trace(aTracer);
                }
                if (mNextExpr == null)
                {
                    aTracer.WriteValue("mNextExpr", "null");
                }
                else
                {
                    mNextExpr.Trace(aTracer);
                }
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(SequenceExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mFirstExpr.CreateEvaluateNode();
                if (mExpr.mNextExpr != null)
                {
                    mNextNode = mExpr.mNextExpr.CreateEvaluateNode();
                }
            }

            //------------------------------------------------------------
            // EvaluateInfoを取得する。
            public EvaluateInfo GetEvaluateInfo()
            {
                return mEvaluateInfo;
            }

            //------------------------------------------------------------
            // 評価イベントを送信する。
            public void SendEvent(SemanticAnalyzeComponent aComp, EvaluateNodeEventKind aEventKind)
            {
                switch (aEventKind)
                {
                    case EvaluateNodeEventKind.Analyze: eventAnalyze(aComp); break;
                    case EvaluateNodeEventKind.Evaluate: eventEvaluate(aComp); break;
                    case EvaluateNodeEventKind.SetupFR: eventSetupFR(aComp); break;
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            SequenceExpression mExpr;
            IEvaluateNode mFirstNode;
            IEvaluateNode mNextNode;
            EvaluateInfo mEvaluateInfo;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // ２つめがあれば２つめを評価
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                }

                // 評価情報を作成
                mEvaluateInfo = mFirstNode.GetEvaluateInfo();
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // ２つめがあれば２つめを評価
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                }
            }

            //------------------------------------------------------------　
            // FR割り当て。　
            void eventSetupFR(SemanticAnalyzeComponent aComp)
            {
                // １つめ
                byte frIndex = aComp.FunctionCallFRNextIndex();
                aComp.BCFunction.AddOPCode_CU1_SR(
                    BCOpCode.OpType.LDFRSR
                    , frIndex
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // ２つめがあれば２つめも
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.SetupFR);
                }
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // 評価終了イベント送信
                if (mNextNode != null)
                {
                    mNextNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                }
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        IExpression mFirstExpr;
        SequenceExpression mNextExpr;
    }
}
