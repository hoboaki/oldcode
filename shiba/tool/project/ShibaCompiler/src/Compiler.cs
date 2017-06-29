using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShibaCompiler
{
    /// <summary>
    /// コンパイラ。
    /// </summary>
    class Compiler
    {
        //------------------------------------------------------------
        // 実行関数。問題なく処理が終わったらtrueを返す。        
        static public bool Execute(string aSrcListFilePath, string aOutputDirPath)
        {
            var compiler = new Compiler(aSrcListFilePath, aOutputDirPath);
            return compiler.IsSuccess();
        }

        //============================================================

        //------------------------------------------------------------
        // コンパイルに成功したか。
        public bool IsSuccess()
        {
            return mIsSuccess;
        }

        //============================================================

        /// <summary>
        /// ソースファイル。
        /// </summary>
        class SrcFile
        {
            public readonly string Path; // ソースファイルのパス。
            public readonly string Text; // ソースファイルの中身。

            //------------------------------------------------------------
            // コンストラクタ。
            public SrcFile(string aPath, string aText)
            {
                Path = aPath;
                Text = aText;
            }
        };

        //------------------------------------------------------------
        // private variable
        List<CompileError> mErrorList;
        bool mIsSuccess;

        //------------------------------------------------------------
        // コンストラクタ。
        Compiler(string aSrcListFilePath, string aOutputDirPath)
        {          
            // 初期化
            mErrorList = new List<CompileError>();
            mIsSuccess = false;

            // ソースファイルリストをオープン
            string[] srcFilePathList = null;
            try
            {
                srcFilePathList = System.IO.File.ReadAllLines(aSrcListFilePath);
            }
            catch (Exception)
            {
                System.Console.Error.WriteLine("'" + aSrcListFilePath + "'を読み込めませんでした。");
                return;
            }

            // シンボルツリーを作成
            SymbolTree symbolTree = new SymbolTree();

            // 各ソースファイルのRead,Lexer,Parserを実行
            // todo: マルチスレッド対応。
            List<SrcFile> srcFiles = new List<SrcFile>();
            foreach (var srcFilePath in srcFilePathList)
            {
                // SrcFile作成
                SrcFile srcFile = null;
                {
                    string srcFileText = null;
                    try
                    {
                        srcFileText = System.IO.File.ReadAllText(srcFilePath);
                    }
                    catch (Exception)
                    {           
                        // コンパイルエラー情報を作成
                        mErrorList.Add(new CompileError(
                            CompileErrorKind.SYSTEM_CANT_OPEN_SRC_FILE
                            , "'" + srcFilePath + "'を読み込めませんでした。"
                            ));

                        // 次のソースへ
                        continue;
                    }
                    srcFile = new SrcFile(srcFilePath, srcFileText);
                }

                // Lexer
                var lexer = new Lexer(srcFile.Text);
                if (lexer.IsError())
                {
                    // todo:
                    // コンパイルエラー情報を作成

                    // 次のソースへ。
                    continue;
                }

                // Parser
                var parser = new Parser(lexer);
                if (parser.GetErrorKind() != Parser.ErrorKind.NONE)
                {
                    // todo:
                    // コンパイルエラー情報を作成

                    // 次のソースへ。
                    continue;
                }

                // Add to SymbolTree
                if (!symbolTree.Add(parser.ModuleContext))
                {
                    // todo:
                    // コンパイルエラー情報を作成

                    // 次のソースへ
                    continue;
                }
            }

            // エラーが発生していたら出力して中断
            if ( mErrorList.Count != 0 )
            {
                dumpError();
                return;
            }

            // 展開
            // todo: マルチスレッド対応
            if (!symbolTree.Expand())
            {
                // todo:
                // コンパイルエラー情報を作成

                // 終了
                return;
            }

            // ファイル出力
            symbolTree.WriteToXML(aOutputDirPath);

            // 問題なく終了
            mIsSuccess = true;
        }

        //------------------------------------------------------------
        // エラーをダンプする。
        void dumpError()
        {
            CompileError.PrintLineSeparator();
            foreach (var error in mErrorList)
            {
                error.Print();
                CompileError.PrintLineSeparator();
            }
        }
    }
}
