using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 二項演算式。
    /// </summary>
    class BinaryOpExpression : IExpression
    {
        //------------------------------------------------------------
        // 演算の種類。
        public enum OpKind
        {
            Unknnown,
            AdditiveAdd,
            AdditiveSub,
            BitwiseAnd,
            BitwiseOr,
            BitwiseXor,
            EqualityEqual,
            EqualityNotEqual,
            IdentityEqual,
            IdentityNotEqual,
            LogicalAnd,
            LogicalOr,
            MultiplicativeDiv,
            MultiplicativeMod,
            MultiplicativeMul,
            RelationalGreater,
            RelationalGreaterEqual,
            RelationalLess,
            RelationalLessEqual,
            ShiftLeft,
            ShiftRight,
        };

        //------------------------------------------------------------
        // コンストラクタ
        public BinaryOpExpression(Token aOpToken, OpKind aOpKind, IExpression aFirst,IExpression aSecond)
        {
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mFirst = aFirst;
            mSecond = aSecond;
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
            aTracer.WriteName("BinaryOpExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mFirst.Trace(aTracer);
                mSecond.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(BinaryOpExpression aExpr)
            {
                mExpr = aExpr;
                mFirstNode = mExpr.mFirst.CreateEvaluateNode();
                mSecondNode = mExpr.mSecond.CreateEvaluateNode();
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
            BinaryOpExpression mExpr;
            IEvaluateNode mFirstNode;
            IEvaluateNode mSecondNode;
            EvaluateInfo mEvaluateInfo;
            TransferredEIHolder mTransferredEIHolder;
            
            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // １つめ
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // ２つめ
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
                
                // 評価情報の作成
                TypeInfo ti;
                switch (mExpr.mOpKind)
                {
                    case OpKind.RelationalGreater:
                    case OpKind.RelationalGreaterEqual:
                    case OpKind.RelationalLess:
                    case OpKind.RelationalLessEqual:
                    case OpKind.EqualityEqual:
                    case OpKind.EqualityNotEqual:
                    case OpKind.IdentityEqual:
                    case OpKind.IdentityNotEqual:
                    case OpKind.LogicalAnd:
                    case OpKind.LogicalOr:
                        // boolの情報を返す　
                        ti = new TypeInfo(new TypeInfo.TypeSymbol(mExpr.mOpToken, BuiltInType.Bool), new TypeInfo.TypeAttribute(true, false));
                        break;

                    default:
                        // 左辺の情報をそのまま返す
                        // todo: 暗黙の変換対応
                        ti = mFirstNode.GetEvaluateInfo().TypeInfo;
                        break;
                }
                mEvaluateInfo = EvaluateInfo.CreateAsValue(ti);

                // Holder作成
                mTransferredEIHolder = new TransferredEIHolder(mEvaluateInfo);
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {                
                // 各演算ごとの処理
                switch (mExpr.mOpKind)
                {
                    case OpKind.AdditiveAdd: evaluateNumberType(aComp, BCOpCode.OpType.ADDI32, false); break;
                    case OpKind.AdditiveSub: evaluateNumberType(aComp, BCOpCode.OpType.SUBI32, false); break;
                    case OpKind.BitwiseAnd: evaluateNumberType(aComp, BCOpCode.OpType.ANDI32, false); break;
                    case OpKind.BitwiseOr: evaluateNumberType(aComp, BCOpCode.OpType.ORI32, false); break;
                    case OpKind.BitwiseXor: evaluateNumberType(aComp, BCOpCode.OpType.XORI32, false); break;
                    case OpKind.EqualityEqual: evaluateEquality(aComp, false); break;
                    case OpKind.EqualityNotEqual: evaluateEquality(aComp, true); break;
                    case OpKind.LogicalAnd: evaluateLogicalOp(aComp, true); break;
                    case OpKind.LogicalOr: evaluateLogicalOp(aComp, false); break;
                    case OpKind.MultiplicativeMul: evaluateNumberType(aComp, BCOpCode.OpType.MULS32, false); break;
                    case OpKind.MultiplicativeDiv: evaluateNumberType(aComp, BCOpCode.OpType.DIVS32, false); break;
                    case OpKind.MultiplicativeMod: evaluateNumberType(aComp, BCOpCode.OpType.MODS32, false); break;
                    case OpKind.RelationalLess: evaluateNumberType(aComp, BCOpCode.OpType.LTS32, false); break;
                    case OpKind.RelationalLessEqual: evaluateNumberType(aComp, BCOpCode.OpType.LES32, false); break;
                    case OpKind.RelationalGreater: evaluateNumberType(aComp, BCOpCode.OpType.LTS32, true); break;
                    case OpKind.RelationalGreaterEqual: evaluateNumberType(aComp, BCOpCode.OpType.LES32, true); break;
                    case OpKind.ShiftLeft: evaluateNumberType(aComp, BCOpCode.OpType.SLLI32, false); break;
                    case OpKind.ShiftRight: evaluateNumberType(aComp, BCOpCode.OpType.SLRI32, false); break;
                    default:
                        {
                            // todo:
                            // いろんな演算の対応
                            aComp.ThrowErrorException(
                                SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION
                                , mExpr.mOpToken
                                );
                        }
                        break;
                }
            }

            //------------------------------------------------------------
            // 同じ数値の演算しか許さないタイプの演算。
            void evaluateNumberType(
                SemanticAnalyzeComponent aComp
                , BCOpCode.OpType aOpType
                , bool aSwapLR // trueなら左辺と右辺を逆転させる。
                )
            {
                // レジスタ設定
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // 伝達設定
                mTransferredEIHolder.TransferIfPossible(aComp);

                // １つめ
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // 可能なら２つめに伝達する
                if (!mFirstNode.GetEvaluateInfo().SR.IsSame(mEvaluateInfo.SR))
                {
                    mTransferredEIHolder.TransferIfPossible(aComp);
                }

                // ２つめ
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // todo:
                // 暗黙の変換の対応
                // 今はintしか対応しない
                if (mFirstNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mFirstNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32
                    || mSecondNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mSecondNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // 演算
                StackRegister leftSR = mFirstNode.GetEvaluateInfo().SR;
                StackRegister rightSR = mSecondNode.GetEvaluateInfo().SR;
                aComp.BCFunction.AddOPCode_SReg1_SReg2_SReg3(
                    aOpType
                    , mEvaluateInfo.SR
                    , aSwapLR ? rightSR : leftSR
                    , aSwapLR ? leftSR : rightSR
                    );

                // 通知
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
            
            //------------------------------------------------------------
            // '==','!='の演算。
            void evaluateEquality(
                SemanticAnalyzeComponent aComp
                , bool aIsNotEqual
                )
            {
                // レジスタ設定
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // 伝達設定
                mTransferredEIHolder.TransferIfPossible(aComp);

                // １つめ
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // 可能なら２つめに伝達する
                if ( !mFirstNode.GetEvaluateInfo().SR.IsSame( mEvaluateInfo.SR ) )
                {
                    mTransferredEIHolder.TransferIfPossible(aComp);
                }

                // ２つめ
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // todo:
                // 暗黙の変換の対応
                // 今はint,boolしか対応しない
                var firstEI = mFirstNode.GetEvaluateInfo();
                var secondEI = mSecondNode.GetEvaluateInfo();
                if (firstEI.Kind != EvaluateInfo.InfoKind.Value
                    || (firstEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && firstEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    || secondEI.Kind != EvaluateInfo.InfoKind.Value
                    || (secondEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && secondEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // 命令の選択
                BCOpCode.OpType opType = BCOpCode.OpType.NOP;
                switch (firstEI.TypeInfo.Symbol.GetBuiltInType())
                {
                    case BuiltInType.SInt32:
                    case BuiltInType.UInt32:
                        opType = aIsNotEqual ? BCOpCode.OpType.NEI32 : BCOpCode.OpType.EQI32;
                        break;

                    case BuiltInType.Bool:
                        opType = aIsNotEqual ? BCOpCode.OpType.NEBOOL : BCOpCode.OpType.EQBOOL;
                        break;

                    default:
                        aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                        break;
                }

                // 追加
                aComp.BCFunction.AddOPCode_SReg1_SReg2_SReg3(opType, mEvaluateInfo.SR, firstEI.SR, secondEI.SR);

                // 通知
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }

            //------------------------------------------------------------
            // '&&','||'の演算。
            void evaluateLogicalOp(
                SemanticAnalyzeComponent aComp
                , bool aIsAnd
                )
            {
                // boolしか対応しない
                var firstEI = mFirstNode.GetEvaluateInfo();
                var secondEI = mSecondNode.GetEvaluateInfo();
                if (firstEI.Kind != EvaluateInfo.InfoKind.Value
                    || firstEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool
                    )
                {
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mOpToken
                        , firstEI.TypeInfo
                        , mEvaluateInfo.TypeInfo
                        ));
                }
                if (secondEI.Kind != EvaluateInfo.InfoKind.Value
                    || secondEI.TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool
                    )
                {
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mOpToken
                        , secondEI.TypeInfo
                        , mEvaluateInfo.TypeInfo
                        ));
                }

                // レジスタ設定
                mTransferredEIHolder.ReceiveAndSetSR(aComp);

                // 伝達設定
                mTransferredEIHolder.TransferIfPossible(aComp);

                // ラベル確保
                BCLabel labelEnd = aComp.BCFunction.LabelCreate(); // End用

                // １つめ
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                mFirstNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // １つめの結果を受けた短絡評価
                // 条件が整っていたら２つ目の式を飛ばす
                aComp.BCFunction.AddOPCode_SReg_Label(
                    aIsAnd ? BCOpCode.OpType.JMPNEG : BCOpCode.OpType.JMPPOS
                    , mEvaluateInfo.SR // 伝達しているので自分のSRに結果が格納されているはず　
                    , labelEnd
                    );

                // 伝達設定
                mTransferredEIHolder.TransferIfPossible(aComp);

                // ２つめ
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);
                mSecondNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // ２つめの結果を代入
                if (!mEvaluateInfo.SR.IsSame(secondEI.SR))
                {
                    aComp.BCFunction.AddOPCode_SReg1_SReg2(
                        BCOpCode.OpType.LDSRSR
                        , mEvaluateInfo.SR
                        , secondEI.SR
                        );
                }
                
                // Endラベル挿入
                aComp.BCFunction.LabelInsert(labelEnd);
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // レジスタ返却
                mTransferredEIHolder.ReleaseIfNeccesary(aComp);
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        Token mOpToken;
        OpKind mOpKind;
        IExpression mFirst;
        IExpression mSecond;
    }
}
