namespace PacketDesignTool.UserInterface
{
    partial class TagInfomation
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ConnectedInfo = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ProfilerDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.ProfilerPathTextBox = new System.Windows.Forms.TextBox();
            this.ProfilerNameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.TagServerTypeTextBox = new System.Windows.Forms.TextBox();
            this.TagSeedTextBox = new System.Windows.Forms.TextBox();
            this.TagNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LanguagesBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ConnectedInfo.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(32, 22);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1201, 582);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ConnectedInfo);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1193, 547);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "태그 정보";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ConnectedInfo
            // 
            this.ConnectedInfo.Controls.Add(this.tabControl2);
            this.ConnectedInfo.Location = new System.Drawing.Point(503, 29);
            this.ConnectedInfo.Name = "ConnectedInfo";
            this.ConnectedInfo.Size = new System.Drawing.Size(644, 473);
            this.ConnectedInfo.TabIndex = 1;
            this.ConnectedInfo.TabStop = false;
            this.ConnectedInfo.Text = "Connected Info";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(6, 31);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(632, 436);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(624, 401);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "결속된 정보";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.LanguagesBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ProfilerDescriptionTextBox);
            this.groupBox2.Controls.Add(this.ProfilerPathTextBox);
            this.groupBox2.Controls.Add(this.ProfilerNameTextBox);
            this.groupBox2.Location = new System.Drawing.Point(18, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(588, 361);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connected Profiler";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 21);
            this.label7.TabIndex = 5;
            this.label7.Text = "Path:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 21);
            this.label6.TabIndex = 4;
            this.label6.Text = "Description";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 21);
            this.label5.TabIndex = 3;
            this.label5.Text = "Name:";
            // 
            // ProfilerDescriptionTextBox
            // 
            this.ProfilerDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProfilerDescriptionTextBox.Location = new System.Drawing.Point(24, 220);
            this.ProfilerDescriptionTextBox.Multiline = true;
            this.ProfilerDescriptionTextBox.Name = "ProfilerDescriptionTextBox";
            this.ProfilerDescriptionTextBox.ReadOnly = true;
            this.ProfilerDescriptionTextBox.Size = new System.Drawing.Size(543, 119);
            this.ProfilerDescriptionTextBox.TabIndex = 2;
            // 
            // ProfilerPathTextBox
            // 
            this.ProfilerPathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ProfilerPathTextBox.Location = new System.Drawing.Point(140, 81);
            this.ProfilerPathTextBox.Name = "ProfilerPathTextBox";
            this.ProfilerPathTextBox.ReadOnly = true;
            this.ProfilerPathTextBox.Size = new System.Drawing.Size(427, 32);
            this.ProfilerPathTextBox.TabIndex = 1;
            // 
            // ProfilerNameTextBox
            // 
            this.ProfilerNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ProfilerNameTextBox.Location = new System.Drawing.Point(140, 43);
            this.ProfilerNameTextBox.Name = "ProfilerNameTextBox";
            this.ProfilerNameTextBox.ReadOnly = true;
            this.ProfilerNameTextBox.Size = new System.Drawing.Size(427, 32);
            this.ProfilerNameTextBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.DescriptionTextBox);
            this.groupBox1.Controls.Add(this.TagServerTypeTextBox);
            this.groupBox1.Controls.Add(this.TagSeedTextBox);
            this.groupBox1.Controls.Add(this.TagNameTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(36, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 473);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Basic Info";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = "Description";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextBox.Location = new System.Drawing.Point(30, 224);
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.ReadOnly = true;
            this.DescriptionTextBox.Size = new System.Drawing.Size(405, 221);
            this.DescriptionTextBox.TabIndex = 6;
            // 
            // TagServerTypeTextBox
            // 
            this.TagServerTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagServerTypeTextBox.Location = new System.Drawing.Point(196, 123);
            this.TagServerTypeTextBox.Name = "TagServerTypeTextBox";
            this.TagServerTypeTextBox.ReadOnly = true;
            this.TagServerTypeTextBox.Size = new System.Drawing.Size(239, 32);
            this.TagServerTypeTextBox.TabIndex = 5;
            // 
            // TagSeedTextBox
            // 
            this.TagSeedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagSeedTextBox.Location = new System.Drawing.Point(196, 84);
            this.TagSeedTextBox.Name = "TagSeedTextBox";
            this.TagSeedTextBox.ReadOnly = true;
            this.TagSeedTextBox.Size = new System.Drawing.Size(239, 32);
            this.TagSeedTextBox.TabIndex = 4;
            // 
            // TagNameTextBox
            // 
            this.TagNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TagNameTextBox.Location = new System.Drawing.Point(196, 47);
            this.TagNameTextBox.Name = "TagNameTextBox";
            this.TagNameTextBox.ReadOnly = true;
            this.TagNameTextBox.Size = new System.Drawing.Size(239, 32);
            this.TagNameTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "ServerType:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Seed:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // LanguagesBox
            // 
            this.LanguagesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LanguagesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LanguagesBox.FormattingEnabled = true;
            this.LanguagesBox.Location = new System.Drawing.Point(140, 125);
            this.LanguagesBox.Name = "LanguagesBox";
            this.LanguagesBox.Size = new System.Drawing.Size(427, 29);
            this.LanguagesBox.TabIndex = 8;
            this.LanguagesBox.SelectedIndexChanged += new System.EventHandler(this.LanguagesBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 21);
            this.label8.TabIndex = 9;
            this.label8.Text = "언어:";
            // 
            // TagInfomation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.tabControl1);
            this.Name = "TagInfomation";
            this.Size = new System.Drawing.Size(1263, 638);
            this.Load += new System.EventHandler(this.TagInfomation_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ConnectedInfo.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox ConnectedInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TagServerTypeTextBox;
        private System.Windows.Forms.TextBox TagSeedTextBox;
        private System.Windows.Forms.TextBox TagNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ProfilerDescriptionTextBox;
        private System.Windows.Forms.TextBox ProfilerPathTextBox;
        private System.Windows.Forms.TextBox ProfilerNameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox LanguagesBox;
    }
}
