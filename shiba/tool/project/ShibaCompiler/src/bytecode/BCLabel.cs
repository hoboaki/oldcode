using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコードのラベルを示すクラス。
    /// </summary>
    class BCLabel
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCLabel()
        {
            mOpCodeIndex = INVALID_OP_CODE_INDEX;
        }

        //------------------------------------------------------------
        // ラベルが示すOpCodeのインデックス。
        public uint OpCodeIndex
        {
            get 
            {
                Assert.Check(mOpCodeIndex != INVALID_OP_CODE_INDEX);
                return mOpCodeIndex; 
            }
        }

        //------------------------------------------------------------
        // OpCodeIndexを設定する。
        public void SetOpCodeIndex(uint aOpCodeIndex)
        {
            Assert.Check(mOpCodeIndex == INVALID_OP_CODE_INDEX);
            mOpCodeIndex = aOpCodeIndex;
        }

        //============================================================
        const uint INVALID_OP_CODE_INDEX = 0xFFFFFFFF;

        uint mOpCodeIndex;
    }
}
