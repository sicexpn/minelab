using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_ORGAPP
    {
        public bool Insert(SYS_ORGAPP data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_ORGAPP(ORGID,APPID) VALUES ( @ORGID, @APPID)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ORGID", data.ORGID),
                    new MySqlParameter("@APPID", data.APPID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Inserts(List<SYS_ORGAPP> datas)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                bool flag = false;
                foreach (SYS_ORGAPP data in datas)
                {
                    flag = Insert(data);
                }
                return flag;
            }
        }
        public bool Update(SYS_ORGAPP data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_ORGAPP SET ORGID = @ORGID,APPID = @APPID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",data.ID),
                    new MySqlParameter("@ORGID", data.ORGID),
                    new MySqlParameter("@APPID", data.APPID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Updates(List<SYS_ORGAPP> datas)
        {
            return false;
        }
        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_ORGAPP WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public List<SYS_ORGAPP> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_ORGAPP> list = new List<SYS_ORGAPP>();
                string strSql = "SELECT * FROM SYS_ORGAPP";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG");
                list = DataChange<SYS_ORGAPP>.FillModel(dt);
                return list;
            }
        }
        public SYS_ORGAPP Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_ORGAPP data = null;
                string strSql = "SELECT * FROM SYS_APORG WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_ORGAPP>.FillEntity(dt.Rows[0]);
                return data;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">orgid</param>
        /// <param name="ids">appids</param>
        /// <returns></returns>
        public bool Deletes(int id, string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_ORGAPP WHERE ORGID=@ORGID AND APPID IN (" + ids + ")";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ORGID",id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

    }
}
