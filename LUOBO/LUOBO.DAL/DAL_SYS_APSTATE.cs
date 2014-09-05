using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_APSTATE
    {
        public bool UpdateHeart(string devicemac, DateTime dateTime, string cpu, string memfree, string powertime, string freetime, string networktotal, string networkrate, string curdatetime)
        {
            DateTime POWERDATETIME;
            double devpowersecond;
            if (string.IsNullOrWhiteSpace(powertime) == false)
            {

                if (double.TryParse(powertime, out devpowersecond) == true)
                {
                    POWERDATETIME = dateTime.AddSeconds(0 - devpowersecond);
                }
                else
                {
                    POWERDATETIME = dateTime;
                }
            }
            else
            {
                POWERDATETIME = dateTime;
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APSTATE SET LASTHB=@LASTHB,CPU=@CPU,MEMFREE=@MEMFREE,POWERTIME=(CASE WHEN POWERTIME<@POWERTIME THEN @POWERTIME ELSE POWERTIME END),FREETIME=@FREETIME,NETWORKTOTAL=@NETWORKTOTAL,NETWORKRATE=@NETWORKRATE,POWERDATETIME=@POWERDATETIME WHERE MAC=@MAC ";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@LASTHB",dateTime),
                new MySqlParameter("@CPU",cpu),
                new MySqlParameter("@MEMFREE",memfree),
                new MySqlParameter("@POWERTIME",powertime),
                new MySqlParameter("@FREETIME",freetime),
                new MySqlParameter("@NETWORKTOTAL",networktotal),
                new MySqlParameter("@NETWORKRATE",networkrate),
                new MySqlParameter("@MAC",devicemac),
                new MySqlParameter("@POWERDATETIME",POWERDATETIME)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public DateTime? SelectPowerTimeByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT POWERDATETIME FROM SYS_APSTATE WHERE MAC=@MAC LIMIT 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@MAC",apMac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APSTATE", parms);
                SYS_APSTATE data = null;
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_APSTATE>.FillEntity(dt.Rows[0]);
                    return data.POWERDATETIME;
                }
                return null;
            }

        }
    }
}
