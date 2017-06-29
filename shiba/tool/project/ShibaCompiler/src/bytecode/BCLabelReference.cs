using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// ラベルのリファレンス情報。
    /// </summary>
    class BCLabelReference
    {
        public readonly BCLabel Label; // 解決すべきラベル。
        public readonly BCOpCode OpCode; // 解決した情報を代入するOpCode。
        public readonly uint OpCodeIndex; // 解決した情報を代入するOpCodeのインデックス。

        //------------------------------------------------------------
        // コンストラクタ。
        public BCLabelReference(BCLabel aLabel,BCOpCode aOpCode, uint aOpCodeIndex)
        {
            Label = aLabel;
            OpCode = aOpCode;
            OpCodeIndex = aOpCodeIndex;
        }
    }
}
