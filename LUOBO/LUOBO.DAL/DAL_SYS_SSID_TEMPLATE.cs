using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_SYS_SSID_TEMPLATE
    {
        public List<SYS_SSID_TEMPLATE> SelectByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_TEMPLATE> data = new List<SYS_SSID_TEMPLATE>();
                string strSql = "SELECT * FROM SYS_SSID_TEMPLATE WHERE OID=@OID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID",OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_TEMPLATE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID_TEMPLATE>.FillModel(dt);
                return data;
            }
        }

        public List<SYS_SSID_TEMPLATE> SelectDefaultByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID_TEMPLATE> data = new List<SYS_SSID_TEMPLATE>();
                string strSql = "SELECT * FROM SYS_SSID_TEMPLATE WHERE OID=@OID and ISDEFAULT=true";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID",OID),
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID_TEMPLATE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID_TEMPLATE>.FillModel(dt);
                return data;
            }
        }
    }
}
