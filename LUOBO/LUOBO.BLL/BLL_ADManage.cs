using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Model;
using LUOBO.Entity;
using System.Text.RegularExpressions;
using LUOBO.Helper;
using System.Configuration;

namespace LUOBO.BLL
{
    public class BLL_ADManage
    {
        //private String AD_TempPath = "ADTemplet";
        //private String UserADPath = "ADUserFile";
        private String ADTempletPath_WEB = ConfigurationSettings.AppSettings["ADTempletPath_WEB"];
        private String ADTempletPath_File = ConfigurationSettings.AppSettings["ADTempletPath_File"];
        private String UserADPath_WEB = ConfigurationSettings.AppSettings["UserADPath_WEB"];
        private String UserADPath_File = ConfigurationSettings.AppSettings["UserADPath_File"];

        DAL_AD_INFO adDAL = new DAL_AD_INFO();
        DAL_SYS_ADTEMPLET adtempletDAL = new DAL_SYS_ADTEMPLET();
        DAL_AD_PUB_CASE adpcDal = new DAL_AD_PUB_CASE();
        DAL_AD_AUDIT audDal = new DAL_AD_AUDIT();
        DAL_SYS_SSID ssidDal = new DAL_SYS_SSID();
        BLL_SYS_ORGANIZATION bll_org = new BLL_SYS_ORGANIZATION();
        BLL_AuditManage bll_audit = new BLL_AuditManage();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="statu"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public M_AD_INFO SelectADByPage(long org_id, int statu, int size, int curPage, String keystr)
        {
            M_AD_INFO m_ad_info = new M_AD_INFO();
            m_ad_info.AllCount = adDAL.SelectCount(org_id, statu, keystr);
            m_ad_info.APCTList = adDAL.Select(org_id, statu, size, curPage, keystr);
            return m_ad_info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="statu"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <param name="keystr"></param>
        /// <returns></returns>
        public M_AD_PUBINFO SelectADPubByPage(long org_id, int statu, int size, int curPage, String keystr)
        {
            M_AD_INFO m_ad_info = SelectADByPage(org_id,  statu,  size,  curPage,  keystr);
            M_AD_PUBINFO result = new M_AD_PUBINFO();
            result.AllCount = m_ad_info.AllCount;
            result.ADList = new List<M_AD_PUB>();
            M_AD_PUB tmp;
            string ids = "";
            for (int i = 0; i < m_ad_info.APCTList.Count; ++i)
            {
                tmp = new M_AD_PUB();
                tmp.PubCount = 0; 
                tmp.ADInfo = m_ad_info.APCTList[i];
                result.ADList.Add(tmp);
                ids += m_ad_info.APCTList[i].AD_ID + ",";
            }
            if (ids.Length > 1)
            {
                ids = ids.Substring(0, ids.Length - 1);
                Dictionary<Int64, Int32> idpubdict = ssidDal.getPubCountBYIds(ids);
                for (int i = 0; i < result.ADList.Count; ++i)
                {
                    if (idpubdict.Keys.Contains(result.ADList[i].ADInfo.AD_ID))
                    {
                        result.ADList[i].PubCount = idpubdict[result.ADList[i].ADInfo.AD_ID];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public List<AD_INFO> SelectADAll(Int64 org_id)
        {
            return adDAL.SelectAll(org_id);
        }

        /// <summary>
        /// 可用模版列表
        /// </summary>
        /// <returns></returns>
        public List<SYS_ADTEMPLET> SelectADTemplet()
        {
            return adtempletDAL.SelectAllPub(); 
        }

        /// <summary>
        /// 模版中文件列表
        /// </summary>
        /// <param name="ADT_ID">模版ID</param>
        /// <returns></returns>
        public List<M_ADTempletFile> GetTempletFiles(int ADT_ID)
        {
            SYS_ADTEMPLET adtemplet = adtempletDAL.SelectPubTemplet(ADT_ID);
            if (adtemplet != null)
            {
                return getTemplet(adtemplet);
            }
            else
            {
                return null;
            }
        }

        private List<M_ADTempletFile> getTemplet(SYS_ADTEMPLET templet)
        {
            //String ADT_Path = AppDomain.CurrentDomain.BaseDirectory + AD_TempPath + "/" + templet.SADT_ID + "/";
            //String ADT_URL = "/" + AD_TempPath + "/" + templet.SADT_ID;
            String ADT_Path = ADTempletPath_File + templet.SADT_ID + "/";
            String ADT_URL = ADTempletPath_WEB + templet.SADT_ID;

            return AD_Templet.ReadTemplet(ADT_Path, ADT_URL, templet.SADT_PORTALFILE);
        }

        public List<M_ADTempletFile> GetADFiles(long ad_id, long org_id)
        {
            AD_INFO ad = adDAL.SelectOne(ad_id);
            //String ADT_Path = AppDomain.CurrentDomain.BaseDirectory + UserADPath + "/" + org_id + "/" + ad_id + "/";
            //String ADT_URL = "/" + UserADPath + "/" + org_id + "/" + ad_id;
            String ADT_Path = UserADPath_File + org_id + "/" + ad_id + "/";
            String ADT_URL = UserADPath_WEB + org_id + "/" + ad_id;
            return AD_Templet.ReadTemplet(ADT_Path, ADT_URL, ad.AD_HomePage);
        }

        /// <summary>
        /// 修改广告内容。（当ad_id>0时修改，否则新增）
        /// </summary>
        /// <param name="ad_id">广告ID</param>
        /// <param name="org_id">机构ID</param>
        /// <param name="ad_title">广告标题</param>
        /// <param name="ad_ssid">SSID显示内容</param>
        /// <param name="ad_model">广告模版ID</param>
        /// <param name="ad_homepage">广告入口页</param>
        /// <param name="ad_type">行业</param>
        /// <param name="pubcount">发布次数</param>
        /// <returns></returns>
        public AD_INFO adModify(long ad_id, long org_id, String ad_title, String ad_ssid, int ad_model, String ad_homepage, int ad_type, int pubcount, String ad_pubpath)
        {
            AD_INFO adinfo = new AD_INFO();
            adinfo.AD_ID = ad_id;
            adinfo.ORG_ID = org_id;
            adinfo.AD_Title = ad_title;
            adinfo.AD_SSID = ad_ssid;
            adinfo.AD_Model = ad_model;
            adinfo.AD_HomePage = ad_homepage;
            adinfo.AD_Type = ad_type;
            adinfo.AD_Release_Count = pubcount;
            adinfo.AD_PUBPATH = ad_pubpath;

            adinfo.AD_Stat = (int)CustomEnum.ENUM_AD_Statu.Modify;
            adinfo.AD_Time = DateTime.Now;

            return adDAL.UpdateAD(adinfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public AD_INFO get_ADInfo(long ad_id)
        {
            return adDAL.SelectOne(ad_id);
        }

        public AD_INFO_FREEHOST SelectOneWithFreeHost(long ad_id)
        {
            return adDAL.SelectOneWithFreeHost(ad_id);
        }
        /// <summary>
        /// 广告发布方案分页内容
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public M_AD_PUB_CASE GetCaseFromPage(long org_id, int size, int curPage)
        {
            M_AD_PUB_CASE data = new M_AD_PUB_CASE();
            data.ACList = adpcDal.Select(org_id, size, curPage);
            data.AllCount = adpcDal.SelectCount(org_id);
            return data;
        }

        /// <summary>
        /// 提交广告审核申请
        /// </summary>
        /// <param name="ad_id">审核广告的ID</param>
        /// <param name="org_id">提交审核的机构ID</param>
        /// <param name="pub_type">审核类型（仅审核广告、审核广告并发布到广告方案或者SSID）</param>
        /// <param name="ids">发布目标列表（发布到SSID时，SSID列表；发布到发布方案是，方案ID列表）</param>
        /// <param name="user_name">管理员名称</param>
        /// <param name="ascase">是否保存为广告发布方案（1-保存；否则不保存）。该参数仅在发布广告到SSID时有效</param>
        /// <returns></returns>
        public String PostAudit(Int64 ad_id, Int64 org_id, int pub_type, String ids, String user_name, int ascase,int isCopyName)
        {
            Int64 masterid = bll_org.GetPrentAuditOID(org_id, true);
            AD_INFO ad = adDAL.SelectOne(ad_id);

            AD_AUDIT audit = new AD_AUDIT();
            audit.AD_ID = ad_id;
            audit.ORG_ID = org_id;
            audit.PUB_TYPE = pub_type;
            audit.PUB_LIST = ids;
            audit.ISCOPYNAME = isCopyName;
            audit.FROM_ORG_ID = org_id;
            audit.FROM_USER = user_name;
            audit.FROM_DATE = DateTime.Now;
            audit.FROM_TYPE = (int)CustomEnum.ENUM_Aud_Type.NewAudit;
            audit.TO_ORG_ID = masterid;
            audit.AUD_STAT = (int)CustomEnum.ENUM_Aud_Stat.WaitAudit;
            audit.AUD_PARENTID = 0;

            if (pub_type == (int)CustomEnum.ENUM_ADC_Type.ToSSID && ascase == 1)
            {
                Int64 caseid = adpcDal.NewCase(org_id, "广告方案-" + ad.AD_Title, ad.AD_ID, ad.AD_Title, ad.AD_SSID, null, null, 1, ids);
                if (caseid < 1)
                {
                    return "广告方案保存失败";
                }
                audit.PUB_TYPE = (int)CustomEnum.ENUM_ADC_Type.ToCase;
                audit.PUB_LIST = caseid.ToString();
            }
            if (ad.AD_Stat == (int)CustomEnum.ENUM_AD_Statu.Audit)
            {
                return "广告处于待审核状态";
            }

            if (ad.AD_Stat == (int)CustomEnum.ENUM_AD_Statu.Publish)
            {
                audit.TO_ORG_ID = -1;
                audit.AUD_STAT = (int)CustomEnum.ENUM_Aud_Stat.AutoPub;
                AD_AUDIT tmp_aud = audDal.Insert(audit);
                if (tmp_aud != null)
                {
                    return bll_audit.HandleAudit(-1, "-", tmp_aud.AUD_ID, 3, "");
                }
                else
                {
                    return "申请提交失败";
                }
            }
            else
            {
                if (!adDAL.ChangeAuditStatu(org_id, ad_id, (int)CustomEnum.ENUM_AD_Statu.Audit, ""))
                {
                    return "广告信息更新失败";
                }

                if (audDal.Insert(audit) != null)
                {
                    BLL_EMAIL bll_email = new BLL_EMAIL();
                    bll_email.ADAuditEmail(audit);
                    return "ok";
                }
                else
                {
                    return "申请提交失败";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public List<M_File> GetServerPics(Int64 org_id)
        {
            String tmpPath = UserADPath_File + org_id;
            if (!System.IO.Directory.Exists(tmpPath))
            {
                System.IO.Directory.CreateDirectory(tmpPath);
                System.IO.Directory.CreateDirectory(tmpPath + "/UserPic");
            }

            String[] pics = System.IO.Directory.GetFiles(tmpPath + "/UserPic");
            List<M_File> flist = new List<M_File>();
            M_File tmpf;
            for (int i = 0; i < pics.Length; ++i)
            {
                tmpf = new M_File();
                //tmpf.FileURL = pics[i].Replace(AppDomain.CurrentDomain.BaseDirectory, "/");
                tmpf.FileName = pics[i].Substring(pics[i].Replace("\\", "/").LastIndexOf("/") + 1);
                tmpf.FileURL = UserADPath_WEB + org_id + "/UserPic/" + tmpf.FileName;
                flist.Add(tmpf);
            }
            return flist;
        }

    }
}
