using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 評価済みシンボルノード。
    /// </summary>
    class EvaluatedSymbolNode
    {
        public readonly ISymbolNode   SymbolNode;
        public readonly IEvaluateNode EvaluateNode;

        //------------------------------------------------------------
        // コンストラクタ。
        public EvaluatedSymbolNode(ISymbolNode aSymbolNode, IEvaluateNode aEvaluateNode)
        {
            SymbolNode = aSymbolNode;
            EvaluateNode = aEvaluateNode;
        }
    }
}
