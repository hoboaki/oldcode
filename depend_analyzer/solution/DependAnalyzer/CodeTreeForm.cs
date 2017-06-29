using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DependAnalyzer
{
    public partial class CodeTreeForm : Form
    {
        public enum Mode
        {
            ReferencedTree
            , IncludeTree
        };

        private Analyzer analyzer;
        private CodeFile rootSourceFile;
        private Mode mode;

        public CodeTreeForm( Analyzer aAnalyzer 
            , CodeFile aRootFile 
            , Mode aMode
            )
        {
            analyzer = aAnalyzer;
            rootSourceFile = aRootFile;
            mode = aMode;
            InitializeComponent();

            // �c�[���`�b�v�\��
            treeView.ShowNodeToolTips = true;
        }

        private void DependTreeForm_Load(object sender, EventArgs e)
        {
            // ���O�̕ύX
            this.Text = mode == Mode.ReferencedTree
                ? "<�ˑ��c���[>"
                : "<�C���N���[�h�c���[>";
            this.Text += " " + rootSourceFile.fileInfo.FullName + " - DependAnalyzer";
            
            // �c���[�̐���
            CodeTreeNode node = new CodeTreeNode(rootSourceFile, false, mode);
            treeView.Nodes.Add(node);
            node.addChildsIfNeccesary();
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // ���m�[�h�̍쐬
            CodeTreeNode expandNode = (CodeTreeNode)e.Node;
            foreach(CodeTreeNode node in expandNode.Nodes)
            {
                node.addChildsIfNeccesary();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // ���g��\��
            CodeTreeNode node = (CodeTreeNode)e.Node;
            codeFileContentTextBox.Clear();
            codeFileContentTextBox.Text = System.IO.File.ReadAllText(node.attachedSourceFile.fileInfo.FullName, Encoding.GetEncoding("SJIS"));
        }

        private void showDependTreeButton_Click(object sender, EventArgs e)
        {
            createTreeFormIfPossible(CodeTreeForm.Mode.ReferencedTree);
        }

        private void showIncludeTreeButton_Click(object sender, EventArgs e)
        {
            createTreeFormIfPossible(CodeTreeForm.Mode.IncludeTree);
        }

        private void createTreeFormIfPossible(CodeTreeForm.Mode aMode)
        {
            // �I������Ă��邩�`�F�b�N
            if (treeView.SelectedNode == null)
            {
                return;
            }

            // �E�C���h�E���쐬
            CodeTreeNode node = (CodeTreeNode)treeView.SelectedNode;
            CodeTreeForm form = new CodeTreeForm(analyzer, node.attachedSourceFile, aMode);
            form.Show();
        }

        private void showExplorerButton_Click(object sender, EventArgs e)
        {
            // �I������Ă��邩�`�F�b�N
            if (treeView.SelectedNode == null)
            {
                return;
            }

            // �G�N�X�v���[���ɕ\��
            CodeTreeNode node = (CodeTreeNode)treeView.SelectedNode;
            Utility.showExplorer(node.attachedSourceFile);
        }
    }

    public class CodeTreeNode : TreeNode
    {
        public CodeFile attachedSourceFile;
        private bool cantAddChilds;
        private CodeTreeForm.Mode mode;

        public CodeTreeNode(CodeFile aSourceFile,bool aCantAddChilds,CodeTreeForm.Mode aMode)
            : base()
        {
            attachedSourceFile = aSourceFile;
            cantAddChilds = aCantAddChilds;
            mode = aMode;
            if ( mode == CodeTreeForm.Mode.ReferencedTree )
            {// �ˑ��c���[
                if (attachedSourceFile.IsHeader())
                {
                    this.Text = aSourceFile.fileInfo.Name + " (" + aSourceFile.includeCount.ToString() + ")";
                }
                else
                {
                    this.Text = aSourceFile.fileInfo.Name;
                }
            }
            else
            {// �C���N���[�h�c���[
                this.Text = aSourceFile.fileInfo.Name + " (" + aSourceFile.includeCodeFiles.GetCodeFiles().Count.ToString() + ")";
            }

            if (cantAddChilds)
            {
                this.Text += " <x>";
            }
            this.ToolTipText = aSourceFile.fileInfo.FullName;
        }

        public void addChildsIfNeccesary()
        {
            if (cantAddChilds)
            {// �q���͒ǉ��ł��Ȃ�
                return;
            }

            if (this.Nodes.Count != 0)
            {// �ǉ��ς�
                return;
            }

            // �m�[�h���쐬����
            ArrayList nodes = new ArrayList();
            if (mode == CodeTreeForm.Mode.ReferencedTree)
            {// �ˑ��c���[
                foreach (CodeFile sourceFile in attachedSourceFile.referencedCodeFiles.GetCodeFiles())
                {
                    if (sourceFile.IsIgnore())
                    {// �����t�@�C��
                        continue;
                    }
                    CodeTreeNode newNode = new CodeTreeNode(
                        sourceFile
                        , findParentNodeSameSourceFile(this, sourceFile)
                        , mode
                        );
                    nodes.Add(newNode);
                }

                // includeCount�Ń\�[�g����
                nodes.Sort(new DependTreeNodeComparer());
            }
            else
            {// �C���N���[�h�c���[
                foreach (CodeFile sourceFile in attachedSourceFile.includeCodeFiles.GetCodeFiles())
                {
                    CodeTreeNode newNode = new CodeTreeNode(
                        sourceFile
                        , findParentNodeSameSourceFile(this, sourceFile)
                        , mode
                        );
                    nodes.Add(newNode);
                }
            }

            // �m�[�h��ǉ�����
            foreach (CodeTreeNode node in nodes)
            {
                this.Nodes.Add(node);
            }
        }

        private bool findParentNodeSameSourceFile(TreeNode aNode, CodeFile aSourceFile)
        {
            if (aNode == null)
            {
                return false;
            }
            CodeTreeNode node = (CodeTreeNode)aNode;
            if (node.attachedSourceFile == aSourceFile)
            {
                return true;
            }
            return findParentNodeSameSourceFile(node.Parent, aSourceFile);
        }
    }

    public class DependTreeNodeComparer : IComparer
    {
        public int Compare(object aLHS, object aRHS)
        {
            CodeTreeNode lhsNode = (CodeTreeNode)aLHS;
            CodeTreeNode rhsNode = (CodeTreeNode)aRHS;
            return rhsNode.attachedSourceFile.includeCount.CompareTo(lhsNode.attachedSourceFile.includeCount);
        }
    };
}