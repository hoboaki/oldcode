using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace DependAnalyzer
{
    public partial class StartUpForm : Form
    {
        const string LABEL_START = "開始";
        const string LABEL_CANCEL = "中止";

        FileInfo settingFile = null;
        Thread thread = null;
        uint addSourceFileCount = 0;
        uint calcSourceFileCount = 0;

        public StartUpForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (thread != null)
            {
                cancelAnalyze();
            }
            else
            {
                startAnalyze();
            }
        }

        private void startAnalyze()
        {
            // 設定ファイル選択
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            settingFile = new FileInfo(openFileDialog.FileName);

            // ログのリセット
            addSourceFileCount = 0;
            calcSourceFileCount = 0;
            logTextBox.Clear();

            // アナライズ
            System.Diagnostics.Debug.Assert(thread == null);
            thread = new Thread(new ThreadStart(analyze));
            thread.Start();

            // キャンセルボタンに変更
            startButton.Text = LABEL_CANCEL;

            // フォーカス設定
            logTextBox.Focus();
        }

        private void cancelAnalyze()
        {
            // スレッド中断
            System.Diagnostics.Debug.Assert(thread != null);
            thread.Abort();
            thread = null;

            // 開始ボタンに変更
            startButton.Text = LABEL_START;
        }

        private void analyze()
        {
            System.Diagnostics.Debug.Assert(settingFile != null);

            // アナライズ
            try
            {
                Analyzer analyzer = new Analyzer(
                    settingFile
                    , new Analyzer.CodeFileCallBack(onAnalyzerStartFunc)
                    , new Analyzer.CodeFileCallBack(onCalcIncludeCountStartFunc)
                    );

                // メインフォーム作成
                this.Invoke(new AnalyzerFunc(onAnalyzeFinishedFunc), analyzer);
            }
            catch (Exception aExp)
            {
                this.Invoke(new InvokeAppendLogText(invokeAppendLogText), aExp.ToString() );
            }
            finally
            {
                // スレッドを消す
                this.Invoke(new VoidFunc(onAnalyzeEndFunc));
                thread = null;
            }
        }

        private delegate void VoidFunc();
        private delegate void AnalyzerFunc(Analyzer analyzer);
        private delegate void InvokeAppendLogText( string text );
        private void invokeAppendLogText(string text)
        {
            logTextBox.AppendText(text);
        }

        private void onAnalyzerStartFunc(FileInfo aFileInfo)
        {
            ++addSourceFileCount;
            this.Invoke( new InvokeAppendLogText(invokeAppendLogText), 
                "[追加:" 
                + addSourceFileCount.ToString() 
                + "] " 
                + aFileInfo.FullName
                + "\n"
                );
        }

        private void onCalcIncludeCountStartFunc(FileInfo aFileInfo)
        {
            ++calcSourceFileCount;
            this.Invoke(new InvokeAppendLogText(invokeAppendLogText), 
                "[解析:" 
                + calcSourceFileCount.ToString()
                + "/"
                + addSourceFileCount.ToString() 
                + "] " 
                + aFileInfo.FullName
                + "\n"
                );
        }

        private void onAnalyzeFinishedFunc(Analyzer aAnalyzer)
        {
            logTextBox.AppendText("解析が終了しました。");

            MainForm mainForm = new MainForm(aAnalyzer);
            mainForm.Show();
        }

        private void onAnalyzeEndFunc()
        {
            startButton.Text = LABEL_START;
        }
    }
}