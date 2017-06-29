using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：シンボルテーブル。
    /// </summary>
    class BCSymbolLinkTable
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCSymbolLinkTable()
        {
            mList = new List<BCSymbolLink>();
        }

        //------------------------------------------------------------
        // 適切なSymbolLinkを取得する。
        public BCSymbolLink CheckAndGet(ISymbolNode aSymbol)
        {
            // 既に存在していればそれを取得する
            foreach (var entry in mList)
            {
                if (System.Object.ReferenceEquals(entry.TargetNode,aSymbol))
                {// あった
                    return entry;
                }
            }

            // なければ追加してそれを返す
            // todo: 0xFFFFを越えたときの処理
            var newLink = new BCSymbolLink(aSymbol, (ushort)mList.Count);
            mList.Add(newLink);
            return newLink;
        }

        //------------------------------------------------------------
        // XDataにリファレンスタグを書き込む。
        public void XDataWriteReference(XDataWriter aWriter)
        {
            aWriter.WriteReferenceLine("symbolTable", XDATA_LABEL);
        }

        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntity(XDataWriter aWriter)
        {
            aWriter.WriteCommentLine("BCSymbolTable");
            using (new XDataWriter.IndentScope(aWriter))
            {
                // アライメントとラベル
                aWriter.WriteAlignLine(8);
                aWriter.WriteLabelLine(XDATA_LABEL);

                // 要素数
                aWriter.WriteUInt32Line("count", (uint)mList.Count);

                // 各シンボル
                foreach (var entry in mList)
                {
                    // 値
                    entry.XDataWriteEntity(aWriter);
                }
            }
        }

        //============================================================
        const string XDATA_LABEL = "LabelSymbolTable";
        List<BCSymbolLink> mList;
    }
}
