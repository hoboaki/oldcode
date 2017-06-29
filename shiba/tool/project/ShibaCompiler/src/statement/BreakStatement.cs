using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// BreakStatement。
    /// </summary>
    class BreakStatement : IStatement
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BreakStatement(Token aToken)
        {
            mToken = aToken;
        }


        //------------------------------------------------------------
        // 意味解析。
        public void SemanticAnalyze(SemanticAnalyzeComponent aComp)
        {
            // break命令をやりつつ、breakラベルを取得する
            BCLabel label = aComp.ExecBreakStatement();
            if (label == null)
            {// 見つからず
                aComp.ThrowErrorException(
                    SymbolTree.ErrorKind.INVALID_BREAK
                    , mToken
                    );
            }
            
            // break場所へジャンプ
            aComp.BCFunction.AddOPCode_Label(
                BCOpCode.OpType.JMP
                , label
                );
        }

        //------------------------------------------------------------
        // トレース。
        public void Trace(Tracer aTracer)
        {
            aTracer.WriteName("BreakStatement");
        }

        //============================================================
        Token mToken;
    }
}
