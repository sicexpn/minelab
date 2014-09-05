using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Helper;
using LUOBO.Model;
using System.Configuration;
using System.IO;

namespace LUOBO.BLL
{
    public class BLL_SYS_SSID
    {
        DAL_SYS_SSID sDAL = new DAL_SYS_SSID();
        DAL_SYS_APDEVICE apDAL = new DAL_SYS_APDEVICE();
        DAL_SYS_SSID_AUDIT sAudDAL = new DAL_SYS_SSID_AUDIT();
        DAL_SYS_SETTINGVER setverDAL = new DAL_SYS_SETTINGVER();
        private string AD_ROOT = ConfigurationSettings.AppSettings["UserFile_Path"];

        public SYS_SSID SelectByID(Int64 id)
        {
            return sDAL.SelectByID(id);
        }

        public List<SYS_SSID> SelectByOID(Int64 oid, bool ison)
        {
            return sDAL.SelectByOID(oid, ison);
        }

        public int SelectCountByOID(Int64 oid, bool ison)
        {
            return sDAL.SelectCountByOID(oid, ison);
        }

        public List<M_Statistical> SelectStatisticalSSIDByToken(string token, Int64 apid, DateTime startTime, DateTime endTime)
        {
            DAL_SYS_USER userDAL = new DAL_SYS_USER();
            SYS_USER user = userDAL.SelectByToken(token);
            List<M_Statistical> list = sDAL.SelectStatisticalSSIDByOID(user.OID, apid, startTime, endTime);
            for (int i = 0; i < list.Count; i++)
                list[i].ID = i + 1;

            return list;
        }

        public M_SSID_VIEW GetSSIDByOID(int size, int curPage, Int64 oid)
        {
            M_SSID_VIEW mSSID = new M_SSID_VIEW();
            mSSID.AllCount = sDAL.SelectCountByOID(oid, true);
            mSSID.SSIDList = sDAL.SelectByOID(size, curPage, oid);
            return mSSID;
        }

        public bool AuditSSID(string ids, Int64 audOID, string account, string auditIntro)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<SYS_SSID_AUDIT> ssid_audit_list = sAudDAL.Select(ids);
                    string ssidIDs = ssid_audit_list.ToString("SSIDID", ",");
                    List<SYS_SSID> ssid_list = sDAL.SelectByIDs(ssidIDs);
                    SYS_SSID ssidTmp;
                    SYS_SETTINGVER setver = null;
                    foreach (var item in ssid_audit_list)
                    {
                        setver = new SYS_SETTINGVER();
                        setver.ID = -1;
                        setver.APID = item.APID;
                        setver.TYPE = (int)CustomEnum.ENUM_Setting_Type.Setting;
                        setver.GUID = Guid.NewGuid().ToString();
                        setver.DATETIME = DateTime.Now;
                        setverDAL.Update(setver);
                        ssidTmp = ssid_list.Where(c => c.ID == item.SSIDID).FirstOrDefault();
                        if(ssidTmp != null)
                        {
                            ssidTmp.NAME = item.SSIDNAME;
                            sDAL.Update(ssidTmp);
                        }
                    }

                    flag = sAudDAL.UpdateForState(ids, audOID, account, auditIntro, (int)CustomEnum.ENUM_Aud_Stat.PassAudit);
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

        public bool NoAuditSSID(string ids, Int64 audOID, string account, string auditIntro)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = sAudDAL.UpdateForState(ids, audOID, account, auditIntro, (int)CustomEnum.ENUM_Aud_Stat.CansleAudit);
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

        public bool BackAuditSSID(string ids, Int64 audOID, string account, string auditIntro)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = sAudDAL.UpdateForState(ids, audOID, account, auditIntro, (int)CustomEnum.ENUM_Aud_Stat.BackAudit);
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

        public bool DelAuditSSID(string ids)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    flag = sAudDAL.Deletes(ids);
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

        public List<M_WCF_SSID_VIEW> SelectWcfSSIDViewByOID(Int64 oid)
        {
            return sDAL.SelectWcfSSIDViewByOID(oid);
        }

        public List<M_WCF_SSID_VIEW> SelectWcfSSIDViewByAPID(Int64 apid)
        {
            List<M_WCF_SSID_VIEW> list = sDAL.SelectWcfSSIDViewByAPID(apid);            
            string fileName = "";
            string path = "";
            string allPath = "";
            foreach(var item in list)
            {
                fileName = item.PATH.Substring(item.PATH.LastIndexOf('/', item.PATH.Length - 2, item.PATH.Length - 1) + 1);
                fileName = fileName.Substring(0, fileName.Length - 1);

                path = item.PATH.Substring(0, item.PATH.LastIndexOf('/'+fileName+'/')+1);
                allPath = AD_ROOT + path.Replace("Pub", "Download") + fileName + ".tar.gz";
                if (!File.Exists(allPath))
                    ;//BLL_ZipQueue.Instance().Push(item.PATH);
                else
                    item.DOWNPATH = path.Replace("Pub", "Download") + fileName + ".tar.gz";
            }
            return list;
        }

        public List<SYS_SSID> SelectByOwnerAndIsOn(Int64 oid, Int64 apid, bool isOn)
        {
            return sDAL.SelectByOwnerAndIsOn(oid, apid, isOn);
        }

        public List<M_SSID_AP> GetSSIDInfoByIDs(Int64 audid)
        {
            DAL.DAL_AD_AUDIT auditDal = new DAL_AD_AUDIT();
            DAL.DAL_AD_AUDIT_HISTORY audHisDal = new DAL_AD_AUDIT_HISTORY();
            DAL.DAL_SYS_SSID ssidDal = new DAL_SYS_SSID();
            AD_AUDIT audit = audHisDal.select(audid);
            if (audit == null)
            {
                audit = auditDal.select(audid);
            }
            if (audit != null)
            {
                return ssidDal.GetSSIDInfoByIDs(audit.PUB_LIST);
            }
            return null;
        }
    }
}
