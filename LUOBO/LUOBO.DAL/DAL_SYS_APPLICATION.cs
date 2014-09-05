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
    public class DAL_SYS_APPLICATION
    {
        public bool Insert(SYS_APPLICATION data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_APPLICATION VALUES (_nextval('ID'), @APPLICATIONNAME, @SYSTEMNAME, @ASSEMBLYNAME, @CLASSNAME, @ISPUBLIC, @ISDEFAULT)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@APPLICATIONNAME", data.APPLICATIONNAME),
                    new MySqlParameter("@SYSTEMNAME", data.SYSTEMNAME),
                    new MySqlParameter("@ASSEMBLYNAME", data.ASSEMBLYNAME),
                    new MySqlParameter("@CLASSNAME", data.CLASSNAME),
                    new MySqlParameter("@ISPUBLIC", data.ISPUBLIC),
                    new MySqlParameter("@ISDEFAULT", data.ISDEFAULT)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Inserts(List<SYS_APPLICATION> datas)
        {
            return false;
        }

        public bool Update(SYS_APPLICATION data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APPLICATION SET APPLICATIONNAME = @APPLICATIONNAME, SYSTEMNAME = @SYSTEMNAME, ASSEMBLYNAME = @ASSEMBLYNAME, CLASSNAME = @CLASSNAME, ISPUBLIC = @ISPUBLIC, ISDEFAULT = @ISDEFAULT WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", data.ID),
                    new MySqlParameter("@APPLICATIONNAME", data.APPLICATIONNAME),
                    new MySqlParameter("@SYSTEMNAME", data.SYSTEMNAME),
                    new MySqlParameter("@ASSEMBLYNAME", data.ASSEMBLYNAME),
                    new MySqlParameter("@CLASSNAME", data.CLASSNAME),
                    new MySqlParameter("@ISPUBLIC", data.ISPUBLIC),
                    new MySqlParameter("@ISDEFAULT", data.ISDEFAULT)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Updates(List<SYS_APPLICATION> datas)
        {
            return false;
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APPLICATION WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public List<SYS_APPLICATION> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APPLICATION> list = new List<SYS_APPLICATION>();
                string strSql = "SELECT * FROM SYS_APPLICATION";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPLICATION");
                list = DataChange<SYS_APPLICATION>.FillModel(dt);
                return list;
            }
        }

        public SYS_APPLICATION Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APPLICATION data = null;
                string strSql = "SELECT * FROM SYS_APPLICATION WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPLICATION", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_APPLICATION>.FillEntity(dt.Rows[0]);

                return data;
            }
        }
    }
}
