using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Rootとなる識別子式。
    /// </summary>
    class RootIdentExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public RootIdentExpression(Identifier aIdent, bool aIsNamespaceRoot)
        {
            mIdent = aIdent;
            mIsNamespaceRoot = aIsNamespaceRoot;
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
            return mIdent.Token;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("RootIdentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                mIdent.Trace(aTracer, "mIdent");
                aTracer.WriteValue("mIsNamespaceRoot", mIsNamespaceRoot.ToString());
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(RootIdentExpression aExpr)
            {
                mExpr = aExpr;
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
                        break;
                }
            }

            //============================================================
            RootIdentExpression mExpr;
            EvaluateInfo mEvaluateInfo;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // todo: Rootの対応
                Assert.Check(!mExpr.mIsNamespaceRoot);

                // シンボルの検索
                ISymbolNode symbolNode = aComp.FindSymbolNode(mExpr.mIdent);
                if (symbolNode == null)
                {// シンボルが見つからない
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.NOT_DECLARATION_IDENT
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mIdent.Token
                        ));
                }

                // 評価情報作成
                if (symbolNode.GetNodeKind() == SymbolNodeKind.Variable)
                {// 変数
                    // 対応する評価済みノードを探す
                    EvaluatedSymbolNode evaluatedSymbolNode = aComp.FindEvaluatedSymbolNode(symbolNode);
                    if (evaluatedSymbolNode == null)
                    {// todo: ローカル変数以外の対応
                        Assert.NotReachHere();
                    }
                    // 作成
                    mEvaluateInfo = evaluatedSymbolNode.EvaluateNode.GetEvaluateInfo();
                }
                else
                {// シンボル
                    mEvaluateInfo = EvaluateInfo.CreateAsStaticSymbol(symbolNode);
                }
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // ローカル変数以外ならここでレジスタを確保することがある
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // ローカル変数以外ならここでレジスタを解放することがある
            }

            //------------------------------------------------------------
            // 文の終了。
            void eventOnStatementEnd(SemanticAnalyzeComponent aComp)
            {
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        Identifier mIdent;
        bool mIsNamespaceRoot;
    }
}
