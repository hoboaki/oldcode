namespace DependAnalyzer
{
    partial class MainForm
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
            this.resultListView = new System.Windows.Forms.ListView();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.showDependTreeButton = new System.Windows.Forms.Button();
            this.showIncludeTreeButton = new System.Windows.Forms.Button();
            this.showExplorerButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // resultListView
            // 
            this.resultListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultListView.FullRowSelect = true;
            this.resultListView.GridLines = true;
            this.resultListView.Location = new System.Drawing.Point(0, 31);
            this.resultListView.Name = "resultListView";
            this.resultListView.Size = new System.Drawing.Size(591, 488);
            this.resultListView.TabIndex = 0;
            this.resultListView.UseCompatibleStateImageBehavior = false;
            this.resultListView.View = System.Windows.Forms.View.Details;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 517);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(591, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // showDependTreeButton
            // 
            this.showDependTreeButton.Location = new System.Drawing.Point(139, 3);
            this.showDependTreeButton.Name = "showDependTreeButton";
            this.showDependTreeButton.Size = new System.Drawing.Size(131, 23);
            this.showDependTreeButton.TabIndex = 3;
            this.showDependTreeButton.Text = "依存ツリーを表示";
            this.showDependTreeButton.UseVisualStyleBackColor = true;
            this.showDependTreeButton.Click += new System.EventHandler(this.showDependTreeButton_Click);
            // 
            // showIncludeTreeButton
            // 
            this.showIncludeTreeButton.Location = new System.Drawing.Point(276, 3);
            this.showIncludeTreeButton.Name = "showIncludeTreeButton";
            this.showIncludeTreeButton.Size = new System.Drawing.Size(131, 23);
            this.showIncludeTreeButton.TabIndex = 4;
            this.showIncludeTreeButton.Text = "インクルードツリーを表示";
            this.showIncludeTreeButton.UseVisualStyleBackColor = true;
            this.showIncludeTreeButton.Click += new System.EventHandler(this.showIncludeTreeButton_Click);
            // 
            // showExplorerButton
            // 
            this.showExplorerButton.Location = new System.Drawing.Point(2, 3);
            this.showExplorerButton.Name = "showExplorerButton";
            this.showExplorerButton.Size = new System.Drawing.Size(131, 23);
            this.showExplorerButton.TabIndex = 5;
            this.showExplorerButton.Text = "エクスプローラに表示";
            this.showExplorerButton.UseVisualStyleBackColor = true;
            this.showExplorerButton.Click += new System.EventHandler(this.showExplorerButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 539);
            this.Controls.Add(this.showExplorerButton);
            this.Controls.Add(this.showIncludeTreeButton);
            this.Controls.Add(this.showDependTreeButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.resultListView);
            this.Name = "MainForm";
            this.Text = "DependAnalyzer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView resultListView;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button showDependTreeButton;
        private System.Windows.Forms.Button showIncludeTreeButton;
        private System.Windows.Forms.Button showExplorerButton;
    }
}

