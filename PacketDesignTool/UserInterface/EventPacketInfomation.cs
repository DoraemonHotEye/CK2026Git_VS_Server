using PacketDesignTool.Managers;
using PacketDesignTool.Managers.Parents;
using PacketDesignTool.Managers.Supports;
using PacketDesignTool.SubWinformConfs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketDesignTool.UserInterface
{
    public partial class EventPacketInfomation : UserControl
    {
        public ProjectPacketPacker myPacker;

        public EventPacketInfomation()
        {
            InitializeComponent();
        }

        private void EventPacketInfomation_Load(object sender, EventArgs e)
        {
            var enumStrings = Enum.GetNames(typeof(PROTOCOL_MSG_TYPE));

            foreach (var enumString in enumStrings)
            {
                ProtocolComboBox.Items.Add(enumString);
            }

            ProtocolComboBox.Enabled = false;
        }

        public void RefreshPacketList(ProjectPacketPacker packer)
        {
            myPacker = packer;

            EventPacketList.Items.Clear();
            foreach (EventPacketPacker epacker in myPacker.Packers.Values)
            {
                EventPacketList.Items.Add(epacker.packer.Name);
            }
        }

        private void EventPacketList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PacketIDTextBox.Text = "";

            EventPacketMemberList.Items.Clear();

            ProtocolComboBox.Enabled = false;

            if (EventPacketList.SelectedIndex == -1)
                return;

            ProtocolComboBox.Enabled = true;

            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            EventPacketPacker evpacker = myPacker.Packers[Key];
            Packer packer = evpacker.packer;

            PacketIDTextBox.Text = myPacker.Packers[Key].PacketID.ToString();

            for (int i = 0; i < packer.MemberTypes.Count; ++i)
            {
                EventPacketMemberList.Items.Add(packer.GetViewStringMember(i));
            }

            ProtocolComboBox.SelectedIndex = (int)evpacker.MProtoType;

            RefreshLinkedList();
        }

        private void EventPacketAddMember_Click(object sender, EventArgs e)
        {
            if (EventPacketList.SelectedIndex == -1)
            {
                MessageBox.Show("이벤트 패킷을 1개를 선택하셔야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            AddMemberSupportsTool addMemberSupportsTool = new AddMemberSupportsTool();
            addMemberSupportsTool.OnAddMemberEvent += new AddMemberSupportsTool.OnEventAddMemberSupportsTool(AddEventPacketMember);
            addMemberSupportsTool.ShowDialog();
        }

        private void EventPacketMemberRemove_Click(object sender, EventArgs e)
        {
            if (EventPacketList.SelectedIndex == -1)
            {
                MessageBox.Show("이벤트 패킷을 1개를 선택하셔야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (EventPacketMemberList.SelectedIndex == -1)
            {
                MessageBox.Show("이벤트 패킷을 1개를 선택하셔야합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            myPacker.Packers[Key].packer.MemberNames.RemoveAt(EventPacketMemberList.SelectedIndex);
            myPacker.Packers[Key].packer.MemberTypes.RemoveAt(EventPacketMemberList.SelectedIndex);

            EventPacketList_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void AddEventPacketMember(object sender, object MemberRecv)
        {
            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            Packer packer = myPacker.Packers[Key].packer;

            string[] splitmember = MemberRecv.ToString().Split('=');

            packer.MemberTypes.Add(splitmember[0]);
            packer.MemberNames.Add(splitmember[1]);

            EventPacketList_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void AddLinkedProject_Click(object sender, EventArgs e)
        {
            if (EventPacketList.SelectedIndex == -1)
            {
                return;
            }

            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            Packer packer = myPacker.Packers[Key].packer;

            AddLinkedProfiler linker = new AddLinkedProfiler();
            linker.ShowDialog(myPacker.Packers[Key], myPacker.profiler);

            RefreshLinkedList();
        }

        private void RefreshLinkedList()
        {
            LinkedList.Items.Clear();

            if (EventPacketList.SelectedIndex == -1)
                return;

            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            EventPacketPacker packer = myPacker.Packers[Key];

            foreach(var item in packer.LinkedProfilers)
            {
                LinkedList.Items.Add(item.Name);
            }
        }

        private void EventPacketRemove_Click(object sender, EventArgs e)
        {
            if (EventPacketList.SelectedIndex == -1)
            {
                return;
            }

            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            EventPacketPacker packer = myPacker.Packers[Key];

            EventPacketManager.Inst.RemovePacker(packer, myPacker.profiler.Name);

            RefreshPacketList(myPacker);
            EventPacketList_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void ProtocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            EventPacketPacker evpacker = myPacker.Packers[Key];

            if (evpacker != null)
            {
                evpacker.MProtoType = (PROTOCOL_MSG_TYPE)ProtocolComboBox.SelectedIndex;
            }
        }

        private void RemoveLink_Click(object sender, EventArgs e)
        {
            if (LinkedList.SelectedIndex == -1)
                return;

            string Key = (ToolStaticLibrary.GetHashSCCode(EventPacketList.SelectedItem.ToString(), myPacker.profiler) + "_" + EventPacketList.SelectedItem.ToString()).ToUpper();

            EventPacketPacker packer = myPacker.Packers[Key];

            var profiler = SettingsManager.Inst.SettingProfilers[LinkedList.SelectedItem.ToString()];
            ProjectSettingProfiler myProfiler = myPacker.profiler;

            EventPacketManager.Inst.RemoveLinkProfiler(myProfiler.Name, packer.PacketID, profiler, this);

            RefreshLinkedList();
        }
    }
}
