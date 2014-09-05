using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Model;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Helper;
using System.Configuration;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_AuditManage
    {
        DAL_AD_INFO dal_ad = new DAL_AD_INFO();
        DAL_AD_AUDIT audDal = new DAL_AD_AUDIT();
        DAL_AD_AUDIT_HISTORY audHisDal = new DAL_AD_AUDIT_HISTORY();
        BLL_SYS_ORGANIZATION bll_org = new BLL_SYS_ORGANIZATION();
        DAL_SYS_SSID dal_ssid = new DAL_SYS_SSID();
        DAL_SYS_SETTINGVER dal_settingver = new DAL_SYS_SETTINGVER();

        //private String UserADPath = "ADUserFile";
        private String UserADPath_WEB = ConfigurationSettings.AppSettings["UserADPath_WEB"];
        private String UserADPath_File = ConfigurationSettings.AppSettings["UserADPath_File"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public M_AD_AUDIT SelectAuditByPage(Int64 org_id, int size, int curPage, int aud_statu)
        {
            M_AD_AUDIT m_info = new M_AD_AUDIT();
            m_info.AllCount = audDal.SelectCount(org_id, aud_statu);
            m_info.AuditList = audDal.Select(org_id, size, curPage, aud_statu);
            return m_info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public M_AD_AUDIT SelectAuditHisrtoryByPage(Int64 org_id, int size, int curPage, int aud_statu)
        {
            M_AD_AUDIT m_info = new M_AD_AUDIT();
            m_info.AllCount = audHisDal.SelectCount(org_id, aud_statu);
            m_info.AuditList = audHisDal.Select(org_id, size, curPage, aud_statu);
            return m_info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public List<AD_AUDIT> GetAuditProgress(Int64 aud_id)
        {
            return audDal.getAuditProgress(aud_id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public List<AD_AUDIT> GetAuditHistoryProgress(Int64 aud_id)
        {
            return audHisDal.getAuditProgress(aud_id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="username"></param>
        /// <param name="aud_id"></param>
        /// <param name="handle"></param>
        /// <param name="auditstr"></param>
        /// <returns></returns>
        public String HandleAudit(Int64 org_id, String username, Int64 aud_id, int handle, String auditstr)
        {
            string resultstr = "";
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    AD_AUDIT theaudit = audDal.select(aud_id);

                    AD_AUDIT audit = new AD_AUDIT();
                    audit.AD_ID = theaudit.AD_ID;
                    audit.ORG_ID = theaudit.ORG_ID;
                    audit.PUB_TYPE = theaudit.PUB_TYPE;
                    audit.SSID_NAME = theaudit.SSID_NAME;
                    audit.PUB_LIST = theaudit.PUB_LIST;
                    audit.ISCOPYNAME = theaudit.ISCOPYNAME;
                    audit.FROM_ORG_ID = org_id;
                    audit.FROM_USER = username;
                    audit.FROM_DATE = DateTime.Now;
                    if (theaudit.AUD_PARENTID > 0)
                    {
                        audit.AUD_PARENTID = theaudit.AUD_PARENTID;
                    }
                    else
                    {
                        audit.AUD_PARENTID = theaudit.AUD_ID;
                    }
                    audit.AUD_CONTENT = auditstr;
                    AD_INFO tmp_adinfo;
                    switch (handle)
                    {
                        case 1: //通过审核
                            audit.FROM_TYPE = (int)CustomEnum.ENUM_Aud_Type.PassAudit;

                            Boolean isEndVerify = false;
                            if (org_id > 0)
                            {
                                SYS_ORGANIZATION theOrgInfo = bll_org.Select(org_id);
                                if (theOrgInfo.ISVERIFY_END)
                                {
                                    audit.TO_ORG_ID = -1;
                                    audit.AUD_STAT = -1;
                                    isEndVerify = true;
                                }
                                else
                                {
                                    audit.TO_ORG_ID = bll_org.GetPrentAuditOID(org_id, false);
                                    audit.AUD_STAT = (int)CustomEnum.ENUM_Aud_Stat.WaitAudit;
                                }
                            }
                            else
                            {
                                isEndVerify = true;
                                audit.TO_ORG_ID = -1;
                                audit.AUD_STAT = -1;
                            }

                            if (audDal.Insert(audit) != null)
                            {
                                if (audDal.ChangeSTAT(aud_id, (int)CustomEnum.ENUM_Aud_Stat.AuditIng))
                                {
                                    if (isEndVerify)
                                    {
                                        if (audDal.ChangeTreeStatu(audit.AUD_PARENTID, (int)CustomEnum.ENUM_Aud_Stat.PassAudit))
                                        {
                                            if (theaudit.PUB_TYPE == (int)Helper.CustomEnum.ENUM_ADC_Type.SSIDOnly)
                                            {
                                                if (PushSSID(theaudit))
                                                {
                                                    resultstr = "ok";
                                                }
                                                else
                                                {
                                                    resultstr = "SSID推送失败";
                                                }
                                            }
                                            else
                                            {
                                                if (PushAD(theaudit))
                                                {
                                                    tmp_adinfo = dal_ad.SelectOne(theaudit.AD_ID);
                                                    BLL_ZipQueue.Instance().Push(tmp_adinfo.AD_PUBPATH);
                                                    resultstr = "ok";
                                                }
                                                else
                                                {
                                                    resultstr = "广告推送失败";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            resultstr = "审核信息修改错误！";
                                        }
                                    }
                                    else
                                    {
                                        resultstr = "ok";
                                    }
                                }
                                else
                                {
                                    resultstr = "审核信息状态错误！";
                                }
                            }
                            else
                            {
                                resultstr = "审核信息写入错误！";
                            }
                            break;
                        case 2:         //拒绝审核
                            audit.FROM_TYPE = (int)CustomEnum.ENUM_Aud_Type.CansleAudit;
                            audit.TO_ORG_ID = -1;
                            audit.AUD_STAT = -1;

                            if (audDal.Insert(audit) != null)
                            {
                                if (audDal.ChangeTreeStatu(theaudit.AUD_PARENTID, (int)CustomEnum.ENUM_Aud_Stat.CansleAudit))
                                {
                                    dal_ad.ChangeAuditStatu(theaudit.ORG_ID, theaudit.AD_ID, (int)CustomEnum.ENUM_AD_Statu.Cansle, "");
                                    resultstr = "ok";
                                }
                                else
                                {
                                    resultstr = "审核信息修改错误！";
                                }
                            }
                            else
                            {
                                resultstr = "审核信息写入错误！";
                            }
                            break;
                        case 3:     //自动审核
                            tmp_adinfo = dal_ad.SelectOne(theaudit.AD_ID);
                            if (tmp_adinfo.AD_Stat == (int)CustomEnum.ENUM_AD_Statu.Publish && tmp_adinfo.AD_PUBPATH.Length > 0)
                            {
                                Boolean ic = false;
                                if (theaudit.ISCOPYNAME == 1)
                                {
                                    ic = true;
                                }
                                if (audDal.MoveTreeToHistory(theaudit.AUD_ID))
                                {
                                    if (dal_ssid.PubSSIDFromAD(theaudit.PUB_TYPE, theaudit.PUB_LIST, theaudit.AD_ID, tmp_adinfo.AD_PUBPATH, ic))
                                    {
                                        resultstr = "ok";
                                    }
                                    else
                                    {
                                        resultstr = "发布出错";
                                    }
                                }
                                else
                                {
                                    resultstr = "自动审核出现错误";
                                }
                            }
                            else
                            {
                                resultstr = "广告信息错误！";
                            }
                            break;
                        default:
                            resultstr = "非法的审核操作";
                            break;
                    }
                    if (resultstr == "ok")
                    {
                        scope.Complete();
                    }
                    else
                    {
                        scope.Dispose();
                    }

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }
            return resultstr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        private Boolean PushAD(AD_AUDIT audit)
        {
            if (audit.PUB_TYPE == (int)CustomEnum.ENUM_ADC_Type.ADOnly)
            {
                return true;
            }
            //String UserPath = AppDomain.CurrentDomain.BaseDirectory + UserADPath + "/" + audit.ORG_ID + "/";
            String UserPath = UserADPath_File + audit.ORG_ID + "/";
            String AD_ID = audit.AD_ID.ToString();
            String AD_Pub_Dir = "Pub";
            String dirname = Helper.AD_Templet.CreatePubAD(UserPath, AD_ID, AD_Pub_Dir);
            if (dirname.Length > 0)
            {
                //String adPath = "/" + UserADPath + "/" + audit.ORG_ID + "/" + AD_Pub_Dir + "/" + dirname + "/";
                String adPath = UserADPath_WEB + audit.ORG_ID + "/" + AD_Pub_Dir + "/" + dirname + "/";

                dal_ad.ChangeAuditStatu(audit.ORG_ID, audit.AD_ID, (int)CustomEnum.ENUM_AD_Statu.Publish, adPath);
                Boolean ic = false;
                if (audit.ISCOPYNAME == 1)
                {
                    ic = true;
                }
                return dal_ssid.PubSSIDFromAD(audit.PUB_TYPE, audit.PUB_LIST, audit.AD_ID, adPath, ic);
            }
            else
            {
                return false;
            }
        }

        private Boolean PushSSID(AD_AUDIT audit)
        {
            bool flag = false;

            List<SYS_SSID> ssid_list = dal_ssid.SelectByIDs(audit.PUB_LIST);
            List<Int64> apids = ssid_list.Select(c => c.APID).Distinct().ToList();

            SYS_SETTINGVER setver = null;
            foreach (var apid in apids)
            {
                setver = new SYS_SETTINGVER();
                setver.ID = -1;
                setver.APID = apid;
                setver.TYPE = (int)CustomEnum.ENUM_Setting_Type.Setting;
                setver.GUID = Guid.NewGuid().ToString();
                setver.DATETIME = DateTime.Now;
                dal_settingver.Update(setver);
            }

            foreach (var item in ssid_list)
            {
                item.NAME = audit.SSID_NAME;
                dal_ssid.Update(item);
            }
            flag = true;
            
            return flag;
        }
    }
}
