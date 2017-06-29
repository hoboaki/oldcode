using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShibaCompiler
{
    /// <summary>
    /// XDataを書き込むクラス。
    /// </summary>
    class XDataWriter
    {
        //------------------------------------------------------------
        // インデントレベルをコントロールするサポートクラス。
        public class IndentScope : IDisposable
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public IndentScope(XDataWriter aWriter)
            {
                // メモ
                mWriter = aWriter;

                // レベルアップ
                mWriter.IndentInc();
            }

            //------------------------------------------------------------
            // IDisposableの実装。
            public void Dispose()
            {
                // レベルダウン
                mWriter.IndentDec();
            }

            //============================================================
            private readonly XDataWriter mWriter;
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public XDataWriter()
        {
            mTexts = new List<string>();
            mIndentLevel = 1; // デフォルト1。
        }

        //------------------------------------------------------------
        // インデントレベルアップ。
        public void IndentInc()
        {
            ++mIndentLevel;
        }

        //------------------------------------------------------------
        // インデントレベルダウン。
        public void IndentDec()
        {
            --mIndentLevel;
        }

        //------------------------------------------------------------
        // インデントを書き込む。
        public void WriteIndent()
        {
            write(indentText());
        }

        //------------------------------------------------------------
        // 改行を書き込む。
        public void WriteLine()
        {
            write(System.Environment.NewLine);
        }

        //------------------------------------------------------------
        // コメントを書き込む。
        public void WriteComment(string aComment)
        {
            write(createComment(aComment));
        }

        //------------------------------------------------------------
        // コメント行を書き込む。
        public void WriteCommentLine(string aComment)
        {
            writeLine(createComment(aComment));
        }

        //------------------------------------------------------------
        // リファレンス行を書き込む。
        public void WriteReferenceLine(string aLabel)
        {
            writeLine(createReference(aLabel));
        }
        
        //------------------------------------------------------------
        // リファレンス行をコメント付きで書き込む。
        public void WriteReferenceLine(string aComment,string aLabel)
        {
            writeLine(aComment, createReference(aLabel));
        }

        //------------------------------------------------------------
        // ラベル行を書き込む。
        public void WriteLabelLine(string aLabel)
        {
            writeLine(createLabel(aLabel));
        }

        //------------------------------------------------------------
        // ラベル行をコメント付きで書き込む。
        public void WriteLabelLine(string aComment, string aLabel)
        {
            writeLine(aComment, createLabel(aLabel));
        }

        //------------------------------------------------------------
        // アライメント行を書き込む。
        public void WriteAlignLine(uint aAlign)
        {
            writeLine(createAlign(aAlign));
        }

        //------------------------------------------------------------　
        // 文字列を書き込む。
        public void WriteStringLine(string aStr)
        {
            writeLine(createStringRef(aStr));
        }

        //------------------------------------------------------------　
        // 文字列をコメント付きで書き込む。
        public void WriteStringLine(string aComment, string aStr)
        {
            writeLine(aComment, createStringRef(aStr));
        }

        //------------------------------------------------------------　
        // SInt16を書き込む。
        public void WriteSInt16(short aVal)
        {
            write(createSInt16(aVal));
        }

        //------------------------------------------------------------　
        // SInt32行をコメント付きで書き込む。
        public void WriteSInt32Line(string aComment, int aVal)
        {
            writeLine(aComment, createSInt32(aVal));
        }

        //------------------------------------------------------------　
        // UInt8を書き込む。
        public void WriteUInt8(byte aVal)
        {
            write(createUInt8(aVal));
        }

        //------------------------------------------------------------　
        // UInt16を書き込む。
        public void WriteUInt16(ushort aVal)
        {
            write(createUInt16(aVal));
        }

        //------------------------------------------------------------　
        // UInt32を書き込む。
        public void WriteUInt32Line(uint aVal)
        {
            writeLine(createUInt32(aVal));
        }

        //------------------------------------------------------------　
        // UInt32をコメント付きで書き込む。
        public void WriteUInt32Line(string aComment, uint aVal)
        {
            writeLine(aComment, createUInt32(aVal));
        }

        //------------------------------------------------------------
        // 文字列に変換する。
        public string ToXMLText()
        {
            var writer = new StringWriter();
            {
                // ヘッダ
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<xdata_root ");
                writer.WriteLine("  xmlns=\"http://10106.net/xdata\"");
                writer.WriteLine("  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                writer.WriteLine("  xsi:schemaLocation=\"http://10106.net/xdata\"");
                writer.WriteLine("  major_version=\"1\"");
                writer.WriteLine("  minor_version=\"2\"");
                writer.WriteLine("  >");

                // 本体
                foreach (var entry in mTexts)
                {
                    writer.Write(entry);
                }

                // フッタ
                writer.WriteLine("</xdata_root>");
            }
            writer.Close();
            return writer.ToString();
        }

        //============================================================
        List<string> mTexts;
        uint mIndentLevel;
        
        //------------------------------------------------------------
        // 書き込み関数。
        void write(string aStr)
        {
            mTexts.Add(aStr);
        }

        //------------------------------------------------------------
        // 改行付き書き込み関数。
        void writeLine(string aStr)
        {
            write(indentText() + aStr + System.Environment.NewLine);
        }

        //------------------------------------------------------------
        // コメント付き書き込み関数。
        void writeLine(string aComment, string aStr)
        {
            writeLine(createComment(aComment) + aStr);
        }

        //------------------------------------------------------------
        // インデントテキストを取得する。
        string indentText()
        {
            string str = "";
            for (uint i = 0; i < mIndentLevel; ++i)
            {
                str += "    ";
            }
            return str;
        }

        //------------------------------------------------------------
        // commentを作成する。
        string createComment(string aStr)
        {
            return "<!--" + aStr + "-->";
        }

        //------------------------------------------------------------
        // referenceを作成する。
        string createReference(string aLabelName)
        {
            return "<reference label_name=\"" + aLabelName + "\"/>";
        }

        //------------------------------------------------------------
        // labelを作成する。
        string createLabel(string aLabelName)
        {
            return "<label name=\"" + aLabelName + "\"/>";
        }

        //------------------------------------------------------------
        // string_refを作成する。
        string createStringRef(string aStr)
        {
            return createValue("string_ref", aStr);
        }

        //------------------------------------------------------------
        // alignを作成する。
        string createAlign(uint aVal)
        {
            return createValue("align", aVal.ToString());
        }

        //------------------------------------------------------------
        // sint16を作成する。
        string createSInt16(short aVal)
        {
            return createValue("sint16", aVal.ToString());
        }

        //------------------------------------------------------------
        // sint32を作成する。
        string createSInt32(int aVal)
        {
            return createValue("sint32", aVal.ToString());
        }
        
        //------------------------------------------------------------
        // uint8を作成する。
        string createUInt8(uint aVal)
        {
            return createValue("uint8", aVal.ToString());
        }

        //------------------------------------------------------------
        // uint16を作成する。
        string createUInt16(ushort aVal)
        {
            return createValue("uint16", aVal.ToString());
        }

        //------------------------------------------------------------
        // uint32を作成する。
        string createUInt32(uint aVal)
        {
            return createValue("uint32", aVal.ToString());
        }

        //------------------------------------------------------------
        // valueノードを作成する。
        string createValue(string aName, string aValue)
        {
            return "<" + aName + " value=\"" + aValue + "\"/>";
        }
    }
}
