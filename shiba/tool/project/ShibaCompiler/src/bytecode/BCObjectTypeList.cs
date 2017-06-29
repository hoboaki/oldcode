using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：ObjectTypeのリスト。
    /// </summary>
    class BCObjectTypeList
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCObjectTypeList()
        {
            mList = new List<BCObjectType>();
        }

        //------------------------------------------------------------
        // 追加。
        public void Add(BCObjectType aEntry)
        {
            mList.Add(aEntry);
        }

        //------------------------------------------------------------
        // XDataにリファレンスタグを書き込む。
        public void XDataWriteReference(XDataWriter aWriter)
        {
            aWriter.WriteReferenceLine("objectTypeList", XDATA_LABEL);
        }

        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntity(XDataWriter aWriter)
        {
            aWriter.WriteCommentLine("BCObjectTypeList");
            using (new XDataWriter.IndentScope(aWriter))
            {
                // アライメントとラベル
                aWriter.WriteAlignLine(4);
                aWriter.WriteLabelLine(XDATA_LABEL);

                // 数
                aWriter.WriteUInt32Line("count", (uint)mList.Count);

                // リファレンス
                foreach (var entry in mList)
                {
                    entry.XDataWriteReference(aWriter);
                }
            }

            // 実体
            {
                foreach (var entry in mList)
                {
                    entry.XDataWriteEntry(aWriter);
                }
            }
        }

        //============================================================
        const string XDATA_LABEL = "LabelObjectTypeList";
        List<BCObjectType> mList;
    }
}
