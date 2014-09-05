using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using System.Data;
using LUOBO.Entity;

namespace LUOBO.DAL
{
    public class DAL_SYS_SSID_AUDIT
    {

        public bool Update(SYS_SSID_AUDIT data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_SSID_AUDIT where 1<>1", "SYS_SSID_AUDIT");
                if (data.ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<SYS_SSID_AUDIT>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_SSID_AUDIT where ID=" + data.ID.ToString(), "SYS_SSID_AUDIT");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<SYS_SSID_AUDIT>.FillRow(data, dt.Rows[0]);
                }
                return mySql.Update(dt);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_SSID_AUDIT WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Deletes(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_SSID_AUDIT WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }


        public SYS_SSID_AUDIT Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_SSID_AUDIT data = null;
                string strSql = "SELECT * FROM SYS_SSID_AUDIT WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_AUDIT", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID_AUDIT>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        public List<SYS_SSID_AUDIT> Select(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_AUDIT> data = null;
                string strSql = "SELECT * FROM SYS_SSID_AUDIT WHERE ID in ("+ids+")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_AUDIT");
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID_AUDIT>.FillModel(dt);
                return data;
            }
        }

        //public List<Int64> SelectAPIDByID(string ids)
        //{
        //    using (MySQLDataAccess mySql = new MySQLDataAccess())
        //    {
        //        List<Int64> data = null;
        //        string strSql = "SELECT DISTINCT APID FROM SYS_SSID_AUDIT WHERE ID in (" + ids + ")";
        //        DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_AUDIT");
        //        if (dt.Rows.Count > 0)
        //            data = DataChange<Int64>.FillModel(dt);
        //        return data;
        //    }
        //}

        public List<SYS_SSID_AUDIT_VIEW> SelectOnPage(string keystr, int state, int curPage, int size)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_AUDIT_VIEW> datas = new List<SYS_SSID_AUDIT_VIEW>();
                string strSql = "SELECT t1.*,t2.NAME AS ONAME FROM SYS_SSID_AUDIT t1,SYS_ORGANIZATION t2 WHERE t1.APPLYOID=t2.ID AND t1.ID NOT IN (SELECT ID FROM ({0}) t) ";
                if (!string.IsNullOrEmpty(keystr))
                    strSql += " AND t2.NAME like '%" + keystr + "%'";
                if (state != -99)
                    strSql += " AND t1.STATE = " + state;
                strSql += " ORDER BY t1.APPLYTIME DESC LIMIT " + size;

                string strChildSql = "SELECT t1.ID FROM SYS_SSID_AUDIT t1,SYS_ORGANIZATION t2 WHERE t1.APPLYOID=t2.ID";
                if (!string.IsNullOrEmpty(keystr))
                    strChildSql += " AND t2.NAME like '%" + keystr + "%'";
                if (state != -99)
                    strChildSql += " AND t1.STATE = " + state;
                strChildSql += " ORDER BY t1.APPLYTIME DESC LIMIT " + ((curPage - 1) * size);
                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "SYS_SSID_AUDIT");
                datas = DataChange<SYS_SSID_AUDIT_VIEW>.FillModel(dt);
                return datas;
            }
        }

        public int SelectCount(string keystr, int state, int curPage, int size)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(1) FROM SYS_SSID_AUDIT t1, SYS_ORGANIZATION t2 WHERE t1.APPLYOID=t2.ID";
                if (!string.IsNullOrEmpty(keystr))
                    strSql += " AND t2.NAME like '%" + keystr + "%'";
                if (state != -99)
                    strSql += " AND t1.STATE = " + state;
                strSql += " ORDER BY t1.APPLYTIME DESC";

                //string strChildSql = "SELECT t1.ID FROM SYS_SSID_AUDIT t1, SYS_ORGANIZATION t2 WHERE t1.APPLYOID=t2.ID";
                //if (!string.IsNullOrEmpty(keystr))
                //    strChildSql += " AND t2.NAME like '%" + keystr + "%'";
                //if (state != -99)
                //    strChildSql += " AND t1.STATE = " + state;
                //strChildSql += " ORDER BY t1.APPLYTIME DESC LIMIT " + ((curPage - 1) * size);

                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }

        public bool UpdateForState(string ids,Int64 auditOID, string account,string auditIntro, int state)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_AUDIT_VIEW> datas = new List<SYS_SSID_AUDIT_VIEW>();
                string strSql = "UPDATE SYS_SSID_AUDIT SET STATE=@STATE, AUDITTIME=now(), AUDITOID=@AUDITOID, AUDITER=@AUDITER,AUDITINTRO=@AUDITINTRO WHERE ID in(" + ids + ")";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@STATE", state),
                    new MySqlParameter("@AUDITOID", auditOID),
                    new MySqlParameter("@AUDITER", account),
                    new MySqlParameter("@AUDITINTRO", auditIntro)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
    }
}