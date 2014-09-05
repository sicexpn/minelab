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
    public class BLL_APManage
    {
        DAL_APCONFIGTEMPLATE apctDAL = new DAL_APCONFIGTEMPLATE();
        DAL_SYS_APDEVICE apdDAL = new DAL_SYS_APDEVICE();
        DAL_SYS_SSID ssidDAL = new DAL_SYS_SSID();
        DAL_SYS_SSID_DEFAULT ssid_defaultDAL = new DAL_SYS_SSID_DEFAULT();
        DAL_SYS_SSID_TEMPLATE ssid_templateDAL = new DAL_SYS_SSID_TEMPLATE();
        DAL_SYS_APORG apOrgDAL = new DAL_SYS_APORG();
        DAL_SYS_APORGLOG apOrgLogDAL = new DAL_SYS_APORGLOG();
        DAL_SYS_ORGANIZATION orgDAL = new DAL_SYS_ORGANIZATION();
        DAL_SYS_BANLIST banDAL = new DAL_SYS_BANLIST();
        DAL_SYS_SETTINGVER settingverDAL = new DAL_SYS_SETTINGVER();
        DAL_SYS_SSID_AUDIT ssidAuditDAL = new DAL_SYS_SSID_AUDIT();
        DAL_SYS_USER userDAL = new DAL_SYS_USER();
        BLL_EMAIL emailBLL = new BLL_EMAIL();
        DAL_AD_AUDIT auditDAL = new DAL_AD_AUDIT();
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
        public bool RegAPDvice(SYS_APDEVICE data, Int64 OID)
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

        public bool RegAPDvices(List<SYS_APDEVICE> datas, Int64 OID)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SYS_APORG aporg = null;
                    SYS_APORGLOG aporglog = null;
                    foreach (SYS_APDEVICE data in datas)
                    {
                        data.LASTHB = DateTime.Parse("2000-1-1 0:0:0");
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

        public bool UpdateAP(Int64 ID, Int64 SERIAL, string MODEL, string MANUFACTURER, string PURCHASER, string FIRMWAREVERSION, Int64 MAXSSIDNUM, bool SUPPORT3G, Int64 APCTID, string DESCRIPTION, Int64 HBINTERVAL, Int64 DATAINTERVAL, bool? ISREBOOT, Int32 CHANNEL, Int32 POWER, Int32 AERIALTYPE, bool ISSSIDON)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = apdDAL.Update(ID, null, SERIAL, MODEL, MANUFACTURER, PURCHASER, FIRMWAREVERSION, MAXSSIDNUM, null, SUPPORT3G, APCTID, DESCRIPTION, null, HBINTERVAL, DATAINTERVAL, true, ISREBOOT, null, null, null, null, CHANNEL, POWER, AERIALTYPE, ISSSIDON);
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

        public bool Update(SYS_APDEVICE data)
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

        public bool Updates(List<SYS_APDEVICE> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_APDEVICE data in datas)
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

        public List<SYS_AP_VIEW> SelectAllAPByOID(Int64 OID, bool isInvalid)
        {
            if (isInvalid)
                return apdDAL.SelectInvalidAPByOID(OID);
            else
                return apdDAL.SelectAllAPByOID(OID, isInvalid);
        }

        public List<SYS_AP_VIEW> SelectAPByOID(Int64 OID, bool isInvalid)
        {
            if (isInvalid)
                return apdDAL.SelectInvalidAPByOID(OID);
            else
                return apdDAL.SelectAPByOID(OID, isInvalid);
        }

        public List<SYS_APDEVICE> SelectAPByOID(Int64 OID)
        {
            return apdDAL.SelectAPByOID(OID);
        }

        public List<SYS_AP_VIEW> SelectAPViewByOID(Int64 OID)
        {
            return apdDAL.SelectAPViewByOID(OID);
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

        public SYS_APDEVICE SelectAPByID(Int64 id)
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

        public List<SYS_APDEVICE> CheckMac(SYS_APDEVICE item)
        {
            return apdDAL.CheckMac(new List<SYS_APDEVICE>() { item });
        }

        public List<SYS_APDEVICE> CheckMacs(List<SYS_APDEVICE> items)
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
                    //
                    List<SYS_SSID_DEFAULT> defaultList = ssid_defaultDAL.SelectByTID(list.First().APCTID);
                    SYS_SSID_DEFAULT defaultSSID = null;
                    List<SYS_SSID> ssidList = ssidDAL.SelectByAPIDs(list.ToString("ID", ","));
                    List<SYS_SSID> ssidTMPList = new List<SYS_SSID>();

                    //string _NAME = xd["Root"]["SSID"]["NAME"].InnerText,
                    //            _ISINTERNET = xd["Root"]["SSID"]["ISINTERNET"].InnerText,
                    //            _MAXLINKCOUNT = xd["Root"]["SSID"]["MAXLINKCOUNT"].InnerText,
                    //            _PORTAL = xd["Root"]["SSID"]["PORTAL"].InnerText,
                    //            _PATH = xd["Root"]["SSID"]["PATH"].InnerText,
                    //            _MAXFLOW = xd["Root"]["SSID"]["MAXFLOW"].InnerText,
                    //            _MAXUS = xd["Root"]["SSID"]["MAXUS"].InnerText,
                    //            _MAXDS = xd["Root"]["SSID"]["MAXDS"].InnerText,
                    //            _VONLINETIME = xd["Root"]["SSID"]["VONLINETIME"].InnerText,
                    //            _VMAXDS = xd["Root"]["SSID"]["VMAXDS"].InnerText,
                    //            _VMAXUS = xd["Root"]["SSID"]["VMAXUS"].InnerText,
                    //            _ISPWD = xd["Root"]["SSID"]["ISPWD"].InnerText,
                    //            _PWD = xd["Root"]["SSID"]["PWD"].InnerText,
                    //            _ACID = xd["Root"]["SSID"]["ACID"].InnerText,
                    //            _ADID = xd["Root"]["SSID"]["ADID"].InnerText;
                    //int isOnCount = Convert.ToInt32(xd["Root"]["SSID"]["ISON"].Attributes["Count"].Value);
                    //int _curSSIDNum = 1;

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

                            SYS_SSID ssid = null;
                            apbyssid = ssidcountList[0];
                            
                            for (int i = 0; i < apbyssid.SSIDCount; i++)
                            {
                                if (i < defaultList.Count)
                                    defaultSSID = defaultList[i];
                                else
                                    defaultSSID = defaultList.Where(c => c.ISDEFAULT).FirstOrDefault();

                                ssid = new SYS_SSID();
                                ssid.ID = -1;
                                ssid.NAME = defaultSSID.NAME;
                                ssid.OID = item.POID;
                                ssid.APID = item.ID;
                                ssid.ISON = defaultSSID.ISON;
                                ssid.ISINTERNET = defaultSSID.ISINTERNET;
                                ssid.MAXLINKCOUNT = defaultSSID.MAXLINKCOUNT;
                                ssid.PORTAL = defaultSSID.PORTAL;
                                ssid.PATH = defaultSSID.PATH;
                                ssid.MAXFLOW = defaultSSID.MAXFLOW;
                                ssid.MAXUS = defaultSSID.MAXUS;
                                ssid.MAXDS = defaultSSID.MAXDS;
                                ssid.VONLINETIME = defaultSSID.VONLINETIME;
                                ssid.VMAXDS = defaultSSID.VMAXDS;
                                ssid.VMAXUS = defaultSSID.VMAXUS;
                                ssid.ISPWD = defaultSSID.ISPWD;
                                ssid.PWD = defaultSSID.PWD;
                                ssid.ACID = defaultSSID.ACID;
                                ssid.ADID =defaultSSID.ADID;
                                ssidList.Add(ssid);
                            }
                        }

                        ssidTMPList = ssidList.Where(c => c.OID == item.POID && c.APID == item.ID).ToList();
                        for (int i = ssidTMPList.Count - 1; i >= 0; i--)
                        {
                            if (i < defaultList.Count)
                                defaultSSID = defaultList[i];
                            else
                                defaultSSID = defaultList.Where(c => c.ISDEFAULT).FirstOrDefault();

                            ssidTMPList[i].OID = item.OID;
                            ssidTMPList[i].NAME = defaultSSID.NAME;                            
                            ssidTMPList[i].ISINTERNET = defaultSSID.ISINTERNET;
                            ssidTMPList[i].MAXLINKCOUNT = defaultSSID.MAXLINKCOUNT;
                            ssidTMPList[i].PORTAL = defaultSSID.PORTAL;
                            ssidTMPList[i].PATH = defaultSSID.PATH;
                            ssidTMPList[i].MAXFLOW = defaultSSID.MAXFLOW;
                            ssidTMPList[i].MAXUS = defaultSSID.MAXUS;
                            ssidTMPList[i].MAXDS = defaultSSID.MAXDS;
                            ssidTMPList[i].VONLINETIME = defaultSSID.VONLINETIME;
                            ssidTMPList[i].VMAXDS = defaultSSID.VMAXDS;
                            ssidTMPList[i].VMAXUS = defaultSSID.VMAXUS;
                            ssidTMPList[i].ISPWD = defaultSSID.ISPWD;
                            ssidTMPList[i].PWD = defaultSSID.PWD;
                            ssidTMPList[i].ACID = defaultSSID.ACID;
                            ssidTMPList[i].ADID = defaultSSID.ADID;

                            if (item.SSIDNUM <= defaultList.Count)
                                ssidTMPList[i].ISON = true;
                            else
                                ssidTMPList[i].ISON = false;
                            item.SSIDNUM--;
                            if (item.SSIDNUM == 0)
                                break;
                        }
                        #endregion

                        #region 生成版本信息
                        settingver = new SYS_SETTINGVER() { ID = -1, APID = item.ID, GUID = Guid.NewGuid().ToString(), DATETIME = DateTime.Now };
                        settingverDAL.Update(settingver);
                        #endregion

                    }

                    foreach (SYS_SSID ssid in ssidList)
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
                apdDAL.Update(PubFun.ChangeNewItem<SYS_APDEVICE, SYS_AP_VIEW>(ap));

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
                    string oids = "";
                    foreach (SYS_APORG apOrg in apOrgList)
                    {
                        SYS_APORGLOG apOrgLog = new SYS_APORGLOG();

                        apOrgLog.APID = apOrg.APID;
                        apOrgLog.EDATE = apOrg.EDATE;
                        apOrgLog.SDATE = apOrg.SDATE;
                        apOrgLog.FOID = apOrg.OID;
                        if (oids != "")
                            oids += ",";
                        oids += apOrg.OID;
                        apOrgLog.TOID = jgID;
                        apOrgLog.OPNAME = "回收";
                        apOrgLog.CREATETIME = DateTime.Now;

                        apOrgLogDAL.Insert(apOrgLog);//回收记录登记
                    }
                    apOrgDAL.Deletes(apOrgList.ToString("ID", ","));
                    flag = apOrgDAL.UpdateBackByOID(jgID, ids);
                    if (flag)
                        flag = ssidDAL.Update(oids, ids, jgID);
                    else
                        throw new Exception("更新AP信息失败!");
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

            List<SYS_ORGANIZATION> orgList = orgDAL.SelectParent(jgID);//获取当前机构的子机构
            List<Int64> orgIds = new List<Int64>();
            String ids = "";
            foreach (SYS_ORGANIZATION org in orgList)
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
        public List<SYS_APDEVICE> SelectApStateListByOID(Int64 orgID, string apname, string column, string orderby)
        {
            //SYS_ORGANIZATION org = orgDAL.Select(orgID);
            return apdDAL.SelectApStateListByOID(orgID, apname, column, orderby);

            //if (org.CATEGORY == ((int)CustomEnum.ENUM_Org_Type.Chain)+"")
            //{
            //List<SYS_APDEVICE> list = null;
            //if(orgID == 0)
            //    list = apdDAL.SelectApStateListByOID(orgID);
            //else {
            //    SYS_ORGANIZATION org = orgDAL.Select(orgID);
            //    if(org != null){
            //        if (org.CATEGORY == ((int)CustomEnum.ENUM_Org_Type.Chain).ToString())
            //            list = apdDAL.SelectApStateListByOID(orgID);
            //        else if(org.CATEGORY == ((int)CustomEnum.ENUM_Org_Type.Single).ToString())
            //            list = apdDAL.SelectApStateListByOIDNoSub(orgID);
            //    }
            //}
            //return list;
        }

        public Int32 GetSSIDNUMByMAC(string mac)
        {
            return apdDAL.GetSSIDNUMByMAC(mac);
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
                        Int64 masterid = orgDAL.SelectPrentAuditOID(user.OID, true);
                        
                        AD_AUDIT audit = new AD_AUDIT();
                        audit.AD_ID = ssid.ADID;
                        audit.ORG_ID = user.OID;
                        audit.PUB_TYPE = (int)Helper.CustomEnum.ENUM_ADC_Type.SSIDOnly;
                        audit.PUB_LIST = ssid.ID.ToString();
                        audit.SSID_NAME = ssid.NAME;                           
                        audit.ISCOPYNAME = 0;
                        audit.FROM_ORG_ID = user.OID;
                        audit.FROM_USER = user.USERNAME;
                        audit.FROM_DATE = DateTime.Now;
                        audit.FROM_TYPE = (int)CustomEnum.ENUM_Aud_Type.NewAudit;
                        audit.TO_ORG_ID = masterid;
                        audit.AUD_STAT = (int)CustomEnum.ENUM_Aud_Stat.WaitAudit;
                        audit.AUD_PARENTID = 0;

                        auditDAL.Insert(audit);

                        //SYS_SSID_AUDIT audSSID = new SYS_SSID_AUDIT();
                        //audSSID.ID = -1;
                        //audSSID.SSIDID = old_ssid.ID;
                        //audSSID.SSIDNAME = ssid.NAME;
                        //audSSID.APID = old_ssid.APID;
                        //audSSID.APPLICANT = user.ACCOUNT;
                        //audSSID.APPLYOID = user.OID;
                        //audSSID.APPLYTIME = DateTime.Now;
                        //audSSID.STATE = (int)CustomEnum.ENUM_Aud_Stat.WaitAudit;
                        //ssidAuditDAL.Update(audSSID);

                        //emailBLL.SSIDAuditEmail(new List<SYS_SSID>() { ssid } );

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

        /// <summary>
        /// 根据机构获取SSID模版
        /// </summary>
        /// <param name="OID"></param>
        /// <returns></returns>
        public List<SYS_SSID_TEMPLATE> GetSSIDTemplateByOID(Int64 OID)
        {
            return ssid_templateDAL.SelectByOID(OID);
        }
        #endregion

        #region 安装
        public bool Install(M_INSTALL info, string token)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    SYS_USER opUser = userDAL.SelectByToken(token);
                    #region 设备信息
                    SYS_APDEVICE ap = apdDAL.SelectByMAC(info.AP_MAC);
                    ap.ALIAS = info.AP_ALIAS;
                    ap.ADDRESS = info.AP_ADDRESS;
                    ap.LAT = info.AP_LAT;
                    ap.LON = info.AP_LON;
                    ap.HBINTERVAL = 300;//info.AP_HBINTERVAL;
                    apdDAL.Update(ap);
                    #endregion

                    #region 如独立管理创建机构和管理员帐号
                    if (info.ORG_ISMANAGE || info.ORG_ID == -1)
                    {
                        SYS_ORGANIZATION org = new SYS_ORGANIZATION();
                        org.CATEGORY = info.ORG_TYPE.ToString();
                        org.ID = info.ORG_ID;
                        org.PID = info.ORG_PID;
                        if (info.ORG_PID > 0)
                            org.PIDHELP = orgDAL.Select(info.ORG_PID).PIDHELP + ",$" + info.ORG_PID + "$";
                        else
                            org.PIDHELP = "$0$";
                        org.NAME = info.ORG_FULLNAME;
                        org.DESCRIPTION = info.ORG_SIMPLENAME;
                        org.CONTACTER = info.ORG_CONTACTER;
                        org.CONTACT = info.ORG_CONTACT;
                        org.PROVINCE = info.ORG_PROVINCE;
                        org.CITY = info.ORG_CITY;
                        org.COUNTIES = info.ORG_COUNTIES;
                        org.INDUSTRY = info.ORG_INDUSTRY;
                        org.AREA = info.ORG_AREA;
                        org.QQ = info.ORG_QQ;
                        org.WEIBO = info.ORG_WEIBO;
                        org.WEIXIN = info.ORG_WEIXIN;
                        info.ORG_ID = orgDAL.Insert(org, 1);

                        SYS_USER user = new SYS_USER();
                        user.ACCOUNT = info.USER_ACCOUNT;
                        user.PWD = info.USER_PWD;
                        user.USERNAME = info.ORG_CONTACTER;
                        user.CONTACT = info.ORG_CONTACT;
                        user.CREATETIME = DateTime.Now;
                        user.USERTYPE = 1;
                        user.STATE = true;
                        user.OID = info.ORG_ID;
                        userDAL.Insert(user);
                    }
                    #endregion

                    #region 设备与机构关联
                    SYS_APORG oaporg = apOrgDAL.SelectByApId(ap.ID, true);
                    SYS_APORG aporg = new SYS_APORG();
                    aporg.APID = ap.ID;
                    aporg.OID = info.ORG_ID;
                    aporg.POID = oaporg.OID;
                    aporg.SSIDNUM = ap.MAXSSIDNUM;
                    aporg.SDATE = DateTime.Parse("2000-01-01");
                    aporg.EDATE = DateTime.Parse("9999-12-31");
                    aporg.ISCHILD = true;
                    apOrgDAL.Insert(aporg);
                    oaporg.ISCHILD = false;
                    apOrgDAL.UpdateCHILD(oaporg);

                    SYS_APORGLOG aporglog = new SYS_APORGLOG();
                    aporglog.APID = ap.ID;
                    aporglog.TOID = info.ORG_ID;
                    aporglog.FOID = oaporg.OID;
                    aporglog.SSIDNUM = ap.MAXSSIDNUM;
                    aporglog.SDATE = DateTime.Parse("2000-01-01");
                    aporglog.EDATE = DateTime.Parse("9999-12-31");
                    aporglog.CREATETIME = DateTime.Now;
                    aporglog.OPNAME = "分配";
                    apOrgLogDAL.Insert(aporglog);
                    #endregion

                    #region 更新SSID信息
                    SYS_SSID ssid = null;
                    // 最终生成的SSID数据
                    List<SYS_SSID> rsList = null;
                    // 生成SSID的数据源，比如前台传来的SSID，代理商的默认设置，门店的默认设置
                    List<M_SSID> sourseList = null;
                    // 安装设备的SSID
                    List<SYS_SSID> sList = ssidDAL.SelectByAPIDs(ap.ID.ToString());
                    List<SYS_SSID_DEFAULT> ssidDefault = null;
                    if (info.DEFAULT == "custom")
                        sourseList = info.SSIDLIST;
                    else if (info.DEFAULT == "agent")
                    {
                        ssidDefault = ssid_defaultDAL.SelectDefaultByOID(info.ORG_PID);
                        sourseList = Helper.PubFun.ChangeNewList<M_SSID, SYS_SSID_DEFAULT>(ssidDefault);
                        while (sourseList.Count < sList.Count)
                        {
                            sourseList.Add(Helper.PubFun.ChangeNewItem<M_SSID, SYS_SSID_DEFAULT>(ssidDefault.Where(c => c.ISDEFAULT).First()));
                        }
                    }
                    else
                        sourseList = Helper.PubFun.ChangeNewList<M_SSID, SYS_SSID>(ssidDAL.SelectByAPIDs(info.DEFAULT));

                    rsList = new List<SYS_SSID>();
                    if (sList.Count > 0)
                    {
                        for (int i = 0; i < sList.Count; i++)
                        {
                            SYS_SSID item = sList[i];
                            if (info.DEFAULT == "agent")
                            {
                                ssid = Helper.PubFun.ChangeNewItem<SYS_SSID, M_SSID>(sourseList[i]);
                                ssid.ID = item.ID;
                                ssid.OID = info.ORG_ID;
                                ssid.APID = ap.ID;
                                if (i < ssidDefault.Count)
                                    ssid.ISON = true;
                                else
                                    ssid.ISON = false;
                            }
                            else
                            {
                                ssid = GetSSIDObj();
                                ssid.ID = item.ID;
                                ssid.OID = info.ORG_ID;
                                ssid.APID = ap.ID;
                                if (sourseList.Count > i)
                                {
                                    ssid.ISPWD = sourseList[i].ISPWD;
                                    ssid.PWD = sourseList[i].PWD;
                                    ssid.NAME = sourseList[i].NAME;
                                    ssid.MAXLINKCOUNT = sourseList[i].MAXLINKCOUNT;
                                    ssid.MAXUS = sourseList[i].MAXUS;
                                    ssid.MAXDS = sourseList[i].MAXDS;
                                    ssid.ISON = true;
                                }
                            }
                            rsList.Add(ssid);
                        }
                    }
                    else
                    {
                        Int32 ssidnum = apdDAL.GetSSIDNUMByMAC(info.AP_MAC);
                        for (int i = 0; i < ssidnum; i++)
                        {
                            ssid = GetSSIDObj();
                            ssid.OID = info.ORG_ID;
                            ssid.APID = ap.ID;
                            if (sourseList.Count > i)
                            {
                                ssid.ISPWD = sourseList[i].ISPWD;
                                ssid.PWD = sourseList[i].PWD;
                                ssid.NAME = sourseList[i].NAME;
                                ssid.MAXLINKCOUNT = sourseList[i].MAXLINKCOUNT;
                                ssid.MAXUS = sourseList[i].MAXUS;
                                ssid.MAXDS = sourseList[i].MAXDS;
                                ssid.ISON = true;
                            }
                            else
                                ssid.NAME = ssid.NAME + (i - sourseList.Count + 1);
                            rsList.Add(ssid);
                        }
                    }

                    SYS_SSID_AUDIT audSSID = new SYS_SSID_AUDIT();

                    SYS_SSID tmp = null;
                    List<SYS_SSID> newSSIDList = new List<SYS_SSID>();
                    XmlDocument xd = new XmlDocument();
                    xd.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultValue.xml"));
                    string _NAME = xd["Root"]["SSID"]["NAME"].InnerText;
                    for (int i = 0; i < rsList.Count;i++ )
                    {
                        tmp = rsList[i];
                        //tmp.NAME += (i + 1);
                        if (info.DEFAULT == "custom")
                        {
                            if (tmp.ISON)
                            {
                                audSSID.ID = -1;
                                audSSID.SSIDID = tmp.ID;
                                audSSID.SSIDNAME = tmp.NAME;
                                audSSID.APID = tmp.APID;
                                audSSID.APPLICANT = opUser.ACCOUNT;
                                audSSID.APPLYOID = info.ORG_ID;
                                audSSID.APPLYTIME = DateTime.Now;
                                audSSID.STATE = (int)CustomEnum.ENUM_Aud_Stat.WaitAudit;
                                ssidAuditDAL.Update(audSSID);
                                newSSIDList.Add(Helper.PubFun.ChangeNewItem<SYS_SSID, SYS_SSID>(tmp));
                            }
                            tmp.NAME = _NAME + (i + 1);
                        }
                        ssidDAL.Update(tmp);
                    }
                    if (info.DEFAULT == "custom")
                        emailBLL.SSIDAuditEmail(newSSIDList);                            


                    #endregion

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
                finally { }
            }
            return flag;
        }

        private SYS_SSID GetSSIDObj()
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultValue.xml"));
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

            SYS_SSID ssid = new SYS_SSID();
            ssid.ID = -1;
            ssid.NAME = _NAME;
            ssid.OID = 0;
            ssid.APID = 0;
            ssid.ISON = false;
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

            return ssid;
        }
        #endregion

        public List<SYS_AP_VIEW> GetAllAPAndORGList()
        {
            return apdDAL.SelectAPAndORG();
        }

        //public M_DeviceStatiscal SelectDeviceStatiscalByOID(long orgId)
        //{
        //    return apdDAL.SelectDeviceStatiscalByOID(orgId);
        //}

        public SYS_APDEVICE GetOrgNameByLatLon(M_LOCATION Location)
        {
            return apdDAL.GetOrgNameByLatLon(Location);
        }
    }
}
