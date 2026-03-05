using Newtonsoft.Json;
using PacketDesignTool.Managers;
using PacketDesignTool.Managers.Parents;
using PacketDesignTool.Managers.Supports;
using PacketDesignTool.SubWinformConfs;
using PacketDesignTool.UserInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketDesignTool
{
    public partial class PacketDesignForm : Form
    {
        public PacketDesignForm()
        {
            InitializeComponent();

        }

        private void ST_CloseStrip_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ST_Help_Click(object sender, EventArgs e)
        {
            HelpDescriptionForm form = new HelpDescriptionForm();
            form.ShowDialog();
        }

        private void ClassCreateButton_Click(object sender, EventArgs e)
        {
            if(ClassNameTextBox.Text.Length < 1)
            {
                MessageBox.Show("클래스 이름을 지정해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewClassMemberList.Items.Count < 1)
            {
                MessageBox.Show("멤버가 하나도 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewClassMemberList.Items.Count > 0)
            {
                string[] MemberTypes = new string[NewClassMemberList.Items.Count];
                string[] MemberNames = new string[NewClassMemberList.Items.Count];

                for (int i = 0; i < NewClassMemberList.Items.Count; ++i)
                {
                    string AllText = NewClassMemberList.Items[i].ToString();
                    string[] sliced = NewClassMemberList.Items[i].ToString().Split('=');

                    MemberTypes[i] = sliced[0];
                    MemberNames[i] = sliced[1];
                }

                //TODO 나중에 바뀌어야 함 예시 멤버타입과 멤버 이름을 분해하는 작업
                ClassPackerManager.Inst.EnterList(ClassNameTextBox.Text, MemberTypes, MemberNames);
            }
            else
            {
                ClassPackerManager.Inst.EnterList(ClassNameTextBox.Text, null, null);
            }

                ClassNameTextBox.Text = "";
            NewClassMemberList.Items.Clear();
        }


        private void CreateStruct_Click(object sender, EventArgs e)
        {
            if (StructNameTextBox.Text.Length < 1)
            {
                MessageBox.Show("클래스 이름을 지정해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewStructMemberList.Items.Count < 1)
            {
                MessageBox.Show("멤버가 하나도 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewStructMemberList.Items.Count > 0)
            {
                string[] MemberTypes = new string[NewStructMemberList.Items.Count];
                string[] MemberNames = new string[NewStructMemberList.Items.Count];

                for (int i = 0; i < NewStructMemberList.Items.Count; ++i)
                {
                    string AllText = NewStructMemberList.Items[i].ToString();
                    string[] sliced = NewStructMemberList.Items[i].ToString().Split('=');

                    MemberTypes[i] = sliced[0];
                    MemberNames[i] = sliced[1];
                }

                //TODO 나중에 바뀌어야 함 예시 멤버타입과 멤버 이름을 분해하는 작업
                StructPackerManager.Inst.EnterList(StructNameTextBox.Text, MemberTypes, MemberNames);
            }
            else
            {
                StructPackerManager.Inst.EnterList(StructNameTextBox.Text, null, null);
            }

            StructNameTextBox.Text = "";
            NewStructMemberList.Items.Clear();
        }

        private void AddClassPacker(object sender, Packer packer)
        {
            PacketClassList.Items.Add(packer.Name);
        }

        private void AddStructPacker(object sender, Packer packer)
        {
            PacketStructList.Items.Add(packer.Name);
        }

        private void AddCreateClassMember_Click(object sender, EventArgs e)
        {
            AddMemberSupportsTool toolWindow = new AddMemberSupportsTool();

            //EventsMapping
            toolWindow.OnAddMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(AddNewClassMember);
            toolWindow.OnRemoveMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(RemoveNewClassMember);

            toolWindow.ShowDialog();
        }

        private void AddNewClassMember(object sender, object Member)
        {
            NewClassMemberList.Items.Add((string)Member);
        }

        private void RemoveNewClassMember(object sender, object Member)
        {
            NewClassMemberList.Items.Remove((string)Member);
        }

        private void AddMemberNewStruct_Click(object sender, EventArgs e)
        {
            AddMemberSupportsTool toolWindow = new AddMemberSupportsTool();

            //EventsMapping
            toolWindow.OnAddMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(AddNewStructMember);
            toolWindow.OnRemoveMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(RemoveNewStructMember);

            toolWindow.ShowDialog();
        }


        private void AddNewStructMember(object sender, object Member)
        {
            NewStructMemberList.Items.Add((string)Member);
        }

        private void RemoveNewStructMember(object sender, object Member)
        {
            NewStructMemberList.Items.Remove((string)Member);
        }

        private void NewClassMemberDelete_Click(object sender, EventArgs e)
        {
            if (NewClassMemberList.SelectedIndex != -1)
            {
                NewClassMemberList.Items.Remove(NewClassMemberList.SelectedItems[0]);
            }
        }

        private void PacketDesignForm_Load(object sender, EventArgs e)
        {
            //OnEnable
            ClassPackerManager.Inst.OnEnable(this, "null");
            StructPackerManager.Inst.OnEnable(this, "null");
            EventPacketManager.Inst.OnEnable(this, "null");
            PackerGeneraterManager.Inst.OnEnable(this, "null");
            PackerImportManager.Inst.OnEnable(this, "null");
            SettingsManager.Inst.OnEnable(this, "null");
            TagManager.Inst.OnEnable(this, "null");

            ClassPackerManager.Inst.OnEnterCompeleted += new PackerManager.OnPackerEventhandler(AddClassPacker);
            StructPackerManager.Inst.OnEnterCompeleted += new PackerManager.OnPackerEventhandler(AddStructPacker);

            PackerImportManager.Inst.onPackerClassImportHandler += new PackerImportManager.OnPackerImportPackerHandler(ClassPackerManager.Inst.OnImport);
            PackerImportManager.Inst.onPackerStructImportHandler += new PackerImportManager.OnPackerImportPackerHandler(StructPackerManager.Inst.OnImport);
            PackerImportManager.Inst.onSettingsImportHandler += new PackerImportManager.OnPackerImportPackerHandler(SettingsManager.Inst.OnImport);
            PackerImportManager.Inst.onTagsImportHandler += new PackerImportManager.OnPackerImportPackerHandler(TagManager.Inst.OnImport);
            PackerImportManager.Inst.onEventPacketImportHandler += new PackerImportManager.OnPackerImportPackerHandler(EventPacketManager.Inst.OnImport);

            //LoadDefaultServerProfilerPackets(this, EventArgs.Empty);
            RefreshServerProfilerTabPages(this, EventArgs.Empty);
        }

        private void LoadDefaultServerProfilerPackets(object sender, EventArgs args)
        {
            myPacketInfomations.Clear();
            ProjectPacketTabs.TabPages.Clear();

            foreach(var project in SettingsManager.Inst.SettingProfilers.Values)
            {
                AddInfomationProfiler(project);
            }
        }

        Dictionary<string, EventPacketInfomation> myPacketInfomations = new Dictionary<string, EventPacketInfomation>();


        private void RefreshServerProfilerTabPages(object sender, EventArgs args)
        {
            myPacketInfomations.Clear();
            ProjectPacketTabs.TabPages.Clear();

            foreach (var project in EventPacketManager.Inst.ProjectPackets.Values)
            {
                AddInfomationCompeleted(this, project);
            }
        }

        private void AddInfomationProfiler(ProjectSettingProfiler Profiler, List<Packer> packers = null)
        {
            EventPacketManager.Inst.Enter(Profiler, packers, this, new EventPacketManager.OnEventPacketHandler(AddInfomationCompeleted), 
                new EventPacketManager.OnEventPacketHandler((o, p)=>
                {
                    MessageBox.Show("알 수 없는 오류", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
        }
        private void AddInfomationCompeleted(object sender, ProjectPacketPacker packer)
        {
            EventPacketInfomation packetInfomation = new EventPacketInfomation();

            ProjectPacketTabs.TabPages.Add(packer.profiler.Name, packer.profiler.Name);
            TabPage tapPage = ProjectPacketTabs.TabPages[packer.profiler.Name];

            myPacketInfomations.Add(packer.profiler.Name, packetInfomation);

            packetInfomation.myPacker = packer;
            packetInfomation.Dock = DockStyle.Fill;

            tapPage.Controls.Add(packetInfomation);
        }

        private void PacketClassList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PacketClassMembersList.Items.Clear();

            if (PacketClassList.SelectedIndex == -1)
                return;

            var item = ClassPackerManager.Inst.PackerList[PacketClassList.SelectedItems[0].ToString()];
            for (int i = 0; i < item.MemberTypes.Count; ++i)
            {
                string AllText = item.GetViewStringMember(i);
                PacketClassMembersList.Items.Add(AllText);
            }
        }

        private void PacketStructList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PacketStructMembersList.Items.Clear();

            if (PacketStructList.SelectedIndex == -1)
                return;

            var item = StructPackerManager.Inst.PackerList[PacketStructList.SelectedItems[0].ToString()];
            for (int i = 0; i < item.MemberTypes.Count; ++i)
            {
                string AllText = item.GetViewStringMember(i);
                PacketStructMembersList.Items.Add(AllText);
            }
        }

        private void RemovePacketClassMember_Click(object sender, EventArgs e)
        {
            if (PacketClassList.SelectedItems.Count < 1)
            {
                MessageBox.Show("클래스를 먼저 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PacketClassMembersList.SelectedItems.Count < 1)
            {
                MessageBox.Show("멤버를 먼저 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string Key = PacketClassList.SelectedItem.ToString();
            ClassPackerManager.Inst[PacketClassList.SelectedItem.ToString()].OnRemoveMember += new Packer.OnPackerMemberEventHandler(RemoveClassMember);

            ClassPackerManager.Inst.RemovePackerMemberAt(Key, PacketClassMembersList.SelectedIndex);
        }


        private void RemovePacketStructMember_Click(object sender, EventArgs e)
        {
            if (PacketStructList.SelectedItems.Count < 1)
            {
                MessageBox.Show("구조체를 먼저 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PacketStructMembersList.SelectedItems.Count < 1)
            {
                MessageBox.Show("멤버를 먼저 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string Key = PacketStructList.SelectedItem.ToString();
            StructPackerManager.Inst[PacketStructList.SelectedItem.ToString()].OnRemoveMember += new Packer.OnPackerMemberEventHandler(RemoveStructMember);

            StructPackerManager.Inst.RemovePackerMemberAt(Key, PacketStructMembersList.SelectedIndex);
        }

        private void RemoveClassMember(object sender, Packer packer, int index)
        {
            PacketClassMembersList.Items.RemoveAt(index);
        }

        private void RemoveStructMember(object sender, Packer packer, int index)
        {
            PacketStructMembersList.Items.RemoveAt(index);
        }

        private void AddPacketClassMember_Click(object sender, EventArgs e)
        {
            if (PacketClassList.SelectedItems.Count < 1)
            {
                MessageBox.Show("클래스를 먼저 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddMemberSupportsTool toolWindow = new AddMemberSupportsTool();
            toolWindow.OnAddMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(AddCurrentClassMember);
            toolWindow.ShowDialog();
        }

        private void AddPacketStructMember_Click(object sender, EventArgs e)
        {
            if (PacketStructList.SelectedItems.Count < 1)
            {
                MessageBox.Show("구조체를 먼저 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddMemberSupportsTool toolWindow = new AddMemberSupportsTool();
            toolWindow.OnAddMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(AddCurrentStructMemeber);
            toolWindow.ShowDialog();
        }

        private void AddCurrentClassMember(object sender, object Member)
        {
            string AllText = (string)Member;
            string[] sliced = AllText.Split('=');

            string MemberType = sliced[0];
            string MemberName = sliced[1];

            ClassPackerManager.Inst.EnterMember(PacketClassList.SelectedItem.ToString(), MemberType, MemberName);

            PacketClassMembersList.Items.Add((string)Member);
        }

        private void AddCurrentStructMemeber(object sender, object Member)
        {
            string AllText = (string)Member;
            string[] sliced = AllText.Split('=');

            string MemberType = sliced[0];
            string MemberName = sliced[1];

            StructPackerManager.Inst.EnterMember(PacketStructList.SelectedItem.ToString(), MemberType, MemberName);

            PacketStructMembersList.Items.Add((string)Member);
        }

        private void ST_File_SavePackets_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON Files (*.json)|*.json";
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                PackerGeneraterManager.Inst.Generate(saveFileDialog.FileName);
            }
        }

        private void ST_File_LoadPackets_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "JSON Files (*.json)|*.json";
            DialogResult result = fileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                AllClear();

                PackerImportManager.Inst.Import(fileDialog.FileName);

                RefreshServerProfilerTabPages(this, EventArgs.Empty);
                RefreshEventPacketAllPages(this, EventArgs.Empty);
            }
        }

        public void AllClear()
        {
            ClassPackerManager.Inst.PackerList.Clear();
            StructPackerManager.Inst.PackerList.Clear();

            PacketClassList.Items.Clear();
            PacketClassMembersList.Items.Clear();

            NewClassMemberList.Items.Clear();
        }

        private void ST_TagSetting_Click(object sender, EventArgs e)
        {
            TagSettingsForm settingsForm = new TagSettingsForm();
            settingsForm.ShowDialog();
        }

        private void ST_ServerProjectsSetting_Click(object sender, EventArgs e)
        {
            ServerProjectSettings projectSettings = new ServerProjectSettings();
            projectSettings.OnExitHandler += new ServerProjectSettings.OnServerProjectSettingHandler(RefreshServerProfilerTabPages);
            DialogResult result = projectSettings.ShowDialog();

            RefreshServerProfilerTabPages(this, EventArgs.Empty);
            RefreshEventPacketAllPages(this, EventArgs.Empty);
        }

        private void PacketDesignForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClassPackerManager.Inst.OnDisable(this, "null");
            StructPackerManager.Inst.OnDisable(this, "null");
            EventPacketManager.Inst.OnDisable(this, "null");
            PackerGeneraterManager.Inst.OnDisable(this, "null");
            PackerImportManager.Inst.OnDisable(this, "null");
            SettingsManager.Inst.OnDisable(this, "null");
            TagManager.Inst.OnDisable(this, "null");
        }

        private void ClassDeleteButton_Click(object sender, EventArgs e)
        {
            if (PacketClassList.SelectedIndex == -1)
            {
                MessageBox.Show("클래스를 선택 하십시오.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            bool isContain = 
                (StructPackerManager.Inst.isContainWithType(PacketClassList.SelectedItem.ToString()) ||
                ClassPackerManager.Inst.isContainWithType(PacketClassList.SelectedItem.ToString()));
            
            if (isContain)
            {
                DialogResult result = MessageBox.Show("결속된 클래스가 어딘가에 있습니다.\n삭제하실 경우 결속된 정보를 전부 삭제합니다.", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            ClassPackerManager.Inst.RemoveAllTypeMatch(PacketClassList.SelectedItem.ToString());
            StructPackerManager.Inst.RemoveAllTypeMatch(PacketClassList.SelectedItem.ToString());
            ClassPackerManager.Inst.Remove(PacketClassList.SelectedItem.ToString());

            RefreshAllMemberList();
        }

        private void StructDeleteButton_Click(object sender, EventArgs e)
        {
            if (PacketStructList.SelectedIndex == -1)
            {
                MessageBox.Show("구조체를 선택 하십시오.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            bool isContain =
                (StructPackerManager.Inst.isContainWithType(PacketStructList.SelectedItem.ToString()) ||
                ClassPackerManager.Inst.isContainWithType(PacketStructList.SelectedItem.ToString()));

            if (isContain)
            {
                DialogResult result = MessageBox.Show("결속된 구조체가 어딘가에 있습니다.\n삭제하실 경우 결속된 정보를 전부 삭제합니다.", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            ClassPackerManager.Inst.RemoveAllTypeMatch(PacketStructList.SelectedItem.ToString());
            StructPackerManager.Inst.RemoveAllTypeMatch(PacketStructList.SelectedItem.ToString());
            StructPackerManager.Inst.Remove(PacketStructList.SelectedItem.ToString());

            RefreshAllMemberList();
        }

        private void RefreshAllMemberList()
        {
            PacketClassList.Items.Clear();
            PacketClassMembersList.Items.Clear();

            PacketStructList.Items.Clear();
            PacketStructMembersList.Items.Clear();

            foreach(var Packer in ClassPackerManager.Inst.PackerList.Values)
            {
                PacketClassList.Items.Add(Packer.Name);
            }

            foreach (var Packer in StructPackerManager.Inst.PackerList.Values)
            {
                PacketStructList.Items.Add(Packer.Name);
            }
        }

        private void PacketMiscLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PacketMiscLists.SelectedIndex == -1)
                return;

        }

        private void ProjectPacketTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectPacketTabs.SelectedIndex == -1)
                return;

            NewPacketNameTextBox_TextChanged(sender, e);
        }

        private void NewPacketNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //MsgID에 대한 Enum타입 포맷을 자동으로 Preview한다.
            if (NewPacketNameTextBox.Text == "")
            {
                NewPacketIDTextBox.Text = "";
                return;
            }

            string SC_CODE = "[NONE]";

            if (ProjectPacketTabs.SelectedIndex != -1)
            {
                if (myPacketInfomations.TryGetValue(ProjectPacketTabs.SelectedTab.Name, out EventPacketInfomation info))
                    SC_CODE = ToolStaticLibrary.GetHashSCCode(NewPacketNameTextBox.Text, info.myPacker.profiler);
            }

            NewPacketIDTextBox.Text = SC_CODE += "_" + NewPacketNameTextBox.Text.ToUpper();
        }

        private void NewPacketMemberAddButton_Click(object sender, EventArgs e)
        {
            AddMemberSupportsTool addMemberSupportsTool = new AddMemberSupportsTool();
            addMemberSupportsTool.OnAddMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(AddNewEventPacketMember);
            addMemberSupportsTool.ShowDialog();
        }

        private void AddNewEventPacketMember(object sender, object Member)
        {
            NewPacketMemberList.Items.Add((string)Member);
        }

        private void NewPacketMemberRemoveButton_Click(object sender, EventArgs e)
        {
            if (NewPacketMemberList.SelectedIndex == -1)
                return;

            NewPacketMemberList.Items.Remove(NewPacketMemberList.SelectedItem);
        }

        private void NewPacketAddButton_Click(object sender, EventArgs e)
        {
            if (ProjectPacketTabs.SelectedIndex == -1)
            {
                return;
            }

            if (NewPacketIDTextBox.Text.Length < 3)
            {
                MessageBox.Show("패킷 이름을 적어야 합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewPacketMemberList.Items.Count < 1)
            {
                MessageBox.Show("멤버가 1개라도 있어야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EventPacketPacker packer = new EventPacketPacker();
            packer.PacketID = NewPacketIDTextBox.Text;
            packer.packer = new Packer();
            packer.packer.Name = NewPacketNameTextBox.Text;

            packer.packer.MemberNames = new List<string>();
            packer.packer.MemberTypes = new List<string>();

            foreach (var Member in NewPacketMemberList.Items)
            {
                string AllText = (string)Member.ToString();
                string[] sliced = AllText.Split('=');

                string MemberType = sliced[0];
                string MemberName = sliced[1];

                packer.packer.MemberTypes.Add(MemberType);
                packer.packer.MemberNames.Add(MemberName);
            }

            EventPacketManager.Inst.EnterPacker(packer, ProjectPacketTabs.SelectedTab.Name, this,
                new EventPacketManager.OnEventPacketHandler(OnEventPackerAddCompeleted),
                new EventPacketManager.OnEventPacketHandler((s, p) =>
                {
                    MessageBox.Show("패킷 추가 실패 알림", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
        }

        private void OnEventPackerAddCompeleted(object sender, ProjectPacketPacker packer)
        {
            if (myPacketInfomations.TryGetValue(packer.profiler.Name, out EventPacketInfomation info))
            {
                info.RefreshPacketList(packer);
            }

            NewPacketMemberList.Items.Clear();
            NewPacketNameTextBox.Text = "";
            NewPacketIDTextBox.Text = "";
        }

        private void RefreshEventPacketAllPages(object sender, EventArgs e)
        {
            foreach(var PPacker in EventPacketManager.Inst.ProjectPackets.Values)
            {
                if (myPacketInfomations.TryGetValue(PPacker.profiler.Name, out EventPacketInfomation info))
                {
                    info.RefreshPacketList(PPacker);
                }
            }
        }

        private void AllResetNewPacketDatas_Click(object sender, EventArgs e)
        {
            NewPacketNameTextBox.Text = "";
            NewPacketIDTextBox.Text = "";

            NewPacketMemberList.Items.Clear();
        }

        private void NewPacketMemberResetButton_Click(object sender, EventArgs e)
        {
            NewPacketMemberList.Items.Clear();
        }

        private void ST_File_GenerateCode_Click(object sender, EventArgs e)
        {
            PackerGeneraterManager.Inst.GenerateCode(EOutPutType.CSharp, "../../../ServerCore/Packets.cs");
        }
    }
}
