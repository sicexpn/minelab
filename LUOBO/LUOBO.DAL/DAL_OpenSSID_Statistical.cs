using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using LUOBO.Model;
using MySql.Data.MySqlClient;
using System.Data;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    public class DAL_OpenSSID_Statistical
    {
        public List<OpenSSID> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<OpenSSID> list = null;
                string strSql = "select * from OpenSSID";
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");
                list = DataChange<OpenSSID>.FillModel(dt);
                return list;
            }
        }

        public bool Insert(OpenSSID data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "insert into OpenSSID(SSID,OID,AdId,AcctSessionId,CalledStationId,CallingStationId,Title,PageUrl,UserAgent,CurrentTime) values(@SSID,@OID,@AdId,@AcctSessionId,@CalledStationId,@CallingStationId,@Title,@PageUrl,@UserAgent,@CurrentTime)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@SSID",data.SSID),
                    new MySqlParameter("@OID",data.OID),
                    new MySqlParameter("@AdId",data.AdId),
                    new MySqlParameter("@AcctSessionId",data.AcctSessionId),
                    new MySqlParameter("@CalledStationId",data.CalledStationId),
                    new MySqlParameter("@CallingStationId",data.CallingStationId),
                    new MySqlParameter("@Title",data.Title),
                    new MySqlParameter("@PageUrl",data.PageUrl),
                    new MySqlParameter("@UserAgent",data.UserAgent),
                    new MySqlParameter("@CurrentTime",data.CurrentTime)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public Int64 GetInstalPeopleCount(string apMac, DateTime? apStartTime)
        {
            if (apStartTime == null)
            {
                return 0;
            }
            //apMac = apMac.Substring(0, apMac.Length - 1);
            //apMac += "_";
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                //string strSql = "SELECT COUNT(AcctSessionId) FROM OPENSSID WHERE CurrentTime > @CurrentTime AND CalledStationId=@CalledStationId";
                string strSql = "SELECT IFNULL(COUNT(AcctSessionId),0) FROM OPENSSID WHERE CurrentTime > @CurrentTime AND CalledStationId like @CalledStationId";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@CurrentTime",apStartTime)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }

        public Int64 GetInstalPeopleNum(string apMac, DateTime? apStartTime)
        {
            if (apStartTime == null)
            {
                return 0;
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                //string strSql = "SELECT COUNT(DISTINCT AcctSessionId) FROM OPENSSID WHERE CurrentTime > @CurrentTime AND CalledStationId=@CalledStationId";
                string strSql = "SELECT IFNULL(COUNT(DISTINCT AcctSessionId),0) FROM OPENSSID WHERE CurrentTime > @CurrentTime AND CalledStationId like @CalledStationId";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@CurrentTime",apStartTime)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }

        public Int64 GetStartPeopleCount(string apMac, DateTime? apPowerTime)
        {
            if (apPowerTime == null)
            {
                return 0;
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                //string strSql = "SELECT COUNT(AcctSessionId) FROM OPENSSID WHERE CalledStationId=@CalledStationId and CurrentTime > @CurrentTime";
                string strSql = "SELECT IFNULL(COUNT(AcctSessionId),0) FROM OPENSSID WHERE CalledStationId like @CalledStationId and CurrentTime > @CurrentTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@CurrentTime",apPowerTime)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }

        public Int64 GetStartPeopleNum(string apMac, DateTime? apPowerTime)
        {
            if (apPowerTime == null)
            {
                return 0;
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                //string strSql = "SELECT COUNT(DISTINCT AcctSessionId) FROM OPENSSID WHERE CalledStationId=@CalledStationId and CurrentTime > @CurrentTime";
                string strSql = "SELECT IFNULL(COUNT(DISTINCT AcctSessionId),0) FROM OPENSSID WHERE CalledStationId like @CalledStationId and CurrentTime > @CurrentTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",apMac),
                new MySqlParameter("@CurrentTime",apPowerTime)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }

        public M_PeopleCount GetInstallPeopleNumCount(M_OrgApTime data)
        {
            if (data.ApPowerTime == null)
            {
                return null;
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                //string strSql = "SELECT COUNT(DISTINCT AcctSessionId) FROM OPENSSID WHERE CalledStationId=@CalledStationId and CurrentTime > @CurrentTime";
                string strSql = "SELECT IFNULL(COUNT(DISTINCT AcctSessionId),0) AS InstalPeopleNum,IFNULL(COUNT(AcctSessionId),0) AS InstalPeopleCount "
                    + "FROM OPENSSID WHERE CalledStationId like @CalledStationId and CurrentTime > @CurrentTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",data.MAC),
                new MySqlParameter("@CurrentTime",data.ApStartTime)
                };
                M_PeopleCount result = null;
                DataTable dt = mySql.GetDataTable(strSql, "M_PeopleCount", parms);
                if (dt.Rows.Count > 0)
                    result = DataChange<M_PeopleCount>.FillEntity(dt.Rows[0]);
                return result;
            }
        }

        public M_PeopleCount GetStartPeopleNumCount(M_OrgApTime data)
        {
            if (data.ApPowerTime == null)
            {
                return null;
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                //string strSql = "SELECT COUNT(DISTINCT AcctSessionId) FROM OPENSSID WHERE CalledStationId=@CalledStationId and CurrentTime > @CurrentTime";
                string strSql = "SELECT IFNULL(COUNT(DISTINCT AcctSessionId),0) AS StartPeopleNum,IFNULL(COUNT(AcctSessionId),0) AS StartPeopleCount "
                    + "FROM OPENSSID WHERE CalledStationId like @CalledStationId and CurrentTime > @CurrentTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@CalledStationId",data.MAC),
                new MySqlParameter("@CurrentTime",data.ApPowerTime)
                };
                M_PeopleCount result = null;
                DataTable dt = mySql.GetDataTable(strSql, "M_PeopleCount", parms);
                if (dt.Rows.Count > 0)
                    result = DataChange<M_PeopleCount>.FillEntity(dt.Rows[0]);
                return result;
            }
        }

        public List<OpenSSID> SelectAllApMacByOID(Int64 orgID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<OpenSSID> list = null;
                string strSql = "SELECT CalledStationId FROM OPENSSID WHERE OID=@OID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@OID",orgID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OPENSSID", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<OpenSSID>.FillModel(dt);
                return list;
            }
        }

        public List<List<Int64>> SelectByDateAndOID(DateTime startTime, DateTime endTime, string oids, Int64 APID, CustomEnum.ENUM_Statistical_Type type)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<List<Int64>> list = new List<List<Int64>>();
                string strSql = "SELECT ifnull(SUM(CallingStationId),0)AS COUNT,ifnull(tmp, 0)AS NUM FROM(";

                switch (type)
                {
                    case CustomEnum.ENUM_Statistical_Type.Year:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyy", type) + ") t1";
                        strSql += " LEFT JOIN(SELECT CurrentTime,COUNT(DISTINCT CallingStationId) CallingStationId FROM openssid";
                        strSql += " WHERE date_format(CurrentTime, '%Y')>= date_format(@STARTTIME, '%Y')";
                        strSql += " AND date_format(CurrentTime, '%Y')<= date_format(@ENDTIME, '%Y')";
                        strSql += " AND OID IN(" + oids + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY SUBSTR(CalledStationId, 4, 12),date_format(CurrentTime, '%Y')";
                        strSql += " )t2 ON t1.DATETIME = date_format(t2.CurrentTime, '%Y')";
                        strSql += " LEFT JOIN(SELECT CurrentTime,COUNT(1) AS tmp FROM openssid";
                        strSql += " WHERE date_format(CurrentTime, '%Y')>= date_format(@STARTTIME, '%Y')";
                        strSql += " AND date_format(CurrentTime, '%Y')<= date_format(@ENDTIME, '%Y')";
                        strSql += " AND OID IN(" + oids + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY date_format(CurrentTime, '%Y')";
                        strSql += " )t3 ON t1.DATETIME = date_format(t3.CurrentTime, '%Y')";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.Month:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyyMM", type) + ") t1";
                        strSql += " LEFT JOIN(SELECT CurrentTime,COUNT(DISTINCT CallingStationId) CallingStationId FROM openssid";
                        strSql += " WHERE date_format(CurrentTime, '%Y%m')>= date_format(@STARTTIME, '%Y%m')";
                        strSql += " AND date_format(CurrentTime, '%Y%m')<= date_format(@ENDTIME, '%Y%m')";
                        strSql += " AND OID IN(" + oids + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY SUBSTR(CalledStationId, 4, 12),date_format(CurrentTime, '%Y%m')";
                        strSql += " )t2 ON t1.DATETIME = date_format(t2.CurrentTime, '%Y%m')";
                        strSql += " LEFT JOIN(SELECT CurrentTime,COUNT(1) AS tmp FROM openssid";
                        strSql += " WHERE date_format(CurrentTime, '%Y%m')>= date_format(@STARTTIME, '%Y%m')";
                        strSql += " AND date_format(CurrentTime, '%Y%m')<= date_format(@ENDTIME, '%Y%m')";
                        strSql += " AND OID IN(" + oids + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY date_format(CurrentTime, '%Y%m')";
                        strSql += " )t3 ON t1.DATETIME = date_format(t3.CurrentTime, '%Y%m')";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.Day:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyyMMdd", type) + ") t1";
                        strSql += " LEFT JOIN(SELECT CurrentTime,COUNT(DISTINCT CallingStationId) CallingStationId FROM openssid";
                        strSql += " WHERE date_format(CurrentTime, '%Y%m%d')>= date_format(@STARTTIME, '%Y%m%d')";
                        strSql += " AND date_format(CurrentTime, '%Y%m%d')<= date_format(@ENDTIME, '%Y%m%d')";
                        strSql += " AND OID IN(" + oids + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY SUBSTR(CalledStationId, 4, 12),date_format(CurrentTime, '%Y%m%d')";
                        strSql += " )t2 ON t1.DATETIME = date_format(t2.CurrentTime, '%Y%m%d')";
                        strSql += " LEFT JOIN(SELECT CurrentTime,COUNT(1) AS tmp FROM openssid";
                        strSql += " WHERE date_format(CurrentTime, '%Y%m%d')>= date_format(@STARTTIME, '%Y%m%d')";
                        strSql += " AND date_format(CurrentTime, '%Y%m%d')<= date_format(@ENDTIME, '%Y%m%d')";
                        strSql += " AND OID IN(" + oids + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY date_format(CurrentTime, '%Y%m%d')";
                        strSql += " )t3 ON t1.DATETIME = date_format(t3.CurrentTime, '%Y%m%d')";
                        break;
                }
                strSql += " GROUP BY t1.DATETIME";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@STARTTIME",startTime),
                    new MySqlParameter("@ENDTIME",endTime)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OPENSSID", parms);

                List<Int64> itemList1 = new List<Int64>();
                List<Int64> itemList2 = new List<Int64>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        itemList1.Add(Convert.ToInt64(row[0]));
                        itemList2.Add(Convert.ToInt64(row[1]));
                    }
                    list.Add(itemList1);
                    list.Add(itemList2);

                }
                return list;
            }
        }

        private string createDateFaildSubSql(DateTime startTime, DateTime endTime, string format, CustomEnum.ENUM_Statistical_Type type)
        {
            string tmp = "";

            tmp += "select '" + startTime.ToString(format) + "' AS DATETIME";
            if (type == CustomEnum.ENUM_Statistical_Type.Day)
                startTime = startTime.AddDays(1);
            else if (type == CustomEnum.ENUM_Statistical_Type.Month)
                startTime = startTime.AddMonths(1);
            else if (type == CustomEnum.ENUM_Statistical_Type.Year)
                startTime = startTime.AddYears(1);

            if (startTime <= endTime)
            {
                tmp += " union ";
                while (startTime <= endTime)
                {

                    tmp += "select '" + startTime.ToString(format) + "'";
                    if (type == CustomEnum.ENUM_Statistical_Type.RealTime)
                        startTime = startTime.AddHours(1);
                    if (type == CustomEnum.ENUM_Statistical_Type.Day)
                        startTime = startTime.AddDays(1);
                    else if (type == CustomEnum.ENUM_Statistical_Type.Month)
                        startTime = startTime.AddMonths(1);
                    else if (type == CustomEnum.ENUM_Statistical_Type.Year)
                        startTime = startTime.AddYears(1);
                    if (startTime <= endTime)
                    {
                        tmp += " union ";
                    }
                }
            }
            return tmp;
        }

        public Model.M_PeopleCount SelectCountByOrg(Int64 orgID)
        {
            Model.M_PeopleCount item = new M_PeopleCount();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                #region

                //string strSql = "select count(distinct(b.ID)) InstalPeopleCount,count(DISTINCT(b.AcctSessionId)) InstalPeopleNum,count(DISTINCT( CASE WHEN b.CurrentTime >a.POWERDATETIME THEN b.CalledStationId ELSE NULL END )) StartPeopleCount,IFNULL(sum(a.ONLINEPEOPLENUM),0) StartPeopleNum ";
                //strSql = strSql + " FROM luobo.sys_apdevice a,statistical.openssid b,luobo.sys_aporg c";
                //strSql = strSql + " where a.ID=c.APID and c.OID=@orgID and b.OID=@orgID ";
                /*string strSql = "select sum(InstalPeopleCount) InstalPeopleCount,sum(InstalPeopleNum) InstalPeopleNum,sum(StartPeopleCount) StartPeopleCount,sum(StartPeopleNum) StartPeopleNum,sum(OnlinePeopleNum) OnlinePeopleNum ";
                strSql = strSql + " from ( ";
                strSql = strSql + " (select count(1) InstalPeopleCount,count(DISTINCT(b.AcctSessionId)) InstalPeopleNum ";
                strSql = strSql + " from statistical.openssid b ";
                strSql = strSql + " where b.OID=@orgID) aa ,";
                strSql = strSql + " (select ";
                strSql = strSql + " count(DISTINCT(d.AcctSessionId)) StartPeopleCount,";
                strSql = strSql + " count(DISTINCT(d.CallingStationId)) StartPeopleNum";
                strSql = strSql + " FROM (select b.*,s.APID from  statistical.openssid b,luobo.sys_ssid s where b.ssid =s.ID and b.OID=@orgID) d,";
                strSql = strSql + " (select a.* ,c.OID ";
                strSql = strSql + " FROM luobo.sys_apdevice a,luobo.sys_aporg c";
                strSql = strSql + " where c.OID=@orgID and a.ID=c.APID and ISCHILD=1) e";
                strSql = strSql + " where d.APID =e.ID and d.CurrentTime > e.POWERDATETIME";
                strSql = strSql + " ) bb ,";
                strSql = strSql + " (select sum(a.OnlinePeopleNum) OnlinePeopleNum";
                strSql = strSql + " FROM luobo.sys_apdevice a,luobo.sys_aporg c";
                strSql = strSql + " where c.OID=@orgID and a.ID=c.APID and ISCHILD=1 and (SYSDATE()- a.lasthb )<a.HBINTERVAL*100) cc )";
                */
                /*string strSql = "SELECT SUM(b.AllVisitCounts) AllVisitCounts,SUM(ROUND(b.AllVisitCounts/b.DAYS)) DayAvageVisitCounts, SUM(cc.OnlinePeopleNum) OnlinePeopleNum,round(ifnull(sum(b.AvageVisitTime),0) ) AvageVisitTime ";
                strSql += " From ";
                //strSql += "(SELECT count(a.AcctSessionID) AllVisitCounts,COUNT(DISTINCT DATE (a.CurrentTime)) DAYS,(sum(time_to_sec(timediff(c.acctstoptime,c.acctstarttime)))/Count(c.acctsessionid)) AvageVisitTime";
                strSql += "(SELECT count(a.AcctSessionID) AllVisitCounts,COUNT(DISTINCT DATE (a.CurrentTime)) DAYS,avg(time_to_sec(timediff(c.acctstoptime,c.acctstarttime))) AvageVisitTime";
                strSql += " FROM (select * from statistical.openssid where OID=@orgID) a left join radius.radacct  c ";
                strSql += "on c.acctstoptime is not null and c.acctsessionid=a.acctsessionid and a.calledstationid=c.calledstationid ) b,";
                strSql += "(select sum(a.OnlinePeopleNum) OnlinePeopleNum ";
                strSql += " FROM luobo.sys_apdevice a,luobo.sys_aporg c ";
                strSql += "where c.OID=@orgID and a.ID=c.APID and ISCHILD=1 and (SYSDATE()- a.lasthb )<a.HBINTERVAL*100) cc";
                */
                #endregion
                string strSql = "SELECT SUM(b.AllVisitCounts) AllVisitCounts,SUM(ROUND(b.AllVisitCounts/b.DAYS)) DayAvageVisitCounts, SUM(cc.OnlinePeopleNum) OnlinePeopleNum,round(ifnull((b.AvageVisitTime/b.allpeople),0) ) AvageVisitTime  ";
                strSql += "From (SELECT count(1) AllVisitCounts,sum(if(c.radacctid is null,0,1)) allpeople,COUNT(DISTINCT DATE (a.CurrentTime)) DAYS,sum(c.acctsessiontime) AvageVisitTime ";
                strSql += "FROM statistical.openssid a left join radius.radacct  c on  c.acctsessionid=a.acctsessionid and a.calledstationid=c.calledstationid  where a.OID=@orgID ) b,(select sum(a.OnlinePeopleNum) OnlinePeopleNum  ";
                strSql += "FROM luobo.sys_apdevice a,luobo.sys_aporg c where c.OID=@orgID and a.ID=c.APID and ISCHILD=1 and (SYSDATE()- a.lasthb )<a.HBINTERVAL*100) cc ";

                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@orgID",orgID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_PeopleCount", parms);
                //if (dt != null && dt.Rows.Count>0)
                //{
                //    item = DataChange<Model.M_PeopleCount>.FillEntity(dt.Rows[0]);
                //}
                DataTable tmpDT = dt.Clone();
                for (int i = 0; i < tmpDT.Columns.Count; i++)
                    tmpDT.Columns[i].DataType = typeof(Int64);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = tmpDT.NewRow();
                    for (int i = 0; i < tmpDT.Columns.Count; i++)
                        row[i] = dt.Rows[0][i];
                    tmpDT.Rows.Add(row);
                    item = DataChange<Model.M_PeopleCount>.FillEntity(tmpDT.Rows[0]);
                }
            }
            return item;
        }

        public List<string> SelectSessionsbySSID(string ssid, DateTime startTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT DISTINCT AcctSessionId FROM OPENSSID WHERE SSID=@SSID AND CurrentTime>@CurrentTime";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@SSID",ssid),
                new MySqlParameter("@CurrentTime",startTime)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OPENSSID", parms);

                List<string> list = new List<string>();
                if (dt.Rows.Count > 0)
                {
                    var dslp = from d in dt.AsEnumerable() select d;
                    foreach (var res in dslp)
                    {
                        list.Add(Convert.ToString(res.Field<string>("AcctSessionId")));
                    }
                }
                return list;
            }
        }

        public List<M_Statistical> SelectStatisticalADByOID(Int64 OID, Int64 APID, DateTime startTime, DateTime endTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<M_Statistical> datas = new List<M_Statistical>();
                string strSql = "select title NAME,count(0) NUM from openssid where date_format(CurrentTime, '%Y%m%d') >= date_format('" + startTime.ToShortDateString() + "', '%Y%m%d') AND date_format(CurrentTime, '%Y%m%d') <= date_format('" + endTime.ToShortDateString() + "', '%Y%m%d') and oid in (select id from luobo.SYS_ORGANIZATION where PIDHELP like '%$" + OID + "$%' or id=" + OID + ")";
                if (APID != -99)
                    strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                strSql += " group by title order by NUM desc limit 5";
                DataTable dt = mySql.GetDataTable(strSql, "M_Statistical");
                datas = DataChange<M_Statistical>.FillModel(dt);
                return datas;
            }
        }

        public List<List<Int64>> SelectStatisticalWIFIByOID(Int64 OID, Int64 APID, DateTime startTime, DateTime endTime, CustomEnum.ENUM_Statistical_Type type)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<List<Int64>> list = new List<List<Int64>>(); ;
                string strSql = "SELECT t1.DATETIME, IFNULL(t2.RZNUM, 0) RZNUM, IFNULL(t3.WRZNUM, 0) WRZNUM, (IFNULL(RZNUM, 0) + IFNULL(WRZNUM, 0)) ZNUM FROM(";
                switch (type)
                {
                    case CustomEnum.ENUM_Statistical_Type.Year:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyy", type) + ") t1 LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y') DATETIME, COUNT(DISTINCT CallingStationId) RZNUM";
                        strSql += " FROM openssid WHERE CallingStationId IN (SELECT CallingStationId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY DATETIME) t2";
                        strSql += " ON t1.DATETIME = t2.DATETIME LEFT JOIN(";
                        strSql += " SELECT date_format(CurrentTime, '%Y') DATETIME, COUNT(DISTINCT CallingStationId) WRZNUM";
                        strSql += " FROM openssid WHERE CallingStationId NOT IN (SELECT CallingStationId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY DATETIME) t3 ON t1.DATETIME = t3.DATETIME";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.Month:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyyMM", type) + ") t1 LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m') DATETIME, COUNT(DISTINCT CallingStationId) RZNUM";
                        strSql += " FROM openssid WHERE CallingStationId IN (SELECT CallingStationId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY DATETIME) t2";
                        strSql += " ON t1.DATETIME = t2.DATETIME LEFT JOIN(";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m') DATETIME, COUNT(DISTINCT CallingStationId) WRZNUM";
                        strSql += " FROM openssid WHERE CallingStationId NOT IN (SELECT CallingStationId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY DATETIME) t3 ON t1.DATETIME = t3.DATETIME";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.Day:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyyMMdd", type) + ") t1 LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m%d') DATETIME, COUNT(DISTINCT CallingStationId) RZNUM";
                        strSql += " FROM openssid WHERE CallingStationId IN (SELECT CallingStationId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY DATETIME) t2";
                        strSql += " ON t1.DATETIME = t2.DATETIME LEFT JOIN(";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m%d') DATETIME, COUNT(DISTINCT CallingStationId) WRZNUM";
                        strSql += " FROM openssid WHERE CallingStationId NOT IN (SELECT CallingStationId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                        strSql += " GROUP BY DATETIME) t3 ON t1.DATETIME = t3.DATETIME";
                        break;
                }
                strSql += " ORDER BY t1.DATETIME";
                DataTable dt = mySql.GetDataTable(strSql, "OPENSSID");

                List<Int64> RZNUM = new List<Int64>();
                List<Int64> WRZNUM = new List<Int64>();
                List<Int64> ZNUM = new List<Int64>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RZNUM.Add(Convert.ToInt64(row["RZNUM"]));
                        WRZNUM.Add(Convert.ToInt64(row["WRZNUM"]));
                        ZNUM.Add(Convert.ToInt64(row["ZNUM"]));
                    }
                    list.Add(RZNUM);
                    list.Add(WRZNUM);
                    list.Add(ZNUM);
                }

                return list;
            }
        }

        public List<List<Int64>> SelectOnlinePeopleNum_MapByOID(Int64 OID, Int64 APID, DateTime startTime, DateTime endTime, CustomEnum.ENUM_Statistical_Type type)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<List<Int64>> list = new List<List<Int64>>(); ;
                string strSql = "SELECT t1.DATETIME,IFNULL(t2.RCNUM, 0) RCNUM,IFNULL(t3.ZNUM, 0) ZNUM,IFNULL(t4.RZNUM, 0) RZNUM FROM (";
                switch (type)
                {
                    case CustomEnum.ENUM_Statistical_Type.Year:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyy", type) + ") t1 LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y') DATETIME, COUNT(AcctSessionId) RCNUM";
                        strSql += " FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t2";
                        else
                            strSql += " GROUP BY DATETIME) t2";
                        strSql += " ON t1.DATETIME = t2.DATETIME LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y') DATETIME, COUNT(DISTINCT AcctSessionId) ZNUM";
                        strSql += " FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t3";
                        else
                            strSql += " GROUP BY DATETIME) t3";
                        strSql += " ON t1.DATETIME = t3.DATETIME LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y') DATETIME, COUNT(DISTINCT AcctSessionId) RZNUM";
                        strSql += " FROM openssid WHERE AcctSessionId IN (SELECT AcctSessionId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t4";
                        else
                            strSql += " GROUP BY DATETIME) t4";
                        strSql += " ON t1.DATETIME = t4.DATETIME";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.Month:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyyMM", type) + ") t1 LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m') DATETIME, COUNT(AcctSessionId) RCNUM";
                        strSql += " FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t2";
                        else
                            strSql += " GROUP BY DATETIME) t2";
                        strSql += " ON t1.DATETIME = t2.DATETIME LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m') DATETIME, COUNT(DISTINCT AcctSessionId) ZNUM";
                        strSql += " FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t3";
                        else
                            strSql += " GROUP BY DATETIME) t3";
                        strSql += " ON t1.DATETIME = t3.DATETIME LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m') DATETIME, COUNT(DISTINCT AcctSessionId) RZNUM";
                        strSql += " FROM openssid WHERE AcctSessionId IN (SELECT AcctSessionId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t4";
                        else
                            strSql += " GROUP BY DATETIME) t4";
                        strSql += " ON t1.DATETIME = t4.DATETIME";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.Day:
                        strSql += createDateFaildSubSql(startTime, endTime, "yyyyMMdd", type) + ") t1 LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m%d') DATETIME, COUNT(AcctSessionId) RCNUM";
                        strSql += " FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t2";
                        else
                            strSql += " GROUP BY DATETIME) t2";
                        strSql += " ON t1.DATETIME = t2.DATETIME LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m%d') DATETIME, COUNT(DISTINCT AcctSessionId) ZNUM";
                        strSql += " FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t3";
                        else
                            strSql += " GROUP BY DATETIME) t3";
                        strSql += " ON t1.DATETIME = t3.DATETIME LEFT JOIN (";
                        strSql += " SELECT date_format(CurrentTime, '%Y%m%d') DATETIME, COUNT(DISTINCT AcctSessionId) RZNUM";
                        strSql += " FROM openssid WHERE AcctSessionId IN (SELECT AcctSessionId FROM radius.radacct)";
                        strSql += " AND OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ") GROUP BY DATETIME) t4";
                        else
                            strSql += " GROUP BY DATETIME) t4";
                        strSql += " ON t1.DATETIME = t4.DATETIME";
                        break;
                    case CustomEnum.ENUM_Statistical_Type.RealTime:

                        strSql = "SELECT * FROM";
                        strSql += " (SELECT IFNULL(SUM(RCNUM),0) RCNUM FROM (SELECT COUNT(AcctSessionId) RCNUM FROM luobo.sys_apdevice a";
                        strSql += " LEFT JOIN openssid b ON SUBSTR(CONVERT(a.MAC USING utf8)COLLATE utf8_unicode_ci,4,12) = SUBSTR(CalledStationId,4,12)";
                        strSql += " WHERE OID IN(SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND a.ID = " + APID;
                        strSql += " AND CurrentTime > adddate(now(), INTERVAL - (a.HBINTERVAL * 2) SECOND)";
                        strSql += " GROUP BY a.MAC)t) t1,";
                        strSql += " (SELECT IFNULL(SUM(ZNUM),0) ZNUM FROM (SELECT COUNT(DISTINCT AcctSessionId) ZNUM FROM luobo.sys_apdevice a";
                        strSql += " LEFT JOIN openssid b ON SUBSTR(CONVERT(a.MAC USING utf8)COLLATE utf8_unicode_ci,4,12) = SUBSTR(CalledStationId,4,12)";
                        strSql += " WHERE OID IN( SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND a.ID = " + APID;
                        strSql += " AND CurrentTime > adddate(now(), INTERVAL - (a.HBINTERVAL * 2) SECOND)";
                        strSql += " GROUP BY a.MAC)t) t2,";
                        strSql += " (SELECT IFNULL(SUM(RZNUM),0) RZNUM FROM (SELECT COUNT(DISTINCT AcctSessionId) RZNUM FROM luobo.sys_apdevice a";
                        strSql += " LEFT JOIN openssid b ON SUBSTR(CONVERT(a.MAC USING utf8)COLLATE utf8_unicode_ci,4,12) = SUBSTR(CalledStationId,4,12)";
                        strSql += " WHERE OID IN(SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                        if (APID != -99)
                            strSql += " AND a.ID = " + APID;
                        strSql += " AND AcctSessionId IN (SELECT AcctSessionId FROM radius.radacct)";
                        strSql += " AND CurrentTime > adddate(now(), INTERVAL - (a.HBINTERVAL * 2) SECOND)";
                        strSql += " GROUP BY a.MAC)t) t3";
                        break;
                }
                if (type != CustomEnum.ENUM_Statistical_Type.RealTime)
                    strSql += " ORDER BY t1.DATETIME";
                DataTable dt = mySql.GetDataTable(strSql, "OPENSSID");

                List<Int64> RCNUM = new List<Int64>();
                List<Int64> ZNUM = new List<Int64>();
                List<Int64> RZNUM = new List<Int64>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        RCNUM.Add(Convert.ToInt64(row["RCNUM"]));
                        ZNUM.Add(Convert.ToInt64(row["ZNUM"]));
                        RZNUM.Add(Convert.ToInt64(row["RZNUM"]));
                    }
                    list.Add(RCNUM);
                    list.Add(ZNUM);
                    list.Add(RZNUM);
                }

                return list;
            }
        }

        public List<OpenSSID> SelectByUser(long userid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select c.* from luobo.sys_user a , statistical.openssid c where  a.OID=c.OID and a.ID=@USERID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@USERID",userid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OPENSSID", parms);
                List<OpenSSID> list = new List<OpenSSID>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = DataChange<OpenSSID>.FillModel(dt);
                }
                return list;
            }
        }

        public List<M_Passager> SelectPassagers(Int64 OID, string apMac, string column, string orderby, Int64 size, Int64 curPage)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT a.ID,a.CallingStationId AS MAC,max(a.CurrentTime) AS ConnectTime,IFNULL(b.AcctSessionTime,0) AS OnLineTime,Count(a.AcctSessionId)AS OnLineCounts,Count(DISTINCT a.AcctSessionId) AS LineCounts,";
                strSql += " IFNULL((b.AcctInputOctets + b.AcctOutputOctets),0) AS UsedTraffic,GROUP_CONCAT(DISTINCT IFNULL(c.userType ,- 1)) AS OnLineType";
                strSql += " FROM OPENSSID AS a LEFT JOIN(radius.RADACCT AS b,radius.RADCHECK AS c)";
                strSql += " ON SUBSTR(a.CalledStationId,4,12) = SUBSTR(b.CalledStationId,4,12) AND a.CallingStationId = b.CallingStationId";
                strSql += " AND b.UserName = c.UserName AND a.AcctSessionId = b.AcctSessionId";
                strSql += " WHERE a.OID = " + OID;
                if (apMac != "")
                {
                    apMac = apMac.Substring(3, apMac.Length - 5);
                    strSql += " AND SUBSTR(a.CalledStationId,4,12) = '" + apMac + "'";
                }
                strSql += " GROUP BY a.CallingStationId HAVING a.ID NOT IN ({0})";
                if (orderby.Trim() != "")
                    strSql += " ORDER BY " + column + " " + orderby;
                else
                    strSql += " ORDER BY max(a.CurrentTime) DESC";
                strSql += " LIMIT " + size;

                string strChildSql = "SELECT ID FROM (SELECT a.ID FROM OPENSSID AS a LEFT JOIN(radius.RADACCT AS b,radius.RADCHECK AS c)";
                strChildSql += " ON SUBSTR(a.CalledStationId,4,12) = SUBSTR(b.CalledStationId,4,12) AND a.CallingStationId = b.CallingStationId AND b.UserName = c.UserName AND a.AcctSessionId = b.AcctSessionId";
                strChildSql += " WHERE a.OID = " + OID;
                if (apMac != "")
                {
                    //apMac = apMac.Substring(3, apMac.Length - 5);
                    strChildSql += " AND SUBSTR(a.CalledStationId,4,12) = '" + apMac + "'";
                }
                strChildSql += " GROUP BY a.CallingStationId";
                if (orderby.Trim() != "")
                    strChildSql += " ORDER BY " + column + " " + orderby;
                else
                    strChildSql += " ORDER BY max(a.CurrentTime) DESC";
                strChildSql += " LIMIT " + ((curPage - 1) * size) + ")t";

                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "M_Passager");
                List<M_Passager> list = new List<M_Passager>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = DataChange<M_Passager>.FillModel(dt);
                }
                return list;
            }
        }

        public Int64 SelectPassagersCount(Int64 OID, string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT COUNT(ID) FROM(SELECT a.ID";
                strSql += " FROM OPENSSID AS a LEFT JOIN(radius.RADACCT AS b)";
                strSql += " ON SUBSTR(a.CalledStationId,4,12) = SUBSTR(b.CalledStationId,4,12) AND a.CallingStationId = b.CallingStationId";
                strSql += " AND a.AcctSessionId = b.AcctSessionId";
                strSql += " WHERE a.OID = " + OID;
                if (apMac != "")
                {
                    apMac = apMac.Substring(3, apMac.Length - 5);
                    strSql += " AND SUBSTR(a.CalledStationId,4,12) = '" + apMac + "'";
                }
                strSql += " GROUP BY a.CallingStationId)t";

                DataTable dt = mySql.GetDataTable(strSql, "M_Passager");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt64(dt.Rows[0][0]);
                }
                return 0;
            }
        }

        public Int64 SelectAvgVisitNumByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT ROUND(IFNULL(AVG(NUM), 0)) NUM FROM";
                strSql += " (SELECT SUBSTR(CalledStationId,4,12),COUNT(1) NUM FROM openssid";
                strSql += " WHERE OID IN(SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                strSql += " GROUP BY SUBSTR(CalledStationId,4,12)) t";
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql));
            }
        }

        public List<Int64> SelectUserForStateByOID(Int64 OID, string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT ONLINEPEOPLENUM, RZNUM, FWRCNUM, FWRCTIME FROM";
                strSql += " (SELECT IFNULL(ONLINEPEOPLENUM,0) ONLINEPEOPLENUM FROM luobo.sys_apdevice a INNER JOIN luobo.sys_aporg b ON a.ID = b.APID";
                strSql += " WHERE (b.OID LIKE '%$" + OID + "$%' OR b.OID = " + OID + ") AND a.MAC = '" + apMac + "') t1,";
                strSql += " (SELECT IFNULL(SUM(RZNUM),0) RZNUM FROM (";
                strSql += " SELECT COUNT(DISTINCT AcctSessionId) RZNUM FROM openssid WHERE AcctSessionId IN (SELECT AcctSessionId FROM radius.radacct)";
                strSql += " AND SUBSTR(CalledStationId,4,12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) MAC FROM luobo.sys_apdevice a";
                strSql += " INNER JOIN luobo.sys_aporg b ON a.ID = b.APID WHERE (b.OID LIKE '%$" + OID + "$%' OR b.OID = " + OID + ") AND a.MAC = '" + apMac + "')";
                strSql += " GROUP BY SUBSTR(CalledStationId,4,12))t) t2,";
                strSql += " (SELECT IFNULL(SUM(FWRCNUM),0) FWRCNUM FROM (";
                strSql += " SELECT ROUND(IFNULL(AVG(FWRCNUM), 0)) FWRCNUM FROM (SELECT SUBSTR(CalledStationId,4,12) MAC,CallingStationId,COUNT(1) FWRCNUM FROM openssid GROUP BY MAC,CallingStationId) t";
                strSql += " WHERE MAC IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) MAC FROM luobo.sys_apdevice a";
                strSql += " INNER JOIN luobo.sys_aporg b ON a.ID = b.APID WHERE (b.OID LIKE '%$" + OID + "$%' OR b.OID = " + OID + ") AND a.MAC = '" + apMac + "')";
                strSql += " GROUP BY MAC)t) t3,";
                strSql += " (SELECT IFNULL(SUM(FWRCTIME),0) FWRCTIME FROM (";
                strSql += " SELECT ROUND(IFNULL(AVG(FWRCTIME), 0)) FWRCTIME FROM (";
                strSql += " SELECT SUBSTR(CalledStationId,4,12) MAC,CallingStationId,SUM(UNIX_TIMESTAMP(acctstoptime) - UNIX_TIMESTAMP(acctstarttime)) FWRCTIME FROM radius.radacct";
                strSql += " GROUP BY MAC,CallingStationId) t";
                strSql += " WHERE MAC IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) MAC FROM luobo.sys_apdevice a";
                strSql += " INNER JOIN luobo.sys_aporg b ON a.ID = b.APID WHERE (b.OID LIKE '%$" + OID + "$%' OR b.OID = " + OID + ") AND a.MAC = '" + apMac + "')";
                strSql += " GROUP BY MAC)t) t4";

                DataTable dt = mySql.GetDataTable(strSql, "M_Passager");
                List<Int64> list = new List<Int64>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    list.Add(Convert.ToInt64(dt.Rows[0]["ONLINEPEOPLENUM"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["RZNUM"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["FWRCNUM"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["FWRCTIME"]));
                }
                return list;
            }
        }

        public List<OpenSSID_VIEW>  SelectInfoByCallingID(Int64 OID, string callingID, string column, string orderby)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<OpenSSID_VIEW> list = new List<OpenSSID_VIEW>();
                string strSql = "select t1.*, t2.ALIAS, t3.NAME as 'SSIDNAME' from statistical.openssid t1, luobo.sys_apdevice t2, luobo.sys_ssid t3 where t1.SSID = t3.ID and t3.APID = t2.ID";
                if (!string.IsNullOrEmpty(callingID))
                {
                    strSql += " and t1.CallingStationId = '" + callingID + "'";

                    if (OID != 0)
                    {
                        strSql += " and t1.OID = " + OID;
                    }
                    if (string.IsNullOrEmpty(column))
                        strSql += " ORDER BY CurrentTime desc";
                    else
                        strSql += " ORDER BY " + column + " " + orderby;
                    DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        list = DataChange<OpenSSID_VIEW>.FillModel(dt);
                    }
                }
                
                return list;
            }
        }

        public List<Int64> SelectOLInfoByCallingID(string callingID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<Int64> list = new List<Int64>();
                string strSql = "select ifnull(sum(acctsessiontime),0) as ZSC, ifnull(sum(acctinputoctets+acctoutputoctets),0) as ZLL from radius.radacct where 1=1";
                if (!string.IsNullOrEmpty(callingID))
                {
                    strSql += " and CallingStationId = '" + callingID + "'";
                    DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        list.Add(Convert.ToInt64(dt.Rows[0]["ZSC"]));
                        list.Add(Convert.ToInt64(dt.Rows[0]["ZLL"]));
                    }
                }
                return list;
            }
        }

        public List<StatisticalAP> SelectSSIDPeopleStatistical(Int64 OID, Int64 APID, DateTime startDate, DateTime endDate)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<StatisticalAP> list = new List<StatisticalAP>();
                string strSql = "SELECT t1.ID, trim(t1.`NAME`) as `NAME`,ifnull(t2.RZS+t2.WRZS,0)LJRS, ifnull(t2.RZS,0)RZS, ifnull(t2.WRZS,0)WRZS FROM luobo.sys_ssid t1 LEFT JOIN(";
                strSql += "select a.*, b.WRZS from (";
                strSql += "(select ssid,COUNT(DISTINCT CallingStationId) as RZS from statistical.openssid where";
                strSql += " date_format(CurrentTime, '%Y%m%d')>= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d')<= date_format(@EndDate, '%Y%m%d')";
                strSql += " and CallingStationId in (select DISTINCT CallingStationId from radius.radacct) group by ssid";
                strSql += " ) a,";
                strSql += " (select ssid,COUNT(DISTINCT CallingStationId) as WRZS from statistical.openssid where";
                strSql += " date_format(CurrentTime, '%Y%m%d')>= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d')<= date_format(@EndDate, '%Y%m%d')";
                strSql += " and CallingStationId not in (select DISTINCT CallingStationId from radius.radacct) group by ssid";
                strSql += " ) b) where a.ssid = b.ssid";
                strSql += " )t2 ON t1.id = t2.ssid";
                strSql += " WHERE t1.oid IN(SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                strSql += " and t1.APID = @APID";
                strSql += " and t1.ISON = true";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",startDate),
                    new MySqlParameter("@EndDate",endDate),
                    new MySqlParameter("@APID",APID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    StatisticalAP stat = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stat = new StatisticalAP();
                        stat.ID = Convert.ToInt64(dt.Rows[i]["ID"]);
                        stat.NAME = dt.Rows[i]["NAME"].ToString();
                        stat.NUM = new List<Int64>();
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["LJRS"]));
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["RZS"]));
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["WRZS"]));
                        list.Add(stat);
                    }
                }
                return list;
            }
        }

        public List<M_Statistical> SelectAuthenticationPeopleStatistical(Int64 OID, Int64 APID, DateTime startDate, DateTime endDate)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<M_Statistical> list = new List<M_Statistical>();
                string strSql = "SELECT IFNULL(c.userType, -1) userType,COUNT(DISTINCT a.CallingStationId) RZS FROM statistical.openssid a";
                strSql += " LEFT JOIN radius.RADACCT b ON a.CallingStationId = b.CallingStationId";
                strSql += " LEFT JOIN radius.RADCHECK c ON b.UserName = c.UserName";
                strSql += " WHERE date_format(a.CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " AND date_format(a.CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " AND a.oid IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                strSql += " AND SUBSTR(a.CalledStationId, 4, 12) IN (SELECT SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE id = @APID)";
                strSql += " GROUP BY c.userType ORDER BY IFNULL(c.userType, -1)";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",startDate),
                    new MySqlParameter("@EndDate",endDate),
                    new MySqlParameter("@APID",APID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    M_Statistical stat = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stat = new M_Statistical();
                        stat.NAME = dt.Rows[i]["userType"].ToString();
                        stat.NUM = Convert.ToInt32(dt.Rows[i]["RZS"]);
                        list.Add(stat);
                    }
                }
                return list;
            }
        }

        public List<StatisticalAP> SelectSSIDUseTimeStatistical(Int64 OID, Int64 APID, DateTime startDate, DateTime endDate)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<StatisticalAP> list = new List<StatisticalAP>();
                string strSql = "select t1.ID, trim(`NAME`)`NAME`, ifnull(t2.ZSC,0)ZSC, ifnull(t2.PJSC,0)PJSC from luobo.sys_ssid t1 left join ";
                strSql += " (select ssid, sum(acctsessiontime)ZSC,avg(acctsessiontime)PJSC from radius.radacct a join ((select ssid,CallingStationId,OID from statistical.openssid GROUP BY CallingStationId) b)";
                strSql += " on a.callingstationid=b.callingstationid";
                strSql += " where date_format(acctstarttime, '%Y%m%d')>= date_format(@startDate, '%Y%m%d')";
                strSql += " and date_format(acctstoptime, '%Y%m%d')<= date_format(@endDate, '%Y%m%d')";
                strSql += " group by ssid) t2 on t1.ID=t2.ssid where t1.oid IN(SELECT id FROM luobo.SYS_ORGANIZATION ";
                strSql += " WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ") and t1.id in (select id from luobo.sys_ssid where apid = @APID) and t1.ISON = true";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",startDate),
                    new MySqlParameter("@EndDate",endDate),
                    new MySqlParameter("@APID",APID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    StatisticalAP stat = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stat = new StatisticalAP();
                        stat.ID = Convert.ToInt64(dt.Rows[i]["ID"]);
                        stat.NAME = dt.Rows[i]["NAME"].ToString();
                        stat.NUM = new List<Int64>();
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["ZSC"]));
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["PJSC"]));
                        list.Add(stat);
                    }
                }
                return list;
            }
        }

        public List<StatisticalAP> SelectSSIDTrafficStatistical(Int64 OID, Int64 APID, DateTime startDate, DateTime endDate)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<StatisticalAP> list = new List<StatisticalAP>();
                string strSql = " select t1.ID, trim(`NAME`)NAME, ifnull(t2.ZLL,0)ZLL, ifnull(t2.PJLL,0)PJLL from luobo.sys_ssid t1 left join ";
                strSql += " (select ssid, sum(acctinputoctets+acctoutputoctets)ZLL,avg(acctinputoctets+acctoutputoctets)PJLL from radius.radacct a join ((select ssid,CallingStationId,OID from statistical.openssid GROUP BY CallingStationId) b)";
                strSql += " on a.callingstationid=b.callingstationid";
                strSql += " where date_format(acctstarttime, '%Y%m%d')>= date_format(@startDate, '%Y%m%d')";
                strSql += " and date_format(acctstoptime, '%Y%m%d')<= date_format(@endDate, '%Y%m%d')";
                strSql += " group by ssid) t2 on t1.ID=t2.ssid where t1.oid IN(SELECT id FROM luobo.SYS_ORGANIZATION ";
                strSql += " WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ") and t1.id in (select id from luobo.sys_ssid where apid = @APID) and t1.ISON = true";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",startDate),
                    new MySqlParameter("@EndDate",endDate),
                    new MySqlParameter("@APID",APID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    StatisticalAP stat = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stat = new StatisticalAP();
                        stat.ID = Convert.ToInt64(dt.Rows[i]["ID"]);
                        stat.NAME = dt.Rows[i]["NAME"].ToString();
                        stat.NUM = new List<Int64>();
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["ZLL"]));
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["PJLL"]));
                        list.Add(stat);
                    }
                }
                return list;
            }
        }

        public List<StatisticalAP> SelectAPOfADStatistical(Int64 OID, Int64 APID, DateTime startDate, DateTime endDate)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<StatisticalAP> list = new List<StatisticalAP>();

                string strSql = "select t1.AD_Title, t2.* from luobo.ad_info t1 left join (";
                strSql += " (select AdId, ifnull(COUNT(1),0)ZRC,ifnull(COUNT(DISTINCT CallingStationId),0)ZRS from statistical.openssid where ";
                strSql += " date_format(CurrentTime, '%Y%m%d')>= date_format(@startDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d')<= date_format(@endDate, '%Y%m%d')";
                strSql += " and ssid in (select id from luobo.sys_ssid WHERE apid=@APID";
                strSql += " AND  OID IN(SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + "))";
                strSql += " GROUP BY AdId) t2) on AD_ID=AdId where AD_ID in (select ADID from luobo.sys_ssid WHERE ISON=true ";
                strSql += " and apid=@APID ";
                strSql += " and OID in(SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + "))";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",startDate),
                    new MySqlParameter("@EndDate",endDate),
                    new MySqlParameter("@APID",APID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt != null && dt.Rows.Count > 0)
                {
                    StatisticalAP stat = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stat = new StatisticalAP();
                        stat.ID = Convert.ToInt64(dt.Rows[i]["AdId"]);
                        stat.NAME = dt.Rows[i]["AD_Title"].ToString();
                        stat.NUM = new List<Int64>();
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["ZRC"]));
                        stat.NUM.Add(Convert.ToInt64(dt.Rows[i]["ZRS"]));
                        list.Add(stat);
                    }
                }
                return list;
            }
        }

        public List<Int64> SelectTowHourIntervalPeopleCount(Int64 OID, Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                DateTime startTime = DateTime.Now.Date;
                DateTime endTime = DateTime.Now;
                string strTimeSql = "";
                while (startTime < endTime)
                {
                    if (strTimeSql != "")
                        strTimeSql += " UNION ";
                    strTimeSql += "SELECT '" + startTime.ToString("yyyyMMddHH") + "' AS DATETIME";
                    startTime = startTime.AddHours(2);
                }

                string strSql = "SELECT t1.DATETIME,IFNULL(t2.RCCount,0) RCCount FROM (" + strTimeSql + ") t1 LEFT JOIN";
                strSql += " (SELECT IF(date_format(CurrentTime, '%Y%m%d%H')%2=1,date_format(DATE_ADD(CurrentTime, INTERVAL -1 HOUR), '%Y%m%d%H'),date_format(CurrentTime, '%Y%m%d%H')) DATETIME";
                strSql += " ,COUNT(AcctSessionId) RCCount FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                if (APID != -99)
                    strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                strSql += " GROUP BY DATETIME) t2 ON t1.DATETIME = t2.DATETIME ORDER BY t1.DATETIME";
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");

                List<Int64> list = new List<Int64>();
                if (dt != null && dt.Rows.Count > 0)
                    foreach (DataRow row in dt.Rows)
                        list.Add(Convert.ToInt64(row["RCCount"]));

                return list;
            }
        }

        public List<StatisticalAP> SelectTowHourIntervalModelCount(Int64 OID, Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                List<string> ModelTyeList = new List<string> { "其他", "iPhone", "iPad", "Android", "Windows Phone", "Windows NT", "Mac OS" };
                DateTime startTime = DateTime.Now.Date;
                DateTime endTime = DateTime.Now;
                string strTimeSql = "";
                while (startTime < endTime)
                {
                    if (strTimeSql != "")
                        strTimeSql += " UNION ";
                    strTimeSql += "SELECT '" + startTime.ToString("yyyyMMddHH") + "' AS DATETIME";
                    startTime = startTime.AddHours(2);
                }

                string strSql = "SELECT t1.DATETIME,IFNULL(MODELTYPE,-99) MODELTYPE,IFNULL(t2.RCCount,0) RCCount FROM (" + strTimeSql + ") t1 LEFT JOIN";
                strSql += " (SELECT IF(date_format(CurrentTime, '%Y%m%d%H')%2=1,date_format(DATE_ADD(CurrentTime, INTERVAL -1 HOUR), '%Y%m%d%H'),date_format(CurrentTime, '%Y%m%d%H')) DATETIME";
                strSql += " ,CASE WHEN INSTR(UserAgent,'iPhone') THEN 1 WHEN INSTR(UserAgent,'iPad') THEN 2 WHEN INSTR(UserAgent,'Android') THEN 3 WHEN INSTR(UserAgent,'Adr ') THEN 3";
                strSql += " WHEN INSTR(UserAgent,'Windows Phone') THEN 4 WHEN INSTR(UserAgent,'Windows NT') THEN 5 WHEN INSTR(UserAgent,'Mac OS') THEN 6 ELSE 0 END MODELTYPE";
                strSql += " ,COUNT(AcctSessionId) RCCount FROM openssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                if (APID != -99)
                    strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                strSql += " GROUP BY DATETIME,MODELTYPE) t2 ON t1.DATETIME = t2.DATETIME ORDER BY t1.DATETIME";
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");

                List<StatisticalAP> list = new List<StatisticalAP>();
                List<string> tmpList = new List<string>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!tmpList.Contains(row["DATETIME"].ToString()))
                            tmpList.Add(row["DATETIME"].ToString());

                        if (Convert.ToInt32(row["MODELTYPE"]) != -99)
                            if (list.Where(c => c.NAME == ModelTyeList[Convert.ToInt32(row["MODELTYPE"])]).Count() == 0)
                                list.Add(new StatisticalAP { NAME = ModelTyeList[Convert.ToInt32(row["MODELTYPE"])], NUM = new List<Int64>() });
                    }

                    bool flag = true;
                    foreach (string item in tmpList)
                    {
                        DataRow[] rowArr = dt.Select("DATETIME='" + item + "'");

                        foreach (StatisticalAP sAP in list)
                        {
                            flag = true;
                            foreach (DataRow row in rowArr)
                            {
                                if (Convert.ToInt32(row["MODELTYPE"]) != -99)
                                {
                                    if (sAP.NAME == ModelTyeList[Convert.ToInt32(row["MODELTYPE"])])
                                    {
                                        sAP.NUM.Add(Convert.ToInt64(row["RCCount"]));
                                        flag = false;
                                    }
                                }
                            }

                            if (flag)
                                sAP.NUM.Add(0);
                        }
                    }
                }

                return list;
            }
        }

        public List<StatisticalAP> SelectTowHourIntervalSSIDCount(Int64 OID, Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                DateTime startTime = DateTime.Now.Date;
                DateTime endTime = DateTime.Now;
                string strTimeSql = "";
                while (startTime < endTime)
                {
                    if (strTimeSql != "")
                        strTimeSql += " UNION ";
                    strTimeSql += "SELECT '" + startTime.ToString("yyyyMMddHH") + "' AS DATETIME";
                    startTime = startTime.AddHours(2);
                }

                string strSql = "SELECT t1.DATETIME,NAME,IFNULL(t2.RCCount,0) RCCount FROM (" + strTimeSql + ") t1 LEFT JOIN";
                strSql += " (SELECT IF(date_format(CurrentTime, '%Y%m%d%H')%2=1,date_format(DATE_ADD(CurrentTime, INTERVAL -1 HOUR), '%Y%m%d%H'),date_format(CurrentTime, '%Y%m%d%H')) DATETIME";
                strSql += " ,TRIM(b.NAME) NAME,COUNT(AcctSessionId) RCCount FROM openssid a LEFT JOIN luobo.sys_ssid b ON a.SSID = b.ID";
                strSql += " WHERE a.OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR id = " + OID + ")";
                if (APID != -99)
                    strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM luobo.sys_apdevice WHERE ID = " + APID + ")";
                strSql += " GROUP BY DATETIME,TRIM(b.NAME)) t2 ON t1.DATETIME = t2.DATETIME ORDER BY t1.DATETIME";
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");

                List<StatisticalAP> list = new List<StatisticalAP>();
                List<string> tmpList = new List<string>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!tmpList.Contains(row["DATETIME"].ToString()))
                            tmpList.Add(row["DATETIME"].ToString());

                        if (row["NAME"].ToString() != "")
                            if (list.Where(c => c.NAME == row["NAME"].ToString()).Count() == 0)
                                list.Add(new StatisticalAP { NAME = row["NAME"].ToString(), NUM = new List<Int64>() });
                    }

                    bool flag = true;
                    foreach (string item in tmpList)
                    {
                        DataRow[] rowArr = dt.Select("DATETIME='" + item + "'");

                        foreach (StatisticalAP sAP in list)
                        {
                            flag = true;
                            foreach (DataRow row in rowArr)
                            {
                                if (sAP.NAME == row["NAME"].ToString())
                                {
                                    sAP.NUM.Add(Convert.ToInt64(row["RCCount"]));
                                    flag = false;
                                }
                            }

                            if (flag)
                                sAP.NUM.Add(0);
                        }
                    }
                }

                return list;
            }
        }

        public List<M_AL_ALARM_OPENSSID> SelectAlarmSSID()
        {
            List<M_AL_ALARM_OPENSSID> list = new List<M_AL_ALARM_OPENSSID>();
            if (Helper.CacheHelper.Instance().GetCache("M_AL_ALARM_OPENSSID") == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
                {
                    string strSql = " (select t1.*, MAX(CurrentTime)AS CurrentTime from ";
                    strSql += " (select a.ID AS SSID, a.OID ,trim(a.`NAME`)AS SSIDNAME,a.APID,b.ALIAS from luobo.sys_ssid a, luobo.sys_apdevice b where a.APID=b.ID ";
                    strSql += " and a.ISON=1 and b.DEVICESTATE=1  and a.ID in (select SSID from luobo.sys_alarmscope)";
                    strSql += " )t1 left join statistical.openssid t2 on t1.SSID=t2.SSID group by t1.SSID,t1.OID)";
                    DataTable dt = mySql.GetDataTable(strSql, "OpenSSID");
                    if (dt.Rows.Count > 0)
                    {
                        list = DataChange<M_AL_ALARM_OPENSSID>.FillModel(dt);
                    }
                    Helper.CacheHelper.Instance().SetCache("M_AL_ALARM_OPENSSID", list);
                }
            }
            else
                list = Helper.CacheHelper.Instance().GetCache("M_AL_ALARM_OPENSSID") as List<M_AL_ALARM_OPENSSID>;
            return list;
        }

        #region 月统计
        /// <summary>
        /// 机构下根据AP不同广告的点击次数
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<M_Statistical> GetADByAPAndDate(Int64 oid, Int64 apid, DateTime date, bool isDown)
        {
            List<M_Statistical> list = new List<M_Statistical>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select Title, count(1) from statistical.openssid where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                if (isDown)
                    strSql += " AND PageUrl NOT LIKE '%portal.html' and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME = 'APP下载模板')";
                else
                    strSql += " and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME != 'APP下载模板')";

                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by Title";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                foreach (DataRow row in dt.Rows)
                    list.Add(new M_Statistical { NAME = row[0].ToString(), NUM = Convert.ToInt64(row[1]) });
            }
            return list;
        }

        /// <summary>
        /// 机构下根据AP不同时段的广告点击次数（6-12 12-18 18-6点）
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<Int64> GetClicksByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            List<Int64> list = new List<Int64>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select sum(if(HOUR(CurrentTime)>=6 && HOUR(CurrentTime)<12,1,0)) as SHANGWU,sum(if(HOUR(CurrentTime)>=12 && HOUR(CurrentTime)<18,1,0)) as XIAWU,sum(if((HOUR(CurrentTime)>=18 && HOUR(CurrentTime)<24) || (HOUR(CurrentTime)>=0 &&HOUR(CurrentTime)<6),1,0)) as WANSHANG";
                strSql += " from statistical.openssid where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    list.Add(Convert.ToInt64(dt.Rows[0][0]));
                    list.Add(Convert.ToInt64(dt.Rows[0][1]));
                    list.Add(Convert.ToInt64(dt.Rows[0][2]));
                }
            }
            return list;
        }

        /// <summary>
        /// 机构下指定AP访问人次 或 总访问人次
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public Int64 GetVisitsByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            Int64 data = 0;
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select count(1) from (select count(1) from statistical.openssid";
                strSql += " where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by AcctSessionId) a";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                    data = Convert.ToInt64(dt.Rows[0][0]);
            }
            return data;
        }

        /// <summary>
        /// 机构下指定AP认证后总访问时长和平均时长
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<Int64> GetApproveTimeByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            List<Int64> list = new List<Int64>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select sum(acctsessiontime)/60 as ZONGSHICHANG,sum(acctsessiontime)/60/count(1) as PINGJUNSHICHANG from radius.radacct where acctsessionid in (";
                strSql += " select acctsessionid from statistical.openssid where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += ")";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == "")
                    {
                        list.Add(0);
                        list.Add(0);
                    }
                    else
                    {
                        list.Add(Convert.ToInt64(dt.Rows[0][0]));
                        list.Add(Convert.ToInt64(dt.Rows[0][1]));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 暂时可以被下边的方法取代GetOSADClicksByAPAndDate
        /// 业务推广分析 下载分析 -- 不同广告页不同时段的查看次数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <param name="isDown">是否下载</param>
        /// <returns></returns>
        public List<StatisticalAP> GetPromotionByAPAndDate(Int64 oid, Int64 apid, DateTime date, bool isDown)
        {
            List<StatisticalAP> list = new List<StatisticalAP>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select Title, count(1),sum(if(HOUR(CurrentTime)>=6 && HOUR(CurrentTime)<12,1,0)) as SHANGWU,sum(if(HOUR(CurrentTime)>=12 && HOUR(CurrentTime)<18,1,0)) as XIAWU,sum(if((HOUR(CurrentTime)>=18 && HOUR(CurrentTime)<24) || (HOUR(CurrentTime)>=0 &&HOUR(CurrentTime)<6),1,0)) as WANSHANG";
                strSql += " from statistical.openssid where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                if (isDown)
                    strSql += " AND PageUrl NOT LIKE '%portal.html' and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME = 'APP下载模板')";
                else
                    strSql += " and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME != 'APP下载模板')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by Title";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                foreach (DataRow row in dt.Rows)
                    list.Add(new StatisticalAP { NAME = row[0].ToString(), NUM = new List<Int64> { Convert.ToInt64(row[1]), Convert.ToInt64(row[2]), Convert.ToInt64(row[3]), Convert.ToInt64(row[4]) } });
            }
            return list;
        }

        /// <summary>
        /// 业务推广分析 下载分析 -- 不同手机操作系统，不同时段的广告或下载点击数
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <param name="isDown">是否下载</param>
        /// <returns></returns>
        public List<StatisticalAP> GetOSADClicksByAPAndDate(Int64 oid, Int64 apid, DateTime date, bool isDown)
        {
            List<StatisticalAP> list = new List<StatisticalAP>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT IFNULL(MODELTYPE , '其他') MODELTYPE,IFNULL(t2.RCCount, 0) RCCount,SHANGWU,XIAWU,WANSHANG FROM";
                strSql += " (SELECT sum(if(HOUR(CurrentTime)>=6 && HOUR(CurrentTime)<12,1,0)) as SHANGWU,sum(if(HOUR(CurrentTime)>=12 && HOUR(CurrentTime)<18,1,0)) as XIAWU,sum(if((HOUR(CurrentTime)>=18 && HOUR(CurrentTime)<24) || (HOUR(CurrentTime)>=0 &&HOUR(CurrentTime)<6),1,0)) as WANSHANG,";
                strSql += " CASE WHEN INSTR(UserAgent, 'iPhone') THEN 'iPhone'";
                strSql += " WHEN INSTR(UserAgent, 'iPad') THEN 'iPad'";
                strSql += " WHEN INSTR(UserAgent, 'Android') THEN 'Android'";
                strSql += " WHEN INSTR(UserAgent, 'Adr ') THEN 'Android'";
                strSql += " WHEN INSTR(UserAgent, 'Windows Phone') THEN 'Windows Phone'";
                strSql += " WHEN INSTR(UserAgent, 'Windows NT') THEN 'Windows NT'";
                strSql += " WHEN INSTR(UserAgent, 'Mac OS') THEN 'Mac OS'";
                strSql += " ELSE '其他' END MODELTYPE,COUNT(AcctSessionId) RCCount FROM statistical.openssid";
                strSql += " where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                if (isDown)
                    strSql += " AND PageUrl NOT LIKE '%portal.html' and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME = 'APP下载模板')";
                else
                    strSql += " and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME != 'APP下载模板')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " GROUP BY MODELTYPE) t2";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                foreach (DataRow row in dt.Rows)
                    list.Add(new StatisticalAP { NAME = row[0].ToString(), NUM = new List<Int64> { Convert.ToInt64(row[1]), Convert.ToInt64(row[2]), Convert.ToInt64(row[3]), Convert.ToInt64(row[4]) } });
            }
            return list;
        }

        /// <summary>
        /// 业务推广分析 下载分析 -- 不同营业厅的广告或下载点击数
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="date"></param>
        /// <param name="isDown"></param>
        /// <returns></returns>
        public List<StatisticalAP> GetOSADClicksByDate(Int64 oid, DateTime date, bool isDown)
        {
            List<StatisticalAP> list = new List<StatisticalAP>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select t3.ALIAS, count(1) from statistical.openssid t1, luobo.sys_ssid t2, luobo.sys_apdevice t3";

                strSql += " where t1.SSID=t2.ID and t2.APID=t3.ID";
                strSql += " and t1.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " and t2.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (isDown)
                    strSql += " AND PageUrl NOT LIKE '%portal.html' and t1.AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME = 'APP下载模板')";
                else
                    strSql += " and t1.AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME != 'APP下载模板')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by t2.APID";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd"))
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                foreach (DataRow row in dt.Rows)
                    list.Add(new StatisticalAP { NAME = row[0].ToString(), NUM = new List<Int64> { Convert.ToInt64(row[1]) } });
            }
            return list;
        }
        /// <summary>
        /// 用户行为与构成分析 -- 不同操作系统不同SSID的点击次数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP<M_Statistical>> GetUserBehaviorByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            List<StatisticalAP<M_Statistical>> list = new List<StatisticalAP<M_Statistical>>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT t2.MODELTYPE MODELTYPE,t2.MNAME,IFNULL(t1.RCCount, 0) RCCount FROM (SELECT TRIM(b.`NAME`) MNAME,";
                strSql += " CASE WHEN INSTR(UserAgent, 'iPhone') THEN 'iPhone'";
                strSql += " WHEN INSTR(UserAgent, 'iPad') THEN 'iPad'";
                strSql += " WHEN INSTR(UserAgent, 'Android') THEN 'Android'";
                strSql += " WHEN INSTR(UserAgent, 'Adr ') THEN 'Android'";
                strSql += " WHEN INSTR(UserAgent, 'Windows Phone') THEN 'Windows Phone'";
                strSql += " WHEN INSTR(UserAgent, 'Windows NT') THEN 'Windows NT'";
                strSql += " WHEN INSTR(UserAgent, 'Mac OS') THEN 'Mac OS'";
                strSql += " ELSE '其他' END MODELTYPE,COUNT(AcctSessionId) RCCount";
                strSql += " FROM statistical.openssid a,luobo.sys_ssid b WHERE a.SSID = b.ID";
                strSql += " AND a.oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and a.ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
               // if (isDown)
               //     strSql += " and a.AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME = 'APP下载模板')";
               // else
                    //strSql += " and a.AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME != 'APP下载模板')";
                    strSql += " and a.AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID)";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " GROUP BY MODELTYPE,TRIM(b.`NAME`)) t1 RIGHT JOIN (SELECT * FROM";
                strSql += " (SELECT 'iPhone' MODELTYPE UNION SELECT 'iPad' MODELTYPE UNION SELECT 'Android' MODELTYPE UNION SELECT 'Windows Phone' MODELTYPE UNION SELECT 'Windows NT' MODELTYPE UNION SELECT 'Mac OS' MODELTYPE UNION SELECT '其他' MODELTYPE) a,";
                strSql += " (SELECT DISTINCT TRIM(`NAME`) MNAME FROM luobo.sys_ssid WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ") and ison=1";
                if (apid != -99)
                    strSql += " and APID = @APID";
                strSql += ") b";
                strSql += ") t2 ON t1.MNAME = t2.MNAME AND t1.MODELTYPE = t2.MODELTYPE ORDER BY t2.MODELTYPE,t2.MNAME desc";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                StatisticalAP<M_Statistical> sap = null;

                var count = (from r in dt.AsEnumerable()
                         select r["MNAME"]).Distinct().ToList().Count;



                for (int i = 0; i < dt.Rows.Count / count; i++)
                {
                    sap = new StatisticalAP<M_Statistical>();
                    sap.NAME = dt.Rows[i * count][0].ToString();
                    sap.VALUE = new List<M_Statistical>();
                    for (int j = 0; j < count; j++)
                    {
                        sap.VALUE.Add(new M_Statistical { NAME = dt.Rows[i * count + j][1].ToString(), NUM = Convert.ToInt64(dt.Rows[i * count + j][2]) });
                    }
                    list.Add(sap);
                    
                }
            }
            return list;
        }

        /// <summary>
        /// 用户行为与构成分析 -- 不同操作系统的人群分布
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP<M_Statistical>> GetDifferentOSPersonAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            List<StatisticalAP<M_Statistical>> list = new List<StatisticalAP<M_Statistical>>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "";
                strSql += "SELECT t2.MODELTYPE MODELTYPE,IFNULL(t1.RENS, 0) RENS FROM (SELECT MODELTYPE, COUNT(RENS) RENS from (SELECT";
                strSql += " CASE WHEN INSTR(UserAgent, 'iPhone') THEN 'iPhone'";
                strSql += " WHEN INSTR(UserAgent, 'iPad') THEN 'iPad' ";
                strSql += " WHEN INSTR(UserAgent, 'Android') THEN 'Android'";
                strSql += " WHEN INSTR(UserAgent, 'Adr ') THEN 'Android'";
                strSql += " WHEN INSTR(UserAgent, 'Windows Phone') THEN 'Windows Phone'";
                strSql += " WHEN INSTR(UserAgent, 'Windows NT') THEN 'Windows NT'";
                strSql += " WHEN INSTR(UserAgent, 'Mac OS') THEN 'Mac OS'";
                strSql += " ELSE '其他' END MODELTYPE,COUNT(1) RENS ";
                strSql += " FROM statistical.openssid a, luobo.sys_ssid b WHERE a.SSID = b.ID";
                strSql += " AND a.oid IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " AND a.AdId IN (SELECT a.AD_ID FROM luobo.ad_info a,luobo.sys_adtemplet b WHERE a.AD_Model = b.SADT_ID)";
                if(apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                strSql += " AND date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " AND date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " GROUP BY a.CallingStationId) a GROUP BY MODELTYPE) t1";
                strSql += " RIGHT JOIN (SELECT * FROM (";
                strSql += " SELECT 'iPhone' MODELTYPE UNION SELECT 'iPad' MODELTYPE UNION SELECT 'Android' MODELTYPE UNION SELECT 'Windows Phone' MODELTYPE UNION SELECT 'Windows NT' MODELTYPE UNION SELECT 'Mac OS' MODELTYPE UNION SELECT '其他' MODELTYPE) a";
                strSql += " ) t2 ON t1.MODELTYPE = t2.MODELTYPE ORDER BY t2.MODELTYPE";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                StatisticalAP<M_Statistical> sap = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sap = new StatisticalAP<M_Statistical>();
                    sap.NAME = dt.Rows[i][0].ToString();
                    sap.VALUE = new List<M_Statistical>();
                    sap.VALUE.Add(new M_Statistical { NUM = Convert.ToInt64(dt.Rows[i][1]) });
                    list.Add(sap);
                }
            }
            return list;
        }

        /// <summary>
        /// 机构下，访问不同SSID的人群分布 -- 目前分广告和下载SSID，这两种ssid的访问人数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<Int64> GetSSIDVisitByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            List<Int64> list = new List<Int64>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "SELECT * FROM";
                strSql += "(SELECT count(1) WIFI from(";
                strSql += "select count(1) from statistical.openssid";
                strSql += " where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")"; 
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                strSql += " and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME != 'APP下载模板')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by CallingStationId,ssid) t) a,";
                strSql += "(SELECT count(1) Download from(";
                strSql += "select count(1) from statistical.openssid";
                strSql += " where oid in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                if (apid != -99)
                    strSql += " and ssid in (SELECT id FROM luobo.sys_ssid WHERE APID = @APID and ison=1)";
                strSql += " and AdId in (select a.AD_ID from luobo.ad_info a, luobo.sys_adtemplet b where a.AD_Model = b.SADT_ID and b.SADT_NAME = 'APP下载模板')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by CallingStationId,ssid) t) b";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd")),
                    new MySqlParameter("@APID",apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    list.Add(Convert.ToInt64(dt.Rows[0][0]));
                    list.Add(Convert.ToInt64(dt.Rows[0][1]));
                }
            }
            return list;
        }


        /// <summary>
        /// 机构下所有营业厅总人次、总人数、下载人数、广告点击次数、下载点击次数、
        /// iphone下载次数、ipad下载次数、安卓下载次数、win phone下载次数、win nt下载次数、mac os下载次数、其他下载次数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP> GetVisitsByDate(Int64 oid, DateTime date)
        {
            List<StatisticalAP> list = new List<StatisticalAP>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = " select alias,SUM(ZONGRC) ZONGRC, sum(if(TYPE is null,0,1)) ZONGRS, sum(if(TYPE=1,1,0)) XIAZRS,SUM(GUANGGCS), SUM(XIAZCS)";
                strSql += " ,sum(iphone),sum(ipad),sum(android),sum(wp),sum(wnt),sum(mac), sum(other)";
                strSql += " from (";
                strSql += " SELECT alias, max(SADT_TYPE) TYPE, count(DISTINCT AcctSessionId) ZONGRC, sum(if(SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) XIAZCS, sum(if(SADT_TYPE=0,1,0)) GUANGGCS";
                strSql += " ,sum(if(INSTR(UserAgent, 'iPhone')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) iphone ,sum(if(INSTR(UserAgent, 'iPad')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) ipad ,sum(if((INSTR(UserAgent, 'Android')||INSTR(UserAgent, 'Adr '))&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) android ,sum(if(INSTR(UserAgent, 'Windows Phone')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) wp ,sum(if(INSTR(UserAgent, 'Windows NT')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) wnt ,sum(if(INSTR(UserAgent, 'Intel Mac OS')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) mac ";
                strSql += " ,sum(if(INSTR(UserAgent, 'iPhone')=0&&INSTR(UserAgent, 'iPad')=0&&INSTR(UserAgent, 'Android')=0&&INSTR(UserAgent, 'Adr ')=0&&INSTR(UserAgent, 'Windows Phone')=0&&INSTR(UserAgent, 'Windows NT')=0&&INSTR(UserAgent, 'Intel Mac OS')=0&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) other";
                strSql += " from(";
                strSql += " select a.ID, a.ALIAS,b.ID SSID, b.OID from luobo.sys_apdevice a, luobo.sys_ssid b ";
                strSql += " where a.ID = b.APID and a.DEVICESTATE != 5  and date_format(a.powerdatetime, '%Y%m%d') != date_format('2000-1-1', '%Y%m%d')";
                strSql += " and b.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " )t1 left join ";
                strSql += " (select CallingStationId, AcctSessionId, SSID, SADT_TYPE,UserAgent,PageUrl from luobo.ad_info a, luobo.sys_adtemplet b , statistical.openssid c where ";
                strSql += " a.AD_Model = b.SADT_ID and c.AdId =a.AD_ID ";
                strSql += " and c.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " ) t2 on t1.SSID=t2.SSID group by alias, CallingStationId) tmp group by alias";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd"))
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new StatisticalAP() { NAME = row[0].ToString(), NUM = new List<long>() { Convert.ToInt64(row[1])
                                                        ,Convert.ToInt64(row[2]), Convert.ToInt64(row[3]), Convert.ToInt64(row[4])
                                                        ,Convert.ToInt64(row[5]),Convert.ToInt64(row[6]),Convert.ToInt64(row[7])
                                                        ,Convert.ToInt64(row[8]),Convert.ToInt64(row[9]),Convert.ToInt64(row[10])
                                                        ,Convert.ToInt64(row[11]),Convert.ToInt64(row[12])}
                        });
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 机构下所有营业厅总人次、总人数、下载人数、广告点击次数、下载点击次数、
        /// iphone下载次数、ipad下载次数、安卓下载次数、win phone下载次数、win nt下载次数、mac os下载次数、其他下载次数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public StatisticalAP GetTotalInfoByDate(Int64 oid, DateTime date)
        {
            StatisticalAP data = new StatisticalAP();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select SUM(ZONGRC) ZONGRC, count(1) ZONGRS, sum(if(TYPE=1,1,0)) XIAZRS,SUM(GUANGGCS), SUM(XIAZCS)";
                strSql += " ,sum(iphone),sum(ipad),sum(android),sum(wp),sum(wnt),sum(mac), sum(other)";
                strSql += " from (";
                strSql += " select max(SADT_TYPE) TYPE, count(DISTINCT AcctSessionId) ZONGRC, sum(if(SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) XIAZCS, sum(if(SADT_TYPE=0,1,0)) GUANGGCS";
                strSql += " ,sum(if(INSTR(UserAgent, 'iPhone')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) iphone ,sum(if(INSTR(UserAgent, 'iPad')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) ipad ,sum(if((INSTR(UserAgent, 'Android')||INSTR(UserAgent, 'Adr '))&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) android ,sum(if(INSTR(UserAgent, 'Windows Phone')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) wp ,sum(if(INSTR(UserAgent, 'Windows NT')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) wnt ,sum(if(INSTR(UserAgent, 'Intel Mac OS')&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) mac ";
                strSql += " ,sum(if(INSTR(UserAgent, 'iPhone')=0&&INSTR(UserAgent, 'iPad')=0&&INSTR(UserAgent, 'Android')=0&&INSTR(UserAgent, 'Adr ')=0&&INSTR(UserAgent, 'Windows Phone')=0&&INSTR(UserAgent, 'Windows NT')=0&&INSTR(UserAgent, 'Intel Mac OS')=0&&SADT_TYPE=1 && INSTR(PageUrl,'portal.html') = 0,1,0)) other";
                strSql += " from luobo.ad_info a, luobo.sys_adtemplet b , statistical.openssid c where ";
                strSql += " a.AD_Model = b.SADT_ID and c.AdId =a.AD_ID ";
                strSql += " and OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " GROUP BY CallingStationId";
                strSql += ") t1";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd"))
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    data.NAME = "合计";
                    if (dt.Rows[0][0].ToString() == "")
                        data.NUM = new List<long> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    else
                        data.NUM = new List<long>() { Convert.ToInt64(dt.Rows[0][0])
                                ,Convert.ToInt64(dt.Rows[0][1]), Convert.ToInt64(dt.Rows[0][2]), Convert.ToInt64(dt.Rows[0][3])
                                ,Convert.ToInt64(dt.Rows[0][4]),Convert.ToInt64(dt.Rows[0][5]),Convert.ToInt64(dt.Rows[0][6])
                                ,Convert.ToInt64(dt.Rows[0][7]),Convert.ToInt64(dt.Rows[0][8]),Convert.ToInt64(dt.Rows[0][9])
                                ,Convert.ToInt64(dt.Rows[0][10]),Convert.ToInt64(dt.Rows[0][11])};
                }
            }
            return data;
        }

        /// <summary>
        /// 机构下不同不同营业厅认证后总访问时长和平均时长
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP<double>> GetApproveTimeByDate(Int64 oid, DateTime date)
        {
            List<StatisticalAP<double>> list = new List<StatisticalAP<double>>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select alias, IFNULL(SUM(ZONGSC)/60,0) ZONGSC,IFNULL(SUM(ZONGSC)/60/RENC,0) PINGJSC ";
                strSql += " from (select a.ID, a.ALIAS,b.ID SSID, b.OID from luobo.sys_apdevice a, luobo.sys_ssid b where a.ID = b.APID and a.DEVICESTATE != 5  and date_format(a.powerdatetime, '%Y%m%d') != date_format('2000-1-1', '%Y%m%d') ";
                strSql += " and b.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")) t1 ";
                strSql += " left join( select sum(acctsessiontime) ZONGSC,count(acctsessionid) RENC, SSID from (select a.acctsessiontime,a.acctsessionid ,b.SSID from radius.radacct a ,statistical.openssid b ";
                strSql += " WHERE a.acctsessionid=b.acctsessionid ";
                strSql += " and b.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " group by a.acctsessionid)c group by ssid) t2 on t1.SSID=t2.SSID group by alias";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd"))
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new StatisticalAP<double>() { NAME = row[0].ToString(), VALUE = new List<double>() { Convert.ToDouble(row[1]), Convert.ToDouble(row[2]) } });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 机构下不同不同营业厅广告点击数或下载点击数
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<StatisticalAP> GetADorDownByDate(Int64 oid, DateTime date)
        {
            List<StatisticalAP> list = new List<StatisticalAP>();
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "select ALIAS, SUM(IF(SADT_TYPE=0,1,0)) AD, SUM(IF(SADT_TYPE=1,1,0)) DOWN from (";
                strSql += " select a.ID, a.ALIAS,b.ID SSID, b.OID from luobo.sys_apdevice a, luobo.sys_ssid b ";
                strSql += " where a.ID = b.APID and a.DEVICESTATE != 5  and date_format(a.powerdatetime, '%Y%m%d') != date_format('2000-1-1', '%Y%m%d')";
                strSql += " and b.OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " ) t1 left join(select ID OSID, SSID,SADT_TYPE from luobo.ad_info a, luobo.sys_adtemplet b , statistical.openssid c where ";
                strSql += " a.AD_Model = b.SADT_ID and c.AdId =a.AD_ID ";
                strSql += " and OID in (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " and date_format(CurrentTime, '%Y%m%d') >= date_format(@StartDate, '%Y%m%d')";
                strSql += " and date_format(CurrentTime, '%Y%m%d') <= date_format(@EndDate, '%Y%m%d')";
                strSql += " )t2 on t1.SSID=t2.SSID  group by alias";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@StartDate",date.ToString("yyyy-MM")+"-1"),
                    new MySqlParameter("@EndDate", DateTime.Parse(date.AddMonths(1).ToString("yyyy-MM")+"-1").AddDays(-1).ToString("yyyy-MM-dd"))
                };
                DataTable dt = mySql.GetDataTable(strSql, "OpenSSID", parms);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new StatisticalAP() { NAME = row[0].ToString(), NUM = new List<long>() { Convert.ToInt64(row[1]), Convert.ToInt64(row[2]) } });
                    }
                }
            }
            return list;
        }


       
        #endregion
    }
}
