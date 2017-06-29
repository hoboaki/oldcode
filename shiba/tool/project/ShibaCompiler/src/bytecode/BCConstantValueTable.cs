using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：定数テーブル。
    /// </summary>
    class BCConstantValueTable
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCConstantValueTable()
        {
            mList = new List<BCConstantValue>();
        }

        //------------------------------------------------------------
        // 適切なConstantValueを取得する。
        public BCConstantValue CheckAndGet(BCConstantValue aValue)
        {
            // 既に存在していればそれを取得する
            foreach (var entry in mList)
            {
                if (entry.Equals(aValue))
                {// あった
                    return entry;
                }
            }

            // なければ追加してそれを返す
            mList.Add(aValue);
            return aValue;
        }

        //------------------------------------------------------------
        // 各定数のオフセット位置を計算する。
        public void CalcOffsetPos()
        {
            // todo:
            // 実行時に解決する定数を後ろに移動させるstable_sort。

            // 計算
            uint pos = 0;
            foreach (var entry in mList)
            {
                pos = entry.CalcOffset(pos);
            }
            mSize = pos;
        }

        //------------------------------------------------------------
        // XDataにリファレンスタグを書き込む。
        public void XDataWriteReference(XDataWriter aWriter)
        {
            aWriter.WriteReferenceLine("constantTable", XDATA_LABEL);
        }
        
        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntity(XDataWriter aWriter)
        {
            aWriter.WriteCommentLine("BCConstantValueTable");
            using (new XDataWriter.IndentScope(aWriter))
            {
                // アライメントとラベル
                aWriter.WriteAlignLine(8);
                aWriter.WriteLabelLine(XDATA_LABEL);

                // テーブルのサイズ
                aWriter.WriteUInt32Line("size", mSize);

                // 各定数
                uint pos = 0;
                foreach (var entry in mList)
                {

                    // オフセット位置までpadding
                    uint offset = entry.Offset();
                    if (pos < offset)
                    {
                        uint padSize = offset - pos;
                        aWriter.WriteIndent();
                        aWriter.WriteComment("padding (" + padSize + ")");
                        for (uint i = 0; i < padSize; ++i)
                        {
                            aWriter.WriteUInt8(0xFF);
                        }
                        aWriter.WriteLine();
                    }
                    pos = offset;

                    // 値
                    entry.XDataWriteEntity(aWriter);
                    pos += entry.Size();
                }
            }
        }

        //============================================================
        const string XDATA_LABEL = "LabelConstantTable";
        List<BCConstantValue> mList;
        uint mSize;
    }
}
