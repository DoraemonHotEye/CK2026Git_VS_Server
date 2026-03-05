namespace PacketDesignTool
{
    partial class AddMemberSupportsTool
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
            this.AddMemberTool = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OnSingleRadioButton = new System.Windows.Forms.RadioButton();
            this.OnListRadioButton = new System.Windows.Forms.RadioButton();
            this.NewMemberNameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MemberList = new System.Windows.Forms.ListBox();
            this.AddMember = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.AddMemberTool.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.AddMemberTool);
            this.tabControl1.Location = new System.Drawing.Point(4, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1195, 640);
            this.tabControl1.TabIndex = 1;
            // 
            // AddMemberTool
            // 
            this.AddMemberTool.Controls.Add(this.groupBox2);
            this.AddMemberTool.Controls.Add(this.groupBox1);
            this.AddMemberTool.Location = new System.Drawing.Point(4, 31);
            this.AddMemberTool.Name = "AddMemberTool";
            this.AddMemberTool.Size = new System.Drawing.Size(1187, 605);
            this.AddMemberTool.TabIndex = 0;
            this.AddMemberTool.Text = "멤버 추가";
            this.AddMemberTool.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.AddMember);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.NewMemberNameTextBox);
            this.groupBox2.Location = new System.Drawing.Point(599, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(556, 532);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "추가 관련 툴";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "멤버 이름";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.OnSingleRadioButton);
            this.groupBox3.Controls.Add(this.OnListRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(18, 43);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(517, 96);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "제네릭";
            // 
            // OnSingleRadioButton
            // 
            this.OnSingleRadioButton.AutoSize = true;
            this.OnSingleRadioButton.Checked = true;
            this.OnSingleRadioButton.Location = new System.Drawing.Point(145, 43);
            this.OnSingleRadioButton.Name = "OnSingleRadioButton";
            this.OnSingleRadioButton.Size = new System.Drawing.Size(77, 25);
            this.OnSingleRadioButton.TabIndex = 4;
            this.OnSingleRadioButton.TabStop = true;
            this.OnSingleRadioButton.Text = "싱글";
            this.OnSingleRadioButton.UseVisualStyleBackColor = true;
            // 
            // OnListRadioButton
            // 
            this.OnListRadioButton.AutoSize = true;
            this.OnListRadioButton.Location = new System.Drawing.Point(31, 43);
            this.OnListRadioButton.Name = "OnListRadioButton";
            this.OnListRadioButton.Size = new System.Drawing.Size(98, 25);
            this.OnListRadioButton.TabIndex = 2;
            this.OnListRadioButton.Text = "리스트";
            this.OnListRadioButton.UseVisualStyleBackColor = true;
            // 
            // NewMemberNameTextBox
            // 
            this.NewMemberNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewMemberNameTextBox.Location = new System.Drawing.Point(134, 156);
            this.NewMemberNameTextBox.Name = "NewMemberNameTextBox";
            this.NewMemberNameTextBox.Size = new System.Drawing.Size(401, 32);
            this.NewMemberNameTextBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.MemberList);
            this.groupBox1.Location = new System.Drawing.Point(36, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 532);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "사용 가능 멤버 타입";
            // 
            // MemberList
            // 
            this.MemberList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemberList.FormattingEnabled = true;
            this.MemberList.ItemHeight = 21;
            this.MemberList.Location = new System.Drawing.Point(23, 43);
            this.MemberList.Name = "MemberList";
            this.MemberList.Size = new System.Drawing.Size(512, 466);
            this.MemberList.TabIndex = 0;
            this.MemberList.SelectedIndexChanged += new System.EventHandler(this.MemberList_SelectedIndexChanged);
            // 
            // AddMember
            // 
            this.AddMember.Location = new System.Drawing.Point(30, 217);
            this.AddMember.Name = "AddMember";
            this.AddMember.Size = new System.Drawing.Size(505, 46);
            this.AddMember.TabIndex = 7;
            this.AddMember.Text = "추가";
            this.AddMember.UseVisualStyleBackColor = true;
            this.AddMember.Click += new System.EventHandler(this.AddMember_Click);
            // 
            // AddMemberSupportsTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1202, 664);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(1226, 728);
            this.MinimumSize = new System.Drawing.Size(1226, 728);
            this.Name = "AddMemberSupportsTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddMemberSupportsTool";
            this.Load += new System.EventHandler(this.AddMemberSupportsTool_Load);
            this.tabControl1.ResumeLayout(false);
            this.AddMemberTool.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage AddMemberTool;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox MemberList;
        private System.Windows.Forms.RadioButton OnSingleRadioButton;
        private System.Windows.Forms.RadioButton OnListRadioButton;
        private System.Windows.Forms.TextBox NewMemberNameTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddMember;
    }
}