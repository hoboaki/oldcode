using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 後置演算式。
    /// </summary>
    class PostfixExpression : IExpression
    {
        //------------------------------------------------------------
        // 演算の種類。
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
        };

        //------------------------------------------------------------
        // 式の種類。
        public enum ExprKind
        {
            IncDec // ++,--
            , FunctionCallExpr // FunctionCallExpression
            , IndexExpr // IndexExpression
            , ChildIdentExpr // ChildIdentExpression
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public PostfixExpression(Token aOpToken, OpKind aOpKind, IExpression aExpr)
        {
            mExprKind = ExprKind.IncDec;
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mFirstExpr = aExpr;
        }

        //------------------------------------------------------------
        // FunctionCallのコンストラクタ。
        public PostfixExpression(IExpression aFirstExpr, FunctionCallExpression aFuncCallExpr)
        {
            mExprKind = ExprKind.FunctionCallExpr;
            mFirstExpr = aFirstExpr;
            mFunctionCallExpr = aFuncCallExpr;
        }

        //------------------------------------------------------------
        // Indexのコンストラクタ。
        public PostfixExpression(IExpression aFirstExpr, IndexExpression aIndexExpr)
        {
            mExprKind = ExprKind.IndexExpr;
            mFirstExpr = aFirstExpr;
            mIndexExpr = aIndexExpr;
        }

        //------------------------------------------------------------
        // ChildIdentのコンストラクタ。
        public PostfixExpression(IExpression aFirstExpr, ChildIdentExpression aChildIdentExpr)
        {
            mExprKind = ExprKind.ChildIdentExpr;
            mFirstExpr = aFirstExpr;
            mChildIdentExpr = aChildIdentExpr;
        }

        //------------------------------------------------------------
        // 評価ノードを作成する。
        public IEvaluateNode CreateEvaluateNode()
        {
            switch (mExprKind)
            {
                case ExprKind.IncDec: return new IncDecEvaluateNode(this);
                case ExprKind.FunctionCallExpr: return mFunctionCallExpr.CreateEvaluateNodeWithFirstExpr(mFirstExpr);
                default:
                    Assert.NotReachHere();
                    return null;
            }
        }

        //------------------------------------------------------------
        // トークンを取得する。
        public Token GetToken()
        {
            return mOpToken != null ? mOpToken : mFirstExpr.GetToken();
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTrace)
        {
            bool isIndent = mOpKind != OpKind.Unknown;
            if (isIndent)
            {
                aTrace.WriteValue("PostfixExpression",mOpKind.ToString());
                aTrace.indentInc();
            }
            if (isIndent)
            {
                aTrace.indentDec();
            }
        }

        //============================================================

        //------------------------------------------------------------
        // IncDec用評価ノード。
        class IncDecEvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public IncDecEvaluateNode(PostfixExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mFirstExpr.CreateEvaluateNode();
            }

            //------------------------------------------------------------
            // EvaluateInfoを取得する。
            public EvaluateInfo GetEvaluateInfo()
            {
                return mFirstNode.GetEvaluateInfo();
            }

            //------------------------------------------------------------
            // 評価イベントを送信する。
            public void SendEvent(SemanticAnalyzeComponent aComp, EvaluateNodeEventKind aEventKind)
            {
                switch (aEventKind)
                {
                    case EvaluateNodeEventKind.Analyze: eventAnalyze(aComp); break;
                    case EvaluateNodeEventKind.Evaluate: eventEvaluate(aComp); break;
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            PostfixExpression mExpr;
            IEvaluateNode mFirstNode;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // 暗黙の変換の対応
                // 今はintしか対応しない
                if (mFirstNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mFirstNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // 命令の選択
                BCOpCode.OpType opType = BCOpCode.OpType.NOP;
                switch(mExpr.mOpKind)
                {
                case OpKind.Inc: opType = BCOpCode.OpType.INCI32; break;
                case OpKind.Dec: opType = BCOpCode.OpType.DECI32; break;
                default:
                    Assert.NotReachHere();
                    break;
                }

                // 命令を追加
                aComp.BCFunction.AddOPCode_SReg(
                    opType
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // OnParentEvaluateEnd
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        ExprKind mExprKind;
        Token mOpToken;
        OpKind mOpKind = OpKind.Unknown;
        IExpression mFirstExpr;
        FunctionCallExpression mFunctionCallExpr;
        IndexExpression mIndexExpr;
        ChildIdentExpression mChildIdentExpr;
    }
}
