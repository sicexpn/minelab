using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using System.Data;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    public class DAL_RadAcct
    {
        public List<RadAcct> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                List<RadAcct> list = new List<RadAcct>();
                string strSql = "select AcctSessionId,UserName from radacct";
                DataTable dt = mySql.GetDataTable(strSql, "radacct");
                list = DataChange<RadAcct>.FillModel(dt);
                return list;
            }
        }
        public string GetTrafficBySSID(string ssid, DateTime startTime, DateTime endTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select sum(AcctInputOctets+AcctOutputOctets) from radacct as acct "
                    + "where acct.acctsessionid in (select openssid.AcctSessionId from OpenSSID as openssid where AcctStartTime>=@AcctStartTime and AcctStopTime<=@AcctStopTime and openssid.SSID=@SSID )";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@SSID",ssid),
                new MySqlParameter("@AcctStartTime",startTime),
                new MySqlParameter("@AcctStopTime",endTime)
                };
                return mySql.GetOnlyOneValue(strSql, parms).ToString();
            }
        }
        public string GetSessionTimeBySSID(string sessionStr, DateTime startTime, DateTime endTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select IFNULL(sum(AcctSessionTime),0) from radacct where AcctStartTime>=@AcctStartTime and AcctStopTime<=@AcctStopTime and AcctSessionId IN (@sessionStr)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@sessionStr",sessionStr),
                new MySqlParameter("@AcctStartTime",startTime),
                new MySqlParameter("@AcctStopTime",endTime)
                };
                return mySql.GetOnlyOneValue(strSql, parms).ToString();
            }
        }

        public string GetSessionTimeByApMac(string apMac, DateTime startTime, DateTime endTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select sum(AcctSessionTime) from radacct where AcctStartTime>=@AcctStartTime and AcctStopTime<=@AcctStopTime and CalledStationId=@CalledStationId";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@AcctStartTime",startTime),
                new MySqlParameter("@AcctStopTime",endTime)
                };
                return mySql.GetOnlyOneValue(strSql, parms).ToString();
            }
        }

        public string GetTrafficByApMac(string apMac, DateTime startTime, DateTime endTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select sum(AcctInputOctets+AcctOutputOctets) from radacct where AcctStartTime>=@AcctStartTime and AcctStopTime<=@AcctStopTime and CalledStationId=@CalledStationId";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@AcctStartTime",startTime),
                new MySqlParameter("@AcctStopTime",endTime)
                };
                return mySql.GetOnlyOneValue(strSql, parms).ToString();
            }
        }

        public Int64 GetUsedTrafficByUser(string userName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select sum(AcctInputOctets+AcctOutputOctets) from radacct where userName = @userName";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@userName",userName)
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public Int64 GetUsedSessionTimeByUser(string userName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select sum(acctSessionTime) from radacct where userName = @userName";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@userName",userName)
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public Int64 GetOnLineLoginUserCountsByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select count(*) from radacct where CalledStationId=@CalledStationId and AcctStopTime=@AcctStopTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@AcctStopTime",null)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }
        public Int64 GetOnLineLoginUserCounts()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select count(*) from radacct where AcctStopTime=@AcctStopTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@AcctStopTime",null)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }

        public List<Int32> GetCheckTypeListByMac(string mac, Int64 oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "SELECT a.callingstationid,b.username,b.userType FROM radacct a LEFT JOIN radcheck b ON a.username = b.username WHERE a.callingstationid = '" + mac + "'";
                strSql += " AND SUBSTR(a.CalledStationId, 4, 12) IN";
                strSql += " (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci, 4, 12) FROM luobo.sys_apdevice t1";
                strSql += " INNER JOIN luobo.sys_aporg t2 ON t1.id = t2.apid WHERE t2.oid IN";
                strSql += " (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + "))";

                List<Int32> list = new List<int>();
                DataTable dt = mySql.GetDataTable(strSql, "radacct");
                foreach (DataRow row in dt.Rows)
                {
                    if (!list.Contains(Convert.ToInt32(row[2])))
                        list.Add(Convert.ToInt32(row[2]));
                }
                return list;
            }
        }

    }
}
