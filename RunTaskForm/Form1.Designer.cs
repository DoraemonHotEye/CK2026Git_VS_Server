namespace RunTaskForm
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgramPath = new System.Windows.Forms.TextBox();
            this.PathFind = new System.Windows.Forms.Button();
            this.RunCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Run = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ProgramPath
            // 
            this.ProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgramPath.Location = new System.Drawing.Point(35, 54);
            this.ProgramPath.Name = "ProgramPath";
            this.ProgramPath.Size = new System.Drawing.Size(731, 32);
            this.ProgramPath.TabIndex = 0;
            // 
            // PathFind
            // 
            this.PathFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PathFind.Location = new System.Drawing.Point(782, 54);
            this.PathFind.Name = "PathFind";
            this.PathFind.Size = new System.Drawing.Size(106, 32);
            this.PathFind.TabIndex = 1;
            this.PathFind.Text = "...";
            this.PathFind.UseVisualStyleBackColor = true;
            this.PathFind.Click += new System.EventHandler(this.PathFind_Click);
            // 
            // RunCount
            // 
            this.RunCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RunCount.Location = new System.Drawing.Point(35, 107);
            this.RunCount.Name = "RunCount";
            this.RunCount.Size = new System.Drawing.Size(239, 32);
            this.RunCount.TabIndex = 2;
            this.RunCount.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Count";
            // 
            // Run
            // 
            this.Run.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Run.Location = new System.Drawing.Point(35, 165);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(853, 45);
            this.Run.TabIndex = 4;
            this.Run.Text = "구동";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 245);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RunCount);
            this.Controls.Add(this.PathFind);
            this.Controls.Add(this.ProgramPath);
            this.Name = "Form1";
            this.Text = "프로그램 구동기";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ProgramPath;
        private System.Windows.Forms.Button PathFind;
        private System.Windows.Forms.TextBox RunCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Run;
    }
}

