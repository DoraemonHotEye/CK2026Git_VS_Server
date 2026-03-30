namespace PacketDesignTool.SubWinformConfs
{
    partial class AddTagForm
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
            this.TagCreateButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TagDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TagTypeDropDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TagNameTextBox = new System.Windows.Forms.TextBox();
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
            this.menuStrip1.Size = new System.Drawing.Size(1140, 38);
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
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 50);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1116, 437);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TagCreateButton);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.TagDescriptionTextBox);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.TagTypeDropDown);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.TagNameTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1108, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tools";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TagCreateButton
            // 
            this.TagCreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagCreateButton.Location = new System.Drawing.Point(52, 342);
            this.TagCreateButton.Name = "TagCreateButton";
            this.TagCreateButton.Size = new System.Drawing.Size(1019, 41);
            this.TagCreateButton.TabIndex = 6;
            this.TagCreateButton.Text = "만들기";
            this.TagCreateButton.UseVisualStyleBackColor = true;
            this.TagCreateButton.Click += new System.EventHandler(this.TagCreateButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "설명";
            // 
            // TagDescriptionTextBox
            // 
            this.TagDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagDescriptionTextBox.Location = new System.Drawing.Point(52, 198);
            this.TagDescriptionTextBox.Multiline = true;
            this.TagDescriptionTextBox.Name = "TagDescriptionTextBox";
            this.TagDescriptionTextBox.Size = new System.Drawing.Size(1019, 126);
            this.TagDescriptionTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "태그 타입";
            // 
            // TagTypeDropDown
            // 
            this.TagTypeDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagTypeDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TagTypeDropDown.FormattingEnabled = true;
            this.TagTypeDropDown.Location = new System.Drawing.Point(166, 93);
            this.TagTypeDropDown.Name = "TagTypeDropDown";
            this.TagTypeDropDown.Size = new System.Drawing.Size(905, 29);
            this.TagTypeDropDown.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "태그 이름";
            // 
            // TagNameTextBox
            // 
            this.TagNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagNameTextBox.Location = new System.Drawing.Point(166, 42);
            this.TagNameTextBox.Name = "TagNameTextBox";
            this.TagNameTextBox.Size = new System.Drawing.Size(905, 32);
            this.TagNameTextBox.TabIndex = 0;
            // 
            // AddTagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 514);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AddTagForm";
            this.Text = "AddTagForm";
            this.Load += new System.EventHandler(this.AddTagForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ST_Close;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox TagNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox TagTypeDropDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TagDescriptionTextBox;
        private System.Windows.Forms.Button TagCreateButton;
    }
}