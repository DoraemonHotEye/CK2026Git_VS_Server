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

namespace PacketDesignTool.UserInterface
{
    public partial class TagInfomation : UserControl
    {
        public TagInfomation()
        {
            InitializeComponent();
        }

        private void TagInfomation_Load(object sender, EventArgs e)
        {

        }

        ProjectSettingProfiler MatchProfiler;

        public void SetNameTextBox(string name)
        {
            TagNameTextBox.Text = name;
        }

        public void SetSeedTextBox(ushort seed)
        {
            TagSeedTextBox.Text = seed.ToString();
        }

        public void SetServerType(TYPE type)
        {
            TagServerTypeTextBox.Text = type.ToString();
        }

        public void SetDescription(string description)
        {
            DescriptionTextBox.Text = description;
        }

        public void SendProfiler(ProjectSettingProfiler copyprofiler)
        {
            MatchProfiler = copyprofiler;

            var AllTypes = Enum.GetNames(typeof(EOutPutType));

            foreach (string type in AllTypes)
            {
                if (type != "NONE")
                    LanguagesBox.Items.Add(type);
            }

            if (MatchProfiler == null)
            {
                ProfilerNameTextBox.Text = "None";
                ProfilerPathTextBox.Text = "None";
                ProfilerDescriptionTextBox.Text = "None";
                LanguagesBox.SelectedIndex = 0;

                return;
            }

            ProfilerNameTextBox.Text = MatchProfiler.Name;
            ProfilerPathTextBox.Text = MatchProfiler.Path;
            ProfilerDescriptionTextBox.Text = MatchProfiler.Description;
            LanguagesBox.SelectedIndex = (int)MatchProfiler.Language;
        }

        private void LanguagesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MatchProfiler == null)
                return;

            if (LanguagesBox.SelectedIndex == -1)
                return;

            MatchProfiler.Language = (EOutPutType)LanguagesBox.SelectedIndex;
        }
    }
}
