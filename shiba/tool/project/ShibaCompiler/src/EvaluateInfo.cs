using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// 評価情報。
    /// </summary>
    class EvaluateInfo
    {
        //------------------------------------------------------------
        // 情報の種類。
        public enum InfoKind
        {
            Unknown,
            Value, //値。
            Address, // アドレス。refを使うとこれになる。
            StaticSymbol, // 静的シンボル。
        };

        //------------------------------------------------------------
        // メンバ変数たち。
        public readonly InfoKind Kind;  // 情報の種類。
        public readonly TypeInfo TypeInfo;  // 型。Value・Addressのときのみ有効
        public readonly ISymbolNode Symbol; // 示しているシンボル。StaticSymbolのときのみ有効。

        //------------------------------------------------------------
        // 評価結果が格納されるStackRegister。Value・Addressのときに割り当てられる。
        public StackRegister SR
        {
            get 
            {
                Assert.Check(Kind == InfoKind.Value || Kind == InfoKind.Address);
                return mSR;
            }
            set
            {
                Assert.Check(Kind == InfoKind.Value || Kind == InfoKind.Address);
                mSR = value;
            }
        }

        //------------------------------------------------------------
        // SRの内容を書き換えて再利用していいか。初期値true。
        public bool IsReusableSR
        {
            get
            {
                Assert.Check(Kind == InfoKind.Value || Kind == InfoKind.Address);
                return mIsReusableSR;
            }
        }

        //------------------------------------------------------------
        // SRを再利用禁止にする。
        public void DisableReuseSR()
        {
            mIsReusableSR = false;
        }
        
        //------------------------------------------------------------
        // 値として作成する。
        static public EvaluateInfo CreateAsValue(TypeInfo aTypeInfo)
        {
            return new EvaluateInfo(
                InfoKind.Value
                , aTypeInfo
                , null
                );
        }

        //------------------------------------------------------------
        // アドレスとして作成する。
        static public EvaluateInfo CreateAsAddress(TypeInfo aTypeInfo)
        {
            return new EvaluateInfo(
                InfoKind.Address
                , aTypeInfo
                , null
                );
        }

        //------------------------------------------------------------
        // 静的シンボルとして作成する。
        static public EvaluateInfo CreateAsStaticSymbol(ISymbolNode aSymbolNode)
        {
            return new EvaluateInfo(
                InfoKind.StaticSymbol
                , null
                , aSymbolNode
                );
        }

        //============================================================

        //------------------------------------------------------------
        // メンバ変数たち。
        private StackRegister mSR;
        private bool mIsReusableSR;

        //------------------------------------------------------------
        // コンストラクタ。
        EvaluateInfo(InfoKind aInfoKind, TypeInfo aTypeInfo, ISymbolNode aSymbol)
        {
            this.Kind = aInfoKind;
            this.TypeInfo = aTypeInfo;
            this.Symbol = aSymbol;
            this.mIsReusableSR = true;
        }

    }
}
