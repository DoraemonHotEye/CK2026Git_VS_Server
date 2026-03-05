namespace PacketDesignTool.SubWinformConfs
{
    partial class AddLinkedProfiler
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ST_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ProjectList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AddLinkerButton = new System.Windows.Forms.Button();
            this.CancelLinker = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ST_Close});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(944, 38);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ST_Close
            // 
            this.ST_Close.Name = "ST_Close";
            this.ST_Close.Size = new System.Drawing.Size(73, 34);
            this.ST_Close.Text = "닫기";
            this.ST_Close.Click += new System.EventHandler(this.ST_Close_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(920, 460);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CancelLinker);
            this.tabPage1.Controls.Add(this.AddLinkerButton);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.ProjectList);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(912, 425);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "프로파일러";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ProjectList
            // 
            this.ProjectList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectList.FormattingEnabled = true;
            this.ProjectList.ItemHeight = 21;
            this.ProjectList.Location = new System.Drawing.Point(19, 19);
            this.ProjectList.Name = "ProjectList";
            this.ProjectList.Size = new System.Drawing.Size(447, 382);
            this.ProjectList.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(482, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(412, 325);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "프로젝트 정보";
            // 
            // AddLinkerButton
            // 
            this.AddLinkerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddLinkerButton.Location = new System.Drawing.Point(482, 360);
            this.AddLinkerButton.Name = "AddLinkerButton";
            this.AddLinkerButton.Size = new System.Drawing.Size(197, 41);
            this.AddLinkerButton.TabIndex = 2;
            this.AddLinkerButton.Text = "추가";
            this.AddLinkerButton.UseVisualStyleBackColor = true;
            this.AddLinkerButton.Click += new System.EventHandler(this.AddLinkerButton_Click);
            // 
            // CancelLinker
            // 
            this.CancelLinker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelLinker.Location = new System.Drawing.Point(697, 360);
            this.CancelLinker.Name = "CancelLinker";
            this.CancelLinker.Size = new System.Drawing.Size(197, 41);
            this.CancelLinker.TabIndex = 3;
            this.CancelLinker.Text = "취소";
            this.CancelLinker.UseVisualStyleBackColor = true;
            // 
            // AddLinkedProfiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 548);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AddLinkedProfiler";
            this.Text = "AddLinkedProfiler";
            this.Load += new System.EventHandler(this.AddLinkedProfiler_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ST_Close;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox ProjectList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CancelLinker;
        private System.Windows.Forms.Button AddLinkerButton;
    }
}