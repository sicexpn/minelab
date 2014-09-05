using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Model;
using LUOBO.Helper;
using System.Transactions;
using System.Xml;
using System.Reflection;
using System.IO;

namespace LUOBO.BLL
{
    public class BLL_PROBEAPManage
    {
        DAL_APCONFIGTEMPLATE apctDAL = new DAL_APCONFIGTEMPLATE();
        DAL_SYS_PROBEDEVICE apdDAL = new DAL_SYS_PROBEDEVICE();//MODIFY
        DAL_SYS_SSID ssidDAL = new DAL_SYS_SSID();
        DAL_SYS_APORG apOrgDAL = new DAL_SYS_APORG();
        DAL_SYS_APORGLOG apOrgLogDAL = new DAL_SYS_APORGLOG();
        DAL_SYS_PROBEORG orgDAL = new DAL_SYS_PROBEORG();//MODIFY
        DAL_SYS_BANLIST banDAL = new DAL_SYS_BANLIST();
        DAL_SYS_SETTINGVER settingverDAL = new DAL_SYS_SETTINGVER();
        DAL_SYS_SSID_AUDIT ssidAuditDAL = new DAL_SYS_SSID_AUDIT();
        DAL_SYS_USER userDAL = new DAL_SYS_USER();

        #region AP配置模版相关操作
        public bool InsertAPCT(APCONFIGTEMPLATE data)
        {
            data.UPDATETIME = DateTime.Now;
            return apctDAL.Insert(data);
        }

        public bool UpdateAPCT(Int64 apctid, string tName, string tContent, string firmName, string firmVersion, string description)
        {
            APCONFIGTEMPLATE apct = new APCONFIGTEMPLATE();
            apct.ID = apctid;
            apct.CONTENT = tContent;
            apct.DESCRIPTION = description;
            apct.FIRMWARE = firmName;
            apct.VERSION = firmVersion;
            apct.ISDELETE = false;
            apct.TNAME = tName;
            apct.UPDATETIME = DateTime.Now;
            return apctDAL.Update(apct);
        }

        public bool DeleteAPCT(Int64 id)
        {
            return apctDAL.Delete(id);
        }

        public bool DisablesAPCT(string ids)
        {
            return apctDAL.Disables(ids);
        }

        public APCONFIGTEMPLATE SelectAPCTByID(Int64 id)
        {
            return apctDAL.Select(id);
        }

        public M_APCT SelectAPCTByPage(int size, int curPage)
        {
            M_APCT mAPCT = new M_APCT();
            mAPCT.AllCount = apctDAL.SelectCount();
            mAPCT.APCTList = apctDAL.Select(size, curPage);
            return mAPCT;
        }

        public List<APCONFIGTEMPLATE> SelectAPCTForSelct()
        {
            return apctDAL.Select();
        }

        public bool CreateAPSettingFile(Int64 APID)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {



                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }
            return flag;
        }

        #endregion

        #region AP相关操作
        public bool RegAPDvice(SYS_PROBEDEVICE data, Int64 OID)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    data.MAC = data.MAC.ToUpper();
                    data.MAC = data.MAC.Replace(':', '-');
                    Int64 apid = apdDAL.Insert(data);

                    SYS_APORG aporg = new SYS_APORG();
                    SYS_APORGLOG aporglog = new SYS_APORGLOG();
                    aporg.ID = -1;
                    aporg.APID = apid;
                    aporg.OID = OID;
                    aporg.POID = -1;
                    aporg.SSIDNUM = data.MAXSSIDNUM;
                    aporg.SDATE = DateTime.Parse("2000-1-1 0:0:0");
                    aporg.EDATE = DateTime.Parse("9999-12-31 0:0:0");
                    aporg.ISCHILD = true;
                    apOrgDAL.Insert(aporg);

                    aporglog.ID = -1;
                    aporglog.APID = apid;
                    aporglog.FOID = -1;
                    aporglog.SSIDNUM = data.MAXSSIDNUM;
                    aporglog.TOID = OID;
                    aporglog.SDATE = aporg.SDATE;
                    aporglog.EDATE = aporg.EDATE;
                    aporglog.CREATETIME = DateTime.Now;
                    aporglog.OPNAME = "分配";
                    apOrgLogDAL.Insert(aporglog);
                    scope.Complete();
                    flag = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool RegAPDvices(List<SYS_PROBEDEVICE> datas, Int64 OID)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SYS_APORG aporg = null;
                    SYS_APORGLOG aporglog = null;
                    foreach (SYS_PROBEDEVICE data in datas)
                    {
                        data.MAC = data.MAC.ToUpper();
                        data.MAC = data.MAC.Replace(':', '-');
                        aporg = new SYS_APORG();
                        aporg.APID = apdDAL.Insert(data);
                        aporg.OID = OID;
                        aporg.POID = -1;
                        aporg.SSIDNUM = data.MAXSSIDNUM;
                        aporg.SDATE = DateTime.Parse("2000-1-1 0:0:0");
                        aporg.EDATE = DateTime.Parse("9999-12-31 0:0:0");
                        aporg.ISCHILD = true;
                        apOrgDAL.Insert(aporg);

                        aporglog = new SYS_APORGLOG();
                        aporglog.ID = -1;
                        aporglog.APID = aporg.APID;
                        aporglog.FOID = -1;
                        aporglog.SSIDNUM = data.MAXSSIDNUM;
                        aporglog.TOID = OID;
                        aporglog.SDATE = aporg.SDATE;
                        aporglog.EDATE = aporg.EDATE;
                        aporglog.CREATETIME = DateTime.Now;
                        aporglog.OPNAME = "分配";
                        apOrgLogDAL.Insert(aporglog);
                    }
                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }
            return flag;
        }

        public bool UpdateAP(Int64 ID, Int64 SERIAL, string MODEL, string MANUFACTURER, string PURCHASER, string FIRMWAREVERSION, Int64 MAXSSIDNUM, bool SUPPORT3G, Int64 APCTID, string DESCRIPTION, Int64 HBINTERVAL, Int64 DATAINTERVAL, bool? ISREBOOT)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = apdDAL.Update(ID, null, SERIAL, MODEL, MANUFACTURER, PURCHASER, FIRMWAREVERSION, MAXSSIDNUM, null, SUPPORT3G, APCTID, DESCRIPTION, null, HBINTERVAL, DATAINTERVAL, true, ISREBOOT, null, null, null, null);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }
            return flag;
        }

        public bool Update(SYS_PROBEDEVICE data)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = apdDAL.Update(data);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool UpdateAP()
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //flag = apdDAL.Update();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool Updates(List<SYS_PROBEDEVICE> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_PROBEDEVICE data in datas)
                        apdDAL.Update(data);

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public List<SYS_AP_VIEW> SelectAPByOID(Int64 OID, bool isInvalid)
        {
            if (isInvalid)
                return apdDAL.SelectInvalidAPByOID(OID);
            else
                return apdDAL.SelectAPByOID(OID, isInvalid);
        }

        public List<SYS_AP_VIEW> SelectAPByOID(Int64 OID)
        {
            return apdDAL.SelectAPByOID(OID);
        }

        public List<Int64> SelectOnlinePeopleNumByToken(string token, Int64 apid)
        {
            SYS_USER user = userDAL.SelectByToken(token);
            return apdDAL.SelectOnlinePeopleNumByOID(user.OID, apid);
        }

        public M_AP_VIEW SelectAllAPByPage(Int64 jgID, Int64 benJGID, Int64? startSerial, Int64? endSerial, string mac, string startDate, string endDate, int? FPState, int curPage, int size)
        {
            M_AP_VIEW mAP = new M_AP_VIEW();
            if (!string.IsNullOrEmpty(mac))
                mac = mac.ToUpper();
            mAP.AllCount = apdDAL.SelectCount(jgID, benJGID, startSerial, endSerial, mac, startDate, endDate, FPState);
            mAP.APList = apdDAL.SelectForManage(jgID, benJGID, startSerial, endSerial, mac, startDate, endDate, FPState, size, curPage);
            return mAP;
        }

        public SYS_PROBEDEVICE SelectAPByID(Int64 id)
        {
            return apdDAL.Select(id);
        }

        public bool DisablesAP(string ids)
        {
            return apdDAL.Disables(ids);
        }

        public SYS_AP_VIEW SelectAPViewByAPID(Int64 APID)
        {
            return apdDAL.SelectAPViewByAPID(APID);
        }

        public List<SYS_PROBEDEVICE> CheckMac(SYS_PROBEDEVICE item)
        {
            return apdDAL.CheckMac(new List<SYS_PROBEDEVICE>() { item });
        }

        public List<SYS_PROBEDEVICE> CheckMacs(List<SYS_PROBEDEVICE> items)
        {
            return apdDAL.CheckMac(items);
        }

        public bool AllotAP(List<SYS_AP_VIEW> list, bool isCreateSSID)
        {
            bool flag = false;
            XmlDocument xd = new XmlDocument();
            xd.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultValue.xml"));

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SYS_APORG aporg = null;
                    SYS_APORGLOG aporglog = null;
                    APBySSIDCount apbyssid = null;
                    List<SYS_SSID> ssidList = ssidDAL.SelectByAPIDs(list.ToString("ID", ","));

                    string _NAME = xd["Root"]["SSID"]["NAME"].InnerText,
                                _ISINTERNET = xd["Root"]["SSID"]["ISINTERNET"].InnerText,
                                _MAXLINKCOUNT = xd["Root"]["SSID"]["MAXLINKCOUNT"].InnerText,
                                _PORTAL = xd["Root"]["SSID"]["PORTAL"].InnerText,
                                _PATH = xd["Root"]["SSID"]["PATH"].InnerText,
                                _MAXFLOW = xd["Root"]["SSID"]["MAXFLOW"].InnerText,
                                _MAXUS = xd["Root"]["SSID"]["MAXUS"].InnerText,
                                _MAXDS = xd["Root"]["SSID"]["MAXDS"].InnerText,
                                _VONLINETIME = xd["Root"]["SSID"]["VONLINETIME"].InnerText,
                                _VMAXDS = xd["Root"]["SSID"]["VMAXDS"].InnerText,
                                _VMAXUS = xd["Root"]["SSID"]["VMAXUS"].InnerText,
                                _ISPWD = xd["Root"]["SSID"]["ISPWD"].InnerText,
                                _PWD = xd["Root"]["SSID"]["PWD"].InnerText,
                                _ACID = xd["Root"]["SSID"]["ACID"].InnerText,
                                _ADID = xd["Root"]["SSID"]["ADID"].InnerText;
                    int isOnCount = Convert.ToInt32(xd["Root"]["SSID"]["ISON"].Attributes["Count"].Value);
                    int _curSSIDNum = 1;

                    SYS_SETTINGVER settingver;

                    foreach (SYS_AP_VIEW item in list)
                    {
                        #region 更新原始数据
                        aporg = new SYS_APORG();
                        aporg.ID = item.APORGID;
                        aporg.APID = item.ID;
                        aporg.POID = item.POID;
                        aporg.OID = item.OID;
                        aporg.SSIDNUM = item.SSIDNUM;
                        aporg.SDATE = item.SDATE;
                        aporg.EDATE = item.EDATE;
                        aporg.ISCHILD = false;
                        apOrgDAL.UpdateCHILD(aporg);
                        #endregion

                        #region 添加分配AP并记录历史
                        aporglog = new SYS_APORGLOG();
                        aporglog.APID = item.ID;
                        aporglog.FOID = item.POID;
                        aporglog.TOID = item.OID;
                        aporglog.SSIDNUM = item.SSIDNUM;
                        aporglog.SDATE = item.SDATE;
                        aporglog.EDATE = item.EDATE;
                        aporglog.CREATETIME = DateTime.Now;
                        aporglog.OPNAME = "分配";
                        aporg.ISCHILD = true;
                        apOrgDAL.Insert(aporg);
                        apOrgLogDAL.Insert(aporglog);
                        #endregion

                        #region 分配SSID
                        if (isCreateSSID)
                        {
                            List<APBySSIDCount> ssidcountList = ssidDAL.SelectCountByAPID(item.ID.ToString());
                            //ssidcountList.ForEach(c => c.OID = item.POID);

                            SYS_SSID ssid = null;
                            apbyssid = ssidcountList[0];
                            _curSSIDNum = 1;
                            for (int i = 0; i < apbyssid.SSIDCount; i++)
                            {
                                ssid = new SYS_SSID();
                                ssid.ID = -1;
                                ssid.NAME = _NAME + (i + 1);
                                ssid.OID = item.POID;
                                ssid.APID = item.ID;
                                ssid.ISON = _curSSIDNum <= isOnCount ? true : false;
                                ssid.ISINTERNET = Convert.ToBoolean(_ISINTERNET);
                                ssid.MAXLINKCOUNT = Convert.ToInt64(_MAXLINKCOUNT);
                                ssid.PORTAL = _PORTAL;
                                ssid.PATH = _PATH;
                                ssid.MAXFLOW = Convert.ToInt64(_MAXFLOW);
                                ssid.MAXUS = Convert.ToInt64(_MAXUS);
                                ssid.MAXDS = Convert.ToInt64(_MAXDS);
                                ssid.VONLINETIME = Convert.ToInt64(_VONLINETIME);
                                ssid.VMAXDS = Convert.ToInt64(_VMAXDS);
                                ssid.VMAXUS = Convert.ToInt64(_VMAXUS);
                                ssid.ISPWD = Convert.ToBoolean(_ISPWD);
                                ssid.PWD = _PWD;
                                ssid.ACID = Convert.ToInt64(_ACID);
                                ssid.ADID = Convert.ToInt64(_ADID);
                                ssidList.Add(ssid);
                                _curSSIDNum++;
                            }

                            if (apbyssid.SSIDCount < isOnCount)
                                isOnCount = ssidList.Count;
                            for (int i = 0; i < isOnCount; i++)
                                ssidList[i].ISON = true;
                        }

                        for (int i = ssidList.Count - 1; i >= 0; i--)
                        {
                            if (ssidList[i].OID == item.POID && ssidList[i].APID == item.ID)
                            {
                                ssidList[i].OID = item.OID;
                                item.SSIDNUM--;
                                if (item.SSIDNUM == 0)
                                    break;
                            }
                        }
                        #endregion

                        #region 生成版本信息
                        settingver = new SYS_SETTINGVER() { ID = -1, APID = item.ID, GUID = Guid.NewGuid().ToString(), DATETIME = DateTime.Now };
                        settingverDAL.Update(settingver);
                        #endregion

                    }

                    foreach (SYS_SSID ssid in ssidList)
                        ssidDAL.Update(ssid);


                    //foreach (var item in list)
                    //{
                    //    CreateSettingFile(item, ssidList.Where(c=>c.APID == item.ID).ToList());
                    //}
                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool CreateSettingFile(SYS_AP_VIEW ap, List<SYS_SSID> ssidList)
        {
            bool flag = false;
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(apctDAL.SelectConfigByAPID(ap.ID));
            FileStream fs = null;
            StreamWriter sw = null;

            int fileCount = Convert.ToInt32(xd["Root"]["OptionFile"].Attributes["FileNum"].Value);
            string fileContent = "";
            string filePath = "";
            string fileName = "";
            string argName = "";
            string baseUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APSettingFile");

            string new_guid = Guid.NewGuid().ToString();
            //string pattern = @"/b%?.*?%/b";
            try
            {
                foreach (XmlNode fileNode in xd["Root"]["OptionFile"])
                {
                    //填充模版参数
                    filePath = fileNode.Attributes["Path"].Value;
                    fileName = fileNode.Attributes["Name"].Value;
                    fileContent = fileNode["Content"].InnerText;
                    foreach (XmlNode argNode in fileNode["Args"])
                    {
                        string argValue = "";
                        //Type t = ap.GetType();
                        switch (argNode.Attributes["From"].Value)
                        {
                            case "Table":
                                argName = argNode.InnerText;
                                if (argNode.Attributes["Source"].Value == typeof(SYS_AP_VIEW).Name)
                                    argValue = ap.GetType().GetProperty(argNode.Attributes["Field"].Value).GetValue(ap, null).ToString();
                                //else if (argNode.Attributes["Source"].Value == typeof(SYS_SSID).Name)
                                //argValue = 
                                fileContent = fileContent.Replace("%?" + argName + "?%", argValue.ToString());

                                break;
                            case "Model":
                                argName = argNode.InnerText;
                                string ssid_tmp = "";
                                string tmp = "";
                                foreach (SYS_SSID ssid in ssidList)
                                {
                                    tmp = xd["Root"]["Models"][argName]["Content"].InnerText;
                                    XmlNode modelArgNode = xd["Root"]["Models"][argName]["Args"];
                                    foreach (XmlNode node in modelArgNode)
                                    {
                                        string _argName = node.InnerText;
                                        if (node.Attributes["Source"].Value == typeof(SYS_SSID).Name)
                                        {
                                            var _tmp = ssid.GetType().GetProperty(node.Attributes["Field"].Value).GetValue(ssid, null);
                                            argValue = _tmp != null ? _tmp.ToString() : "";
                                        }
                                        if (node.Attributes["IsEnum"].Value == "true")
                                        {
                                            XmlNode enumNode = xd["Root"]["Enums"][node.Attributes["EnumName"].Value];
                                            foreach (XmlNode t in enumNode)
                                            {
                                                if (argValue == t.Attributes["key"].Value)
                                                {
                                                    argValue = t.InnerText;
                                                    break;
                                                }
                                            }
                                        }
                                        tmp = tmp.Replace("%?" + _argName + "?%", argValue.ToString());
                                    }
                                    ssid_tmp += "\n" + tmp;
                                }
                                fileContent = fileContent.Replace("%?" + argName + "?%", ssid_tmp);
                                break;
                        }
                    }
                    //fileContent = fileNode["Content"].InnerText;

                    // 创建文件夹，写配置文件
                    filePath = Path.Combine(baseUrl, ap.SERIAL.ToString(), new_guid, filePath.Replace('/', '\\').Substring(1));
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
                    sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
                    fileContent = fileContent.Replace("\r\n", "\n");
                    sw.Write(fileContent);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }


                // Chilli配置文件.单独提出来了.写死了
                XmlNode chilliConfigNode = xd["Root"]["Models"]["Model_Config"];
                for (int i = 0; i < ssidList.Count; i++)
                {
                    filePath = chilliConfigNode.Attributes["Path"].Value + (i + 1);
                    fileName = chilliConfigNode.Attributes["Name"].Value;
                    fileContent = chilliConfigNode["Content"].InnerText;


                    foreach (XmlNode argNode in chilliConfigNode["Args"])
                    {
                        string argValue = "";
                        switch (argNode.Attributes["From"].Value)
                        {
                            case "Table":
                                argName = argNode.InnerText;
                                if (argNode.Attributes["Source"].Value == typeof(SYS_SSID).Name)
                                    argValue = ssidList[i].GetType().GetProperty(argNode.Attributes["Field"].Value).GetValue(ssidList[i], null).ToString();
                                fileContent = fileContent.Replace("%?" + argName + "?%", argValue.ToString());
                                break;
                            case "Manual":
                                argName = argNode.InnerText;
                                if (argName == "NETWORK")
                                {
                                    fileContent = fileContent.Replace("%?" + argName + "?%", "10." + (i + 1) + ".0.0");
                                }
                                break;
                        }
                    }

                    filePath = Path.Combine(baseUrl, ap.SERIAL.ToString(), new_guid, filePath.Replace('/', '\\').Substring(1));
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    fs = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
                    sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
                    fileContent = fileContent.Replace("\r\n", "\n");
                    sw.Write(fileContent);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                flag = true;

                foreach (var item in ssidList)
                {
                    if (item.ID == -1)
                        continue;
                    item.ISUPDATE = false;
                    ssidDAL.Update(item);
                }

                ap.ISUPDATE = false;
                ap.ISREBOOT = false;
                apdDAL.Update(PubFun.ChangeNewItem<SYS_PROBEDEVICE, SYS_AP_VIEW>(ap));

                SYS_SETTINGVER setting = new SYS_SETTINGVER() { ID = -1, APID = ap.ID, GUID = new_guid, DATETIME = DateTime.Now };
                settingverDAL.Update(setting);

            }
            catch (Exception ex)
            {
                flag = false;
                if (sw != null)
                {
                    sw.Close();
                    fs.Close();
                }
            }
            return flag;
        }

        public bool CheckIsRebootByAPID(Int64 APID)
        {
            return apdDAL.CheckIsRebootByAPID(APID);
        }

        public bool RebootComplete(Int64 serial)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = apdDAL.RebootComplete(serial);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public List<SYS_AP_GIS> SelectAPListForGIS(Int64 OID)
        {
            List<SYS_AP_GIS> list = apdDAL.SelectAPListForGIS(OID);
            Random rd = new Random();
            foreach (var item in list)
            {
                var date = item.LASTHB;
                item.ISLIVE = date.AddSeconds(item.HBINTERVAL * 2) < DateTime.Now ? false : true;
                if (!item.ISLIVE)
                    item.ONLINEPEOPLENUM = 0;
                //item.ONLINEPEOPLENUM = rd.Next(0, 100);
                item.POWERDATETIME = item.POWERDATETIME == DateTime.Parse("0001-01-01") ? DateTime.Parse("2000-01-01") : item.POWERDATETIME;
            }
            return list;
        }
        /// <summary>
        /// 回收Ap-xpn
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool BackAp(int jgID, string ids)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<SYS_APORG> apOrgList = new List<SYS_APORG>();
                    apOrgList = apOrgDAL.SelectByBackApId(jgID, ids);
                    foreach (SYS_APORG apOrg in apOrgList)
                    {
                        SYS_APORGLOG apOrgLog = new SYS_APORGLOG();

                        apOrgLog.APID = apOrg.APID;
                        apOrgLog.EDATE = apOrg.EDATE;
                        apOrgLog.SDATE = apOrg.SDATE;
                        apOrgLog.FOID = apOrg.OID;
                        apOrgLog.TOID = jgID;
                        apOrgLog.OPNAME = "回收";
                        apOrgLog.CREATETIME = DateTime.Now;

                        apOrgLogDAL.Insert(apOrgLog);//回收记录登记
                    }
                    apOrgDAL.Deletes(apOrgList.ToString("ID", ","));
                    flag = apOrgDAL.UpdateBackByOID(jgID, ids);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;

            //return apdDAL.DeletesAp(ids);
        }

        /// <summary>
        /// xpn
        /// </summary>
        /// <param name="jgID"></param>
        /// <param name="curPage"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public M_AP_VIEW SelectBackAPByPage(Int64 jgID, int curPage, int size)
        {
            M_AP_VIEW mAp = new M_AP_VIEW();

            List<SYS_PROBEORG> orgList = orgDAL.SelectSub(jgID);//获取当前机构的子机构
            List<Int64> orgIds = new List<Int64>();
            String ids = "";
            foreach (SYS_PROBEORG org in orgList)
            {
                orgIds.Add(org.ID);
                ids += org.ID.ToString();
                ids += ",";
            }
            if (ids.Length > 0)
            {
                ids = ids.Remove(ids.Length - 1);
                mAp.AllCount = apdDAL.CountsBackApByOrgId(ids);
                mAp.APList = apdDAL.SelectBackApByOrgId(ids, curPage, size);//根据子机构ID集合，获取对应的Ap列表

            }

            //orgIds.ToString();


            return mAp;
        }
        public List<SYS_PROBEDEVICE> SelectApStateListByOID(Int64 orgID, string apname, string column, string orderby)
        {
            //SYS_PROBEORG org = orgDAL.Select(orgID);
            return apdDAL.SelectApStateListByOID(orgID, apname, column, orderby);

            //if (org.CATEGORY == ((int)CustomEnum.ENUM_Org_Type.Chain)+"")
            //{
            //List<SYS_PROBEDEVICE> list = null;
            //if(orgID == 0)
            //    list = apdDAL.SelectApStateListByOID(orgID);
            //else {
            //    SYS_PROBEORG org = orgDAL.Select(orgID);
            //    if(org != null){
            //        if (org.CATEGORY == ((int)CustomEnum.ENUM_Org_Type.Chain).ToString())
            //            list = apdDAL.SelectApStateListByOID(orgID);
            //        else if(org.CATEGORY == ((int)CustomEnum.ENUM_Org_Type.Single).ToString())
            //            list = apdDAL.SelectApStateListByOIDNoSub(orgID);
            //    }
            //}
            //return list;
        }
        #endregion

        #region SSID相关操作
        public List<M_SSID> SelectM_SSIDByAPID(Int64 APID, Int64? oID)
        {

            List<M_SSID> m_list = new List<M_SSID>();
            List<SYS_SSID> ssids = ssidDAL.SelectByAPID(APID, oID);
            List<SYS_BANLIST> banlist = banDAL.SelectByAPID(APID);
            List<SYS_BANLIST> oneBanList = null;
            M_SSID mtemp = null;

            foreach (var item in ssids)
            {
                mtemp = PubFun.ChangeNewItem<M_SSID, SYS_SSID>(item);
                oneBanList = banlist.Where(c => c.SSIDID == item.ID).ToList();
                mtemp.ports = oneBanList.Where(c => c.TYPE == (int)CustomEnum.ENUM_Ban_Type.Port).ToString("RES", ",");
                mtemp.macs = oneBanList.Where(c => c.TYPE == (int)CustomEnum.ENUM_Ban_Type.Mac).ToString("RES", ",");
                mtemp.urls = oneBanList.Where(c => c.TYPE == (int)CustomEnum.ENUM_Ban_Type.Url).ToString("RES", ",");
                m_list.Add(mtemp);
            }
            return m_list;
        }

        public List<SYS_SSID> SelectSSIDByAPID(Int64 APID)
        {
            return ssidDAL.SelectByAPID(APID, null);
        }

        public List<APBySSIDCount> GetCountByAPID(string APIDs)
        {
            return ssidDAL.SelectCountByAPID(APIDs);
        }

        /// <summary>
        /// 更新SSID信息，当ID为-1时为插入SSID
        /// </summary>
        /// <param name="ssid"></param>
        /// <returns></returns>
        public bool UpdateSSID(M_SSID m_ssid)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SYS_SSID ssid = PubFun.ChangeNewItem<SYS_SSID, M_SSID>(m_ssid);
                    //ssid.NAME = m_ssid.NEWNAME;
                    //if (m_ssid.ISAUDIT && m_ssid.NEWNAME != m_ssid.NAME)
                    //{
                    //    SYS_SSID_AUDIT audit = new SYS_SSID_AUDIT();
                    //    audit.ID = -1;
                    //    audit.SSIDID = ssid.ID;
                    //    audit.SSIDNAME = m_ssid.NEWNAME;
                    //    audit.APPLYTIME = DateTime.Now;
                    //    //audit.APPLICANT = 
                    //}
                    //else
                    //{
                    //}
                    SYS_SSID old_ssid = ssidDAL.SelectByID(ssid.ID);
                    if (old_ssid.NAME.Trim() != ssid.NAME.Trim() || old_ssid.ISON != ssid.ISON)
                    {
                        SYS_SETTINGVER setver = new SYS_SETTINGVER();
                        setver.ID = -1;
                        setver.APID = old_ssid.APID;
                        setver.TYPE = (int)CustomEnum.ENUM_Setting_Type.Setting;
                        setver.GUID = Guid.NewGuid().ToString();
                        setver.DATETIME = DateTime.Now;
                        settingverDAL.Update(setver);
                    }

                    ssidDAL.Update(ssid);

                    #region 更新禁用列表
                    SYS_BANLIST btmp = null;
                    List<SYS_BANLIST> banList = new List<SYS_BANLIST>();
                    string[] macs = string.IsNullOrEmpty(m_ssid.macs) ? null : m_ssid.macs.Split(',');
                    string[] urls = string.IsNullOrEmpty(m_ssid.urls) ? null : m_ssid.urls.Split(',');
                    string[] ports = string.IsNullOrEmpty(m_ssid.ports) ? null : m_ssid.ports.Split(',');

                    banDAL.DeleteBySSIDID(m_ssid.ID);
                    if (macs != null)
                        foreach (var item in macs)
                        {
                            btmp = new SYS_BANLIST();
                            btmp.ID = -1;
                            btmp.TYPE = (int)CustomEnum.ENUM_Ban_Type.Mac;
                            btmp.SSIDID = m_ssid.ID;
                            btmp.RES = item;
                            banList.Add(btmp);
                        }

                    if (urls != null)
                        foreach (var item in urls)
                        {
                            btmp = new SYS_BANLIST();
                            btmp.ID = -1;
                            btmp.TYPE = (int)CustomEnum.ENUM_Ban_Type.Url;
                            btmp.SSIDID = m_ssid.ID;
                            btmp.RES = item;
                            banList.Add(btmp);
                        }

                    if (ports != null)
                        foreach (var item in ports)
                        {
                            btmp = new SYS_BANLIST();
                            btmp.ID = -1;
                            btmp.TYPE = (int)CustomEnum.ENUM_Ban_Type.Port;
                            btmp.SSIDID = m_ssid.ID;
                            btmp.RES = item;
                            banList.Add(btmp);
                        }

                    foreach (var item in banList)
                        banDAL.Update(item);
                    #endregion

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public bool UpdateSSID(SYS_SSID ssid, string token)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SYS_SSID old_ssid = ssidDAL.SelectByID(ssid.ID);

                    if (old_ssid.NAME.Trim() != ssid.NAME.Trim())
                    {
                        //生成审核信息
                        SYS_USER user = userDAL.SelectByToken(token);
                        SYS_SSID_AUDIT audSSID = new SYS_SSID_AUDIT();
                        audSSID.ID = -1;
                        audSSID.SSIDID = old_ssid.ID;
                        audSSID.SSIDNAME = ssid.NAME;
                        audSSID.APID = old_ssid.APID;
                        audSSID.APPLICANT = user.ACCOUNT;
                        audSSID.APPLYOID = user.OID;
                        audSSID.APPLYTIME = DateTime.Now;
                        audSSID.STATE = (int)CustomEnum.ENUM_Aud_Stat.WaitAudit;
                        ssidAuditDAL.Update(audSSID);

                        ssid.NAME = old_ssid.NAME.Trim();
                    }
                    if ((old_ssid.ISON != ssid.ISON) || (old_ssid.ISPWD != ssid.ISPWD))
                    {
                        ssid.ISUPDATE = true;
                        SYS_SETTINGVER setver = new SYS_SETTINGVER();

                        setver.ID = -1;
                        setver.APID = old_ssid.APID;
                        setver.TYPE = (int)CustomEnum.ENUM_Setting_Type.Setting;
                        setver.GUID = Guid.NewGuid().ToString();
                        setver.DATETIME = DateTime.Now;
                        settingverDAL.Update(setver);
                    }

                    ssidDAL.Update(ssid);

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public M_SSID_AUDIT SelectSSIDAuditOnPage(string keystr, int state, int curPage, int size)
        {
            M_SSID_AUDIT m_ssid_aud = new M_SSID_AUDIT();
            m_ssid_aud.SSIDList = ssidAuditDAL.SelectOnPage(keystr, state, curPage, size);
            m_ssid_aud.AllCount = ssidAuditDAL.SelectCount(keystr, state, curPage, size);
            return m_ssid_aud;
        }
        #endregion





        //public M_DeviceStatiscal SelectDeviceStatiscalByOID(long orgId)
        //{
        //    return apdDAL.SelectDeviceStatiscalByOID(orgId);
        //}
    }
}
