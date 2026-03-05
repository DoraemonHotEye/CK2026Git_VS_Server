using PacketDesignTool.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketDesignTool.SubWinformConfs
{
    public partial class DeleteTagForm : Form
    {

        public DeleteTagForm()
        {
            InitializeComponent();
        }

        public void SendTagList(List<Tag> AllTagList)
        {
            foreach (Tag Tag in AllTagList)
            {
                TagList.Items.Add(Tag.Name);
            }
        }

        private void DeleteTagForm_Load(object sender, EventArgs e)
        {

        }

        public delegate void OnDeleteTagFormHandler(object sender, Tag tag);

        public OnDeleteTagFormHandler OnDeletedTagHandler;

        private ProjectSettingProfiler CurrentProfiler;
        private Tag CurrentTag;

        private void TagList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectedDescriptionTextBox.Text = "";
            ConnectedNameTextBox.Text = "";
            ConnectedPathTextBox.Text = "";
            CurrentProfiler = null;

            if (TagList.SelectedIndex == -1)
                return;

            CurrentTag = TagManager.Inst.GetTag(TagList.SelectedItem.ToString());
            
            if (CurrentTag != null)
            {
                CurrentProfiler = SettingsManager.Inst.GetProfilerSchemaMatchWithTag(CurrentTag);
                if (CurrentProfiler != null)
                {
                    ConnectedNameTextBox.Text = CurrentProfiler.Name;
                    ConnectedDescriptionTextBox.Text = CurrentProfiler.Description;
                    ConnectedPathTextBox.Text = CurrentProfiler.Path;
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (TagList.SelectedIndex == -1)
            {
                MessageBox.Show("배열을 선택하십시오.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            DialogResult result = MessageBox.Show("정말 삭제 하시겠습니까?\n결속된 프로젝트가 있을 경우 자동 삭제됩니다.", "경고문", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (SettingsManager.Inst.isContainTagName(CurrentTag.Name))
                {
                    //TODO 결속 된 프로젝트 삭제
                    SettingsManager.Inst.Remove(CurrentProfiler.Name);
                }

                //TODO 태그 삭제
                if (TagManager.Inst.RemoveTag(CurrentTag.Name))
                {
                    //TODO 완료 이벤트 실행
                    if (OnDeletedTagHandler != null)
                        OnDeletedTagHandler.Invoke(this, CurrentTag);
                }

                this.Close();
            }
        }
    }
}
