using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 単項演算式。
    /// </summary>
    class UnaryOpExpression : IExpression
    {
        /// <summary>
        /// 演算の種類。
        /// </summary>
        public enum OpKind
        {
            Unknown
            , Inc // ++
            , Dec // --
            , Positive // +
            , Negative // -
            , LogicalNot // !
            , BitwiseNot // ~ 
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public UnaryOpExpression(Token aOpToken, OpKind aOpKind, IExpression aExpr)
        {
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mExpr = aExpr;
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
            return mOpToken;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("UnaryOpExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mExpr.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(UnaryOpExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mExpr.CreateEvaluateNode();
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
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            UnaryOpExpression mExpr;
            IEvaluateNode mFirstNode;
            EvaluateInfo mEvaluateInfo;
            TransferredEIHolder mTransferredEIHolder;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // EvaluateInfoの用意
                switch (mExpr.mOpKind)
                {
                    case OpKind.Inc:
                    case OpKind.Dec:
                    case OpKind.Positive:
                        // 式のものをそのまま
                        mEvaluateInfo = mFirstNode.GetEvaluateInfo();
                        break;

                    case OpKind.Negative:
                    case OpKind.BitwiseNot:
                        // タイプは一緒だけど別のレジスタを使う可能性があるのでEvaluateInfoを作成
                        mEvaluateInfo = EvaluateInfo.CreateAsValue(mFirstNode.GetEvaluateInfo().TypeInfo);
                        break;

                    case OpKind.LogicalNot:
                        // boolで
                        mEvaluateInfo = EvaluateInfo.CreateAsValue(new TypeInfo(new TypeInfo.TypeSymbol(mExpr.mOpToken, BuiltInType.Bool), new TypeInfo.TypeAttribute(true, false)));
                        break;

                    default:
                        Assert.NotReachHere();
                        break;
                }

                // Holder作成
                mTransferredEIHolder = new TransferredEIHolder(mEvaluateInfo);
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                switch (mExpr.mOpKind)
                {
                    case OpKind.Inc: evaluateIncDec(aComp, true); break;
                    case OpKind.Dec: evaluateIncDec(aComp, false); break;
                    case OpKind.Positive: evaluatePositive(aComp); break;
                    case OpKind.Negative: evaluateNegativeBitwiseNot(aComp,true); break;
                    case OpKind.BitwiseNot: evaluateNegativeBitwiseNot(aComp, false); break;
                    case OpKind.LogicalNot: evaluateLogicalNot(aComp); break;
                    default:
                        Assert.NotReachHere();
                        break;
                }
            }

            //------------------------------------------------------------
            // Inc,Dec用評価関数。
            void evaluateIncDec(SemanticAnalyzeComponent aComp, bool aIsInc)
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

                // 命令を追加
                aComp.BCFunction.AddOPCode_SReg(
                    aIsInc ? BCOpCode.OpType.INCI32 : BCOpCode.OpType.DECI32 
                    , mEvaluateInfo.SR
                    );
            }

            //------------------------------------------------------------
            // Positive用評価関数。
            void evaluatePositive(SemanticAnalyzeComponent aComp)
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
            }

            //------------------------------------------------------------
            // Negative,BitwiseNot用評価関数。
            void evaluateNegativeBitwiseNot(SemanticAnalyzeComponent aComp,bool aIsNegative)
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

                // SR設定
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // 伝達できるなら伝達する
                mTransferredEIHolder.TransferIfPossible(aComp);

                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達情報リセット
                aComp.TransferredEvaluateInfoReset();

                // 命令追加
                aComp.BCFunction.AddOPCode_SReg1_SReg2(
                    aIsNegative ? BCOpCode.OpType.NEGS32 : BCOpCode.OpType.NTI32
                    , mEvaluateInfo.SR
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // イベント送信
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // LogicalNot用評価関数。
            void evaluateLogicalNot(SemanticAnalyzeComponent aComp)
            {
                // boolしか対応しない
                StatementUtil.CheckBoolExpression(aComp, mFirstNode, mExpr.mExpr);

                // SR設定
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // 伝達できるなら伝達する
                mTransferredEIHolder.TransferIfPossible(aComp);

                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達情報リセット
                aComp.TransferredEvaluateInfoReset();

                // 命令追加
                aComp.BCFunction.AddOPCode_SReg1_SReg2(
                    BCOpCode.OpType.NTBOOL
                    , mEvaluateInfo.SR
                    , mFirstNode.GetEvaluateInfo().SR
                    );

                // イベント送信
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                switch (mExpr.mOpKind)
                {
                    case OpKind.Inc:
                    case OpKind.Dec:
                    case OpKind.Positive:
                        // このタイミングでイベント送信
                        mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                        break;

                    case OpKind.Negative:
                    case OpKind.BitwiseNot:
                    case OpKind.LogicalNot:
                        // レジスタ返却
                        mTransferredEIHolder.ReleaseIfNeccesary(aComp);
                        break;

                    default:
                        break;
                }
            }
        };

        //------------------------------------------------------------
        Token mOpToken;
        OpKind mOpKind;
        IExpression mExpr;
    }
}
