using PacketDesignTool.Managers;
using PacketDesignTool.Managers.Supports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PacketDesignTool.SubWinformConfs
{
    public partial class ServerProjectSettings : Form
    {
        public delegate void OnServerProjectSettingHandler(object sender, EventArgs Message);

        public ServerProjectSettings()
        {
            InitializeComponent();
        }

        //프로파일링 규칙
        //1. ServerCore는 무조건 무시한다.
        //2. 문자열 안에 Client, Server가 붙으면 무조건 프로파일링 한다.

        public OnServerProjectSettingHandler OnEnterHandler;
        public OnServerProjectSettingHandler OnExitHandler;

        private void ServerProjectSettings_Load(object sender, EventArgs e)
        {
            ProjectInfo.Visible = false;

            if (OnEnterHandler != null)
                OnEnterHandler(this, EventArgs.Empty);

            Refresh();
        }

        private void LoadOpenDailogButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                PathTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void ProjectProfileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectProfileList.SelectedIndex == -1)
                return;

            PathTextBox.Text = ProjectProfileList.SelectedItems[0].ToString();
        }

        private void AddProject_Click(object sender, EventArgs e)
        {
            if (TagComboBox.SelectedIndex == -1 || PathTextBox.Text == "" || ProjectNameTextBox.Text == "")
            {
                MessageBox.Show("현재 설정값이 문제가 있습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            ProjectSettingProfiler profiler = new ProjectSettingProfiler(ProjectNameTextBox.Text, ProjectDescTextBox.Text, PathTextBox.Text, TagManager.Inst.GetTag(TagComboBox.SelectedItem.ToString()));

            //TODO 등록 완료 프로젝트 추가 코드
            SettingsManager.Inst.Enter(profiler, this, new SettingsManager.OnSettingManagerEventHandler(OnCompeletedAddProfiler), 
                new SettingsManager.OnSettingManagerEventHandler((o, p) =>
                {
                    MessageBox.Show("프로젝트 세팅 추가중 에러 발생", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
        }

        private void OnCompeletedAddProfiler(object sender, ProjectSettingProfiler profiler)
        {
            PathTextBox.Text = "";
            ProjectNameTextBox.Text = "";
            ProjectDescTextBox.Text = "";

            AddedCompeletedProjectsList.Items.Add(profiler.Name);

            EventPacketManager.Inst.Enter(profiler, null);

            Refresh();
        }

        private void Refresh()
        {
            ProjectProfileList.Items.Clear();
            AddedCompeletedProjectsList.Items.Clear();
            TagComboBox.Items.Clear();

            List<string> AllowPaths = PathProfilerSupportClass.GetProjectPaths();

            foreach (string folder in AllowPaths)
            {
                if (!SettingsManager.Inst.isContainPath(folder))
                    ProjectProfileList.Items.Add(folder);
            }

            //기본 등록이 완료된 프로젝트 전체 추가
            List<ProjectSettingProfiler> profilers = SettingsManager.Inst.GetLoadedProjectList();

            foreach (var profiler in profilers)
            {
                AddedCompeletedProjectsList.Items.Add(profiler.Name);
            }

            List<Tag> Tags = TagManager.Inst.GetTagList();

            foreach (var tag in Tags)
            {
                if (!SettingsManager.Inst.isContainTagName(tag.Name))
                    TagComboBox.Items.Add(tag.Name);
            }

            if (TagComboBox.Items.Count > 0)
            {
                TagComboBox.SelectedIndex = 0;
            }
        }

        private void ProjectSettingsRemove_Click(object sender, EventArgs e)
        {
            if (AddedCompeletedProjectsList.SelectedIndex == -1)
            {
                MessageBox.Show("프로젝트 선택 후 진행바랍니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("결속된 패킷이 있을 수 있습니다.\n정말 삭제 하시겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SettingsManager.Inst.Remove(AddedCompeletedProjectsList.SelectedItem.ToString(), this, new SettingsManager.OnSettingManagerEventHandler(OnDeletedCompeleted),
                    new SettingsManager.OnSettingManagerEventHandler((s, p) =>
                    {
                        MessageBox.Show("삭제 실패 에러", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
            }
        }

        private void OnDeletedCompeleted(object sender, ProjectSettingProfiler profiler)
        {
            AddedCompeletedProjectsList.Items.Remove(profiler.Name);

            Refresh();
        }

        private void ServerProjectSettings_Leave(object sender, EventArgs e)
        {
            if (OnExitHandler != null)
                OnExitHandler(this, EventArgs.Empty);
        }

        private void AddedCompeletedProjectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AddedCompeletedProjectsList.SelectedIndex == -1)
            {
                ProjectInfo.Visible = false;

                return;
            }

            ProjectInfo.Visible = true;

            var item = SettingsManager.Inst.SettingProfilers[AddedCompeletedProjectsList.SelectedItem.ToString()];

            PrjNameTextBox.Text = item.Name.ToString();
            PrjDescriptionTextBox.Text = item.Description.ToString();
            PrjPathTextBox.Text = item.Path;
            PrjTagTextBox.Text = item.tag.Type.ToString();
            isUseUDPCheckBox.Checked = item.isUseUDP;
        }

        private void isUseUDPCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            var item = SettingsManager.Inst.SettingProfilers[AddedCompeletedProjectsList.SelectedItem.ToString()];
            item.isUseUDP = isUseUDPCheckBox.Checked;
        }

        private void RePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var item = SettingsManager.Inst.SettingProfilers[AddedCompeletedProjectsList.SelectedItem.ToString()];

                if (item != null)
                {
                    item.Path = folderBrowserDialog.SelectedPath;

                    var MBresult = MessageBox.Show("정말로 경로를 변경하시겠습니까?", "설정", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (MBresult == DialogResult.Yes)
                    {
                        var MB2Result = MessageBox.Show("정말로요?", "설정", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                        if (MB2Result == DialogResult.Yes)
                        {
                            Refresh();
                        }
                    }
                }
            }
        }
    }
}
