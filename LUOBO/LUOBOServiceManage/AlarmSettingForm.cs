using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LUOBO.ServiceManage.Model;
using System.Xml;

namespace LUOBO.ServiceManage
{
    public partial class AlarmSettingForm : Form
    {
        public List<Alarm> aList = new List<Alarm>();
        string path = System.Environment.CurrentDirectory;

        public AlarmSettingForm()
        {
            InitializeComponent();
        }

        private void AlarmSettingForm_Load(object sender, EventArgs e)
        {
            lbAlarm.DataSource = aList;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Alarm a = new Alarm();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (lbAlarm.SelectedIndex > -1)
            {
                foreach (Alarm item in aList)
                    item.Type = 1;

                ((Alarm)lbAlarm.SelectedItem).Type = 0;

                lbAlarm.DataSource = aList;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要放弃此次操作?", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                ReadAlarmXml();
            }
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {

        }

        private void ReadAlarmXml()
        {
            aList = new List<Alarm>();
            Alarm a = null;
            XmlDocument xd = new XmlDocument();
            xd.Load(path + "\\Alarm.xml");
            XmlNodeList nodeList = xd.SelectNodes("Root//Alarms//Alarm");
            foreach (XmlNode item in nodeList)
            {
                a = new Alarm();
                a.Name = item["Name"].InnerText;
                a.Remark = item["Remark"].InnerText;
                a.Time = Convert.ToInt64(item["Time"].InnerText);
                a.Unit = item["Time"].Attributes["Unit"].Value;
                a.Min = Convert.ToInt64(item["Min"].InnerText);
                a.Max = Convert.ToInt64(item["Max"].InnerText);
                a.STime = DateTime.Parse(item["STime"].InnerText);
                a.ETime = DateTime.Parse(item["ETime"].InnerText);
                a.Type = Convert.ToInt16(item.Attributes["Type"].Value);
                a.Mode = Convert.ToInt16(item.Attributes["Mode"].Value);
                aList.Add(a);
            }
        }
    }
}
