using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// トレースをサポートするクラス。
    /// </summary>
    class Tracer
    {
        //------------------------------------------------------------
        // インデントレベルをコントロールするサポートクラス。
        public class IndentScope : IDisposable
        {
            //------------------------------------------------------------
            // コンストラクタ。
            public IndentScope(Tracer aTracer)
            {
                // メモ
                tracer = aTracer;

                // レベルアップ
                tracer.indentInc();
            }

            //------------------------------------------------------------
            // IDisposableの実装。
            public void Dispose()
            {
                // レベルダウン
                tracer.indentDec();
            }

            //============================================================
            private readonly Tracer tracer;
        };

        //------------------------------------------------------------
        // コンストラクタ。
        public Tracer()
        {
        }

        //------------------------------------------------------------
        // インデントレベルを上げる。
        public void indentInc()
        {
            ++indentLevel;
        }

        //------------------------------------------------------------
        // インデントレベルを下げる。
        public void indentDec()
        {
            --indentLevel;
        }

        //------------------------------------------------------------
        // 書き込む。
        public void Write(string aMessage)
        {
            // インデント
            for (uint i = 0; i < indentLevel; ++i)
            {
                System.Console.Write("    ");
            }

            // メッセージ
            System.Console.Write(aMessage);

            // 改行
            System.Console.Write(System.Environment.NewLine);
        }

        //------------------------------------------------------------
        // 名前を書き込む。
        public void WriteName(string aName)
        {
            Write(aName + ":");
        }

        //------------------------------------------------------------
        // 名前+配列数を書き込む。
        public void WriteNameWithCount(string aName, int aCount)
        {
            WriteValue(aName, "{" + aCount + "}");
        }

        // 値を書き込む。
        public void WriteValue(string aName, string aValue)
        {
            Write(aName + " => " + aValue);
        }

        //============================================================
        private uint indentLevel = 0;
    }
}
