using System;
using System.Collections.Generic;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// プログラム本体。
    /// </summary>
    class Program
    {
        //------------------------------------------------------------
        // エントリーポイント。
        static int Main(string[] aArgs)
        {
            // test
            if (false)
            {
                LexerTest.Execute();
                ParserTest.Execute();
                SymbolTreeTest.Execute();
            }

            // 引数解釈
            if (aArgs.Length != 2)
            {// 引数が違う。
                System.Console.Error.WriteLine("useage: ShibaCompiler.exe src_list_file_path output_dir_path");
                return -1;
            }
            string srcListFilePath = aArgs[0];
            string outputDirPath = aArgs[1];

            // コンパイル
            if (!Compiler.Execute(srcListFilePath, outputDirPath))
            {// 失敗。
                return -1;
            }

            // 成功
            return 0;
        }
    }
}
