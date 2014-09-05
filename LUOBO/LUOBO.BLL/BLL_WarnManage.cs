using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.DAL;
using LUOBO.Model;
using System.Transactions;
using System.Net;
using System.Net.Mail;

namespace LUOBO.BLL
{
    public class BLL_WarnManage
    {
        DAL_SYS_APDEVICE apdDAL = new DAL_SYS_APDEVICE();
        DAL_SYS_APORG aporg = new DAL_SYS_APORG();
        DAL_SYS_LOG_ALERT dal_sys_log_alert = new DAL_SYS_LOG_ALERT();
        DAL_SYS_LOG_APNEAR dal_apNear = new DAL_SYS_LOG_APNEAR();
        DAL_SYS_LOG_ALERTWHITELIST dal_whiteList = new DAL_SYS_LOG_ALERTWHITELIST();
        DAL_SYS_LOG_ALERTKEYWORD dal_keyWord = new DAL_SYS_LOG_ALERTKEYWORD();
        DAL_SYS_APCONTACT dal_apcontact = new DAL_SYS_APCONTACT();
        DAL_SYS_ORGANIZATION dal_org = new DAL_SYS_ORGANIZATION();
        //public List<SYS_LOG_APNEAR> getWarnList(Int64 org_id)
        //{
        //    List<SYS_APDEVICE> aplist = apdDAL.SelectApStateListByOID(org_id, "", "", "");
        //    List<SYS_LOG_APNEAR> result = new List<SYS_LOG_APNEAR>();

        //    return result;
        //}
        public List<M_Alert_Object> getWarnList(Int64 org_id)
        {
            //List<SYS_APDEVICE> aplist = apdDAL.SelectApStateListByOID(org_id, "", "", "");
            //List<SYS_LOG_ALERT> result = new List<SYS_LOG_ALERT>();
            return dal_apNear.Select(org_id);
        }
        public long? GetAlertCount(long oid)
        {
            return dal_sys_log_alert.GetAlertCounts(oid);
        }

        public List<M_Alert_Object> GetAlertListNotHandle(long oid)
        {
            return dal_sys_log_alert.GetAlertListNotHandle(oid);
        }

        public bool AddWhiteList(long oid, string MAC)
        {
            SYS_LOG_ALERTWHITELIST data = new SYS_LOG_ALERTWHITELIST();
            data.OID = oid;
            data.MAC = MAC;
            return dal_whiteList.Insert(data);
        }
        public bool DeleteWhiteList(long oid, string MAC)
        {
            SYS_LOG_ALERTWHITELIST data = new SYS_LOG_ALERTWHITELIST();
            data.OID = oid;
            data.MAC = MAC;
            return dal_whiteList.Delete(data);
        }
        public bool AddKeyWordList(long oid, string KeyWord)
        {
            SYS_LOG_ALERTKEYWORD data = new SYS_LOG_ALERTKEYWORD();
            data.OID = oid;
            data.KEYWORD = KeyWord;
            return dal_keyWord.Insert(data);
        }

        public bool DeleteKeyWordList(Int64 ID)
        {
            return dal_keyWord.Delete(ID);
        }

        /// <summary>
        /// 根据机构ID和mac地址获取所有的警告信息
        /// </summary>
        /// <param name="oid">机构id</param>
        /// <param name="MAC">MAC地址</param>
        /// <returns></returns>
        public List<M_Alert_Object> GetAlertListByMAC(long oid, string MAC)
        {
            return dal_sys_log_alert.GetAlertListByMAC(oid, MAC);
        }
        /// <summary>
        /// 存储扫描的ssid数据
        /// </summary>
        /// <param name="list"></param>
        public void SSIDScand(List<Upload> list)
        {
            if (list.Count > 0)
            {
                string mac = list[0].AP_MAC;
                Int64? oid = aporg.SelectOIDByApMac(mac);
                if (oid != null)
                {
                    List<Int64> oList = dal_org.SelectParentOrgIDs((Int64)oid);
                    List<SYS_LOG_ALERTKEYWORD> keywords = new List<SYS_LOG_ALERTKEYWORD>();
                    foreach (var item in oList)
                    {
                        keywords.AddRange(dal_keyWord.getKeyWord(item));
                    }
                    List<SYS_LOG_ALERTWHITELIST> whitelist = dal_whiteList.getWhiteList((Int64)oid);
                    List<SYS_LOG_APNEAR> apnears = new List<SYS_LOG_APNEAR>();
                    if (whitelist.Where(c => c.MAC == mac).Count() == 0)
                    {
                        List<SYS_LOG_ALERT> listalert = new List<SYS_LOG_ALERT>();
                        DateTime dtime = DateTime.Now;
                        decimal DistancePercent = 0;
                        for (int i = 0; i < list.Count; ++i)
                        {
                            for (int j = 0; j < keywords.Count; ++j)
                            {
                                if (list[i].SSID.ToUpper().IndexOf(keywords[j].KEYWORD.ToUpper()) >= 0)
                                {
                                    var p = listalert.Where(c => c.G_MAC == list[i].MAC && c.G_SSID == list[i].SSID);
                                    if (p.Count() > 0)
                                    {
                                        ///如果存在多个词都相似，则 取最相近的词付给相似度
                                        DistancePercent = LUOBO.Helper.LevenshteinDistance.Instance.LevenshteinDistancePercent(list[i].SSID.ToUpper(), keywords[j].KEYWORD.ToUpper());
                                        if (p.First().Similarity < DistancePercent)
                                        {
                                            p.First().Similarity = DistancePercent;
                                        }
                                        p.First().KEYWORD = p.First().KEYWORD + "," + keywords[j].KEYWORD;
                                    }
                                    else
                                    {
                                        DistancePercent = LUOBO.Helper.LevenshteinDistance.Instance.LevenshteinDistancePercent(list[i].SSID.ToUpper(), keywords[j].KEYWORD.ToUpper());
                                        listalert.Add(new SYS_LOG_ALERT { OID = (Int64)oid, AP_MAC = mac, G_SSID = list[i].SSID, G_MAC = list[i].MAC, ISPROCESS = false, ISWHITELIST = false, KEYWORD = keywords[j].KEYWORD, G_TIME = dtime, G_STRONG = Decimal.Parse(list[i].Signal), CHANNEL = Int32.Parse(list[i].DS), Similarity = DistancePercent });
                                    }
                                }
                            }
                            apnears.Add(new SYS_LOG_APNEAR { OID = (Int64)oid, AP_MAC = mac, G_SSID = list[i].SSID, G_MAC = list[i].MAC, G_TIME = dtime, G_STRONG = Decimal.Parse(list[i].Signal), CHANNEL = Int32.Parse(list[i].DS) });
                        }
                        using (TransactionScope scope = new TransactionScope())
                        {
                            try
                            {
                                dal_apNear.Save(mac, apnears);
                                dal_sys_log_alert.Save(mac, listalert);
                                scope.Complete();
                            }
                            catch (Exception ex)
                            {
                                scope.Dispose();
                                throw new Exception("错误原因是：" + ex.Message);
                            }
                        }

                    }
                }
            }
        }

        public List<M_SECURITY_SSID> GetSameSSIDList(Int64 oid,string ssidName, Int64 apid)
        {
            return dal_apNear.GetSameSSIDList(oid, ssidName, apid);
        }

        public List<M_SECURITY_SSID> GetFilterSSIDList(long oid, int pageSize, int curPage, M_SECURITY_SSID_Filter filter)
        {
            return dal_apNear.GetFilterSSIDList(oid, pageSize, curPage, filter);
        }

        /// <summary>
        /// 根据APID获取AP探测SSID结果
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="pageSize"></param>
        /// <param name="curPage"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<M_SECURITY_SSID> GetFilterSSIDListByAPID(long oid, int pageSize, int curPage, M_SECURITY_SSID_Filter filter)
        {
            return dal_apNear.GetFilterSSIDListByAPID(oid, pageSize, curPage, filter);
        }

        public List<M_Alert_Graph> getWarnGraphList(long oid, bool isRealTime)
        {
            return dal_apNear.SelectGraph(oid, isRealTime);
        }

        public int GetFilterSSIDListCounts(long oid, M_SECURITY_SSID_Filter filter)
        {
            return dal_apNear.GetFilterSSIDListCounts(oid, filter);
        }
        public int GetFilterSSIDListCountsByAPID(long oid, M_SECURITY_SSID_Filter filter)
        {
            return dal_apNear.GetFilterSSIDListCounts(oid, filter);
        }

        public List<SYS_LOG_ALERTWHITELIST> GetAlertWhiteListByOID(Int64 oid)
        {
            return dal_whiteList.getWhiteList(oid);
        }

        public List<SYS_LOG_ALERTKEYWORD> GetAlertKeyWordByOID(Int64 oid)
        {
            return dal_keyWord.getKeyWord(oid);
        }

        public List<SYS_APCONTACT> GetAPContactByOID(Int64 oid)
        {
            return dal_apcontact.SelectByOID(oid);
        }

        public bool AddAPContact(SYS_APCONTACT apcontact)
        {
            return dal_apcontact.Insert(apcontact);
        }

        public bool SaveAPContact(SYS_APCONTACT apcontact)
        {
            return dal_apcontact.Update(apcontact);
        }

        public bool DelAPContact(Int64 id)
        {
            return dal_apcontact.Delete(id);
        }

        public List<Int64> GetAPNearCountByOID(Int64 OID, M_SECURITY_SSID_Filter filter)
        {
            return dal_apNear.GetAPNearCountByOID(OID, filter);
        }

        public List<Int64> GetAPNearCountByAPID(Int64 OID, M_SECURITY_SSID_Filter filter)
        {
            return dal_apNear.GetAPNearCountByAPID(OID, filter);
        }

        public bool ProcessForWhiteList(long log_id)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = false;

                try
                {
                    SYS_LOG_ALERT alert = dal_sys_log_alert.Select(log_id);
                    alert.ISWHITELIST = true;
                    dal_sys_log_alert.Update(alert);
                    SYS_LOG_ALERTWHITELIST white = new SYS_LOG_ALERTWHITELIST();
                    white.OID = alert.OID;
                    white.MAC = alert.G_MAC;
                    dal_whiteList.Insert(white);
                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }

                return flag;
            }
        }

        public bool ProcessForNotice(long log_id)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = false;
                try
                {
                    M_Alert_Object m_alert = dal_sys_log_alert.SelectByID(log_id);
                    List<SYS_APCONTACT> contact = dal_apcontact.SelectByLogID(log_id);

                    SYS_LOG_ALERT alert = Helper.PubFun.ChangeNewItem<SYS_LOG_ALERT, M_Alert_Object>(m_alert);
                    sendNoticeEmail(contact, m_alert);
                    alert.ISPROCESS = true;
                    dal_sys_log_alert.Update(alert);
                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }

                return flag;
            }
        }

        /// <summary>
        /// 发送告警邮件
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="rule"></param>
        private void sendNoticeEmail(List<SYS_APCONTACT> contact, M_Alert_Object alert)
        {
            NetworkCredential cred = new NetworkCredential("next_wifi@163.com", "wqyz@2014");
            MailMessage msg = new MailMessage();
            msg.Subject = "【告警】在【" + alert.APNAME + "】附近发现可疑SSID，名为【" + alert.G_SSID + "】，请核查";
            msg.From = new MailAddress("next_wifi@163.com");
            foreach (var item in contact)
            {
                msg.To.Add(item.EMAIL);
            }
            string body = "<div style='font-size:13px;line-height: 1.8em'>";
            body += "请对营业厅附近的可疑SSID进行检查";
            body += "</div>";
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.163.com", 25);
            client.Credentials = cred;
            client.Send(msg);
        }
    }
}
