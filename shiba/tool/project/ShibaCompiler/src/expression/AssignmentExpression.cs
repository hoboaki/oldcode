using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 代入式。
    /// </summary>
    class AssignmentExpression : IExpression
    {
        //------------------------------------------------------------
        // 演算の種類。
        public enum OpKind
        {
            Unknown,
            Assign,
            AddAssign,
            SubAssign,
            MulAssign,
            DivAssign,
            ModAssign,
            RShiftAssign,
            LShiftAssign,
            AndAssign,
            OrAssign,
            XorAssign,
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public AssignmentExpression(Token aOpToken, OpKind aOpKind, IExpression aLeftExpr, IExpression aRightExpr)
        {
            mOpToken = aOpToken;
            mOpKind = aOpKind;
            mLeftExpr = aLeftExpr;
            mRightExpr = aRightExpr;
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
            aTracer.WriteName("AssignmentExpression");
            using (new Tracer.IndentScope(aTracer))
            {
                aTracer.WriteValue("mOpKind", mOpKind.ToString());
                mLeftExpr.Trace(aTracer);
                mRightExpr.Trace(aTracer);
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 評価ノード。
        class EvaluateNode : IEvaluateNode
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public EvaluateNode(AssignmentExpression aExpr)
            {
                mExpr = aExpr;
                mLeftNode = mExpr.mLeftExpr.CreateEvaluateNode();
                mRightNode = mExpr.mRightExpr.CreateEvaluateNode();
            }

            //------------------------------------------------------------
            // EvaluateInfoを取得する。
            public EvaluateInfo GetEvaluateInfo()
            {
                return mLeftNode.GetEvaluateInfo();
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
            AssignmentExpression mExpr;
            IEvaluateNode mLeftNode;
            IEvaluateNode mRightNode;

            //------------------------------------------------------------　
            // 評価準備。　
            void eventAnalyze(SemanticAnalyzeComponent aComp)
            {
                // 左を評価
                mLeftNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // 右を評価
                mRightNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);
            }

            //------------------------------------------------------------　
            // 評価実行。　
            void eventEvaluate(SemanticAnalyzeComponent aComp)
            {
                // todo:
                // 暗黙の変換の対応
                // 今はint,boolしか対応しない
                if (mLeftNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || mRightNode.GetEvaluateInfo().Kind != EvaluateInfo.InfoKind.Value
                    || (mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != mRightNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType())
                    || (mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    || (mRightNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && mRightNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_EXPRESSION, mExpr.mOpToken);
                }

                // 左を評価
                mLeftNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // 左のEvaluateInfoを伝達設定
                if (mExpr.mOpKind == OpKind.Assign)
                {// Assignのときだけ
                    // 再利用禁止設定
                    var ei = mLeftNode.GetEvaluateInfo();
                    ei.DisableReuseSR();

                    // 伝達
                    aComp.TransferredEvaluateInfoSet(ei);
                }

                // 右を評価
                mRightNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // Assignならここで終了
                if (mExpr.mOpKind == OpKind.Assign)
                {
                    if (!mLeftNode.GetEvaluateInfo().SR.IsSame(mRightNode.GetEvaluateInfo().SR))
                    {// レジスタが異なればロード命令を追加する
                        aComp.BCFunction.AddOPCode_SReg1_SReg2(
                            BCOpCode.OpType.LDSRSR
                            , mLeftNode.GetEvaluateInfo().SR
                            , mRightNode.GetEvaluateInfo().SR
                            );
                    }
                    return;
                }

                // boolなら演算ができないのでエラー扱い
                if (mLeftNode.GetEvaluateInfo().TypeInfo.Symbol.GetBuiltInType() == BuiltInType.Bool)
                {
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mExpr.mOpToken
                        , mLeftNode.GetEvaluateInfo().TypeInfo
                        , new TypeInfo(new TypeInfo.TypeSymbol(null, BuiltInType.SInt32), new TypeInfo.TypeAttribute(true,false))
                        ));
                }

                // 演算子に対応する命令を選択
                BCOpCode.OpType opType = BCOpCode.OpType.NOP;
                switch (mExpr.mOpKind)
                {
                    case OpKind.AddAssign: opType = BCOpCode.OpType.ADDI32; break;
                    case OpKind.SubAssign: opType = BCOpCode.OpType.SUBI32; break;
                    case OpKind.MulAssign: opType = BCOpCode.OpType.MULS32; break;
                    case OpKind.DivAssign: opType = BCOpCode.OpType.DIVS32; break;
                    case OpKind.ModAssign: opType = BCOpCode.OpType.MODS32; break;
                    case OpKind.AndAssign: opType = BCOpCode.OpType.ANDI32; break;
                    case OpKind.OrAssign: opType = BCOpCode.OpType.ORI32; break;
                    case OpKind.XorAssign: opType = BCOpCode.OpType.XORI32; break;
                    case OpKind.LShiftAssign: opType = BCOpCode.OpType.SLLI32; break;
                    case OpKind.RShiftAssign: opType = BCOpCode.OpType.SLRI32; break;
                    default:
                        Assert.NotReachHere();
                        break;
                }

                // 命令追加
                aComp.BCFunction.AddOPCode_SReg1_SReg2_SReg3(
                    opType
                    , mLeftNode.GetEvaluateInfo().SR
                    , mLeftNode.GetEvaluateInfo().SR
                    , mRightNode.GetEvaluateInfo().SR
                    );
            }

            //------------------------------------------------------------
            // 後始末。
            void eventRelease(SemanticAnalyzeComponent aComp)
            {
                // OnParentEvaluateEnd
                mRightNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
                mLeftNode.SendEvent(aComp, EvaluateNodeEventKind.Release);
            }
        };

        //------------------------------------------------------------
        // メンバ変数たち
        Token mOpToken;
        OpKind mOpKind;
        IExpression mLeftExpr;
        IExpression mRightExpr;
    }
}
