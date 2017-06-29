using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：定数。
    /// </summary>
    class BCConstantValue
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCConstantValue(int aValue)
        {
            mKind = Kind.SInt32;
            mValSInt32 = aValue;
        }

        //------------------------------------------------------------
        // 等しいか。
        public bool Equals(BCConstantValue aRHS)
        {
            // 種類チェック
            if (mKind != aRHS.mKind)
            {
                return false;
            }

            // 種類ごとのチェック
            switch (mKind)
            {
                case Kind.SInt32: return mValSInt32 == aRHS.mValSInt32;
                default:
                    Assert.NotReachHere();
                    return false;
            }
        }

        //------------------------------------------------------------
        // オフセット計算。計算し、末尾（使用領域の一番後ろ+1）の位置を返す。
        public uint CalcOffset(uint aPos)
        {
            // アライメントが求める位置まで進める
            uint pos = aPos;
            uint align = requireAlign();
            uint mod = aPos % align;
            if (mod != 0)
            {
                pos += align - mod;
            }

            // オフセットはこの位置
            mOffset = pos;

            // サイズを進める
            pos += requireSize();

            // 返す
            return pos;
        }

        //------------------------------------------------------------
        // オフセット位置を返す。
        public uint Offset()
        {
            return mOffset;
        }
        
        //------------------------------------------------------------
        // サイズを返す。
        public uint Size()
        {
            return requireSize();
        }

        //------------------------------------------------------------
        public void XDataWriteEntity(XDataWriter aWriter)
        {
            switch (mKind)
            {
                case Kind.SInt32:
                    aWriter.WriteSInt32Line("0x" + mOffset.ToString("X4") + ":",mValSInt32);
                    break;

                default:
                    Assert.NotReachHere();
                    break;
            }
        }

        //============================================================

        //------------------------------------------------------------
        // 種類。
        enum Kind
        {
            UNKNOWN,
            SInt32,
        };

        //------------------------------------------------------------
        // アライメント値を取得する。
        uint requireAlign()
        {
            switch (mKind)
            {
                case Kind.SInt32: 
                    return 4;

                default:
                    Assert.NotReachHere();
                    return 0;
            }
        }

        //------------------------------------------------------------
        // 必要なサイズを取得する。
        uint requireSize()
        {
            switch (mKind)
            {
                case Kind.SInt32:
                    return 4;

                default:
                    Assert.NotReachHere();
                    return 0;
            }            
        }

        //------------------------------------------------------------
        // メンバ変数たち。
        Kind mKind;
        uint mOffset;
        int mValSInt32;
    }
}
