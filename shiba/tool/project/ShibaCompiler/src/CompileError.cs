using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// コンパイルエラーを示すクラス。
    /// </summary>
    class CompileError
    {
        //------------------------------------------------------------
        // 行区切りをプリントする。
        static public void PrintLineSeparator()
        {
            System.Console.Error.WriteLine(@"##################################################");
        }

        //------------------------------------------------------------
        // メッセージのみ。
        public CompileError(CompileErrorKind aErrorKind, string aMessage)
            : this( aErrorKind , aMessage , null , 0 , 0 )
        {
        }

        //------------------------------------------------------------
        // ソースコードつき。
        public CompileError(
            CompileErrorKind aErrorKind
            , string aMessage
            , string aSrcPath
            , ushort aSrcLine
            , ushort aSrcColumn
            )
        {
            mErrorKind = aErrorKind;
            mMessage = aMessage;
            mSrcPath = aSrcPath;
            mSrcLine = aSrcLine;
            mSrcColumn = aSrcColumn;
        }

        //------------------------------------------------------------
        // エラーIOに出力。
        public void Print()
        {
            // ソースパスがある場合
            if (mSrcPath != null)
            {
                System.Console.Error.WriteLine(@"# " + mSrcPath);
                System.Console.Error.WriteLine(@"# (" + mSrcLine + "," + mSrcColumn + ")");
                System.Console.Error.WriteLine(@"#");
            }
            System.Console.Error.WriteLine("@# " + mMessage);
        }

        //============================================================
        // private variable
        readonly CompileErrorKind mErrorKind;
        readonly string mMessage;
        readonly string mSrcPath;
        readonly ushort mSrcLine;
        readonly ushort mSrcColumn;
    }
}
