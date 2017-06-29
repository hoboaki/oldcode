using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 式を示すインターフェース。
    /// </summary>
    interface IExpression
    {
        //------------------------------------------------------------
        // 評価ノードを作成する。
        IEvaluateNode CreateEvaluateNode();

        //------------------------------------------------------------
        // トークンを取得する。
        Token GetToken();

        //------------------------------------------------------------
        // トレース。
        void Trace(Tracer aTracer);
    }
}
