namespace LUOBO.ServiceManage
{
    partial class AlarmSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbAlarm = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.txtMax = new System.Windows.Forms.TextBox();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.lbName = new System.Windows.Forms.Label();
            this.lbRemark = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.lbUnit = new System.Windows.Forms.Label();
            this.lbMin = new System.Windows.Forms.Label();
            this.lvMax = new System.Windows.Forms.Label();
            this.lbType = new System.Windows.Forms.Label();
            this.lbMode = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbAlarm
            // 
            this.lbAlarm.FormattingEnabled = true;
            this.lbAlarm.ItemHeight = 12;
            this.lbAlarm.Location = new System.Drawing.Point(12, 36);
            this.lbAlarm.Name = "lbAlarm";
            this.lbAlarm.Size = new System.Drawing.Size(154, 232);
            this.lbAlarm.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "分组列表";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(174, 245);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "重新读配置";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(336, 245);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(193, 60);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(121, 21);
            this.txtName.TabIndex = 4;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(363, 60);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(121, 21);
            this.txtRemark.TabIndex = 5;
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(193, 108);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(121, 21);
            this.txtTime.TabIndex = 6;
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(193, 156);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(121, 21);
            this.txtMin.TabIndex = 7;
            // 
            // txtMax
            // 
            this.txtMax.Location = new System.Drawing.Point(363, 155);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(121, 21);
            this.txtMax.TabIndex = 8;
            // 
            // cbUnit
            // 
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(363, 108);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(121, 20);
            this.cbUnit.TabIndex = 9;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(193, 204);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 20);
            this.cbType.TabIndex = 10;
            // 
            // cbMode
            // 
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(363, 203);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(121, 20);
            this.cbMode.TabIndex = 11;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(255, 245);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 12;
            this.btnDefault.Text = "设为默认";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(191, 45);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(29, 12);
            this.lbName.TabIndex = 13;
            this.lbName.Text = "名称";
            // 
            // lbRemark
            // 
            this.lbRemark.AutoSize = true;
            this.lbRemark.Location = new System.Drawing.Point(361, 45);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(29, 12);
            this.lbRemark.TabIndex = 14;
            this.lbRemark.Text = "描述";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Location = new System.Drawing.Point(191, 93);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(53, 12);
            this.lbTime.TabIndex = 15;
            this.lbTime.Text = "时间间隔";
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Location = new System.Drawing.Point(361, 93);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(53, 12);
            this.lbUnit.TabIndex = 16;
            this.lbUnit.Text = "时间单位";
            // 
            // lbMin
            // 
            this.lbMin.AutoSize = true;
            this.lbMin.Location = new System.Drawing.Point(191, 141);
            this.lbMin.Name = "lbMin";
            this.lbMin.Size = new System.Drawing.Size(41, 12);
            this.lbMin.TabIndex = 17;
            this.lbMin.Text = "最小值";
            // 
            // lvMax
            // 
            this.lvMax.AutoSize = true;
            this.lvMax.Location = new System.Drawing.Point(361, 140);
            this.lvMax.Name = "lvMax";
            this.lvMax.Size = new System.Drawing.Size(41, 12);
            this.lvMax.TabIndex = 18;
            this.lvMax.Text = "最大值";
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Location = new System.Drawing.Point(191, 189);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(29, 12);
            this.lbType.TabIndex = 19;
            this.lbType.Text = "类型";
            // 
            // lbMode
            // 
            this.lbMode.AutoSize = true;
            this.lbMode.Location = new System.Drawing.Point(361, 188);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(29, 12);
            this.lbMode.TabIndex = 20;
            this.lbMode.Text = "模式";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(417, 245);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Location = new System.Drawing.Point(390, 10);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(94, 23);
            this.btnSaveSetting.TabIndex = 22;
            this.btnSaveSetting.Text = "保存告警分组";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // AlarmSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 282);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbMode);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.lvMax);
            this.Controls.Add(this.lbMin);
            this.Controls.Add(this.lbUnit);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.lbRemark);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.cbUnit);
            this.Controls.Add(this.txtMax);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbAlarm);
            this.Name = "AlarmSettingForm";
            this.Text = "配置告警分组";
            this.Load += new System.EventHandler(this.AlarmSettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAlarm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.TextBox txtMax;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbRemark;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.Label lbMin;
        private System.Windows.Forms.Label lvMax;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSaveSetting;
    }
}