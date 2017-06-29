using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 定数式。
    /// </summary>
    class ConstantLiteralExpression : IExpression
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ConstantLiteralExpression(Token aT)
        {
            mToken = aT;
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
            return mToken;
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ConstantLiteralExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mToken.Value", mToken.Value.ToString());
                switch (mToken.Value)
                {
                    case Token.Kind.StringChar8:
                    case Token.Kind.StringChar16:
                    case Token.Kind.StringChar32:
                        aTracer.WriteValue("value", mToken.UString);
                        break;

                    case Token.Kind.NumSInt32:
                    case Token.Kind.NumSInt64:
                        aTracer.WriteValue("value", mToken.Int64Value.ToString());
                        break;

                    case Token.Kind.NumUInt32:
                    case Token.Kind.NumUInt64:
                        aTracer.WriteValue("value", mToken.UInt64Value.ToString());
                        break;

                    case Token.Kind.NumFloat32:
                        aTracer.WriteValue("value", mToken.Float32Value.ToString());
                        break;

                    case Token.Kind.NumFloat64:
                        aTracer.WriteValue("value", mToken.Float64Value.ToString());
                        break;

                    default:
                        break;
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
            public EvaluateNode(ConstantLiteralExpression aExpr)
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
                        return;
                }
            }

            //============================================================
            ConstantLiteralExpression mExpr;
            EvaluateInfo mEvaluateInfo;
            EvaluateInfo mTransferredEI;
            
            //------------------------------------------------------------　
            // 評価準備。　
            bool eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // BuiltInTypeの選択
                BuiltInType builtInType;
                switch (mExpr.mToken.Value)
                {
                    case Token.Kind.NumSInt32:
                        builtInType = BuiltInType.SInt32;
                        break;

                    case Token.Kind.KeyTrue:
                    case Token.Kind.KeyFalse:
                        builtInType = BuiltInType.Bool;
                        break;

                    default:
                        // todo: いろんな型の対応
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mToken);
                        return false;
                }

                // EvaluateInfo作成
                TypePath typePath = new TypePath(mExpr.mToken, builtInType);
                mEvaluateInfo = EvaluateInfo.CreateAsValue(
                    aComp.CreateTypeInfo(typePath, true, true)
                    );

                return true;
            }

            //------------------------------------------------------------　
            // 評価実行。　
            bool eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // 伝達された評価情報を取得
                mTransferredEI = aComp.TransferredEvaluateInfoReceive();

                // レジスタ確保　
                mEvaluateInfo.SR = mTransferredEI == null
                    ? aComp.SRReserve()
                    : mTransferredEI.SR;

                // ロードする
                switch (mExpr.mToken.Value)
                {
                    case Token.Kind.NumSInt32:
                        aComp.BCFunction.AddOPCode_SReg_ConstantTableIndex(
                            BCOpCode.OpType.LDSRC4
                            , mEvaluateInfo.SR
                            , (int)mExpr.mToken.Int64Value
                            );
                        break;

                    case Token.Kind.KeyTrue:
                        aComp.BCFunction.AddOPCode_SReg(
                            BCOpCode.OpType.LDSRBT
                            , mEvaluateInfo.SR
                            );
                        break;

                    case Token.Kind.KeyFalse:
                        aComp.BCFunction.AddOPCode_SReg(
                            BCOpCode.OpType.LDSRZR
                            , mEvaluateInfo.SR
                            );
                        break;

                    default:
                        // todo: いろんな型の対応
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mToken);
                        return false;
                }

                // 成功
                return true;
            }

            //------------------------------------------------------------
            // 後始末。
            bool eventRelease(SemanticAnalyzeComponent aComp)
            {
                // レジスタ返却
                if (mTransferredEI == null)
                {
                    aComp.SRRelease(mEvaluateInfo.SR);
                }

                return true;
            }

            //------------------------------------------------------------
            // 文の終了。
            bool eventOnStatementEnd(SemanticAnalyzeComponent aComp)
            {
                // 何もやることはないはず
                return true;
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        Token mToken;
    }
}
