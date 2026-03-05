using PacketDesignTool.DefaultClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketDesignTool
{
    public partial class AddMemberSupportsTool : Form
    {
        public delegate void OnEventAddMemberSupportsTool(object sender, object object_);

        public OnEventAddMemberSupportsTool OnAddMemberEvent;
        public OnEventAddMemberSupportsTool OnRemoveMemberEvent;

        public AddMemberSupportsTool()
        {
            InitializeComponent();
        }

        private void MemberList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddMemberSupportsTool_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < StaticMembers.memberList.Length; ++i)
            {
                MemberList.Items.Add(StaticMembers.memberList[i]);
            }

            foreach(var member in ClassPackerManager.Inst.PackerList)
            {
                MemberList.Items.Add(member.Value.Name);
            }

            foreach (var member in StructPackerManager.Inst.PackerList)
            {
                MemberList.Items.Add(member.Value.Name);
            }
        }

        private void AddMember_Click(object sender, EventArgs e)
        {
            if (MemberList.SelectedItems.Count < 1)
            {
                MessageBox.Show("멤버타입은 선택하셔야합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (NewMemberNameTextBox.Text.Length < 1)
            {
                MessageBox.Show("멤버이름은 작성 하셔야 합니다..", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (OnListRadioButton.Checked == false && OnSingleRadioButton.Checked == false)
            {
                MessageBox.Show("알 수 없는 오류 RadioButtons", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string formatMember = @"{0}:{1}={2}";
            string formatMemberType = @"{0}";

            string format_CurrentType = "";

            if (OnListRadioButton.Checked == true)
            {
                format_CurrentType = String.Format(formatMemberType, "list");
            }
            else if (OnSingleRadioButton.Checked == true)
            {
                format_CurrentType = String.Format(formatMemberType, "single");
            }

            string CurrentFormat = String.Format(formatMember, format_CurrentType, MemberList.SelectedItems[0].ToString(), NewMemberNameTextBox.Text);

            if (OnAddMemberEvent != null)
                OnAddMemberEvent.Invoke(this, CurrentFormat);

            this.Close();
        }
    }
}
