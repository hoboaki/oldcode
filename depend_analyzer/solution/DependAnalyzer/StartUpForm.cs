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
        const string LABEL_START = "�J�n";
        const string LABEL_CANCEL = "���~";

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
            // �ݒ�t�@�C���I��
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                return;
            }
            settingFile = new FileInfo(openFileDialog.FileName);

            // ���O�̃��Z�b�g
            addSourceFileCount = 0;
            calcSourceFileCount = 0;
            logTextBox.Clear();

            // �A�i���C�Y
            System.Diagnostics.Debug.Assert(thread == null);
            thread = new Thread(new ThreadStart(analyze));
            thread.Start();

            // �L�����Z���{�^���ɕύX
            startButton.Text = LABEL_CANCEL;

            // �t�H�[�J�X�ݒ�
            logTextBox.Focus();
        }

        private void cancelAnalyze()
        {
            // �X���b�h���f
            System.Diagnostics.Debug.Assert(thread != null);
            thread.Abort();
            thread = null;

            // �J�n�{�^���ɕύX
            startButton.Text = LABEL_START;
        }

        private void analyze()
        {
            System.Diagnostics.Debug.Assert(settingFile != null);

            // �A�i���C�Y
            try
            {
                Analyzer analyzer = new Analyzer(
                    settingFile
                    , new Analyzer.CodeFileCallBack(onAnalyzerStartFunc)
                    , new Analyzer.CodeFileCallBack(onCalcIncludeCountStartFunc)
                    );

                // ���C���t�H�[���쐬
                this.Invoke(new AnalyzerFunc(onAnalyzeFinishedFunc), analyzer);
            }
            catch (Exception aExp)
            {
                this.Invoke(new InvokeAppendLogText(invokeAppendLogText), aExp.ToString() );
            }
            finally
            {
                // �X���b�h������
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
                "[�ǉ�:" 
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
                "[���:" 
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
            logTextBox.AppendText("��͂��I�����܂����B");

            MainForm mainForm = new MainForm(aAnalyzer);
            mainForm.Show();
        }

        private void onAnalyzeEndFunc()
        {
            startButton.Text = LABEL_START;
        }
    }
}