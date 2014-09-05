using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_SYS_USERAPP
    {
        public bool Insert(SYS_USERAPP data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_USERAPP VALUES (_nextval('ID'), @USERID, @APPID)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@USERID", data.USERID),
                    new MySqlParameter("@APPID", data.APPID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Inserts(List<SYS_USERAPP> datas)
        {
            return false;
        }

        public bool Update(SYS_USERAPP data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_USERAPP SET USERID = @USERID, APPID = @APPID WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", data.ID),
                    new MySqlParameter("@USERID", data.USERID),
                    new MySqlParameter("@APPID", data.APPID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Updates(List<SYS_USERAPP> datas)
        {
            return false;
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_USERAPP WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public List<SYS_USERAPP> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_USERAPP> list = new List<SYS_USERAPP>();
                string strSql = "SELECT * FROM SYS_USERAPP";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USERAPP");
                list = DataChange<SYS_USERAPP>.FillModel(dt);
                return list;
            }
        }

        public SYS_USERAPP Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USERAPP data = null;
                string strSql = "SELECT * FROM SYS_USERAPP WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USERAPP", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USERAPP>.FillEntity(dt.Rows[0]);

                return data;
            }
        }
    }
}
