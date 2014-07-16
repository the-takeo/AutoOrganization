namespace AutoOrganization
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDoSelectedAction = new System.Windows.Forms.Button();
            this.lbAction = new System.Windows.Forms.ListBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logInIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abountApplicationAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.tbAddTags = new System.Windows.Forms.TextBox();
            this.chbAddTags = new System.Windows.Forms.CheckBox();
            this.cbMoteToNotebook = new System.Windows.Forms.ComboBox();
            this.chbMoveToNotebook = new System.Windows.Forms.CheckBox();
            this.gbTarget = new System.Windows.Forms.GroupBox();
            this.lblTargetURL = new System.Windows.Forms.Label();
            this.tbTargetUrl = new System.Windows.Forms.TextBox();
            this.tbTargetTags = new System.Windows.Forms.TextBox();
            this.lblTargetNotebook = new System.Windows.Forms.Label();
            this.lblTargetTag = new System.Windows.Forms.Label();
            this.cbTargetNotebook = new System.Windows.Forms.ComboBox();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.btnDeleteAction = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbAction.SuspendLayout();
            this.gbTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDoSelectedAction
            // 
            this.btnDoSelectedAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDoSelectedAction.Location = new System.Drawing.Point(231, 426);
            this.btnDoSelectedAction.Name = "btnDoSelectedAction";
            this.btnDoSelectedAction.Size = new System.Drawing.Size(141, 23);
            this.btnDoSelectedAction.TabIndex = 9;
            this.btnDoSelectedAction.Text = "選択されたアクションを実行";
            this.btnDoSelectedAction.UseVisualStyleBackColor = true;
            this.btnDoSelectedAction.Click += new System.EventHandler(this.btnDoSelectedAction_Click);
            // 
            // lbAction
            // 
            this.lbAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAction.FormattingEnabled = true;
            this.lbAction.ItemHeight = 12;
            this.lbAction.Location = new System.Drawing.Point(12, 27);
            this.lbAction.Name = "lbAction";
            this.lbAction.Size = new System.Drawing.Size(177, 388);
            this.lbAction.TabIndex = 0;
            this.lbAction.SelectedIndexChanged += new System.EventHandler(this.lbAction_SelectedIndexChanged);
            this.lbAction.DoubleClick += new System.EventHandler(this.lbAction_DoubleClick);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.abountApplicationAToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(384, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logInIToolStripMenuItem,
            this.closeCToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // logInIToolStripMenuItem
            // 
            this.logInIToolStripMenuItem.Name = "logInIToolStripMenuItem";
            this.logInIToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.logInIToolStripMenuItem.Text = "LogIn(&I)";
            this.logInIToolStripMenuItem.Click += new System.EventHandler(this.logInIToolStripMenuItem_Click);
            // 
            // closeCToolStripMenuItem
            // 
            this.closeCToolStripMenuItem.Name = "closeCToolStripMenuItem";
            this.closeCToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.closeCToolStripMenuItem.Text = "Close(&C)";
            this.closeCToolStripMenuItem.Click += new System.EventHandler(this.closeCToolStripMenuItem_Click);
            // 
            // abountApplicationAToolStripMenuItem
            // 
            this.abountApplicationAToolStripMenuItem.Name = "abountApplicationAToolStripMenuItem";
            this.abountApplicationAToolStripMenuItem.Size = new System.Drawing.Size(145, 20);
            this.abountApplicationAToolStripMenuItem.Text = "Abount Application(&A)";
            this.abountApplicationAToolStripMenuItem.Click += new System.EventHandler(this.abountApplicationAToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gbAction);
            this.groupBox1.Controls.Add(this.gbTarget);
            this.groupBox1.Location = new System.Drawing.Point(195, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 388);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "設定内容";
            // 
            // gbAction
            // 
            this.gbAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAction.Controls.Add(this.tbAddTags);
            this.gbAction.Controls.Add(this.chbAddTags);
            this.gbAction.Controls.Add(this.cbMoteToNotebook);
            this.gbAction.Controls.Add(this.chbMoveToNotebook);
            this.gbAction.Location = new System.Drawing.Point(6, 191);
            this.gbAction.Name = "gbAction";
            this.gbAction.Size = new System.Drawing.Size(165, 191);
            this.gbAction.TabIndex = 9;
            this.gbAction.TabStop = false;
            this.gbAction.Text = "実行内容";
            // 
            // tbAddTags
            // 
            this.tbAddTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddTags.Location = new System.Drawing.Point(6, 88);
            this.tbAddTags.Name = "tbAddTags";
            this.tbAddTags.Size = new System.Drawing.Size(153, 19);
            this.tbAddTags.TabIndex = 7;
            this.tbAddTags.Leave += new System.EventHandler(this.tbAddTags_Leave);
            // 
            // chbAddTags
            // 
            this.chbAddTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbAddTags.AutoSize = true;
            this.chbAddTags.Location = new System.Drawing.Point(6, 66);
            this.chbAddTags.Name = "chbAddTags";
            this.chbAddTags.Size = new System.Drawing.Size(76, 16);
            this.chbAddTags.TabIndex = 6;
            this.chbAddTags.Text = "Tagを付加";
            this.chbAddTags.UseVisualStyleBackColor = true;
            this.chbAddTags.CheckedChanged += new System.EventHandler(this.chbAddTags_CheckedChanged);
            // 
            // cbMoteToNotebook
            // 
            this.cbMoteToNotebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMoteToNotebook.FormattingEnabled = true;
            this.cbMoteToNotebook.Location = new System.Drawing.Point(6, 40);
            this.cbMoteToNotebook.Name = "cbMoteToNotebook";
            this.cbMoteToNotebook.Size = new System.Drawing.Size(153, 20);
            this.cbMoteToNotebook.TabIndex = 5;
            this.cbMoteToNotebook.Tag = "";
            this.cbMoteToNotebook.SelectedIndexChanged += new System.EventHandler(this.cbMoteToNotebook_SelectedIndexChanged);
            // 
            // chbMoveToNotebook
            // 
            this.chbMoveToNotebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbMoveToNotebook.AutoSize = true;
            this.chbMoveToNotebook.Location = new System.Drawing.Point(8, 18);
            this.chbMoveToNotebook.Name = "chbMoveToNotebook";
            this.chbMoveToNotebook.Size = new System.Drawing.Size(105, 16);
            this.chbMoveToNotebook.TabIndex = 4;
            this.chbMoveToNotebook.Text = "Notebookを移動";
            this.chbMoveToNotebook.UseVisualStyleBackColor = true;
            this.chbMoveToNotebook.CheckedChanged += new System.EventHandler(this.chbMoveToNotebook_CheckedChanged);
            // 
            // gbTarget
            // 
            this.gbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTarget.Controls.Add(this.lblTargetURL);
            this.gbTarget.Controls.Add(this.tbTargetUrl);
            this.gbTarget.Controls.Add(this.tbTargetTags);
            this.gbTarget.Controls.Add(this.lblTargetNotebook);
            this.gbTarget.Controls.Add(this.lblTargetTag);
            this.gbTarget.Controls.Add(this.cbTargetNotebook);
            this.gbTarget.Location = new System.Drawing.Point(6, 18);
            this.gbTarget.Name = "gbTarget";
            this.gbTarget.Size = new System.Drawing.Size(165, 167);
            this.gbTarget.TabIndex = 8;
            this.gbTarget.TabStop = false;
            this.gbTarget.Text = "対象";
            // 
            // lblTargetURL
            // 
            this.lblTargetURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTargetURL.AutoSize = true;
            this.lblTargetURL.Location = new System.Drawing.Point(6, 90);
            this.lblTargetURL.Name = "lblTargetURL";
            this.lblTargetURL.Size = new System.Drawing.Size(27, 12);
            this.lblTargetURL.TabIndex = 5;
            this.lblTargetURL.Text = "URL";
            // 
            // tbTargetUrl
            // 
            this.tbTargetUrl.Location = new System.Drawing.Point(8, 105);
            this.tbTargetUrl.Name = "tbTargetUrl";
            this.tbTargetUrl.Size = new System.Drawing.Size(151, 19);
            this.tbTargetUrl.TabIndex = 3;
            this.tbTargetUrl.Leave += new System.EventHandler(this.tbTargetUrl_Leave);
            // 
            // tbTargetTags
            // 
            this.tbTargetTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTargetTags.Location = new System.Drawing.Point(8, 68);
            this.tbTargetTags.Name = "tbTargetTags";
            this.tbTargetTags.Size = new System.Drawing.Size(151, 19);
            this.tbTargetTags.TabIndex = 2;
            this.tbTargetTags.Leave += new System.EventHandler(this.tbTargetTags_Leave);
            // 
            // lblTargetNotebook
            // 
            this.lblTargetNotebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTargetNotebook.AutoSize = true;
            this.lblTargetNotebook.Location = new System.Drawing.Point(6, 15);
            this.lblTargetNotebook.Name = "lblTargetNotebook";
            this.lblTargetNotebook.Size = new System.Drawing.Size(55, 12);
            this.lblTargetNotebook.TabIndex = 5;
            this.lblTargetNotebook.Text = "NoteBook";
            // 
            // lblTargetTag
            // 
            this.lblTargetTag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTargetTag.AutoSize = true;
            this.lblTargetTag.Location = new System.Drawing.Point(6, 53);
            this.lblTargetTag.Name = "lblTargetTag";
            this.lblTargetTag.Size = new System.Drawing.Size(24, 12);
            this.lblTargetTag.TabIndex = 7;
            this.lblTargetTag.Text = "Tag";
            // 
            // cbTargetNotebook
            // 
            this.cbTargetNotebook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTargetNotebook.FormattingEnabled = true;
            this.cbTargetNotebook.Location = new System.Drawing.Point(8, 30);
            this.cbTargetNotebook.Name = "cbTargetNotebook";
            this.cbTargetNotebook.Size = new System.Drawing.Size(151, 20);
            this.cbTargetNotebook.TabIndex = 1;
            this.cbTargetNotebook.SelectedIndexChanged += new System.EventHandler(this.cbTargetNotebook_SelectedIndexChanged);
            // 
            // btnAddAction
            // 
            this.btnAddAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAction.Location = new System.Drawing.Point(12, 426);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(75, 23);
            this.btnAddAction.TabIndex = 8;
            this.btnAddAction.Text = "追加";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // btnDeleteAction
            // 
            this.btnDeleteAction.Location = new System.Drawing.Point(114, 426);
            this.btnDeleteAction.Name = "btnDeleteAction";
            this.btnDeleteAction.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAction.TabIndex = 10;
            this.btnDeleteAction.Text = "削除";
            this.btnDeleteAction.UseVisualStyleBackColor = true;
            this.btnDeleteAction.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.btnDeleteAction);
            this.Controls.Add(this.btnAddAction);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbAction);
            this.Controls.Add(this.btnDoSelectedAction);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.Text = "AutoOrganization";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gbAction.ResumeLayout(false);
            this.gbAction.PerformLayout();
            this.gbTarget.ResumeLayout(false);
            this.gbTarget.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDoSelectedAction;
        private System.Windows.Forms.ListBox lbAction;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeCToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.TextBox tbAddTags;
        private System.Windows.Forms.CheckBox chbAddTags;
        private System.Windows.Forms.ComboBox cbMoteToNotebook;
        private System.Windows.Forms.CheckBox chbMoveToNotebook;
        private System.Windows.Forms.GroupBox gbTarget;
        private System.Windows.Forms.TextBox tbTargetTags;
        private System.Windows.Forms.Label lblTargetNotebook;
        private System.Windows.Forms.Label lblTargetTag;
        private System.Windows.Forms.ComboBox cbTargetNotebook;
        private System.Windows.Forms.Button btnAddAction;
        private System.Windows.Forms.ToolStripMenuItem logInIToolStripMenuItem;
        private System.Windows.Forms.Label lblTargetURL;
        private System.Windows.Forms.TextBox tbTargetUrl;
        private System.Windows.Forms.Button btnDeleteAction;
        private System.Windows.Forms.ToolStripMenuItem abountApplicationAToolStripMenuItem;
    }
}

