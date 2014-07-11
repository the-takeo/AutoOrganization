namespace AutoOrganization
{
    partial class RenameDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblOldName = new System.Windows.Forms.Label();
            this.lblNewName = new System.Windows.Forms.Label();
            this.tbNewname = new System.Windows.Forms.TextBox();
            this.tbOldName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblOldName
            // 
            this.lblOldName.AutoSize = true;
            this.lblOldName.Location = new System.Drawing.Point(12, 23);
            this.lblOldName.Name = "lblOldName";
            this.lblOldName.Size = new System.Drawing.Size(93, 12);
            this.lblOldName.TabIndex = 0;
            this.lblOldName.Text = "現在のアクション名";
            // 
            // lblNewName
            // 
            this.lblNewName.AutoSize = true;
            this.lblNewName.Location = new System.Drawing.Point(12, 53);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(90, 12);
            this.lblNewName.TabIndex = 1;
            this.lblNewName.Text = "新しいアクション名";
            // 
            // tbNewname
            // 
            this.tbNewname.Location = new System.Drawing.Point(108, 50);
            this.tbNewname.Name = "tbNewname";
            this.tbNewname.Size = new System.Drawing.Size(164, 19);
            this.tbNewname.TabIndex = 2;
            // 
            // tbOldName
            // 
            this.tbOldName.Enabled = false;
            this.tbOldName.Location = new System.Drawing.Point(108, 20);
            this.tbOldName.Name = "tbOldName";
            this.tbOldName.Size = new System.Drawing.Size(164, 19);
            this.tbOldName.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(116, 76);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 76);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // RenameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 111);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbOldName);
            this.Controls.Add(this.tbNewname);
            this.Controls.Add(this.lblNewName);
            this.Controls.Add(this.lblOldName);
            this.Name = "RenameDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RenameDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOldName;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.TextBox tbNewname;
        private System.Windows.Forms.TextBox tbOldName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}