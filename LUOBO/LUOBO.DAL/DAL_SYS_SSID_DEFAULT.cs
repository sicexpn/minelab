using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using LUOBO.Helper;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_SSID_DEFAULT
    {
        public List<SYS_SSID_DEFAULT> SelectByTID(Int64 TID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_DEFAULT> data = new List<SYS_SSID_DEFAULT>();
                string strSql = "SELECT * FROM SYS_SSID_DEFAULT WHERE TID=@TID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@TID",TID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_DEFAULT", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID_DEFAULT>.FillModel(dt);
                return data;
            }
        }

        public List<SYS_SSID_DEFAULT> SelectDefaultByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_DEFAULT> data = new List<SYS_SSID_DEFAULT>();
                string strSql = "select * from sys_ssid_default where TID = (select ID from sys_ssid_template where OID=@OID and ISDEFAULT=true)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID",OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_DEFAULT", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID_DEFAULT>.FillModel(dt);
                return data;
            }
        }
    }
}
