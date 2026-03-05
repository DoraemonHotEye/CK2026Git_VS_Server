using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunTaskForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PathFind_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                ProgramPath.Text = dlg.FileName;
            }
        }

        private void Run_Click(object sender, EventArgs e)
        {
            string path = ProgramPath.Text;

            if (System.IO.File.Exists(path))
            {
                try
                {
                    for (int i = 0; i < int.Parse(RunCount.Text); ++i)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(path);
                        startInfo.UseShellExecute = true; // GUI 프로그램 실행 시 필요
                        Process.Start(startInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"프로그램 실행 중 오류 발생: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("유효한 실행 파일 경로를 선택해주세요.");
            }

        }
    }
}
