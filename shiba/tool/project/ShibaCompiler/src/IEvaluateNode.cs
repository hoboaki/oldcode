using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 評価ノード。
    /// </summary>
    interface IEvaluateNode
    {
        //------------------------------------------------------------
        // 評価情報を取得する。
        EvaluateInfo GetEvaluateInfo();

        //------------------------------------------------------------
        // 評価イベントを送信する。
        void SendEvent(SemanticAnalyzeComponent aComponent,EvaluateNodeEventKind aEventKind);
    }
}
