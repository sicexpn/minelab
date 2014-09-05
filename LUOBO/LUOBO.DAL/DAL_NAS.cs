using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using LUOBO.Helper;
using MySql.Data.MySqlClient;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_NAS
    {
        public NAS Select(int id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                List<NAS> list = null;
                NAS data = null;
                string strSql = "select NasName,Secret,Server from nas where id=@ID";//to edit
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "NAS", parms);
                //list = DataChange<NAS>.FillModel(dt);
                //data = list[0];
                if (dt.Rows.Count > 0)
                    data = DataChange<NAS>.FillEntity(dt.Rows[0]);
                return data;

            }
        }
    }
}
