using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using LUOBO.BLL;
using LUOBO.Entity;
using LUOBO.ServiceManage.Model;

namespace LUOBO.ServiceManage
{
    public partial class MainForm : Form
    {
        BLL_SYS_ORGANIZATION oBll = new BLL_SYS_ORGANIZATION();
        BLL_APManage apBll = new BLL_APManage();
        List<Alarm> aList = new List<Alarm>();
        List<AlarmAP> aApList = new List<AlarmAP>();
        List<SYS_ORGANIZATION> oList = new List<SYS_ORGANIZATION>();
        List<SYS_AP_VIEW> aporgList = new List<SYS_AP_VIEW>();
        string path = System.Environment.CurrentDirectory;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oList = oBll.Select().OrgList;
            aporgList = apBll.GetAllAPAndORGList();
            ReadAlarmXml();
            ReadAlarmAPXml();
            InitAlarm();
            InitOrganization();
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

        private void ReadAlarmAPXml()
        {
            aApList = new List<AlarmAP>();
            AlarmAP aAp = null;
            XmlDocument xd = new XmlDocument();
            xd.Load(path + "\\AlarmAP.xml");
            XmlNodeList nodeList = xd.SelectNodes("Root//APList//AP");
            foreach (XmlNode item in nodeList)
            {
                aAp = new AlarmAP();
                aAp.ID = Convert.ToInt64(item["ID"].InnerText);
                aAp.OID = Convert.ToInt64(item["OID"].InnerText);
                aAp.Alarm = item.Attributes["Alarm"].Value;
                aAp.Alias = item["Alias"].InnerText;
                aAp.MAC = item["MAC"].InnerText;
                aAp.LAT = Convert.ToDouble(item["LAT"].InnerText);
                aAp.LON = Convert.ToDouble(item["LON"].InnerText);
                aApList.Add(aAp);
            }
        }

        private void InitAlarm()
        {
            cbAlarmGroup.DisplayMember = "Name";
            cbAlarmGroup.ValueMember = "Name";
            cbAlarmGroup.DataSource = aList.Where(c => c.Type == 1).ToList();
        }

        private void InitAlarmAP()
        {
            lbAlarmAPList.DisplayMember = "ALIAS";
            lbAlarmAPList.ValueMember = "ID";
            lbAlarmAPList.DataSource = aApList.Where(c => c.Alarm == cbAlarmGroup.SelectedValue.ToString()).ToList();
        }

        private void InitOrganization()
        {
            cbOrganization.DisplayMember = "NAME";
            cbOrganization.ValueMember = "ID";
            cbOrganization.DataSource = oList;
        }

        private void InitAP()
        {
            lbAPList.DisplayMember = "ALIAS";
            lbAPList.ValueMember = "ID";
            lbAPList.DataSource = aporgList.Where(c => c.OID == (Int64)cbOrganization.SelectedValue && aApList.Where(d => d.ID == c.ID).Count() == 0).ToList();
        }

        private void cbOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOrganization.SelectedIndex > -1)
                InitAP();
        }

        private void cbAlarmGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAlarmGroup.SelectedIndex > -1)
                InitAlarmAP();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要放弃此次操作?", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                oList = oBll.Select().OrgList;
                aporgList = apBll.GetAllAPAndORGList();
                ReadAlarmXml();
                ReadAlarmAPXml();
                InitAlarm();
                InitOrganization();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            XmlElement child = null;
            XmlDocument xd = new XmlDocument();
            xd.Load(path + "\\AlarmAP.xml");
            xd["Root"]["APList"].RemoveAll();
            XmlNode aplistNode = xd.SelectSingleNode("Root//APList");

            foreach (AlarmAP item in aApList)
            {
                child = xd.CreateElement("AP");
                child.SetAttribute("Alarm", item.Alarm);
                child.AppendChild(GetXmlElement(xd, "ID", item.ID.ToString()));
                child.AppendChild(GetXmlElement(xd, "OID", item.OID.ToString()));
                child.AppendChild(GetXmlElement(xd, "Alias", item.Alias));
                child.AppendChild(GetXmlElement(xd, "MAC", item.MAC));
                child.AppendChild(GetXmlElement(xd, "LAT", item.LAT.ToString()));
                child.AppendChild(GetXmlElement(xd, "LON", item.LON.ToString()));
                aplistNode.AppendChild(child);
            }

            xd.Save(path + "\\AlarmAP.xml");
        }

        private void btnAdds_Click(object sender, EventArgs e)
        {
            AlarmAP aAp = null;
            foreach (SYS_AP_VIEW item in lbAPList.SelectedItems)
            {
                aAp = new AlarmAP();
                aAp.ID = item.ID;
                aAp.OID = item.OID;
                aAp.Alarm = cbAlarmGroup.SelectedValue.ToString();
                aAp.Alias = item.ALIAS;
                aAp.MAC = item.MAC;
                aAp.LAT = item.LAT;
                aAp.LON = item.LON;
                aApList.Add(aAp);
            }
            InitAP();
            InitAlarmAP();
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            AlarmAP aAp = null;
            foreach (SYS_AP_VIEW item in lbAPList.DataSource as List<SYS_AP_VIEW>)
            {
                aAp = new AlarmAP();
                aAp.ID = item.ID;
                aAp.OID = item.OID;
                aAp.Alarm = cbAlarmGroup.SelectedValue.ToString();
                aAp.Alias = item.ALIAS;
                aAp.MAC = item.MAC;
                aAp.LAT = item.LAT;
                aAp.LON = item.LON;
                aApList.Add(aAp);
            }
            InitAP();
            InitAlarmAP();
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            foreach (AlarmAP item in lbAlarmAPList.DataSource as List<AlarmAP>)
                aApList.Remove(item);
            InitAP();
            InitAlarmAP();
        }

        private void btnDels_Click(object sender, EventArgs e)
        {
            foreach (AlarmAP item in lbAlarmAPList.SelectedItems)
                aApList.Remove(item);
            InitAP();
            InitAlarmAP();
        }

        private void btnSettingAlarm_Click(object sender, EventArgs e)
        {
            AlarmSettingForm f = new AlarmSettingForm();
            f.aList.AddRange(this.aList.ToArray());
            f.ShowDialog();
        }

        #region 其他方法
        private XmlElement GetXmlElement(XmlDocument doc, string elementName, string elementValue)
        {
            XmlElement element = doc.CreateElement(elementName);
            element.InnerText = elementValue;
            return element;
        }

        private XmlElement GetXmlElement(XmlDocument doc, string elementName, string elementValue, string attrName, string attrValue)
        {
            XmlElement element = GetXmlElement(doc, elementName, elementValue);
            if (attrName != null)
                element.SetAttribute(attrName, attrValue);
            return element;
        }
        #endregion
    }
}
