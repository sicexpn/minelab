using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_AD_INFO
    {
        MySQLDataAccess mySql = new MySQLDataAccess();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ORG_ID"></param>
        /// <returns></returns>
        public AD_INFO SelectOne(long AD_ID)
        {
            List<AD_INFO> list = new List<AD_INFO>();
            string strSql = "SELECT * FROM AD_INFO where AD_ID = " + AD_ID + " limit 0,1 ";
            DataTable dt = mySql.GetDataTable(strSql, "AD_INFO");
            list = DataChange<AD_INFO>.FillModel(dt);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public AD_INFO_FREEHOST SelectOneWithFreeHost(long AD_ID)
        {
            AD_INFO_FREEHOST data = null;
            string strSql = "SELECT a.*,b.AF_ID,b.F_Host,b.F_Domain,b.F_Default FROM ad_info a left join ad_freehost b on a.AD_ID = b.AD_ID where a.AD_ID=@AD_ID";
            
            MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@AD_ID",AD_ID)
            };
            DataTable dt = mySql.GetDataTable(strSql, "ad_info", parms);

            if (dt.Rows.Count > 0)
            {
                data = DataChange<AD_INFO_FREEHOST>.FillEntity(dt.Rows[0]);
            }
            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="adinfo"></param>
        /// <returns></returns>
        public AD_INFO UpdateAD(AD_INFO adinfo)
        {
            if (adinfo.AD_ID > 0)
            {
                string strSql = "UPDATE AD_INFO SET ORG_ID = @ORG_ID, AD_Title = @AD_Title,AD_SSID = @AD_SSID, AD_HomePage = @AD_HomePage, AD_Type = @AD_Type, AD_Model = @AD_Model, AD_Time = @AD_Time, AD_Stat = @AD_Stat, AD_Release_Count = @AD_Release_Count, AD_PUBPATH = @AD_PUBPATH WHERE AD_ID = @AD_ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ORG_ID",adinfo.ORG_ID),
                    new MySqlParameter("@AD_Title",adinfo.AD_Title),
                    new MySqlParameter("@AD_SSID",adinfo.AD_SSID),
                    new MySqlParameter("@AD_HomePage",adinfo.AD_HomePage),
                    new MySqlParameter("@AD_Type",adinfo.AD_Type),
                    new MySqlParameter("@AD_Model",adinfo.AD_Model),
                    new MySqlParameter("@AD_Time",adinfo.AD_Time),
                    new MySqlParameter("@AD_Stat",adinfo.AD_Stat),
                    new MySqlParameter("@AD_Release_Count",adinfo.AD_Release_Count),
                    new MySqlParameter("@AD_PUBPATH",adinfo.AD_PUBPATH),
                    new MySqlParameter("@AD_ID",adinfo.AD_ID)
                };
                if (mySql.ExecuteSQL(strSql, parms))
                {
                    return adinfo;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string strSql = "INSERT INTO AD_INFO(ORG_ID, AD_Title, AD_SSID, AD_HomePage, AD_Type, AD_Model, AD_Time,AD_Stat, AD_Release_Count, AD_PUBPATH) VALUES(@ORG_ID, @AD_Title, @AD_SSID, @AD_HomePage, @AD_Type, @AD_Model, @AD_Time, @AD_Stat, @AD_Release_Count, @AD_PUBPATH)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ORG_ID",adinfo.ORG_ID),
                    new MySqlParameter("@AD_Title",adinfo.AD_Title),
                    new MySqlParameter("@AD_SSID",adinfo.AD_SSID),
                    new MySqlParameter("@AD_HomePage",adinfo.AD_HomePage),
                    new MySqlParameter("@AD_Type",adinfo.AD_Type),
                    new MySqlParameter("@AD_Model",adinfo.AD_Model),
                    new MySqlParameter("@AD_Time",adinfo.AD_Time),
                    new MySqlParameter("@AD_Stat",adinfo.AD_Stat),
                    new MySqlParameter("@AD_Release_Count",adinfo.AD_Release_Count),
                    new MySqlParameter("@AD_PUBPATH",adinfo.AD_PUBPATH)
                };
                if (mySql.ExecuteSQL(strSql, parms))
                {
                    return SelectNew(adinfo.ORG_ID);
                }
                else
                {
                    return null;
                }
            }
        }

        private AD_INFO SelectNew(long ORG_ID)
        {
            List<AD_INFO> list = new List<AD_INFO>();
            string strSql = "SELECT * FROM AD_INFO where ORG_ID = " + ORG_ID + " order by AD_ID desc limit 0,1 ";
            DataTable dt = mySql.GetDataTable(strSql, "AD_INFO");
            list = DataChange<AD_INFO>.FillModel(dt);
            return list[0];
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<AD_INFO> SelectAll(long ORG_ID)
        {
            List<AD_INFO> list = new List<AD_INFO>();
            string strSql = "SELECT * FROM AD_INFO where ORG_ID = " + ORG_ID;
            DataTable dt = mySql.GetDataTable(strSql, "AD_INFO");
            list = DataChange<AD_INFO>.FillModel(dt);
            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="AuditStatu"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public List<AD_INFO> Select(long ORG_ID, int AuditStatu, int size, int curPage, String keystr)
        {
            List<AD_INFO> datas = new List<AD_INFO>();
            string strw = "";
            if (AuditStatu >= 0)
            {
                strw += " and AD_Stat = " + AuditStatu;
            }
            if (keystr.Trim().Length > 0)
            {
                strw += " and ( AD_Title like '%" + keystr.Trim() + "%' or AD_SSID like '%" + keystr.Trim() + "%' )";
            }
            string strSql = "SELECT * FROM AD_INFO";
            strSql += " WHERE ORG_ID = " + ORG_ID + strw;
            strSql += " ORDER BY AD_ID ASC LIMIT " + ((curPage - 1) * size) + "," + size;
            DataTable dt = mySql.GetDataTable(strSql, "AD_INFO");
            datas = DataChange<AD_INFO>.FillModel(dt);
            return datas;
        }


        /// <summary>
        /// 查询数据总量
        /// </summary>
        /// <param name="jgName"></param>
        /// <returns></returns>
        public int SelectCount(long ORG_ID, int AuditStatu, String keystr)
        {
            string strw = "";
            if (AuditStatu >= 0)
            {
                strw += " and AD_Stat = " + AuditStatu;
            }
            if (keystr.Trim().Length > 0)
            {
                strw += " and ( AD_Title like '%" + keystr.Trim() + "%' or AD_SSID like '%" + keystr.Trim() + "%' )";
            }
            string strSql = "SELECT COUNT(1) FROM AD_INFO WHERE ORG_ID = " + ORG_ID + strw;
            int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
            return count;
        }


        /// <summary>
        /// 更改广告审核状态
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="ad_id"></param>
        /// <param name="statu"></param>
        /// <returns></returns>
        public Boolean ChangeAuditStatu(long org_id, long ad_id, int statu, String pub_path)
        {
            string strSql = "UPDATE AD_INFO SET AD_Stat = @AD_Stat, AD_PUBPATH = @AD_PUBPATH WHERE ORG_ID = @ORG_ID and AD_ID = @AD_ID ";
            MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@AD_Stat",statu),
                    new MySqlParameter("@AD_PUBPATH",pub_path),
                    new MySqlParameter("@ORG_ID",org_id),
                    new MySqlParameter("@AD_ID",ad_id)
                };
            return mySql.ExecuteSQL(strSql, parms);
        }
    }
}
