using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;
using LUOBO.Model;
namespace LUOBO.DAL
{
    public class DAL_SYS_LOG_ALERT
    {
        public Int64 GetAlertCounts(long oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(*) FROM SYS_LOG_ALERT WHERE OID=@OID";
                MySqlParameter[] parms = new MySqlParameter[]{
                    new MySqlParameter("@OID",oid)
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public List<LUOBO.Model.M_Alert_Object> GetAlertListNotHandle(long oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<LUOBO.Model.M_Alert_Object> list = new List<LUOBO.Model.M_Alert_Object>();
                //string strSql = "SELECT a.*,b.ALIAS FROM SYS_LOG_ALERT a,sys_apdevice b WHERE  a.AP_MAC=b.MAC and a.OID=@OID AND a.ISPROCESS=@ISPROCESS order by a.G_TIME DESC";
                ////string strSql = "SELECT * FROM SYS_LOG_ALERT WHERE OID=@OID AND ISPROCESS=@ISPROCESS";
                //MySqlParameter[] parms = new MySqlParameter[]{
                //    new MySqlParameter("@OID",oid),
                //    new MySqlParameter("@ISPROCESS",0)
                //};
                //DataTable dt = mySql.GetDataTable(strSql, "M_Alert_Object", parms);
                string strSql = "SELECT a.LOG_ID,a.OID,a.AP_MAC,a.G_SSID,a.G_MAC,a.G_TIME,a.G_STRONG,a.CHANNEL,a.ISPROCESS,a.ISWHITELIST,a.KEYWORD,a.Similarity,a.FIRSTTIME,a.SCANCOUNT,b.ALIAS as APNAME FROM SYS_LOG_ALERT a,sys_apdevice b WHERE  a.AP_MAC=b.MAC and a.OID=" + oid + " AND a.ISPROCESS=0 order by a.G_TIME DESC";
                DataTable dt = mySql.GetDataTable(strSql, "M_Alert_Object");
                if (dt.Rows.Count > 0)
                    list = DataChange<LUOBO.Model.M_Alert_Object>.FillModel(dt);
                return list;
            }
        }

        public List<Model.M_Alert_Object> GetAlertListByMAC(long oid, string MAC)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<LUOBO.Model.M_Alert_Object> list = new List<LUOBO.Model.M_Alert_Object>();
                //string strSql = "SELECT a.*,b.ALIAS FROM SYS_LOG_ALERT a,sys_apdevice b WHERE  a.AP_MAC=b.MAC and a.OID=@OID AND a.ISPROCESS=@ISPROCESS order by a.G_TIME DESC";
                ////string strSql = "SELECT * FROM SYS_LOG_ALERT WHERE OID=@OID AND ISPROCESS=@ISPROCESS";
                //MySqlParameter[] parms = new MySqlParameter[]{
                //    new MySqlParameter("@OID",oid),
                //    new MySqlParameter("@ISPROCESS",0)
                //};
                //DataTable dt = mySql.GetDataTable(strSql, "M_Alert_Object", parms);
                string strSql = "SELECT a.*,b.ALIAS as APNAME FROM SYS_LOG_ALERT a,sys_apdevice b WHERE  a.AP_MAC=b.MAC and a.OID=" + oid + " AND a.G_MAC='" + MAC + "' order by a.G_TIME DESC";
                DataTable dt = mySql.GetDataTable(strSql, "M_Alert_Object");
                if (dt.Rows.Count > 0)
                    list = DataChange<LUOBO.Model.M_Alert_Object>.FillModel(dt);
                return list;
            }
        }

        public void Save(string mac, List<SYS_LOG_ALERT> listalert)
        {
            //using (MySQLDataAccess mySql = new MySQLDataAccess())
            //{
            //    DataTable dt = mySql.GetDataTable("Select * from SYS_LOG_ALERT where AP_MAC='" + mac + "'", "SYS_LOG_ALERT");
            //    for (int i = 0; i < dt.Rows.Count; ++i)
            //    {
            //        dt.Rows[i].Delete();
            //    }
            //    for (int i = 0; i < listalert.Count; ++i)
            //    {
            //        DataRow dr = dt.NewRow();
            //        dt.Rows.Add(DataChange<Entity.SYS_LOG_ALERT>.FillRow(listalert[i], dr));
            //    }
            //    mySql.Update(dt);
            //}
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_LOG_ALERT where AP_MAC='" + mac + "'", "SYS_LOG_ALERT");
                DataRow[] rs;
                DataRow dr = null;
                for (int i = 0; i < listalert.Count; ++i)
                {
                    rs = dt.Select("G_MAC='" + listalert[i].G_MAC + "'");
                    if (rs.Length > 0)
                    {
                        rs[0]["KEYWORD"] = listalert[i].KEYWORD;
                        rs[0]["G_TIME"] = listalert[i].G_TIME;
                        rs[0]["SCANCOUNT"] = (Int64)rs[0]["SCANCOUNT"] + 1;
                    }
                    else
                    {
                        listalert[i].FIRSTTIME = listalert[i].G_TIME;
                        listalert[i].SCANCOUNT = 1;
                        dr = dt.NewRow();
                        dt.Rows.Add(DataChange<Entity.SYS_LOG_ALERT>.FillRow(listalert[i], dr));
                    }
                }
                mySql.Update(dt);
            }
        }

        public bool Update(SYS_LOG_ALERT data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_LOG_ALERT where 1<>1", "SYS_LOG_ALERT");
                if (data.LOG_ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<SYS_LOG_ALERT>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_LOG_ALERT where LOG_ID=" + data.LOG_ID.ToString(), "SYS_LOG_ALERT");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<SYS_LOG_ALERT>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public SYS_LOG_ALERT Select(Int64 ID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_LOG_ALERT alert = null;
                string strSql = "SELECT * FROM SYS_LOG_ALERT WHERE LOG_ID=" + ID;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_LOG_ALERT");
                if (dt.Rows.Count > 0)
                    alert = DataChange<SYS_LOG_ALERT>.FillEntity(dt.Rows[0]);
                return alert;
            }
        }

        public M_Alert_Object SelectByID(Int64 ID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                M_Alert_Object alert = null;
                string strSql = "SELECT a.LOG_ID,a.OID,a.AP_MAC,a.G_SSID,a.G_MAC,a.G_TIME,a.G_STRONG,a.CHANNEL,a.ISPROCESS,a.ISWHITELIST,a.KEYWORD,a.Similarity,a.FIRSTTIME,a.SCANCOUNT,b.ALIAS as APNAME FROM SYS_LOG_ALERT a,sys_apdevice b WHERE a.AP_MAC=b.MAC and LOG_ID=" + ID;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_LOG_ALERT");
                if (dt.Rows.Count > 0)
                    alert = DataChange<M_Alert_Object>.FillEntity(dt.Rows[0]);
                return alert;
            }
        }
    }
}
