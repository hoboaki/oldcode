using System;
using System.Collections.Generic;
using System.Text;

namespace ACScript
{
    /// <summary>
    /// プログラム本体。
    /// </summary>
    class Program
    {
        //------------------------------------------------------------
        // エントリーポイント。
        static void Main(string[] aArgs)
        {
            // test
            LexerTest.Execute();
            ParserTest.Execute();
        }
    }
}
