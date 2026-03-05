namespace PacketDesignTool.UserInterface
{
    partial class EventPacketInfomation
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
            this.EventPacketList = new System.Windows.Forms.ListBox();
            this.EventPacketMemberList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ProtocolComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.RemoveLink = new System.Windows.Forms.Button();
            this.AddLinkedProject = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LinkedList = new System.Windows.Forms.ListBox();
            this.EventPacketMemberRemove = new System.Windows.Forms.Button();
            this.EventPacketAddMember = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PacketIDTextBox = new System.Windows.Forms.TextBox();
            this.EventPacketRemove = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventPacketList
            // 
            this.EventPacketList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.EventPacketList.FormattingEnabled = true;
            this.EventPacketList.ItemHeight = 21;
            this.EventPacketList.Location = new System.Drawing.Point(18, 30);
            this.EventPacketList.Name = "EventPacketList";
            this.EventPacketList.ScrollAlwaysVisible = true;
            this.EventPacketList.Size = new System.Drawing.Size(300, 340);
            this.EventPacketList.TabIndex = 0;
            this.EventPacketList.SelectedIndexChanged += new System.EventHandler(this.EventPacketList_SelectedIndexChanged);
            // 
            // EventPacketMemberList
            // 
            this.EventPacketMemberList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.EventPacketMemberList.FormattingEnabled = true;
            this.EventPacketMemberList.ItemHeight = 21;
            this.EventPacketMemberList.Location = new System.Drawing.Point(332, 30);
            this.EventPacketMemberList.Name = "EventPacketMemberList";
            this.EventPacketMemberList.ScrollAlwaysVisible = true;
            this.EventPacketMemberList.Size = new System.Drawing.Size(335, 382);
            this.EventPacketMemberList.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ProtocolComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.RemoveLink);
            this.groupBox1.Controls.Add(this.AddLinkedProject);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.LinkedList);
            this.groupBox1.Controls.Add(this.EventPacketMemberRemove);
            this.groupBox1.Controls.Add(this.EventPacketAddMember);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.PacketIDTextBox);
            this.groupBox1.Location = new System.Drawing.Point(683, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 403);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "패킷 정보";
            // 
            // ProtocolComboBox
            // 
            this.ProtocolComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProtocolComboBox.FormattingEnabled = true;
            this.ProtocolComboBox.Location = new System.Drawing.Point(136, 83);
            this.ProtocolComboBox.Name = "ProtocolComboBox";
            this.ProtocolComboBox.Size = new System.Drawing.Size(331, 29);
            this.ProtocolComboBox.TabIndex = 10;
            this.ProtocolComboBox.SelectedIndexChanged += new System.EventHandler(this.ProtocolComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 21);
            this.label3.TabIndex = 9;
            this.label3.Text = "Protocol";
            // 
            // RemoveLink
            // 
            this.RemoveLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveLink.Location = new System.Drawing.Point(256, 303);
            this.RemoveLink.Name = "RemoveLink";
            this.RemoveLink.Size = new System.Drawing.Size(211, 37);
            this.RemoveLink.TabIndex = 8;
            this.RemoveLink.Text = "링크 삭제";
            this.RemoveLink.UseVisualStyleBackColor = true;
            this.RemoveLink.Click += new System.EventHandler(this.RemoveLink_Click);
            // 
            // AddLinkedProject
            // 
            this.AddLinkedProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddLinkedProject.Location = new System.Drawing.Point(33, 303);
            this.AddLinkedProject.Name = "AddLinkedProject";
            this.AddLinkedProject.Size = new System.Drawing.Size(211, 37);
            this.AddLinkedProject.TabIndex = 7;
            this.AddLinkedProject.Text = "링크 추가";
            this.AddLinkedProject.UseVisualStyleBackColor = true;
            this.AddLinkedProject.Click += new System.EventHandler(this.AddLinkedProject_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "링크된 프로젝트";
            // 
            // LinkedList
            // 
            this.LinkedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkedList.FormattingEnabled = true;
            this.LinkedList.ItemHeight = 21;
            this.LinkedList.Location = new System.Drawing.Point(33, 161);
            this.LinkedList.Name = "LinkedList";
            this.LinkedList.Size = new System.Drawing.Size(434, 130);
            this.LinkedList.TabIndex = 5;
            // 
            // EventPacketMemberRemove
            // 
            this.EventPacketMemberRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EventPacketMemberRemove.Location = new System.Drawing.Point(256, 349);
            this.EventPacketMemberRemove.Name = "EventPacketMemberRemove";
            this.EventPacketMemberRemove.Size = new System.Drawing.Size(211, 37);
            this.EventPacketMemberRemove.TabIndex = 4;
            this.EventPacketMemberRemove.Text = "멤버 삭제";
            this.EventPacketMemberRemove.UseVisualStyleBackColor = true;
            this.EventPacketMemberRemove.Click += new System.EventHandler(this.EventPacketMemberRemove_Click);
            // 
            // EventPacketAddMember
            // 
            this.EventPacketAddMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EventPacketAddMember.Location = new System.Drawing.Point(33, 349);
            this.EventPacketAddMember.Name = "EventPacketAddMember";
            this.EventPacketAddMember.Size = new System.Drawing.Size(211, 37);
            this.EventPacketAddMember.TabIndex = 3;
            this.EventPacketAddMember.Text = "멤버 추가";
            this.EventPacketAddMember.UseVisualStyleBackColor = true;
            this.EventPacketAddMember.Click += new System.EventHandler(this.EventPacketAddMember_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "PacketID";
            // 
            // PacketIDTextBox
            // 
            this.PacketIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PacketIDTextBox.Location = new System.Drawing.Point(136, 32);
            this.PacketIDTextBox.Name = "PacketIDTextBox";
            this.PacketIDTextBox.ReadOnly = true;
            this.PacketIDTextBox.Size = new System.Drawing.Size(331, 32);
            this.PacketIDTextBox.TabIndex = 1;
            // 
            // EventPacketRemove
            // 
            this.EventPacketRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EventPacketRemove.Location = new System.Drawing.Point(18, 379);
            this.EventPacketRemove.Name = "EventPacketRemove";
            this.EventPacketRemove.Size = new System.Drawing.Size(300, 37);
            this.EventPacketRemove.TabIndex = 9;
            this.EventPacketRemove.Text = "이벤트 패킷 삭제";
            this.EventPacketRemove.UseVisualStyleBackColor = true;
            this.EventPacketRemove.Click += new System.EventHandler(this.EventPacketRemove_Click);
            // 
            // EventPacketInfomation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EventPacketRemove);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.EventPacketMemberList);
            this.Controls.Add(this.EventPacketList);
            this.Name = "EventPacketInfomation";
            this.Size = new System.Drawing.Size(1213, 445);
            this.Load += new System.EventHandler(this.EventPacketInfomation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox EventPacketList;
        private System.Windows.Forms.ListBox EventPacketMemberList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PacketIDTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button EventPacketAddMember;
        private System.Windows.Forms.Button EventPacketMemberRemove;
        private System.Windows.Forms.ListBox LinkedList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddLinkedProject;
        private System.Windows.Forms.Button RemoveLink;
        private System.Windows.Forms.Button EventPacketRemove;
        private System.Windows.Forms.ComboBox ProtocolComboBox;
        private System.Windows.Forms.Label label3;
    }
}
