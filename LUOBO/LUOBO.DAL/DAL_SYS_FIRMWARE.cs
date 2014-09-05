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
    public class DAL_SYS_FIRMWARE
    {
        public SYS_FIRMWARE GetCurrentVersion(string mac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT a.* FROM SYS_FIRMWARE a,SYS_APDEVICE b WHERE a.FIREWARENAME=b.FIRMWAREVERSION and b.MAC=@MAC";
                MySqlParameter[] parms = new MySqlParameter[] {
                new MySqlParameter("@MAC",mac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_FIRMWARE", parms);
                SYS_FIRMWARE data=null;
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_FIRMWARE>.FillEntity(dt.Rows[0]);
                }
                return data;
            }
        }
        /// <summary>
        /// 判断该mac地址和版本号能否得到正确的固件
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="firmwarever"></param>
        /// <param name="verno"></param>
        /// <returns></returns>
        public SYS_FIRMWARE CheckVersion(string mac,string firmwarever,string verno)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT a.* FROM SYS_FIRMWARE a,SYS_APDEVICE b WHERE a.FIREWARENAME=b.FIRMWAREVERSION and b.MAC=@MAC and a.FIREWARENAME=@FIREWARENAME and a.VERNO=@VERNO";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@MAC",mac),
                    new MySqlParameter("@FIREWARENAME",firmwarever),
                    new MySqlParameter("@VERNO",verno)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_FIRMWARE", parms);
                SYS_FIRMWARE data = null;
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_FIRMWARE>.FillEntity(dt.Rows[0]);
                }
                return data;
            }
        }

        public SYS_FIRMWARE CheckVersion(string mac, string verno)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT a.* FROM SYS_FIRMWARE a,SYS_APDEVICE b WHERE a.FIREWARENAME=b.FIRMWAREVERSION and b.MAC=@MAC";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@MAC",mac.ToUpper())
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_FIRMWARE", parms);
                SYS_FIRMWARE data = null;
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_FIRMWARE>.FillEntity(dt.Rows[0]);
                }
                return data;
            }
        }
    }
}
