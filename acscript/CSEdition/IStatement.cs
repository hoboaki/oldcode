using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 文を示すインターフェース。
    /// </summary>
    interface IStatement
    {
        //------------------------------------------------------------
        // 意味解析。
        bool SemanticAnalyze(SemanticAnalyzeComponent aComp);

        //------------------------------------------------------------
        // トレース。
        void Trace(Tracer aTracer);
    }
}
