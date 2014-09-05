using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using System.Data;
using LUOBO.Access;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_AD_AUDIT_HISTORY
    {
        /// <summary>
        /// 获取指定审核记录
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public AD_AUDIT select(Int64 aud_id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                AD_AUDIT data = null;
                string strSql = "SELECT * FROM ad_audit_history WHERE AUD_ID = @AUD_ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@AUD_ID",aud_id)
            };
                DataTable dt = mySql.GetDataTable(strSql, "ad_audit_history", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<AD_AUDIT>.FillEntity(dt.Rows[0]);
                return data;
            }
        }


        /// <summary>
        /// 获取审核记录列表
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="pagesize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<AD_AUDIT> Select(long org_id, int pagesize, int page, int aud_statu)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strw = "";
                if (aud_statu >= 0)
                {
                    strw += " and a.AUD_STAT = " + aud_statu;
                }
                List<AD_AUDIT> datas = new List<AD_AUDIT>();
                string strSql = "SELECT a.*,b.AD_Title as AD_NAME_V,b.AD_SSID as AD_SSID_V,c.NAME as ORG_NAME_V,d.NAME as FROM_ORG_NAME_V FROM ad_audit_history a ";
                strSql += " LEFT JOIN ad_info b ON a.AD_ID = b.AD_ID ";
                strSql += " LEFT JOIN sys_organization c on a.ORG_ID = c.ID ";
                strSql += " LEFT JOIN sys_organization d on a.FROM_ORG_ID = d.ID ";
                strSql += " WHERE a.TO_ORG_ID = " + org_id + strw + " and a.AUD_PARENTID=0";
                strSql += " ORDER BY a.FROM_DATE DESC LIMIT " + ((page - 1) * pagesize) + "," + pagesize;
                DataTable dt = mySql.GetDataTable(strSql, "ad_audit_history");
                datas = DataChange<AD_AUDIT>.FillModel(dt);
                return datas;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public int SelectCount(long org_id, int aud_statu)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strw = "";
                if (aud_statu >= 0)
                {
                    strw += " and AUD_STAT = " + aud_statu;
                }
                string strSql = "SELECT COUNT(1) FROM ad_audit_history WHERE TO_ORG_ID = " + org_id + strw + " and AUD_PARENTID=0";
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }

        /// <summary>
        /// 获取历史记录的审核进度
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public List<AD_AUDIT> getAuditProgress(long aud_id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<AD_AUDIT> datas = new List<AD_AUDIT>();
                string strSql = "SELECT a.*,ifnull(d.NAME,'总部') as FROM_ORG_NAME_V FROM ad_audit_history a ";
                strSql += " LEFT JOIN sys_organization d on a.FROM_ORG_ID = d.ID ";
                strSql += " WHERE a.AUD_ID = " + aud_id + " or AUD_PARENTID = " + aud_id;
                strSql += " ORDER BY a.AUD_ID ASC ";
                DataTable dt = mySql.GetDataTable(strSql, "ad_audit_history");
                datas = DataChange<AD_AUDIT>.FillModel(dt);
                return datas;
            }
        }
    }
}
