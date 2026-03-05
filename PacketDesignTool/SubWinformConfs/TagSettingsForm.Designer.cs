namespace PacketDesignTool.SubWinformConfs
{
    partial class TagSettingsForm
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
            this.태그설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_CreateTag = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_DeleteTag = new System.Windows.Forms.ToolStripMenuItem();
            this.편집ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.닫기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabTagsControll = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.태그설정ToolStripMenuItem,
            this.닫기ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1311, 38);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 태그설정ToolStripMenuItem
            // 
            this.태그설정ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ST_CreateTag,
            this.ST_DeleteTag,
            this.편집ToolStripMenuItem});
            this.태그설정ToolStripMenuItem.Name = "태그설정ToolStripMenuItem";
            this.태그설정ToolStripMenuItem.Size = new System.Drawing.Size(122, 34);
            this.태그설정ToolStripMenuItem.Text = "태그 설정";
            // 
            // ST_CreateTag
            // 
            this.ST_CreateTag.Name = "ST_CreateTag";
            this.ST_CreateTag.Size = new System.Drawing.Size(315, 40);
            this.ST_CreateTag.Text = "만들기";
            this.ST_CreateTag.Click += new System.EventHandler(this.ST_CreateTag_Click);
            // 
            // ST_DeleteTag
            // 
            this.ST_DeleteTag.Name = "ST_DeleteTag";
            this.ST_DeleteTag.Size = new System.Drawing.Size(315, 40);
            this.ST_DeleteTag.Text = "삭제";
            this.ST_DeleteTag.Click += new System.EventHandler(this.ST_DeleteTag_Click);
            // 
            // 편집ToolStripMenuItem
            // 
            this.편집ToolStripMenuItem.Name = "편집ToolStripMenuItem";
            this.편집ToolStripMenuItem.Size = new System.Drawing.Size(315, 40);
            this.편집ToolStripMenuItem.Text = "편집";
            // 
            // 닫기ToolStripMenuItem
            // 
            this.닫기ToolStripMenuItem.Name = "닫기ToolStripMenuItem";
            this.닫기ToolStripMenuItem.Size = new System.Drawing.Size(73, 34);
            this.닫기ToolStripMenuItem.Text = "닫기";
            // 
            // TabTagsControll
            // 
            this.TabTagsControll.Location = new System.Drawing.Point(23, 63);
            this.TabTagsControll.Name = "TabTagsControll";
            this.TabTagsControll.SelectedIndex = 0;
            this.TabTagsControll.Size = new System.Drawing.Size(1263, 638);
            this.TabTagsControll.TabIndex = 1;
            // 
            // TagSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 738);
            this.Controls.Add(this.TabTagsControll);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TagSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TagSettingsForm";
            this.Load += new System.EventHandler(this.TagSettingsForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 태그설정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ST_CreateTag;
        private System.Windows.Forms.ToolStripMenuItem ST_DeleteTag;
        private System.Windows.Forms.ToolStripMenuItem 편집ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 닫기ToolStripMenuItem;
        private System.Windows.Forms.TabControl TabTagsControll;
    }
}