using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// スタックレジスタを示すクラス。
    /// </summary>
    struct StackRegister
    {
        public readonly bool IsValid; // 有効か。structなので既定値falseで初期化される。

        //------------------------------------------------------------
        // コンストラクタ。
        public StackRegister(uint aIndex)
        {
            IsValid = true;
            mIndex = aIndex;
        }

        //------------------------------------------------------------
        // インデックス値の取得。
        public byte Index()
        {
            Assert.Check(IsValid);
            return (byte)mIndex;
        }

        //------------------------------------------------------------
        // アセンブラテキストに変換。
        public string ToASMText()
        {
            return "SR" + mIndex.ToString("X2");
        }

        //------------------------------------------------------------
        // 等しいか。
        public bool IsSame(StackRegister aRHS)
        {
            return IsValid == aRHS.IsValid && mIndex == aRHS.mIndex;
        }

        //============================================================
        readonly uint mIndex;   // インデックス値。
    }
}
