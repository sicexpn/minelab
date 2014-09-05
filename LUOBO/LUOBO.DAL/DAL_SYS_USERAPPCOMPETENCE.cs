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
    public class DAL_SYS_USERAPPCOMPETENCE
    {
        public bool Insert(SYS_USERAPPCOMPETENCE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_USERAPPCOMPETENCE (UID, APPCID) VALUES (@UID, @APPCID)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@UID", data.UID),
                    new MySqlParameter("@APPCID", data.APPCID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Update(SYS_USERAPPCOMPETENCE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_USERAPPCOMPETENCE SET UID = @UID, APPCID = @APPCID WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", data.ID),
                    new MySqlParameter("@UID", data.UID),
                    new MySqlParameter("@APPCID", data.APPCID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_USERAPPCOMPETENCE WHERE ID = @ID";
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
                string strSql = "DELETE FROM SYS_USERAPPCOMPETENCE WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public bool DeleteByUID(Int64 uid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_USERAPPCOMPETENCE WHERE UID = @UID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@UID", uid)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public List<SYS_USERAPPCOMPETENCE> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_USERAPPCOMPETENCE> list = new List<SYS_USERAPPCOMPETENCE>();
                string strSql = "SELECT * FROM SYS_USERAPPCOMPETENCE";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USERAPPCOMPETENCE");
                list = DataChange<SYS_USERAPPCOMPETENCE>.FillModel(dt);
                return list;
            }
        }

        public SYS_USERAPPCOMPETENCE Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USERAPPCOMPETENCE data = null;
                string strSql = "SELECT * FROM SYS_USERAPPCOMPETENCE WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USERAPPCOMPETENCE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USERAPPCOMPETENCE>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public List<SYS_USERAPPCOMPETENCE> SelectByUID(Int64 uid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_USERAPPCOMPETENCE> list = new List<SYS_USERAPPCOMPETENCE>();
                string strSql = "SELECT * FROM SYS_USERAPPCOMPETENCE WHERE UID = @UID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@UID", uid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USERAPPCOMPETENCE", parms);
                list = DataChange<SYS_USERAPPCOMPETENCE>.FillModel(dt);
                return list;
            }
        }
    }
}
