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
    public class DAL_SYS_DICT_ZONE
    {

        public List<SYS_DICT_ZONE> SelectAllProvices()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_DICT_ZONE> list = null;
                string strSql = "SELECT * FROM SYS_DICT_ZONE";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_DICT_ZONE");
                if (dt.Columns.Count > 0)
                    list = DataChange<SYS_DICT_ZONE>.FillModel(dt);
                return list;
            }
        }
    }
}
