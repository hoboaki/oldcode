using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// Return文。
    /// </summary>
    class ReturnStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ReturnStatement(Token aKeyToken, IExpression aExpression)
        {
            mKeyToken = aKeyToken;
            mExpression = aExpression;
        }

        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // 戻り値用の評価情報を取得
            EvaluateInfo returnEI = aComp.ReturnEvaluateInfoGet();
            if (returnEI == null)
            {// 戻り値がないとき
                // 式がないことをチェック
                if (mExpression != null)
                {// エラー
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.SEMICOLON_EXPECTED, mExpression.GetToken());
                }

                // Return実行
                execReturn(aComp);
            }
            else
            {
                // 式があることをチェック
                if (mExpression == null)
                {// エラー
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.EXPRESSION_EXPECTED, mKeyToken);
                }

                // 評価ノードを作成
                var exprNode = mExpression.CreateEvaluateNode();

                // 評価準備
                exprNode.SendEvent(aComp, EvaluateNodeEventKind.Analyze);

                // 評価情報を伝達
                aComp.TransferredEvaluateInfoSet(returnEI);

                // 評価実行
                exprNode.SendEvent(aComp, EvaluateNodeEventKind.Evaluate);

                // 伝達リセット
                aComp.TransferredEvaluateInfoReset();

                // 型確認
                Assert.Check(exprNode.GetEvaluateInfo().Kind == EvaluateInfo.InfoKind.Value);
                EvaluateInfo exprEI = exprNode.GetEvaluateInfo();
                TypeInfo exprTypeInfo = exprEI.TypeInfo;
                if (exprTypeInfo.Symbol.GetKind() != TypeInfo.TypeSymbol.Kind.BuiltIn
                    || (exprTypeInfo.Symbol.GetBuiltInType() != BuiltInType.SInt32 && exprTypeInfo.Symbol.GetBuiltInType() != BuiltInType.Bool)
                    )
                {// todo: 今はboolとintしかサポートしない
                    aComp.ThrowErrorException(SymbolTree.ErrorKind.NOT_SUPPORTED_TYPENAME, exprTypeInfo.Symbol.GetToken());
                }

                // 型が同じか確認
                if (returnEI.TypeInfo.Symbol.GetBuiltInType() != exprTypeInfo.Symbol.GetBuiltInType())
                {// 違う型
                    // エラーメッセージ
                    throw new SymbolTree.ErrorException(new SymbolTree.ErrorInfo(
                        SymbolTree.ErrorKind.CANT_IMPLICIT_CAST_TYPEA_TO_TYPEB
                        , aComp.TypeSymbolNode.ModuleContext()
                        , mKeyToken
                        , exprTypeInfo
                        , returnEI.TypeInfo
                        ));
                }

                // レジスタが異なればロード命令を追加する
                if (!returnEI.SR.IsSame(exprEI.SR))
                {
                    aComp.BCFunction.AddOPCode_SReg1_SReg2(
                        BCOpCode.OpType.LDSRSR
                        , returnEI.SR
                        , exprEI.SR
                        );
                }

                // 親の評価終了 
                exprNode.SendEvent(aComp, EvaluateNodeEventKind.Release);

                // return実行
                execReturn(aComp);
            }
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ReturnStatement");
        }

        //============================================================

        //------------------------------------------------------------
        // returnを実行。
        void execReturn(SemanticAnalyzeComponent aComp)
        {
            // Returnを実行
            BCLabel label = aComp.ExecReturnStatement();
            Assert.Check(label != null);

            // Returnのラベルにジャンプ
            aComp.BCFunction.AddOPCode_Label(
                BCOpCode.OpType.JMP
                , label
                );
        }

        //------------------------------------------------------------
        // メンバ変数たち。
        Token mKeyToken;
        IExpression mExpression;
    }
}
