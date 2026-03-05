namespace PacketDesignTool.SubWinformConfs
{
    partial class DeleteTagForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TagList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Delete = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.ConnectedNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ConnectedDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ConnectedPathTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(13, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(970, 472);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Cancel);
            this.tabPage1.Controls.Add(this.Delete);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.TagList);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(962, 437);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "삭제";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TagList
            // 
            this.TagList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TagList.FormattingEnabled = true;
            this.TagList.ItemHeight = 21;
            this.TagList.Location = new System.Drawing.Point(19, 26);
            this.TagList.Name = "TagList";
            this.TagList.Size = new System.Drawing.Size(496, 382);
            this.TagList.TabIndex = 0;
            this.TagList.SelectedIndexChanged += new System.EventHandler(this.TagList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ConnectedPathTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ConnectedDescriptionTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ConnectedNameTextBox);
            this.groupBox1.Location = new System.Drawing.Point(532, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 329);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "결속 정보";
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(532, 361);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(195, 47);
            this.Delete.TabIndex = 2;
            this.Delete.Text = "삭제";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(742, 361);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(195, 47);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "취소";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // ConnectedNameTextBox
            // 
            this.ConnectedNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectedNameTextBox.Location = new System.Drawing.Point(115, 40);
            this.ConnectedNameTextBox.Name = "ConnectedNameTextBox";
            this.ConnectedNameTextBox.ReadOnly = true;
            this.ConnectedNameTextBox.Size = new System.Drawing.Size(268, 32);
            this.ConnectedNameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "결속 이름";
            // 
            // ConnectedDescriptionTextBox
            // 
            this.ConnectedDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectedDescriptionTextBox.Location = new System.Drawing.Point(115, 88);
            this.ConnectedDescriptionTextBox.Multiline = true;
            this.ConnectedDescriptionTextBox.Name = "ConnectedDescriptionTextBox";
            this.ConnectedDescriptionTextBox.ReadOnly = true;
            this.ConnectedDescriptionTextBox.Size = new System.Drawing.Size(268, 116);
            this.ConnectedDescriptionTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "설명";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "결속 경로";
            // 
            // ConnectedPathTextBox
            // 
            this.ConnectedPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectedPathTextBox.Location = new System.Drawing.Point(115, 222);
            this.ConnectedPathTextBox.Name = "ConnectedPathTextBox";
            this.ConnectedPathTextBox.ReadOnly = true;
            this.ConnectedPathTextBox.Size = new System.Drawing.Size(268, 32);
            this.ConnectedPathTextBox.TabIndex = 5;
            // 
            // DeleteTagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 508);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DeleteTagForm";
            this.Text = "DeleteTagForm";
            this.Load += new System.EventHandler(this.DeleteTagForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox TagList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.TextBox ConnectedNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ConnectedDescriptionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ConnectedPathTextBox;
        private System.Windows.Forms.Label label3;
    }
}