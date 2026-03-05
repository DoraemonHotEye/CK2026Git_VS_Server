namespace PacketDesignTool
{
    partial class PacketDesignForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ST_File = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_File_LoadPackets = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_File_SavePackets = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_File_GenerateCode = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_TagSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_ServerProjectsSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.ST_CloseStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.WN_GroupBox = new System.Windows.Forms.GroupBox();
            this.PacketMiscLists = new System.Windows.Forms.TabControl();
            this.TP1_ClassList = new System.Windows.Forms.TabPage();
            this.ClassDeleteButton = new System.Windows.Forms.Button();
            this.AddPacketClassMember = new System.Windows.Forms.Button();
            this.RemovePacketClassMember = new System.Windows.Forms.Button();
            this.PacketClassMembersList = new System.Windows.Forms.ListBox();
            this.ToolGroupBox = new System.Windows.Forms.GroupBox();
            this.ToolTab = new System.Windows.Forms.TabControl();
            this.Class_Create = new System.Windows.Forms.TabPage();
            this.NewClassMemberDelete = new System.Windows.Forms.Button();
            this.AddCreateClassMember = new System.Windows.Forms.Button();
            this.NewClassMemberList = new System.Windows.Forms.ListBox();
            this.ClassCreateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ClassNameTextBox = new System.Windows.Forms.TextBox();
            this.PacketClassList = new System.Windows.Forms.ListBox();
            this.TP1_StructList = new System.Windows.Forms.TabPage();
            this.StructDeleteButton = new System.Windows.Forms.Button();
            this.AddPacketStructMember = new System.Windows.Forms.Button();
            this.RemovePacketStructMember = new System.Windows.Forms.Button();
            this.PacketStructMembersList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.NewStructMemberRemove = new System.Windows.Forms.Button();
            this.AddMemberNewStruct = new System.Windows.Forms.Button();
            this.NewStructMemberList = new System.Windows.Forms.ListBox();
            this.CreateStruct = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.StructNameTextBox = new System.Windows.Forms.TextBox();
            this.PacketStructList = new System.Windows.Forms.ListBox();
            this.TP1_EventPackets = new System.Windows.Forms.TabPage();
            this.ProjectPacketTabs = new System.Windows.Forms.TabControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.NewPacketAddButton = new System.Windows.Forms.Button();
            this.AllResetNewPacketDatas = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.NewPacketMemberResetButton = new System.Windows.Forms.Button();
            this.NewPacketMemberRemoveButton = new System.Windows.Forms.Button();
            this.NewPacketMemberAddButton = new System.Windows.Forms.Button();
            this.NewPacketMemberList = new System.Windows.Forms.ListBox();
            this.NewPacket = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NewPacketIDTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NewPacketNameTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.WN_GroupBox.SuspendLayout();
            this.PacketMiscLists.SuspendLayout();
            this.TP1_ClassList.SuspendLayout();
            this.ToolGroupBox.SuspendLayout();
            this.ToolTab.SuspendLayout();
            this.Class_Create.SuspendLayout();
            this.TP1_StructList.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.TP1_EventPackets.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.NewPacket.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Location = new System.Drawing.Point(0, 973);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1332, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ST_File,
            this.ST_Help,
            this.설정ToolStripMenuItem,
            this.ST_CloseStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1332, 38);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ST_File
            // 
            this.ST_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ST_File_LoadPackets,
            this.ST_File_SavePackets,
            this.ST_File_GenerateCode});
            this.ST_File.Name = "ST_File";
            this.ST_File.Size = new System.Drawing.Size(73, 34);
            this.ST_File.Text = "파일";
            // 
            // ST_File_LoadPackets
            // 
            this.ST_File_LoadPackets.Name = "ST_File_LoadPackets";
            this.ST_File_LoadPackets.Size = new System.Drawing.Size(315, 40);
            this.ST_File_LoadPackets.Text = "불러오기";
            this.ST_File_LoadPackets.Click += new System.EventHandler(this.ST_File_LoadPackets_Click);
            // 
            // ST_File_SavePackets
            // 
            this.ST_File_SavePackets.Name = "ST_File_SavePackets";
            this.ST_File_SavePackets.Size = new System.Drawing.Size(315, 40);
            this.ST_File_SavePackets.Text = "저장하기";
            this.ST_File_SavePackets.Click += new System.EventHandler(this.ST_File_SavePackets_Click);
            // 
            // ST_File_GenerateCode
            // 
            this.ST_File_GenerateCode.Name = "ST_File_GenerateCode";
            this.ST_File_GenerateCode.Size = new System.Drawing.Size(315, 40);
            this.ST_File_GenerateCode.Text = "코드 추출";
            this.ST_File_GenerateCode.Click += new System.EventHandler(this.ST_File_GenerateCode_Click);
            // 
            // ST_Help
            // 
            this.ST_Help.Name = "ST_Help";
            this.ST_Help.Size = new System.Drawing.Size(94, 34);
            this.ST_Help.Text = "도움말";
            this.ST_Help.Click += new System.EventHandler(this.ST_Help_Click);
            // 
            // 설정ToolStripMenuItem
            // 
            this.설정ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ST_TagSetting,
            this.ST_ServerProjectsSetting});
            this.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem";
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(73, 34);
            this.설정ToolStripMenuItem.Text = "설정";
            // 
            // ST_TagSetting
            // 
            this.ST_TagSetting.Name = "ST_TagSetting";
            this.ST_TagSetting.Size = new System.Drawing.Size(313, 40);
            this.ST_TagSetting.Text = "태그 설정";
            this.ST_TagSetting.Click += new System.EventHandler(this.ST_TagSetting_Click);
            // 
            // ST_ServerProjectsSetting
            // 
            this.ST_ServerProjectsSetting.Name = "ST_ServerProjectsSetting";
            this.ST_ServerProjectsSetting.Size = new System.Drawing.Size(313, 40);
            this.ST_ServerProjectsSetting.Text = "서버 프로젝트 설정";
            this.ST_ServerProjectsSetting.Click += new System.EventHandler(this.ST_ServerProjectsSetting_Click);
            // 
            // ST_CloseStrip
            // 
            this.ST_CloseStrip.Name = "ST_CloseStrip";
            this.ST_CloseStrip.Size = new System.Drawing.Size(73, 34);
            this.ST_CloseStrip.Text = "닫기";
            this.ST_CloseStrip.Click += new System.EventHandler(this.ST_CloseStrip_Click);
            // 
            // WN_GroupBox
            // 
            this.WN_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WN_GroupBox.Controls.Add(this.PacketMiscLists);
            this.WN_GroupBox.Location = new System.Drawing.Point(13, 59);
            this.WN_GroupBox.Name = "WN_GroupBox";
            this.WN_GroupBox.Size = new System.Drawing.Size(1307, 879);
            this.WN_GroupBox.TabIndex = 2;
            this.WN_GroupBox.TabStop = false;
            this.WN_GroupBox.Text = "패킷 디자인 그룹";
            // 
            // PacketMiscLists
            // 
            this.PacketMiscLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketMiscLists.Controls.Add(this.TP1_ClassList);
            this.PacketMiscLists.Controls.Add(this.TP1_StructList);
            this.PacketMiscLists.Controls.Add(this.TP1_EventPackets);
            this.PacketMiscLists.Location = new System.Drawing.Point(18, 31);
            this.PacketMiscLists.Name = "PacketMiscLists";
            this.PacketMiscLists.SelectedIndex = 0;
            this.PacketMiscLists.Size = new System.Drawing.Size(1273, 819);
            this.PacketMiscLists.TabIndex = 0;
            this.PacketMiscLists.SelectedIndexChanged += new System.EventHandler(this.PacketMiscLists_SelectedIndexChanged);
            // 
            // TP1_ClassList
            // 
            this.TP1_ClassList.Controls.Add(this.ClassDeleteButton);
            this.TP1_ClassList.Controls.Add(this.AddPacketClassMember);
            this.TP1_ClassList.Controls.Add(this.RemovePacketClassMember);
            this.TP1_ClassList.Controls.Add(this.PacketClassMembersList);
            this.TP1_ClassList.Controls.Add(this.ToolGroupBox);
            this.TP1_ClassList.Controls.Add(this.PacketClassList);
            this.TP1_ClassList.Location = new System.Drawing.Point(4, 31);
            this.TP1_ClassList.Name = "TP1_ClassList";
            this.TP1_ClassList.Padding = new System.Windows.Forms.Padding(3);
            this.TP1_ClassList.Size = new System.Drawing.Size(1265, 784);
            this.TP1_ClassList.TabIndex = 0;
            this.TP1_ClassList.Text = "패킷 클래스 종류";
            this.TP1_ClassList.UseVisualStyleBackColor = true;
            // 
            // ClassDeleteButton
            // 
            this.ClassDeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClassDeleteButton.Location = new System.Drawing.Point(26, 491);
            this.ClassDeleteButton.Name = "ClassDeleteButton";
            this.ClassDeleteButton.Size = new System.Drawing.Size(152, 45);
            this.ClassDeleteButton.TabIndex = 6;
            this.ClassDeleteButton.Text = "삭제";
            this.ClassDeleteButton.UseVisualStyleBackColor = true;
            this.ClassDeleteButton.Click += new System.EventHandler(this.ClassDeleteButton_Click);
            // 
            // AddPacketClassMember
            // 
            this.AddPacketClassMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddPacketClassMember.Location = new System.Drawing.Point(772, 491);
            this.AddPacketClassMember.Name = "AddPacketClassMember";
            this.AddPacketClassMember.Size = new System.Drawing.Size(236, 45);
            this.AddPacketClassMember.TabIndex = 4;
            this.AddPacketClassMember.Text = "추가";
            this.AddPacketClassMember.UseVisualStyleBackColor = true;
            this.AddPacketClassMember.Click += new System.EventHandler(this.AddPacketClassMember_Click);
            // 
            // RemovePacketClassMember
            // 
            this.RemovePacketClassMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemovePacketClassMember.Location = new System.Drawing.Point(1014, 491);
            this.RemovePacketClassMember.Name = "RemovePacketClassMember";
            this.RemovePacketClassMember.Size = new System.Drawing.Size(229, 45);
            this.RemovePacketClassMember.TabIndex = 3;
            this.RemovePacketClassMember.Text = "삭제";
            this.RemovePacketClassMember.UseVisualStyleBackColor = true;
            this.RemovePacketClassMember.Click += new System.EventHandler(this.RemovePacketClassMember_Click);
            // 
            // PacketClassMembersList
            // 
            this.PacketClassMembersList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketClassMembersList.FormattingEnabled = true;
            this.PacketClassMembersList.ItemHeight = 21;
            this.PacketClassMembersList.Location = new System.Drawing.Point(772, 30);
            this.PacketClassMembersList.Name = "PacketClassMembersList";
            this.PacketClassMembersList.ScrollAlwaysVisible = true;
            this.PacketClassMembersList.Size = new System.Drawing.Size(471, 445);
            this.PacketClassMembersList.TabIndex = 2;
            // 
            // ToolGroupBox
            // 
            this.ToolGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolGroupBox.Controls.Add(this.ToolTab);
            this.ToolGroupBox.Location = new System.Drawing.Point(26, 551);
            this.ToolGroupBox.Name = "ToolGroupBox";
            this.ToolGroupBox.Size = new System.Drawing.Size(1217, 217);
            this.ToolGroupBox.TabIndex = 1;
            this.ToolGroupBox.TabStop = false;
            this.ToolGroupBox.Text = "Tools";
            // 
            // ToolTab
            // 
            this.ToolTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolTab.Controls.Add(this.Class_Create);
            this.ToolTab.Location = new System.Drawing.Point(23, 32);
            this.ToolTab.Name = "ToolTab";
            this.ToolTab.SelectedIndex = 0;
            this.ToolTab.Size = new System.Drawing.Size(1173, 165);
            this.ToolTab.TabIndex = 0;
            // 
            // Class_Create
            // 
            this.Class_Create.Controls.Add(this.NewClassMemberDelete);
            this.Class_Create.Controls.Add(this.AddCreateClassMember);
            this.Class_Create.Controls.Add(this.NewClassMemberList);
            this.Class_Create.Controls.Add(this.ClassCreateButton);
            this.Class_Create.Controls.Add(this.label1);
            this.Class_Create.Controls.Add(this.ClassNameTextBox);
            this.Class_Create.Location = new System.Drawing.Point(4, 31);
            this.Class_Create.Name = "Class_Create";
            this.Class_Create.Padding = new System.Windows.Forms.Padding(3);
            this.Class_Create.Size = new System.Drawing.Size(1165, 130);
            this.Class_Create.TabIndex = 0;
            this.Class_Create.Text = "클래스 새로 만들기";
            this.Class_Create.UseVisualStyleBackColor = true;
            // 
            // NewClassMemberDelete
            // 
            this.NewClassMemberDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewClassMemberDelete.Location = new System.Drawing.Point(987, 21);
            this.NewClassMemberDelete.Name = "NewClassMemberDelete";
            this.NewClassMemberDelete.Size = new System.Drawing.Size(171, 42);
            this.NewClassMemberDelete.TabIndex = 5;
            this.NewClassMemberDelete.Text = "멤버 삭제";
            this.NewClassMemberDelete.UseVisualStyleBackColor = true;
            this.NewClassMemberDelete.Click += new System.EventHandler(this.NewClassMemberDelete_Click);
            // 
            // AddCreateClassMember
            // 
            this.AddCreateClassMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddCreateClassMember.Location = new System.Drawing.Point(810, 21);
            this.AddCreateClassMember.Name = "AddCreateClassMember";
            this.AddCreateClassMember.Size = new System.Drawing.Size(171, 42);
            this.AddCreateClassMember.TabIndex = 4;
            this.AddCreateClassMember.Text = "멤버 추가";
            this.AddCreateClassMember.UseVisualStyleBackColor = true;
            this.AddCreateClassMember.Click += new System.EventHandler(this.AddCreateClassMember_Click);
            // 
            // NewClassMemberList
            // 
            this.NewClassMemberList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewClassMemberList.FormattingEnabled = true;
            this.NewClassMemberList.ItemHeight = 21;
            this.NewClassMemberList.Location = new System.Drawing.Point(432, 19);
            this.NewClassMemberList.Name = "NewClassMemberList";
            this.NewClassMemberList.Size = new System.Drawing.Size(372, 88);
            this.NewClassMemberList.TabIndex = 3;
            // 
            // ClassCreateButton
            // 
            this.ClassCreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClassCreateButton.Location = new System.Drawing.Point(810, 65);
            this.ClassCreateButton.Name = "ClassCreateButton";
            this.ClassCreateButton.Size = new System.Drawing.Size(171, 42);
            this.ClassCreateButton.TabIndex = 2;
            this.ClassCreateButton.Text = "만들기";
            this.ClassCreateButton.UseVisualStyleBackColor = true;
            this.ClassCreateButton.Click += new System.EventHandler(this.ClassCreateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "클래스 이름";
            // 
            // ClassNameTextBox
            // 
            this.ClassNameTextBox.Location = new System.Drawing.Point(147, 16);
            this.ClassNameTextBox.Name = "ClassNameTextBox";
            this.ClassNameTextBox.Size = new System.Drawing.Size(266, 32);
            this.ClassNameTextBox.TabIndex = 0;
            // 
            // PacketClassList
            // 
            this.PacketClassList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketClassList.FormattingEnabled = true;
            this.PacketClassList.ItemHeight = 21;
            this.PacketClassList.Location = new System.Drawing.Point(26, 30);
            this.PacketClassList.Name = "PacketClassList";
            this.PacketClassList.ScrollAlwaysVisible = true;
            this.PacketClassList.Size = new System.Drawing.Size(740, 445);
            this.PacketClassList.TabIndex = 0;
            this.PacketClassList.SelectedIndexChanged += new System.EventHandler(this.PacketClassList_SelectedIndexChanged);
            // 
            // TP1_StructList
            // 
            this.TP1_StructList.Controls.Add(this.StructDeleteButton);
            this.TP1_StructList.Controls.Add(this.AddPacketStructMember);
            this.TP1_StructList.Controls.Add(this.RemovePacketStructMember);
            this.TP1_StructList.Controls.Add(this.PacketStructMembersList);
            this.TP1_StructList.Controls.Add(this.groupBox1);
            this.TP1_StructList.Controls.Add(this.PacketStructList);
            this.TP1_StructList.Location = new System.Drawing.Point(4, 31);
            this.TP1_StructList.Name = "TP1_StructList";
            this.TP1_StructList.Padding = new System.Windows.Forms.Padding(3);
            this.TP1_StructList.Size = new System.Drawing.Size(1265, 784);
            this.TP1_StructList.TabIndex = 1;
            this.TP1_StructList.Text = "패킷 구조체 종류";
            this.TP1_StructList.UseVisualStyleBackColor = true;
            // 
            // StructDeleteButton
            // 
            this.StructDeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StructDeleteButton.Location = new System.Drawing.Point(24, 484);
            this.StructDeleteButton.Name = "StructDeleteButton";
            this.StructDeleteButton.Size = new System.Drawing.Size(152, 45);
            this.StructDeleteButton.TabIndex = 12;
            this.StructDeleteButton.Text = "삭제";
            this.StructDeleteButton.UseVisualStyleBackColor = true;
            this.StructDeleteButton.Click += new System.EventHandler(this.StructDeleteButton_Click);
            // 
            // AddPacketStructMember
            // 
            this.AddPacketStructMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddPacketStructMember.Location = new System.Drawing.Point(770, 484);
            this.AddPacketStructMember.Name = "AddPacketStructMember";
            this.AddPacketStructMember.Size = new System.Drawing.Size(236, 45);
            this.AddPacketStructMember.TabIndex = 11;
            this.AddPacketStructMember.Text = "추가";
            this.AddPacketStructMember.UseVisualStyleBackColor = true;
            this.AddPacketStructMember.Click += new System.EventHandler(this.AddPacketStructMember_Click);
            // 
            // RemovePacketStructMember
            // 
            this.RemovePacketStructMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemovePacketStructMember.Location = new System.Drawing.Point(1012, 484);
            this.RemovePacketStructMember.Name = "RemovePacketStructMember";
            this.RemovePacketStructMember.Size = new System.Drawing.Size(229, 45);
            this.RemovePacketStructMember.TabIndex = 10;
            this.RemovePacketStructMember.Text = "삭제";
            this.RemovePacketStructMember.UseVisualStyleBackColor = true;
            this.RemovePacketStructMember.Click += new System.EventHandler(this.RemovePacketStructMember_Click);
            // 
            // PacketStructMembersList
            // 
            this.PacketStructMembersList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketStructMembersList.FormattingEnabled = true;
            this.PacketStructMembersList.ItemHeight = 21;
            this.PacketStructMembersList.Location = new System.Drawing.Point(770, 23);
            this.PacketStructMembersList.Name = "PacketStructMembersList";
            this.PacketStructMembersList.ScrollAlwaysVisible = true;
            this.PacketStructMembersList.Size = new System.Drawing.Size(471, 445);
            this.PacketStructMembersList.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(24, 544);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1217, 217);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tools";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(23, 32);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1173, 165);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.NewStructMemberRemove);
            this.tabPage1.Controls.Add(this.AddMemberNewStruct);
            this.tabPage1.Controls.Add(this.NewStructMemberList);
            this.tabPage1.Controls.Add(this.CreateStruct);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.StructNameTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1165, 130);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "구조체 새로 만들기";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // NewStructMemberRemove
            // 
            this.NewStructMemberRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewStructMemberRemove.Location = new System.Drawing.Point(979, 16);
            this.NewStructMemberRemove.Name = "NewStructMemberRemove";
            this.NewStructMemberRemove.Size = new System.Drawing.Size(171, 42);
            this.NewStructMemberRemove.TabIndex = 6;
            this.NewStructMemberRemove.Text = "멤버 삭제";
            this.NewStructMemberRemove.UseVisualStyleBackColor = true;
            // 
            // AddMemberNewStruct
            // 
            this.AddMemberNewStruct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddMemberNewStruct.Location = new System.Drawing.Point(802, 17);
            this.AddMemberNewStruct.Name = "AddMemberNewStruct";
            this.AddMemberNewStruct.Size = new System.Drawing.Size(171, 42);
            this.AddMemberNewStruct.TabIndex = 4;
            this.AddMemberNewStruct.Text = "멤버 추가";
            this.AddMemberNewStruct.UseVisualStyleBackColor = true;
            this.AddMemberNewStruct.Click += new System.EventHandler(this.AddMemberNewStruct_Click);
            // 
            // NewStructMemberList
            // 
            this.NewStructMemberList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewStructMemberList.FormattingEnabled = true;
            this.NewStructMemberList.ItemHeight = 21;
            this.NewStructMemberList.Location = new System.Drawing.Point(419, 17);
            this.NewStructMemberList.Name = "NewStructMemberList";
            this.NewStructMemberList.Size = new System.Drawing.Size(377, 88);
            this.NewStructMemberList.TabIndex = 3;
            // 
            // CreateStruct
            // 
            this.CreateStruct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateStruct.Location = new System.Drawing.Point(802, 67);
            this.CreateStruct.Name = "CreateStruct";
            this.CreateStruct.Size = new System.Drawing.Size(171, 42);
            this.CreateStruct.TabIndex = 2;
            this.CreateStruct.Text = "만들기";
            this.CreateStruct.UseVisualStyleBackColor = true;
            this.CreateStruct.Click += new System.EventHandler(this.CreateStruct_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "구조체 이름";
            // 
            // StructNameTextBox
            // 
            this.StructNameTextBox.Location = new System.Drawing.Point(147, 16);
            this.StructNameTextBox.Name = "StructNameTextBox";
            this.StructNameTextBox.Size = new System.Drawing.Size(266, 32);
            this.StructNameTextBox.TabIndex = 0;
            // 
            // PacketStructList
            // 
            this.PacketStructList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketStructList.FormattingEnabled = true;
            this.PacketStructList.ItemHeight = 21;
            this.PacketStructList.Location = new System.Drawing.Point(24, 23);
            this.PacketStructList.Name = "PacketStructList";
            this.PacketStructList.ScrollAlwaysVisible = true;
            this.PacketStructList.Size = new System.Drawing.Size(740, 445);
            this.PacketStructList.TabIndex = 7;
            this.PacketStructList.SelectedIndexChanged += new System.EventHandler(this.PacketStructList_SelectedIndexChanged);
            // 
            // TP1_EventPackets
            // 
            this.TP1_EventPackets.Controls.Add(this.ProjectPacketTabs);
            this.TP1_EventPackets.Controls.Add(this.tabControl2);
            this.TP1_EventPackets.Location = new System.Drawing.Point(4, 31);
            this.TP1_EventPackets.Name = "TP1_EventPackets";
            this.TP1_EventPackets.Size = new System.Drawing.Size(1265, 784);
            this.TP1_EventPackets.TabIndex = 2;
            this.TP1_EventPackets.Text = "이벤트 패킷 종류";
            this.TP1_EventPackets.UseVisualStyleBackColor = true;
            // 
            // ProjectPacketTabs
            // 
            this.ProjectPacketTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectPacketTabs.Location = new System.Drawing.Point(28, 30);
            this.ProjectPacketTabs.Name = "ProjectPacketTabs";
            this.ProjectPacketTabs.SelectedIndex = 0;
            this.ProjectPacketTabs.Size = new System.Drawing.Size(1213, 445);
            this.ProjectPacketTabs.TabIndex = 3;
            this.ProjectPacketTabs.SelectedIndexChanged += new System.EventHandler(this.ProjectPacketTabs_SelectedIndexChanged);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Location = new System.Drawing.Point(28, 491);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1217, 261);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.NewPacket);
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1209, 226);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "이벤트 추가툴";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.NewPacketAddButton);
            this.groupBox3.Controls.Add(this.AllResetNewPacketDatas);
            this.groupBox3.Location = new System.Drawing.Point(977, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(213, 183);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tools";
            // 
            // NewPacketAddButton
            // 
            this.NewPacketAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketAddButton.Location = new System.Drawing.Point(24, 59);
            this.NewPacketAddButton.Name = "NewPacketAddButton";
            this.NewPacketAddButton.Size = new System.Drawing.Size(169, 35);
            this.NewPacketAddButton.TabIndex = 1;
            this.NewPacketAddButton.Text = "만들기";
            this.NewPacketAddButton.UseVisualStyleBackColor = true;
            this.NewPacketAddButton.Click += new System.EventHandler(this.NewPacketAddButton_Click);
            // 
            // AllResetNewPacketDatas
            // 
            this.AllResetNewPacketDatas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllResetNewPacketDatas.Location = new System.Drawing.Point(24, 110);
            this.AllResetNewPacketDatas.Name = "AllResetNewPacketDatas";
            this.AllResetNewPacketDatas.Size = new System.Drawing.Size(169, 35);
            this.AllResetNewPacketDatas.TabIndex = 0;
            this.AllResetNewPacketDatas.Text = "전체 초기화";
            this.AllResetNewPacketDatas.UseVisualStyleBackColor = true;
            this.AllResetNewPacketDatas.Click += new System.EventHandler(this.AllResetNewPacketDatas_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.NewPacketMemberResetButton);
            this.groupBox2.Controls.Add(this.NewPacketMemberRemoveButton);
            this.groupBox2.Controls.Add(this.NewPacketMemberAddButton);
            this.groupBox2.Controls.Add(this.NewPacketMemberList);
            this.groupBox2.Location = new System.Drawing.Point(516, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(445, 183);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "새로운 패킷 멤버 정보";
            // 
            // NewPacketMemberResetButton
            // 
            this.NewPacketMemberResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketMemberResetButton.Location = new System.Drawing.Point(299, 123);
            this.NewPacketMemberResetButton.Name = "NewPacketMemberResetButton";
            this.NewPacketMemberResetButton.Size = new System.Drawing.Size(140, 32);
            this.NewPacketMemberResetButton.TabIndex = 3;
            this.NewPacketMemberResetButton.Text = "초기화";
            this.NewPacketMemberResetButton.UseVisualStyleBackColor = true;
            this.NewPacketMemberResetButton.Click += new System.EventHandler(this.NewPacketMemberResetButton_Click);
            // 
            // NewPacketMemberRemoveButton
            // 
            this.NewPacketMemberRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketMemberRemoveButton.Location = new System.Drawing.Point(298, 85);
            this.NewPacketMemberRemoveButton.Name = "NewPacketMemberRemoveButton";
            this.NewPacketMemberRemoveButton.Size = new System.Drawing.Size(140, 32);
            this.NewPacketMemberRemoveButton.TabIndex = 2;
            this.NewPacketMemberRemoveButton.Text = "멤버 삭제";
            this.NewPacketMemberRemoveButton.UseVisualStyleBackColor = true;
            this.NewPacketMemberRemoveButton.Click += new System.EventHandler(this.NewPacketMemberRemoveButton_Click);
            // 
            // NewPacketMemberAddButton
            // 
            this.NewPacketMemberAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketMemberAddButton.Location = new System.Drawing.Point(299, 47);
            this.NewPacketMemberAddButton.Name = "NewPacketMemberAddButton";
            this.NewPacketMemberAddButton.Size = new System.Drawing.Size(140, 32);
            this.NewPacketMemberAddButton.TabIndex = 1;
            this.NewPacketMemberAddButton.Text = "멤버 추가";
            this.NewPacketMemberAddButton.UseVisualStyleBackColor = true;
            this.NewPacketMemberAddButton.Click += new System.EventHandler(this.NewPacketMemberAddButton_Click);
            // 
            // NewPacketMemberList
            // 
            this.NewPacketMemberList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketMemberList.FormattingEnabled = true;
            this.NewPacketMemberList.ItemHeight = 21;
            this.NewPacketMemberList.Location = new System.Drawing.Point(19, 47);
            this.NewPacketMemberList.Name = "NewPacketMemberList";
            this.NewPacketMemberList.ScrollAlwaysVisible = true;
            this.NewPacketMemberList.Size = new System.Drawing.Size(273, 109);
            this.NewPacketMemberList.TabIndex = 0;
            // 
            // NewPacket
            // 
            this.NewPacket.Controls.Add(this.label4);
            this.NewPacket.Controls.Add(this.NewPacketIDTextBox);
            this.NewPacket.Controls.Add(this.label3);
            this.NewPacket.Controls.Add(this.NewPacketNameTextBox);
            this.NewPacket.Location = new System.Drawing.Point(18, 20);
            this.NewPacket.Name = "NewPacket";
            this.NewPacket.Size = new System.Drawing.Size(476, 183);
            this.NewPacket.TabIndex = 0;
            this.NewPacket.TabStop = false;
            this.NewPacket.Text = "새로운 패킷 정보";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "ID:";
            // 
            // NewPacketIDTextBox
            // 
            this.NewPacketIDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NewPacketIDTextBox.Location = new System.Drawing.Point(125, 110);
            this.NewPacketIDTextBox.Name = "NewPacketIDTextBox";
            this.NewPacketIDTextBox.ReadOnly = true;
            this.NewPacketIDTextBox.Size = new System.Drawing.Size(325, 32);
            this.NewPacketIDTextBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 21);
            this.label3.TabIndex = 1;
            this.label3.Text = "Name:";
            // 
            // NewPacketNameTextBox
            // 
            this.NewPacketNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewPacketNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NewPacketNameTextBox.Location = new System.Drawing.Point(125, 62);
            this.NewPacketNameTextBox.Name = "NewPacketNameTextBox";
            this.NewPacketNameTextBox.Size = new System.Drawing.Size(325, 32);
            this.NewPacketNameTextBox.TabIndex = 0;
            this.NewPacketNameTextBox.TextChanged += new System.EventHandler(this.NewPacketNameTextBox_TextChanged);
            // 
            // PacketDesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1332, 995);
            this.Controls.Add(this.WN_GroupBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1356, 1059);
            this.Name = "PacketDesignForm";
            this.Text = "패킷 자동화 툴 (정보 공유 전용)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PacketDesignForm_FormClosed);
            this.Load += new System.EventHandler(this.PacketDesignForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.WN_GroupBox.ResumeLayout(false);
            this.PacketMiscLists.ResumeLayout(false);
            this.TP1_ClassList.ResumeLayout(false);
            this.ToolGroupBox.ResumeLayout(false);
            this.ToolTab.ResumeLayout(false);
            this.Class_Create.ResumeLayout(false);
            this.Class_Create.PerformLayout();
            this.TP1_StructList.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.TP1_EventPackets.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.NewPacket.ResumeLayout(false);
            this.NewPacket.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ST_File;
        private System.Windows.Forms.ToolStripMenuItem ST_File_LoadPackets;
        private System.Windows.Forms.ToolStripMenuItem ST_File_SavePackets;
        private System.Windows.Forms.ToolStripMenuItem ST_File_GenerateCode;
        private System.Windows.Forms.ToolStripMenuItem ST_CloseStrip;
        private System.Windows.Forms.GroupBox WN_GroupBox;
        private System.Windows.Forms.TabControl PacketMiscLists;
        private System.Windows.Forms.TabPage TP1_ClassList;
        private System.Windows.Forms.TabPage TP1_StructList;
        private System.Windows.Forms.TabPage TP1_EventPackets;
        private System.Windows.Forms.ToolStripMenuItem ST_Help;
        private System.Windows.Forms.ListBox PacketClassList;
        private System.Windows.Forms.ListBox PacketClassMembersList;
        private System.Windows.Forms.GroupBox ToolGroupBox;
        private System.Windows.Forms.TabControl ToolTab;
        private System.Windows.Forms.TabPage Class_Create;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ClassNameTextBox;
        private System.Windows.Forms.Button ClassCreateButton;
        private System.Windows.Forms.ListBox NewClassMemberList;
        private System.Windows.Forms.Button AddCreateClassMember;
        private System.Windows.Forms.Button ClassDeleteButton;
        private System.Windows.Forms.Button AddPacketClassMember;
        private System.Windows.Forms.Button RemovePacketClassMember;
        private System.Windows.Forms.Button StructDeleteButton;
        private System.Windows.Forms.Button AddPacketStructMember;
        private System.Windows.Forms.Button RemovePacketStructMember;
        private System.Windows.Forms.ListBox PacketStructMembersList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button AddMemberNewStruct;
        private System.Windows.Forms.ListBox NewStructMemberList;
        private System.Windows.Forms.Button CreateStruct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox StructNameTextBox;
        private System.Windows.Forms.ListBox PacketStructList;
        private System.Windows.Forms.Button NewClassMemberDelete;
        private System.Windows.Forms.Button NewStructMemberRemove;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem 설정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ST_TagSetting;
        private System.Windows.Forms.ToolStripMenuItem ST_ServerProjectsSetting;
        private System.Windows.Forms.TabControl ProjectPacketTabs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox NewPacket;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NewPacketNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NewPacketIDTextBox;
        private System.Windows.Forms.Button AllResetNewPacketDatas;
        private System.Windows.Forms.ListBox NewPacketMemberList;
        private System.Windows.Forms.Button NewPacketMemberResetButton;
        private System.Windows.Forms.Button NewPacketMemberRemoveButton;
        private System.Windows.Forms.Button NewPacketMemberAddButton;
        private System.Windows.Forms.Button NewPacketAddButton;
    }
}

