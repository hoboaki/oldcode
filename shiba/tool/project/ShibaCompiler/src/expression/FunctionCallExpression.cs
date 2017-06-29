using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 関数呼び出し式。
    /// </summary>
    class FunctionCallExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionCallExpression(Token aOpToken)
        {
            mOpToken = aOpToken;
        }

        //------------------------------------------------------------
        // コンストラクタ。
        public FunctionCallExpression(Token aOpToken, SequenceExpression aSequenceExpr)
        {
            mOpToken = aOpToken;
            mSequenceExpr = aSequenceExpr;
        }

        //------------------------------------------------------------
        // 評価ノードを作成する。
        public IEvaluateNode CreateEvaluateNode()
        {
            // この関数で作成することを今の時点では禁止する
            //（禁止しなくてよい方法を思いついたらそのときに対処する）
            Assert.NotReachHere();
            return null;
        }

        //------------------------------------------------------------
        // 評価ノードを作成する。
        public IEvaluateNode CreateEvaluateNodeWithFirstExpr(IExpression aFirstExpr)
        {
            return new EvaluateNode(aFirstExpr, this);
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
            aTracer.WriteName("FunctionCallExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mSequenceExpr.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(IExpression aFirstExpr, FunctionCallExpression aFuncExpr)
            {
                mFirstExpr = aFirstExpr;
                mFirstNode = mFirstExpr.CreateEvaluateNode();
                mFuncExpr = aFuncExpr;
                if (mFuncExpr.mSequenceExpr != null)
                {
                    mSeqNode = mFuncExpr.mSequenceExpr.CreateEvaluateNode();
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
                    case EvaluateNodeEventKind.Release: eventRelease(aComp); break;

                    default:
                        Assert.NotReachHere();
                        return;
                }
            }

            //============================================================
            IExpression mFirstExpr;
            IEvaluateNode mFirstNode;
            IEvaluateNode mSeqNode;
            FunctionCallExpression mFuncExpr;
            EvaluateInfo mEvaluateInfo;
            TransferredEIHolder mTransferredEI;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // 関数シンボルかどうかのチェック
                if (mFirstNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.StaticSymbol
                    || mFirstNode.GetEvaluateInfo().Symbol.GetNodeKind() != SymbolNodeKind.Function
                    )
                {// エラー
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.FUNCTION_SYMBOL_EXPECTED, mFirstExpr.GetToken());
                }

                // 引数ありの対応
                if (mSeqNode != null)
                {
                    mSeqNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                }

                // シンボルの取得
                FunctionSymbolNode funcSymbol = (FunctionSymbolNode)mFirstNode.GetEvaluateInfo().Symbol;

                // 評価情報を作成
                mEvaluateInfo = EvaluateInfo.CreateAsValue(funcSymbol.ReturnTypeInfo());

                // 伝搬情報ホルダーの作成
                mTransferredEI = new TransferredEIHolder(mEvaluateInfo);
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // レジスタの確保
                if (mEvaluateInfo.TypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn // 組み込み型じゃない
                    || mEvaluateInfo.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Void // voidじゃない
                    )
                {// レジスタが必要
                    // 伝搬情報を取得してみる
                    mTransferredEI.ReceiveAndSetSR(aComp);
                }

                // １つめを評価
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // シンボルの取得
                FunctionSymbolNode funcSymbol = (FunctionSymbolNode)mFirstNode.GetEvaluateInfo().Symbol;

                // 引数の対応
                if (mSeqNode != null)
                {
                    // まず評価設定
                    mSeqNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                    // 関数情報設定
                    aComp.FunctionCallTargetSet(funcSymbol);

                    // 戻り値用のFRを確保
                    if (mEvaluateInfo.SR.IsValid)
                    {
                        aComp.FunctionCallFRNextIndex();
                    }

                    // 引数のFRを確保
                    mSeqNode.SendEvent(aComp, EvaluateNodeEventKind.SetupFR);

                    // 関数情報リセット
                    aComp.FunctionCallTargetReset();
                }

                // 関数コール命令
                aComp.BCFunction.AddOPCode_SymbolTableIndex(
                    BCOpCode.OpType.CALL
                    , funcSymbol
                    );

                // 関数の結果を受け取る
                if (mEvaluateInfo.SR.IsValid)
                {
                    aComp.BCFunction.AddOPCode_SReg(
                        BCOpCode.OpType.LDSRFZ
                        , mEvaluateInfo.SR
                        );
                }

                // 評価終了イベント送信
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // レジスタ解放
                mTransferredEI.ReleaseIfNeccesary(aComp);
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        Token mOpToken;
        SequenceExpression mSequenceExpr = null;
    }
}
