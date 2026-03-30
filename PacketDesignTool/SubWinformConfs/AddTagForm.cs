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
    public partial class AddTagForm : Form
    {
        public delegate void OnAddTagFormEventHandler(object sender, Tag CreatedTag);

        public AddTagForm()
        {
            InitializeComponent();
        }

        private void ST_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public OnAddTagFormEventHandler onAddTagEventHandler;

        private void TagCreateButton_Click(object sender, EventArgs e)
        {
            if (TagNameTextBox.Text.Length < 1 || TagTypeDropDown.SelectedIndex == -1)
            {
                MessageBox.Show("태그 만들기중 오류 지점이 있습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Tag NewTag = new Tag(TagNameTextBox.Text, (TYPE)TagTypeDropDown.SelectedIndex + 1, 0, TagDescriptionTextBox.Text);
        
            if (onAddTagEventHandler != null)
            {
                onAddTagEventHandler.Invoke(this, NewTag);
            }
        }

        private void AddTagForm_Load(object sender, EventArgs e)
        {
            string[] AllTypes = Enum.GetNames(typeof(TYPE));

            foreach (string type in AllTypes)
            {
                if (type != "NONE")
                    TagTypeDropDown.Items.Add(type);
            }

            AllTypes = Enum.GetNames(typeof(EOutPutType));

            foreach (string type in AllTypes)
            {
                if (type != "NONE")
                    LanguagesBox.Items.Add(type);
            }

            if (TagTypeDropDown.Items.Count > 0)
            {
                TagTypeDropDown.SelectedIndex = 0;
            }

            if (LanguagesBox.Items.Count > 0)
            {
                LanguagesBox.SelectedIndex = 0;
            }
        }
    }
}
