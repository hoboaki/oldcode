using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace DependAnalyzer
{
    // ���C���t�H�[��
    public partial class MainForm : Form
    {
        const int COL_IDX_INCLUDE = 0;
        const int COL_IDX_PERCENT = 1;
        const int COL_IDX_FILE = 2;
        const int COL_IDX_PATH = 3;

        private Analyzer analyzer = null;
        private int selectedColumn = COL_IDX_INCLUDE;
        private SortOrder sortOrder = SortOrder.Descending;

        public MainForm( Analyzer aAnalyzer )
        {
            analyzer = aAnalyzer;
            InitializeComponent();
            setupResultListView();   
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void setupResultListView()
        {
            // �w�b�_�̒ǉ�
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = "include";
                header.TextAlign = HorizontalAlignment.Right;
                header.Width = 50;
                resultListView.Columns.Add( header );
            }
            resultListView.Columns.Add("percent",60);
            resultListView.Columns.Add("file",200);
            resultListView.Columns.Add("path",400);

            // �R�[���o�b�N�̐ݒ�
            resultListView.ColumnClick += new ColumnClickEventHandler(onColumnClicked);

            // �ǉ�
            {
                uint totalCount = analyzer.GetSourceFilesCount();
                foreach (CodeFile sourceFile in analyzer.GetCodeFiles())
                {
                    if (sourceFile.IsIgnore())
                    {// �����t�@�C���̓X�L�b�v
                        continue;
                    }
                    resultListView.Items.Add(new MainFormListItem(sourceFile, totalCount));
                }
            }

            // �����\�[�^�[�̐ݒ�
            resultListView.ListViewItemSorter = new ListViewComparer(selectedColumn, sortOrder);
            resultListView.Sort();

            // �X�e�[�^�X�o�[
            toolStripStatusLabel.Text = "�w�b�_�t�@�C���F" + analyzer.GetHeaderFilesCount()
                + "�@�\�[�X�t�@�C���F"  + analyzer.GetSourceFilesCount().ToString()
                + "�@���v�F" + analyzer.GetCodeFiles().Count; ;

            // �^�C�g���ύX
            this.Text = analyzer.settingFile.FullName + " - " + "DependAnalyzer";
        }

        private void onColumnClicked(object sender, ColumnClickEventArgs e)
        {
            if ( e.Column != selectedColumn )
            {// �ʂ̗��I��
                selectedColumn = e.Column;
                if (selectedColumn == COL_IDX_INCLUDE
                    || selectedColumn == COL_IDX_PERCENT
                    )
                {
                    sortOrder = SortOrder.Descending;
                }
                else
                {
                    sortOrder = SortOrder.Ascending;
                }
            }
            else
            {// �������I��
                sortOrder = sortOrder == SortOrder.Descending
                    ? SortOrder.Ascending
                    : SortOrder.Descending;
            }
            resultListView.ListViewItemSorter = new ListViewComparer( e.Column , sortOrder );
        }

        private void showDependTreeButton_Click(object sender, EventArgs e)
        {
            if (resultListView.SelectedItems.Count != 1)
            {// �P���I�����Ă��Ȃ��悤�Ȃ̂ŉ������Ȃ�
                return;
            }
            
            // �E�C���h�E�̍쐬�ƕ\��
            MainFormListItem listItem = (MainFormListItem)resultListView.SelectedItems[0];
            CodeTreeForm dependTreeForm = new CodeTreeForm( analyzer , listItem.attachedSourceFile , CodeTreeForm.Mode.ReferencedTree );
            dependTreeForm.Show();
        }

        private void showIncludeTreeButton_Click(object sender, EventArgs e)
        {
            if (resultListView.SelectedItems.Count != 1)
            {// �P���I�����Ă��Ȃ��悤�Ȃ̂ŉ������Ȃ�
                return;
            }

            // �E�C���h�E�̍쐬�ƕ\��
            MainFormListItem listItem = (MainFormListItem)resultListView.SelectedItems[0];
            CodeTreeForm dependTreeForm = new CodeTreeForm(analyzer, listItem.attachedSourceFile, CodeTreeForm.Mode.IncludeTree );
            dependTreeForm.Show();
        }

        private void showExplorerButton_Click(object sender, EventArgs e)
        {
            if (resultListView.SelectedItems.Count != 1)
            {// �P���I�����Ă��Ȃ��悤�Ȃ̂ŉ������Ȃ�
                return;
            }

            // �G�N�X�v���[���ɕ\��
            MainFormListItem listItem = (MainFormListItem)resultListView.SelectedItems[0];
            Utility.showExplorer(listItem.attachedSourceFile);
        }
    }

    // MainFormListNode
    public class MainFormListItem : ListViewItem
    {
        public CodeFile attachedSourceFile;

        public MainFormListItem(CodeFile aSourceFile,uint aTotalCount)
            : base()
        {
            attachedSourceFile = aSourceFile;
            this.SubItems[0].Text = aSourceFile.includeCount.ToString();
            this.SubItems.Add(new ListViewSubItem(this, (100.0f * (float)aSourceFile.includeCount / aTotalCount).ToString()));
            this.SubItems.Add(new ListViewSubItem(this, aSourceFile.fileInfo.Name));
            this.SubItems.Add(new ListViewSubItem(this, aSourceFile.fileInfo.FullName));
        }
    };

    // string�p,comparer
    public class ListViewComparer : IComparer
    {
        private int col;
        private SortOrder sortOrder;

        public ListViewComparer(int aColumn, SortOrder aSortOrder)
        {
            col = aColumn;
            sortOrder = aSortOrder;
        }

        public int Compare( object aLHS , object aRHS )
        {
            MainFormListItem itemA = (MainFormListItem)aLHS;
            MainFormListItem itemB = (MainFormListItem)aRHS;
            string strA = itemA.SubItems[col].Text;
            string strB = itemB.SubItems[col].Text;
            int sign = sortOrder == SortOrder.Descending ? -1 : 1;

            int compareResult = 0;
            if (col == 0)
            {
                uint numA, numB;
                bool resultA = UInt32.TryParse(strA, out numA);
                bool resultB = UInt32.TryParse(strB, out numB);
                compareResult = numA.CompareTo(numB);
            }
            else if (col == 1)
            {
                float numA, numB;
                bool resultA = float.TryParse(strA, out numA);
                bool resultB = float.TryParse(strB, out numB);
                compareResult = numA.CompareTo(numB);
            }
            else
            {
                compareResult = strA.CompareTo( strB );
            }
            return sign * compareResult;
        }
    };

}