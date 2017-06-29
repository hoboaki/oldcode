using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// バイトコード：Functionのリスト。
    /// </summary>
    class BCFunctionList
    {
        //------------------------------------------------------------
        // コンストラクタ。
        public BCFunctionList()
        {
            mList = new List<BCFunction>();
        }

        //------------------------------------------------------------
        // 追加。
        public void Add(BCFunction aEntry)
        {
            mList.Add(aEntry);
        }

        //------------------------------------------------------------
        // XDataにリファレンスタグを書き込む。
        public void XDataWriteReference(XDataWriter aWriter,string aOwnerPath)
        {
            aWriter.WriteReferenceLine("FunctionList", XDATA_LABEL + ":" + aOwnerPath);
        }

        //------------------------------------------------------------
        // XDataに実体を書き込む。
        public void XDataWriteEntity(XDataWriter aWriter, string aOwnerPath)
        {
            aWriter.WriteCommentLine("BCFunctionList(" + aOwnerPath + ")");
            using (new XDataWriter.IndentScope(aWriter))
            {
                // アライメントとラベル
                aWriter.WriteAlignLine(4);
                aWriter.WriteLabelLine(XDATA_LABEL + ":" + aOwnerPath);

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
                    entry.XDataWriteEntity(aWriter);
                }
            }
        }

        //============================================================
        const string XDATA_LABEL = "LabelFunctionList";
        List<BCFunction> mList;
    }
}
