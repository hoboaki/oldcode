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

            // ツールチップ表示
            treeView.ShowNodeToolTips = true;
        }

        private void DependTreeForm_Load(object sender, EventArgs e)
        {
            // 名前の変更
            this.Text = mode == Mode.ReferencedTree
                ? "<依存ツリー>"
                : "<インクルードツリー>";
            this.Text += " " + rootSourceFile.fileInfo.FullName + " - DependAnalyzer";
            
            // ツリーの生成
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
            // 孫ノードの作成
            CodeTreeNode expandNode = (CodeTreeNode)e.Node;
            foreach(CodeTreeNode node in expandNode.Nodes)
            {
                node.addChildsIfNeccesary();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 中身を表示
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
            // 選択されているかチェック
            if (treeView.SelectedNode == null)
            {
                return;
            }

            // ウインドウを作成
            CodeTreeNode node = (CodeTreeNode)treeView.SelectedNode;
            CodeTreeForm form = new CodeTreeForm(analyzer, node.attachedSourceFile, aMode);
            form.Show();
        }

        private void showExplorerButton_Click(object sender, EventArgs e)
        {
            // 選択されているかチェック
            if (treeView.SelectedNode == null)
            {
                return;
            }

            // エクスプローラに表示
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
            {// 依存ツリー
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
            {// インクルードツリー
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
            {// 子供は追加できない
                return;
            }

            if (this.Nodes.Count != 0)
            {// 追加済み
                return;
            }

            // ノードを作成する
            ArrayList nodes = new ArrayList();
            if (mode == CodeTreeForm.Mode.ReferencedTree)
            {// 依存ツリー
                foreach (CodeFile sourceFile in attachedSourceFile.referencedCodeFiles.GetCodeFiles())
                {
                    if (sourceFile.IsIgnore())
                    {// 無視ファイル
                        continue;
                    }
                    CodeTreeNode newNode = new CodeTreeNode(
                        sourceFile
                        , findParentNodeSameSourceFile(this, sourceFile)
                        , mode
                        );
                    nodes.Add(newNode);
                }

                // includeCountでソートする
                nodes.Sort(new DependTreeNodeComparer());
            }
            else
            {// インクルードツリー
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

            // ノードを追加する
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