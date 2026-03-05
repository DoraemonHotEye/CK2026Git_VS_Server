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

            if (MatchProfiler == null)
            {
                ProfilerNameTextBox.Text = "None";
                ProfilerPathTextBox.Text = "None";
                ProfilerDescriptionTextBox.Text = "None";

                return;
            }

            ProfilerNameTextBox.Text = MatchProfiler.Name;
            ProfilerPathTextBox.Text = MatchProfiler.Path;
            ProfilerDescriptionTextBox.Text = MatchProfiler.Description;
        }
    }
}
