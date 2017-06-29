using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ContinueStatement。
    /// </summary>
    class ContinueStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public ContinueStatement(Token aToken)
        {
            mToken = aToken;
        }


        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // Continue命令をやりつつ、Continueラベルを取得する
            BCLabel label = aComp.ExecContinueStatement();
            if (label == null)
            {// 見つからず
                aComp.ThrowErrorException(
                    SymbolTree.ErrorKind.INVALID_CONTINUE
                    , mToken
                    );
            }

            // Continue場所へジャンプ
            aComp.BCFunction.AddOPCode_Label(
                BCOpCode.OpType.JMP
                , label
                );
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("ContinueStatement");
        }

        //============================================================
        Token mToken;
    }
}
