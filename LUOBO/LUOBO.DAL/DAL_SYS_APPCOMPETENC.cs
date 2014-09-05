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
    public class DAL_SYS_APPCOMPETENC
    {
        public bool Insert(SYS_APPCOMPETENC data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_APPCOMPETENC (APPID, NAME, CONTROLLER, ACTION) VALUES (@APPID, @NAME, @CONTROLLER, @ACTION)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@APPID", data.APPID),
                    new MySqlParameter("@NAME", data.NAME),
                    new MySqlParameter("@CONTROLLER", data.CONTROLLER),
                    new MySqlParameter("@ACTION", data.ACTION)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Inserts(List<SYS_APPCOMPETENC> datas)
        {
            return false;
        }

        public bool Update(SYS_APPCOMPETENC data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APPCOMPETENC SET APPID = @APPID, NAME = @NAME, CONTROLLER = @CONTROLLER, ACTION = @ACTION WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", data.ID),
                    new MySqlParameter("@APPID", data.APPID),
                    new MySqlParameter("@NAME", data.NAME),
                    new MySqlParameter("@CONTROLLER", data.CONTROLLER),
                    new MySqlParameter("@ACTION", data.ACTION)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Updates(List<SYS_APPCOMPETENC> datas)
        {
            return false;
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APPCOMPETENC WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public List<SYS_APPCOMPETENC> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APPCOMPETENC> list = new List<SYS_APPCOMPETENC>();
                string strSql = "SELECT * FROM SYS_APPCOMPETENC";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPCOMPETENC");
                list = DataChange<SYS_APPCOMPETENC>.FillModel(dt);
                return list;
            }
        }

        public SYS_APPCOMPETENC Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APPCOMPETENC data = null;
                string strSql = "SELECT * FROM SYS_APPCOMPETENC WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPCOMPETENC", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_APPCOMPETENC>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public List<SYS_APPCOMPETENC_VIEW> Select(int size, Int64 curPage, string name, Int64 appID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APPCOMPETENC_VIEW> datas = new List<SYS_APPCOMPETENC_VIEW>();
                string strSql = "SELECT t1.*,t2.APPLICATIONNAME FROM SYS_APPCOMPETENC t1 LEFT JOIN SYS_APPLICATION t2 ON t1.APPID = t2.ID WHERE t1.ID NOT IN (SELECT ID FROM ({0}) t)";
                if (name.Trim() != "")
                    strSql += " AND t1.NAME like '%" + name + "%'";
                if (appID != 0)
                    strSql += " AND t1.APPID = " + appID;
                strSql += " ORDER BY t1.ID ASC LIMIT " + size;

                string strChildSql = "SELECT ID FROM SYS_APPCOMPETENC WHERE 1 = 1";
                if (name.Trim() != "")
                    strChildSql += " AND NAME like '%" + name + "%'";
                if (appID != 0)
                    strChildSql += " AND APPID = " + appID;
                strChildSql += " ORDER BY ID ASC LIMIT " + ((curPage - 1) * size);

                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "SYS_APPCOMPETENC_VIEW");
                datas = DataChange<SYS_APPCOMPETENC_VIEW>.FillModel(dt);
                return datas;
            }
        }

        public int SelectCount(string name, Int64 appID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(1) FROM SYS_APPCOMPETENC WHERE 1 = 1";
                if (name.Trim() != "")
                    strSql += " AND NAME like '%" + name + "%'";
                if (appID != 0)
                    strSql += " AND APPID = " + appID;
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }

        public List<SYS_APPCOMPETENC_VIEW> Select_view(Int64 uid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APPCOMPETENC_VIEW> datas = new List<SYS_APPCOMPETENC_VIEW>();
                string strSql = "SELECT t1.*,t2.APPLICATIONNAME,t2.SYSTEMNAME,t2.ASSEMBLYNAME,t2.CLASSNAME,t2.ISPUBLIC,t2.ISDEFAULT FROM SYS_APPCOMPETENC t1 LEFT JOIN SYS_APPLICATION t2 ON t1.APPID = t2.ID WHERE 1 = 1";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPCOMPETENC_VIEW");
                datas = DataChange<SYS_APPCOMPETENC_VIEW>.FillModel(dt);
                return datas;
            }
        }
    }
}
