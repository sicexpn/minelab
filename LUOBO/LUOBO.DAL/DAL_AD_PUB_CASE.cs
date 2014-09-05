using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Model;
using System.Data;
using LUOBO.Entity;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_AD_PUB_CASE
    {
        MySQLDataAccess mySql = new MySQLDataAccess();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="AuditStatu"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public List<M_AD_PUB_CASE_S> Select(long ORG_ID, int size, int curPage)
        {
            List<AD_PUB_CASE> datas = new List<AD_PUB_CASE>();
            string strSql = "SELECT * FROM AD_PUB_CASE ";
            strSql += " WHERE ORG_ID = " + ORG_ID ;
            strSql += " ORDER BY AD_ID ASC LIMIT " + ((curPage - 1) * size) + "," + size;

            DataTable dt = mySql.GetDataTable(strSql, "AD_PUB_CASE");
            datas = DataChange<AD_PUB_CASE>.FillModel(dt);


            List<M_AD_PUB_CASE_S> outData = new List<M_AD_PUB_CASE_S>();
            for (int i = 0; i < datas.Count; ++i)
            {
                outData.Add(new M_AD_PUB_CASE_S
                {
                    CASE = datas[i],
                    SSID_Count = i
                });
            }

            return outData;
        }


        /// <summary>
        /// 查询数据总量
        /// </summary>
        /// <param name="jgName"></param>
        /// <returns></returns>
        public int SelectCount(long ORG_ID)
        {
            string strSql = "SELECT COUNT(1) FROM AD_PUB_CASE where ORG_ID = " + ORG_ID;
            int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
            return count;
        }

        public Boolean Insert(AD_PUB_CASE adcase)
        {
            string strSql = "INSERT INTO AD_AUDIT(ORG_ID, AC_TITLE, AD_ID, AD_TITLE, AD_SSID, AD_PATH, AD_PORTAL, IS_COPYSSID) VALUES(@ORG_ID, @AC_TITLE, @AD_ID, @AD_TITLE, @AD_SSID, @AD_PATH, @AD_PORTAL, @IS_COPYSSID)";
            MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ORG_ID",adcase.ORG_ID),
                    new MySqlParameter("@AC_TITLE",adcase.AC_TITLE),
                    new MySqlParameter("@AD_ID",adcase.AD_ID),
                    new MySqlParameter("@AD_TITLE",adcase.AD_TITLE),
                    new MySqlParameter("@AD_SSID",adcase.AD_SSID),
                    new MySqlParameter("@AD_PATH",adcase.AD_PATH),
                    new MySqlParameter("@AD_PORTAL",adcase.AD_PORTAL),
                    new MySqlParameter("@IS_COPYSSID",adcase.IS_COPYSSID)
                };
            return mySql.ExecuteSQL(strSql, parms);
        }

        public AD_PUB_CASE getNew(long org_id)
        {
            List<AD_PUB_CASE> list = new List<AD_PUB_CASE>();
            string strSql = "SELECT * FROM AD_PUB_CASE where ORG_ID = " + org_id + " order by AC_ID desc limit 0,1 ";
            DataTable dt = mySql.GetDataTable(strSql, "AD_PUB_CASE");
            list = DataChange<AD_PUB_CASE>.FillModel(dt);
            return list[0];
        }

        /// <summary>
        /// 新建广告发布方案
        /// </summary>
        /// <param name="org_id">机构ID</param>
        /// <param name="title">方案名</param>
        /// <param name="ad_id">广告ID</param>
        /// <param name="ad_title">广告名称</param>
        /// <param name="SSID">SSID显示内容</param>
        /// <param name="ad_path">广告路径</param>
        /// <param name="ad_portal">广告入口页</param>
        /// <param name="iscopy">是否同步SSID显示名称</param>
        /// <param name="ids">应用本发布方案的SSID列表</param>
        /// <returns></returns>
        public long NewCase(long org_id,String title,long ad_id,String ad_title,String SSID,String ad_path,String ad_portal,int iscopy,String ids)
        {
            AD_PUB_CASE adcase = new AD_PUB_CASE();
            adcase.AC_ID = 0;
            adcase.ORG_ID = org_id;
            adcase.AC_TITLE = title;
            adcase.AD_ID = ad_id;
            adcase.AD_TITLE = ad_title;
            adcase.AD_SSID = SSID;
            adcase.AD_PATH = ad_path;
            adcase.AD_PORTAL = ad_portal;
            adcase.IS_COPYSSID = iscopy;
            if (Insert(adcase))
            {
                adcase = getNew(adcase.ORG_ID);
                if (ids.Trim().Length > 0)
                {
                    string strSql = "UPDATE SYS_SSID SET ACID = " + adcase.AC_ID + " WHERE ID IN (" + ids + ")";
                    mySql.ExecuteSQL(strSql);
                }
                return adcase.AC_ID;
            }
            else
            {
                return 0;
            }
        }
    }
}
