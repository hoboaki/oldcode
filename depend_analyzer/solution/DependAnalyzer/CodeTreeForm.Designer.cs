namespace DependAnalyzer
{
    partial class CodeTreeForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView = new System.Windows.Forms.TreeView();
            this.showDependTreeButton = new System.Windows.Forms.Button();
            this.showIncludeTreeButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.codeFileContentTextBox = new System.Windows.Forms.RichTextBox();
            this.showExplorerButton = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(321, 613);
            this.treeView.TabIndex = 0;
            this.treeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            // 
            // showDependTreeButton
            // 
            this.showDependTreeButton.Location = new System.Drawing.Point(147, 4);
            this.showDependTreeButton.Name = "showDependTreeButton";
            this.showDependTreeButton.Size = new System.Drawing.Size(139, 23);
            this.showDependTreeButton.TabIndex = 1;
            this.showDependTreeButton.Text = "依存ツリーを表示";
            this.showDependTreeButton.UseVisualStyleBackColor = true;
            this.showDependTreeButton.Click += new System.EventHandler(this.showDependTreeButton_Click);
            // 
            // showIncludeTreeButton
            // 
            this.showIncludeTreeButton.Location = new System.Drawing.Point(292, 4);
            this.showIncludeTreeButton.Name = "showIncludeTreeButton";
            this.showIncludeTreeButton.Size = new System.Drawing.Size(131, 23);
            this.showIncludeTreeButton.TabIndex = 2;
            this.showIncludeTreeButton.Text = "インクルードツリーを表示";
            this.showIncludeTreeButton.UseVisualStyleBackColor = true;
            this.showIncludeTreeButton.Click += new System.EventHandler(this.showIncludeTreeButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.codeFileContentTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(864, 613);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 3;
            // 
            // codeFileContentTextBox
            // 
            this.codeFileContentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.codeFileContentTextBox.Location = new System.Drawing.Point(1, 0);
            this.codeFileContentTextBox.Name = "codeFileContentTextBox";
            this.codeFileContentTextBox.ReadOnly = true;
            this.codeFileContentTextBox.Size = new System.Drawing.Size(534, 613);
            this.codeFileContentTextBox.TabIndex = 0;
            this.codeFileContentTextBox.Text = "選択したファイルの内容を表示します...";
            this.codeFileContentTextBox.WordWrap = false;
            // 
            // showExplorerButton
            // 
            this.showExplorerButton.Location = new System.Drawing.Point(2, 4);
            this.showExplorerButton.Name = "showExplorerButton";
            this.showExplorerButton.Size = new System.Drawing.Size(139, 23);
            this.showExplorerButton.TabIndex = 4;
            this.showExplorerButton.Text = "エクスプローラに表示";
            this.showExplorerButton.UseVisualStyleBackColor = true;
            this.showExplorerButton.Click += new System.EventHandler(this.showExplorerButton_Click);
            // 
            // CodeTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 647);
            this.Controls.Add(this.showExplorerButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.showIncludeTreeButton);
            this.Controls.Add(this.showDependTreeButton);
            this.Name = "CodeTreeForm";
            this.Text = "DependTreeForm";
            this.Load += new System.EventHandler(this.DependTreeForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button showDependTreeButton;
        private System.Windows.Forms.Button showIncludeTreeButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox codeFileContentTextBox;
        private System.Windows.Forms.Button showExplorerButton;
    }
}