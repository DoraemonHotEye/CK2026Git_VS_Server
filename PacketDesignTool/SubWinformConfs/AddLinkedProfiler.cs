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
    public partial class AddLinkedProfiler : Form
    {
        public AddLinkedProfiler()
        {
            InitializeComponent();
        }

        private void ST_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        EventPacketPacker myEvPacker;
        ProjectSettingProfiler myProfiler;

        public void ShowDialog(EventPacketPacker packer, ProjectSettingProfiler profiler)
        {
            myEvPacker = packer;
            myProfiler = profiler;

            this.ShowDialog();
        }

        private void AddLinkedProfiler_Load(object sender, EventArgs e)
        {
            foreach (var profiler in SettingsManager.Inst.SettingProfilers.Values)
            {
                ProjectList.Items.Add(profiler.Name);
            }
        }

        private void AddLinkerButton_Click(object sender, EventArgs e)
        {
            if (ProjectList.SelectedIndex == -1)
                return;

            var profiler = SettingsManager.Inst.SettingProfilers[ProjectList.SelectedItem.ToString()];

            EventPacketManager.Inst.AddLinkProfiler(myProfiler.Name, myEvPacker.PacketID, profiler, this);

            this.Close();
        }
    }
}
