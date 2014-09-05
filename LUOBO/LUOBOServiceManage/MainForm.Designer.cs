namespace LUOBO.ServiceManage
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbOrganization = new System.Windows.Forms.ComboBox();
            this.lbAPList = new System.Windows.Forms.ListBox();
            this.lbAlarmAPList = new System.Windows.Forms.ListBox();
            this.cbAlarmGroup = new System.Windows.Forms.ComboBox();
            this.btnAdds = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnDelAll = new System.Windows.Forms.Button();
            this.btnDels = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnResetService = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSettingAlarm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备列表";
            // 
            // cbOrganization
            // 
            this.cbOrganization.FormattingEnabled = true;
            this.cbOrganization.Location = new System.Drawing.Point(14, 41);
            this.cbOrganization.Name = "cbOrganization";
            this.cbOrganization.Size = new System.Drawing.Size(148, 20);
            this.cbOrganization.TabIndex = 2;
            this.cbOrganization.SelectedIndexChanged += new System.EventHandler(this.cbOrganization_SelectedIndexChanged);
            // 
            // lbAPList
            // 
            this.lbAPList.FormattingEnabled = true;
            this.lbAPList.ItemHeight = 12;
            this.lbAPList.Location = new System.Drawing.Point(14, 68);
            this.lbAPList.Name = "lbAPList";
            this.lbAPList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAPList.Size = new System.Drawing.Size(148, 328);
            this.lbAPList.TabIndex = 3;
            // 
            // lbAlarmAPList
            // 
            this.lbAlarmAPList.FormattingEnabled = true;
            this.lbAlarmAPList.ItemHeight = 12;
            this.lbAlarmAPList.Location = new System.Drawing.Point(274, 68);
            this.lbAlarmAPList.Name = "lbAlarmAPList";
            this.lbAlarmAPList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAlarmAPList.Size = new System.Drawing.Size(148, 328);
            this.lbAlarmAPList.TabIndex = 4;
            // 
            // cbAlarmGroup
            // 
            this.cbAlarmGroup.FormattingEnabled = true;
            this.cbAlarmGroup.Location = new System.Drawing.Point(274, 41);
            this.cbAlarmGroup.Name = "cbAlarmGroup";
            this.cbAlarmGroup.Size = new System.Drawing.Size(148, 20);
            this.cbAlarmGroup.TabIndex = 5;
            this.cbAlarmGroup.SelectedIndexChanged += new System.EventHandler(this.cbAlarmGroup_SelectedIndexChanged);
            // 
            // btnAdds
            // 
            this.btnAdds.Location = new System.Drawing.Point(180, 99);
            this.btnAdds.Name = "btnAdds";
            this.btnAdds.Size = new System.Drawing.Size(75, 23);
            this.btnAdds.TabIndex = 6;
            this.btnAdds.Text = ">";
            this.btnAdds.UseVisualStyleBackColor = true;
            this.btnAdds.Click += new System.EventHandler(this.btnAdds_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(180, 160);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(75, 23);
            this.btnAddAll.TabIndex = 7;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnDelAll
            // 
            this.btnDelAll.Location = new System.Drawing.Point(180, 221);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(75, 23);
            this.btnDelAll.TabIndex = 8;
            this.btnDelAll.Text = "<<";
            this.btnDelAll.UseVisualStyleBackColor = true;
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // btnDels
            // 
            this.btnDels.Location = new System.Drawing.Point(180, 282);
            this.btnDels.Name = "btnDels";
            this.btnDels.Size = new System.Drawing.Size(75, 23);
            this.btnDels.TabIndex = 9;
            this.btnDels.Text = "<";
            this.btnDels.UseVisualStyleBackColor = true;
            this.btnDels.Click += new System.EventHandler(this.btnDels_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(266, 414);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnResetService
            // 
            this.btnResetService.Location = new System.Drawing.Point(347, 414);
            this.btnResetService.Name = "btnResetService";
            this.btnResetService.Size = new System.Drawing.Size(75, 23);
            this.btnResetService.TabIndex = 11;
            this.btnResetService.Text = "重启服务";
            this.btnResetService.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(185, 414);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 12;
            this.btnOpen.Text = "重新读取配置";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSettingAlarm
            // 
            this.btnSettingAlarm.Location = new System.Drawing.Point(326, 12);
            this.btnSettingAlarm.Name = "btnSettingAlarm";
            this.btnSettingAlarm.Size = new System.Drawing.Size(96, 23);
            this.btnSettingAlarm.TabIndex = 13;
            this.btnSettingAlarm.Text = "配置告警分组";
            this.btnSettingAlarm.UseVisualStyleBackColor = true;
            this.btnSettingAlarm.Click += new System.EventHandler(this.btnSettingAlarm_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 449);
            this.Controls.Add(this.btnSettingAlarm);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnResetService);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDels);
            this.Controls.Add(this.btnDelAll);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnAdds);
            this.Controls.Add(this.cbAlarmGroup);
            this.Controls.Add(this.lbAlarmAPList);
            this.Controls.Add(this.lbAPList);
            this.Controls.Add(this.cbOrganization);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "告警设置";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbOrganization;
        private System.Windows.Forms.ListBox lbAPList;
        private System.Windows.Forms.ListBox lbAlarmAPList;
        private System.Windows.Forms.ComboBox cbAlarmGroup;
        private System.Windows.Forms.Button btnAdds;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnDelAll;
        private System.Windows.Forms.Button btnDels;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnResetService;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSettingAlarm;
    }
}

