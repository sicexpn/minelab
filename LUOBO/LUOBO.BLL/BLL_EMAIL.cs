using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using LUOBO.Entity;
using LUOBO.DAL;
using LUOBO.Helper;

namespace LUOBO.BLL
{
    public class BLL_EMAIL
    {
        DAL_SYS_ORGANIZATION org_dal = new DAL.DAL_SYS_ORGANIZATION();
        DAL_SYS_APDEVICE ap_dal = new DAL.DAL_SYS_APDEVICE();
        DAL_SYS_SSID ssid_dal = new DAL_SYS_SSID();
        public void ADAuditEmail(AD_AUDIT audit)
        {
            string oname = "";
            if (audit.ORG_ID == 0)
                oname = "总部";
            else
                oname = org_dal.SelectONameByOID(audit.ORG_ID);
            SendEmail("service@next-wifi.com", "【审核提醒】" + oname + "机构申请了广告审核，请审核", "请同时检查是否有SSID需要审核", false);
        }

        public void SSIDAuditEmail(List<SYS_SSID> ssidList)
        {
            string oname = "";
            string apname = ap_dal.SelectALIASByID(ssidList.First().APID);
            if (ssidList.First().OID == 0)
                oname = "总部";
            else
                oname = org_dal.SelectONameByOID(ssidList.First().OID);
            
            List<SYS_SSID> old_ssid = ssid_dal.SelectByIDs(ssidList.ToString("ID",","));

            string body = "";

            foreach (var item in ssidList)
            {
                body += "【" + apname + "】设备的SSID名称从【" + old_ssid.Where(c => c.ID == item.ID).Select(c => c.NAME).First() + "】修改为【" + item.NAME + "】。\n";
            }

            SendEmail("service@next-wifi.com", "【审核提醒】" + oname + "机构申请了SSID审核，请审核", body, false);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sendTo">收件人</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="isBodyHtml">内容是否是Html</param>
        public void SendEmail(string sendTo, string title, string body, bool isBodyHtml)
        {
            NetworkCredential cred = new NetworkCredential("next_wifi@163.com", "wqyz@2014");
            MailMessage msg = new MailMessage();
            msg.Subject = title;
            msg.From = new MailAddress("next_wifi@163.com");
            msg.To.Add(sendTo);
            msg.Body = body;
            msg.IsBodyHtml = isBodyHtml;
            SmtpClient client = new SmtpClient("smtp.163.com", 25);
            client.Credentials = cred;
            client.Send(msg);
        }
    }
}
