using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// 式を示すインターフェース。
    /// </summary>
    interface IExpression
    {
        //------------------------------------------------------------
        // トレース。
        void Trace(Tracer aTracer);
    }
}
