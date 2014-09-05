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
    public class DAL_SYS_DICT
    {
        public List<SYS_DICT> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_DICT> list = new List<SYS_DICT>();
                string strSql = "SELECT * FROM SYS_DICT";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_DICT");
                list = DataChange<SYS_DICT>.FillModel(dt);
                return list;
            }
        }

        public SYS_DICT Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_DICT data = null;
                string strSql = "SELECT * FROM SYS_DICT WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_DICT", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_DICT>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public List<SYS_DICT_EXTPROP> SelectExtProperty()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_DICT_EXTPROP> list = new List<SYS_DICT_EXTPROP>();
                string strSql = "SELECT * FROM sys_dict_extprop";
                DataTable dt = mySql.GetDataTable(strSql, "sys_dict_extprop");
                list = DataChange<SYS_DICT_EXTPROP>.FillModel(dt);
                return list;
            }
        }
    }
}
