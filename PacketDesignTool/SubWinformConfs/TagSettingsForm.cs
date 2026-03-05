using PacketDesignTool.Managers;
using PacketDesignTool.UserInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PacketDesignTool.SubWinformConfs
{
    public partial class TagSettingsForm : Form
    {
        public TagSettingsForm()
        {
            InitializeComponent();
        }

        private void TagSettingsForm_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            TabTagsControll.TabPages.Clear();

            List<Tag> tags = TagManager.Inst.GetTagList();

            foreach (var tag in tags)
            {
                AddTagInfomation(tag);
            }
        }

        private void AddTagInfomation(Tag NewTag)
        {
            TagInfomation taginfomation = new TagInfomation();

            TabTagsControll.TabPages.Add(NewTag.Name, NewTag.Name, NewTag.Name);
            TabPage tapPage = TabTagsControll.TabPages[NewTag.Name];

            taginfomation.SendProfiler(SettingsManager.Inst.GetProfilerSchemaMatchWithTag(NewTag));

            taginfomation.SetNameTextBox(NewTag.Name);
            taginfomation.SetSeedTextBox(NewTag.ClientSeed);
            taginfomation.SetServerType(NewTag.Type);
            taginfomation.SetDescription(NewTag.Description);

            taginfomation.Dock = DockStyle.Fill;

            tapPage.Controls.Add(taginfomation);
        }

        private void ST_CreateTag_Click(object sender, EventArgs e)
        {
            AddTagForm addTagform = new AddTagForm();
            addTagform.onAddTagEventHandler += new AddTagForm.OnAddTagFormEventHandler(AddNewTag);
            addTagform.ShowDialog();
        }

        private void AddNewTag(object sender, Tag NewTag)
        {
            TagManager.Inst.EnterTag(NewTag, sender, 
                new TagManager.OnTagEnterEventHandler(OnEnterTagCompeleted), 
                new TagManager.OnTagEnterEventHandler((o, t)=>
                {
                    MessageBox.Show("태그 추가중 오류 발생", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
        }

        private void OnEnterTagCompeleted(object sender, Tag NewTag)
        {
            AddTagForm form = sender as AddTagForm;
            form.Close();

            AddTagInfomation(NewTag);
        }

        private void ST_DeleteTag_Click(object sender, EventArgs e)
        {
            DeleteTagForm deleteTagForm = new DeleteTagForm();
            deleteTagForm.OnDeletedTagHandler += new DeleteTagForm.OnDeleteTagFormHandler(OnDeletedCompeleted);
            deleteTagForm.SendTagList(TagManager.Inst.GetTagList());
            deleteTagForm.ShowDialog();
        }

        private void OnDeletedCompeleted(object sender, Tag tag)
        {
            Refresh();
        }
    }
}
