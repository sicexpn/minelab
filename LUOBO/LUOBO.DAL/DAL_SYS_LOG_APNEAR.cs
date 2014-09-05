using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;
using LUOBO.Model;

namespace LUOBO.DAL
{
    public class DAL_SYS_LOG_APNEAR
    {
        public List<M_Alert_Object> Select(long oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_Alert_Object> list = new List<M_Alert_Object>();
                string strSql = "select c.*,d.ALIAS as APNAME from  ";
                strSql += "(select a.*,b.ISPROCESS,b.ISWHITELIST,b.KEYWORD from sys_log_apnear a left join sys_log_alert b on a.OID=b.OID and a.G_MAC = b.G_MAC and a.AP_MAC=b.AP_MAC and a.G_TIME=b.G_TIME where a.OID = @OID ORDER BY a.G_TIME) ";
                strSql += "as c,sys_apdevice d where c.AP_MAC=d.MAC";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_Alert_Object", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<M_Alert_Object>.FillModel(dt);
                return list;
            }
        }
        public List<M_Alert_Graph> SelectGraph(long oid, bool isRealTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_Alert_Graph> list = new List<M_Alert_Graph>();
                string strSql = "select G_STRONG,G_SSID,CHANNEL from sys_log_apnear where oid=@OID";
                if (isRealTime)
                    strSql += " and G_TIME > adddate(now(), INTERVAL -60 SECOND)";
                strSql += " group by G_STRONG,G_SSID,CHANNEL";

                //string strSql = "select c.*,d.ALIAS as APNAME from  ";
                // strSql += "(select a.*,b.ISPROCESS,b.ISWHITELIST,b.KEYWORD from sys_log_apnear a left join sys_log_alert b on a.OID=b.OID and a.G_MAC = b.G_MAC and a.AP_MAC=b.AP_MAC and a.G_TIME=b.G_TIME where a.OID = @OID ORDER BY a.G_TIME) ";
                //strSql += "as c,sys_apdevice d where c.AP_MAC=d.MAC";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_Alert_Graph", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<M_Alert_Graph>.FillModel(dt);
                return list;
            }
        }
        public List<SYS_LOG_APNEAR> SelectByOID(long oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_LOG_APNEAR> list = new List<SYS_LOG_APNEAR>();
                string strSql = "select a.* from sys_log_apnear a where OID=@OID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "sys_log_apnear", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<SYS_LOG_APNEAR>.FillModel(dt);
                return list;
            }
        }
        public List<SYS_LOG_APNEAR> SelectByAPMac(string mac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_LOG_APNEAR> list = new List<SYS_LOG_APNEAR>();
                string strSql = "select a.* from sys_log_apnear a where AP_MAC=@AP_MAC";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@AP_MAC", mac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "sys_log_apnear", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<SYS_LOG_APNEAR>.FillModel(dt);
                return list;
            }
        }

        public void Save(string mac, List<SYS_LOG_APNEAR> apnears)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_LOG_APNEAR where AP_MAC='" + mac + "'", "SYS_LOG_APNEAR");
                DataRow[] rs;
                DataRow dr = null;
                for (int i = 0; i < apnears.Count; ++i)
                {
                    rs = dt.Select("G_MAC='" + apnears[i].G_MAC + "'");
                    if (rs.Length > 0)
                    {
                        rs[0]["G_TIME"] = apnears[i].G_TIME;
                        rs[0]["SCANCOUNT"] = (Int64)rs[0]["SCANCOUNT"] + 1;
                        rs[0]["G_SSID"] = apnears[i].G_SSID;
                        rs[0]["G_STRONG"] = apnears[i].G_STRONG;
                    }
                    else
                    {
                        apnears[i].FIRSTTIME = apnears[i].G_TIME;
                        apnears[i].SCANCOUNT = 1;
                        dr = dt.NewRow();
                        dt.Rows.Add(DataChange<Entity.SYS_LOG_APNEAR>.FillRow(apnears[i], dr));
                    }
                }
                //DataRow dr = null;
                //for (int i = 0; i < apnears.Count; ++i)
                //{
                //    dr = dt.NewRow();
                //    dt.Rows.Add(DataChange<Entity.SYS_LOG_APNEAR>.FillRow(apnears[i], dr));
                //}
                mySql.Update(dt);
            }
        }

        public List<M_SECURITY_SSID> GetSameSSIDList(long oid, string ssidName, Int64 apid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_SECURITY_SSID> list = new List<M_SECURITY_SSID>();
                string strSql = "select t.APID, t.ALIAS as CapturerName,t.G_SSID,t.G_MAC,t.G_STRONG,t.CHANNEL,t.G_TIME,c.KEYWORD,c.Similarity,t.FirstTime from ";
                strSql += "(select a.*, b.ALIAS, b.ID as APID from ";
                strSql += "(select *  FROM SYS_LOG_APNEAR  where OID=@OID and G_SSID=@SSID ";
                strSql += ") a ";
                strSql += "join sys_apdevice b on a.AP_MAC=b.MAC and b.id=@APID ";
                strSql += ") t left join sys_log_alert c on t.AP_MAC=c.AP_MAC and t.OID=c.OID and t.G_SSID=c.G_SSID and t.G_MAC=c.G_MAC ";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid),
                    new MySqlParameter("@APID", apid),
                    new MySqlParameter("@SSID",ssidName)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_SECURITY_SSID", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<M_SECURITY_SSID>.FillModel(dt);
                return list;
            }
        }

        public List<M_SECURITY_SSID> GetFilterSSIDList(long oid, int pageSize, int curPage, M_SECURITY_SSID_Filter filter)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_SECURITY_SSID> list = new List<M_SECURITY_SSID>();
                //string strSql = "Select t.* from (SELECT b.ALIAS AS CapturerName,a.G_SSID,a.G_MAC,a.G_STRONG,a.CHANNEL,a.G_TIME,c.KEYWORD,d.Similarity,a.GroupCount ";
                //strSql += "from (select *,count(1) as GroupCount FROM SYS_LOG_APNEAR where OID=@OID ";
                string strSql = "select t.ALIAS as CapturerName,t.ID AS APID,t.G_SSID,t.G_MAC,t.G_STRONG,t.CHANNEL,t.G_TIME,t.GroupCount,c.KEYWORD,c.Similarity,t.LevelFlag,t.FirstTime from ";
                strSql += "(select a.*,b.* from ";
                strSql += "(select *,count(1) as GroupCount,";
                strSql += "case when G_MAC in(select MAC from sys_log_alertwhitelist where oid=@OID) then 2 ";
                strSql += " when G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID) then 1 ";
                strSql += " when G_SSID REGEXP (select GROUP_CONCAT(RIVAL SEPARATOR '|') from sys_log_alertrival where oid=@OID) then 5 ";
                strSql += " when length(G_SSID)<>CHARACTER_LENGTH(G_SSID) then 3 ";
                strSql += " when date(firsttime)=date(now()) then 4 ";
                strSql += " else 0 end as LevelFlag ";
                strSql += " FROM SYS_LOG_APNEAR where 1=1 ";
                if (filter.isSubOrg)
                {
                    strSql += " and OID in (select ID from SYS_ORGANIZATION where PIDHELP like '%$@OID$%' or ID=@OID)";
                }
                else
                {
                    strSql += " and OID=@OID ";
                }

                if (filter.isRealTime)
                {
                    strSql += " and G_TIME > adddate(now(), INTERVAL -60 SECOND) ";
                }

                switch (filter.filtermod)
                {
                    case "1":
                        strSql += " and G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID) ";
                        strSql += " and G_MAC not in(select MAC from sys_log_alertwhitelist where oid=@OID)";

                        break;
                    case "2":
                        strSql += " and G_MAC in(select MAC from sys_log_alertwhitelist where oid=@OID)";
                        break;
                    case "3":
                        //strSql += " and not (HEX(t.G_SSID) REGEXP '[[:<:]](e[4-9][0-9a-f]{4}|3[0-9]|4[0-9A-F]|5[0-9A]|6[0-9A-F]|7[0-9A]|5F)+[[:>:]]')";
                        strSql += " and length(G_SSID)<>CHARACTER_LENGTH(G_SSID)";
                        break;
                    case "4":
                        strSql += " and date(firsttime)=date(now())";
                        break;
                    case "5":
                        strSql += " and G_SSID REGEXP (select GROUP_CONCAT(RIVAL SEPARATOR '|') from sys_log_alertrival where oid=@OID)";
                        break;
                    default:
                        break;
                }

                strSql += "group by G_SSID ";
                strSql += ") a left join (SYS_APDEVICE b) on a.AP_MAC=b.MAC ";
                strSql += ") t left join sys_log_alert  c on ";
                strSql += "t.AP_MAC=c.AP_MAC and t.OID=c.OID and t.G_SSID=c.G_SSID and t.G_MAC=c.G_MAC";

                strSql += " group by t.G_SSID ";
                if (filter.ordercul != null)
                {
                    strSql += " order by " + filter.ordercul + " " + filter.sortstr;
                }
                strSql += " LIMIT " + ((curPage - 1) * pageSize).ToString() + "," + pageSize.ToString();
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_SECURITY_SSID", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<M_SECURITY_SSID>.FillModel(dt);
                return list;
            }
        }

        public List<M_SECURITY_SSID> GetFilterSSIDListByAPID(long oid, int pageSize, int curPage, M_SECURITY_SSID_Filter filter)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_SECURITY_SSID> list = new List<M_SECURITY_SSID>();
                string strSql = "select t.ALIAS as CapturerName,t.ID as APID,t.G_SSID,t.G_MAC,t.G_STRONG,t.CHANNEL,t.G_TIME,t.GroupCount,c.KEYWORD,c.Similarity,t.LevelFlag,t.FirstTime from ";
                strSql += "(select a.*,b.* from ";
                strSql += "(select t1.*,count(1) as GroupCount, IFNULL(t2.LevelFlag, 0) LevelFlag";
                //strSql += "case when G_MAC in(select MAC from sys_log_alertwhitelist where oid=@OID) then 2 ";
                //strSql += " when G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID) then 1 ";
                //strSql += " when G_SSID REGEXP (select GROUP_CONCAT(RIVAL SEPARATOR '|') from sys_log_alertrival where oid=@OID) then 5 ";
                //strSql += " when length(G_SSID)<>CHARACTER_LENGTH(G_SSID) then 3 ";
                //strSql += " when date(firsttime)=date(now()) then 4 ";
                //strSql += " else 0 end as LevelFlag ";
                strSql += " FROM SYS_LOG_APNEAR t1 LEFT JOIN (";
                strSql += " SELECT LOG_ID,GROUP_CONCAT(LevelFlag) LevelFlag FROM";
                strSql += " (SELECT LOG_ID, 2 LevelFlag FROM SYS_LOG_APNEAR t1 INNER JOIN sys_log_alertwhitelist t2 ON t1.G_MAC = t2.MAC";
                strSql += " WHERE t1.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' or ID=" + oid + ")";
                if (filter.apid != 0)
                    strSql += " AND AP_MAC = (select MAC from sys_apdevice where ID=@APID) ";
                if (filter.isRealTime)
                    strSql += " AND G_TIME > adddate(now(), INTERVAL - 60 SECOND)";
                strSql += " UNION";
                strSql += " SELECT LOG_ID, 1 LevelFlag FROM SYS_LOG_APNEAR t1 INNER JOIN (SELECT GROUP_CONCAT(keyword SEPARATOR '|') KEYWORD FROM sys_log_alertkeyword WHERE OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$10049$%' OR ID = 10049)) t2";
                strSql += " ON t1.G_SSID REGEXP (t2.KEYWORD)";
                strSql += " WHERE t1.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' or ID=" + oid + ")";
                strSql += " AND t1.G_MAC NOT IN (SELECT MAC FROM sys_log_alertwhitelist WHERE OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' or ID=" + oid + "))";
                if (filter.apid != 0)
                    strSql += " AND AP_MAC = (select MAC from sys_apdevice where ID=@APID) ";
                if (filter.isRealTime)
                    strSql += " AND G_TIME > adddate(now(), INTERVAL - 60 SECOND)";
                strSql += " UNION";
                strSql += " SELECT LOG_ID, 5 LevelFlag FROM SYS_LOG_APNEAR t1 INNER JOIN (SELECT GROUP_CONCAT(RIVAL SEPARATOR '|') RIVAL FROM sys_log_alertrival WHERE OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$10049$%' OR ID = 10049)) t2";
                strSql += " ON t1.G_SSID REGEXP (RIVAL)";
                strSql += " WHERE t1.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' or ID=" + oid + ")";
                if (filter.apid != 0)
                    strSql += " AND AP_MAC = (select MAC from sys_apdevice where ID=@APID) ";
                if (filter.isRealTime)
                    strSql += " AND G_TIME > adddate(now(), INTERVAL - 60 SECOND)";
                strSql += " UNION";
                strSql += " SELECT LOG_ID, 3 LevelFlag FROM SYS_LOG_APNEAR";
                strSql += " WHERE OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' or ID=" + oid + ")";
                strSql += " AND LENGTH(G_SSID) <> CHARACTER_LENGTH(G_SSID)";
                if (filter.apid != 0)
                    strSql += " AND AP_MAC = (select MAC from sys_apdevice where ID=@APID) ";
                if (filter.isRealTime)
                    strSql += " AND G_TIME > adddate(now(), INTERVAL - 60 SECOND)";
                strSql += " UNION";
                strSql += " SELECT LOG_ID, 4 LevelFlag FROM SYS_LOG_APNEAR";
                strSql += " WHERE OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' or ID=" + oid + ")";
                strSql += " AND DATE(FIRSTTIME) = DATE(NOW())";
                if (filter.apid != 0)
                    strSql += " AND AP_MAC = (select MAC from sys_apdevice where ID=@APID) ";
                if (filter.isRealTime)
                    strSql += " AND G_TIME > adddate(now(), INTERVAL - 60 SECOND)";
                strSql += " ) t GROUP BY LOG_ID) t2 ON t1.LOG_ID = t2.LOG_ID";
                strSql += " where OID in (select ID from SYS_ORGANIZATION where PIDHELP like '%$" + oid + "$%' or ID=" + oid + ")";
                if (filter.apid != 0)
                    strSql += " and AP_MAC = (select MAC from sys_apdevice where ID=@APID) ";
                if (filter.isRealTime)
                    strSql += " and G_TIME > adddate(now(), INTERVAL -60 SECOND) ";
                strSql += " group by G_SSID,AP_MAC";
                strSql += " ) a left join (SYS_APDEVICE b) on a.AP_MAC=b.MAC ";
                strSql += " ) t left join sys_log_alert  c on ";
                strSql += " t.AP_MAC=c.AP_MAC and t.OID=c.OID and t.G_SSID=c.G_SSID and t.G_MAC=c.G_MAC";
                
                if (filter.ordercul != null)
                {
                    strSql += " order by " + filter.ordercul + " " + filter.sortstr;
                }
                //strSql += " LIMIT " + ((curPage - 1) * pageSize).ToString() + "," + pageSize.ToString();
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid),
                    new MySqlParameter("@APID", filter.apid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_SECURITY_SSID", parms);
                if (dt.Rows.Count > 0)
                    list = DataChange<M_SECURITY_SSID>.FillModel(dt);
                return list;
            }
        }

        public Int32 GetFilterSSIDListCounts(long oid, M_SECURITY_SSID_Filter filter)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_SECURITY_SSID> list = new List<M_SECURITY_SSID>();
                //string strSql1 = "SELECT b.ALIAS AS CapturerName,a.AP_MAC,a.G_SSID,a.G_MAC,a.G_STRONG,a.CHANNEL,a.G_TIME FROM SYS_LOG_APNEAR a ,SYS_APDEVICE b WHERE a.OID=@OID AND a.G_SSID=@SSID AND b.MAC=a.AP_MAC ";

                //string strSql = "Select count(*) from (SELECT b.ALIAS AS CapturerName,a.G_SSID,a.G_MAC,a.G_STRONG,a.CHANNEL,a.G_TIME,c.KEYWORD ";
                ////strSql += "FROM SYS_LOG_APNEAR a left join (SYS_APDEVICE b ,SYS_LOG_ALERTKEYWORD c,sys_log_alert d) ON  c.keyword like  concat('%',a.G_SSID,'%') and c.OID=a.OID and a.OID=@OID AND b.MAC=a.AP_MAC) t where 1=1";
                //strSql += " From (SYS_LOG_APNEAR a ,SYS_APDEVICE b ) left join (SYS_LOG_ALERTKEYWORD c,sys_log_alert d) ON  c.keyword like  concat('%',a.G_SSID,'%') and c.OID=a.OID and a.OID=@OID AND b.MAC=a.AP_MAC and a.OID=d.OID and a.AP_MAC=d.AP_MAC and a.G_SSID=d.G_SSID) t where 1=1";
                string strSql = "select count(1) from (select t.ALIAS as CapturerName,t.G_SSID,t.G_MAC,t.G_STRONG,t.CHANNEL,t.G_TIME,t.GroupCount,c.KEYWORD,c.Similarity from ";
                strSql += "(select a.*,b.* from ";
                strSql += "(select *,count(1) as GroupCount FROM SYS_LOG_APNEAR where 1=1  ";
                if (filter.isSubOrg)
                {
                    strSql += " and OID in (select ID from SYS_ORGANIZATION where PIDHELP like '%$@OID$%' or ID=@OID) ";
                }
                else
                {
                    strSql += " and OID=@OID ";
                }

                if (filter.isRealTime)
                {
                    strSql += " and G_TIME > adddate(now(), INTERVAL -60 SECOND) ";
                }

                switch (filter.filtermod)
                {
                    case "1":
                        strSql += " and G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID)";
                        strSql += " and G_MAC not in(select MAC from sys_log_alertwhitelist where oid=@OID)";
                        break;
                    case "2":
                        strSql += " and G_MAC in(select MAC from sys_log_alertwhitelist where oid=@OID)";
                        break;
                    case "3":
                        //strSql += " and not (HEX(t.G_SSID) REGEXP '[[:<:]](e[4-9][0-9a-f]{4}|3[0-9]|4[0-9A-F]|5[0-9A]|6[0-9A-F]|7[0-9A]|5F)+[[:>:]]')";
                        strSql += " and length(G_SSID)<>CHARACTER_LENGTH(G_SSID)";
                        break;
                    case "4":
                        strSql += " and date(firsttime)=date(now())";
                        break;
                    case "5":
                        strSql += " and G_SSID REGEXP (select GROUP_CONCAT(RIVAL SEPARATOR '|') from sys_log_alertrival where oid=@OID)";
                        break;
                    default:
                        break;
                }

                strSql += "group by G_SSID ";
                strSql += ") a left join (SYS_APDEVICE b) on a.AP_MAC=b.MAC ";
                strSql += ") t left join sys_log_alert  c on ";
                strSql += "t.AP_MAC=c.AP_MAC and t.OID=c.OID and t.G_SSID=c.G_SSID and t.G_MAC=c.G_MAC";

                strSql += " group by t.G_SSID ) tt";
                //strSql += " LIMIT " + ((curPage - 1) * pageSize).ToString() + "," + pageSize.ToString();
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid)
                };
                return Convert.ToInt32(mySql.GetOnlyOneValue(strSql, parms));
            }
        }


        /// <summary>
        /// 获取告警列表统计
        /// </summary>
        /// <param name="OID">机构ID</param>
        /// <returns>元素0:可疑,元素1:中文,元素:2新增</returns>
        public List<Int64> GetAPNearCountByOID(Int64 OID, M_SECURITY_SSID_Filter filter)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<Int64> list = new List<Int64>();
                /*string strSql = "SELECT SUM(IFNULL(WarningCount,0)) WarningCount,SUM(IFNULL(ChineseCount,0)) ChineseCount,SUM(IFNULL(AddCount,0)) AddCount,SUM(IFNULL(WhiteCount, 0)) WhiteCount FROM";
                strSql += " (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR ID=" + OID + ") t1 LEFT JOIN";
                strSql += " (SELECT OID,COUNT(0) WarningCount FROM sys_log_alert";
                strSql += " WHERE ISPROCESS=0 AND OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR ID=" + OID + ")";
                strSql += " GROUP BY OID) t2 ON t1.ID = t2.OID LEFT JOIN";
                strSql += " (SELECT OID,COUNT(0) ChineseCount FROM sys_log_apnear";
                strSql += " WHERE length(G_SSID)<> CHARACTER_LENGTH(G_SSID)";
                strSql += " AND OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR ID=" + OID + ")";
                strSql += " GROUP BY OID) t3 ON t1.ID = t3.OID LEFT JOIN";
                strSql += " (SELECT OID,COUNT(0) AddCount FROM sys_log_apnear";
                strSql += " WHERE DATE(FIRSTTIME) = DATE(NOW())";
                strSql += " AND OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR ID=" + OID + ")";
                strSql += " GROUP BY OID) t4 ON t1.ID = t4.OID left join";
                strSql += " (SELECT OID,COUNT(0) WhiteCount FROM sys_log_apnear";
                strSql += " WHERE OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR ID = " + OID + ")";
                strSql += " and G_MAC in (select MAC from sys_log_alertwhitelist where OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' OR ID = " + OID + "))";
                strSql += " GROUP BY OID ) t5 ON t1.ID = t5.OID";
                */
                String isCurrent = " ";
                if (filter.isRealTime)
                {
                    isCurrent = " and G_TIME > adddate(now(), INTERVAL -60 SECOND) ";
                }

                String isSuborg = " OID=@OID ";
                if (filter.isSubOrg)
                {
                    isSuborg = " and OID in (select ID from SYS_ORGANIZATION where PIDHELP like '%$@OID$%' or ID=@OID) ";
                }

                isSuborg = " OID=@OID ";
                String strSql = "select t1.AllCount,t2.WarningCount,t3.BelievedCount,t4.ChineseCount,t5.TodayCount,t6.RivalCount from ";

                strSql += "(select count(1) as AllCount FROM SYS_LOG_APNEAR where " + isSuborg + isCurrent + ")t1,";

                strSql += "(select count(1) as WarningCount FROM SYS_LOG_APNEAR where  " + isSuborg + isCurrent;
                strSql += " and G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID)";
                strSql += " and G_MAC not in(select MAC from sys_log_alertwhitelist where " + isSuborg + ")) t2,";

                strSql += "(select count(1) as BelievedCount FROM SYS_LOG_APNEAR where " + isSuborg + isCurrent;
                strSql += " and G_MAC in(select MAC from sys_log_alertwhitelist where " + isSuborg + "))t3,";

                strSql += "(select count(1) as ChineseCount FROM SYS_LOG_APNEAR where " + isSuborg + isCurrent;
                strSql += " and length(G_SSID)<>CHARACTER_LENGTH(G_SSID)) t4,";

                strSql += "(select count(1) as TodayCount FROM SYS_LOG_APNEAR where  " + isSuborg + isCurrent;
                strSql += " and date(firsttime)=date(now()) )t5,";

                strSql += "(select count(1) as RivalCount FROM SYS_LOG_APNEAR where  " + isSuborg + isCurrent;
                strSql += " and G_SSID REGEXP (select GROUP_CONCAT(rival SEPARATOR '|') from sys_log_alertrival where " + isSuborg + ")";
                strSql += " ) t6";

                //strSql += "(select count(1) as RivalCount from sys_log_alertrival where " + isSuborg + ")t6 ";


                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@OID",OID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "M_SECURITY_SSID", parms);
                if (dt.Rows.Count > 0)
                {
                    list.Add(Convert.ToInt64(dt.Rows[0]["AllCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["WarningCount"]));
                    list.Add(Convert.ToInt16(dt.Rows[0]["BelievedCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["ChineseCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["TodayCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["RivalCount"]));
                }
                return list;
            }
        }

        /// <summary>
        /// 获取告警列表统计
        /// </summary>
        /// <param name="OID">机构ID</param>
        /// <returns>元素0:可疑,元素1:中文,元素:2新增</returns>
        public List<Int64> GetAPNearCountByAPID(Int64 OID, M_SECURITY_SSID_Filter filter)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<Int64> list = new List<Int64>();
                String isCurrent = " ";
                if (filter.isRealTime)
                    isCurrent = " and G_TIME > adddate(now(), INTERVAL -60 SECOND) ";

                String isSuborg = " OID=@OID ";
                if (filter.isSubOrg)
                    isSuborg = " and OID in (select ID from SYS_ORGANIZATION where PIDHELP like '%$@OID$%' or ID=@OID) ";
                else
                    isSuborg = " OID=@OID ";
                string sqlAPID = "";
                string quanbu_keyi = "";
                
                if (filter.apid != 0)
                    sqlAPID = " and AP_MAC = (select MAC from sys_apdevice where ID=" + filter.apid + ") ";
                else
                    ;// quanbu_keyi = "and G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID) and G_MAC not in(select MAC from sys_log_alertwhitelist where " + isSuborg + ")";

                String strSql = "select t1.AllCount,t2.WarningCount,t3.BelievedCount,t4.ChineseCount,t5.TodayCount,t6.RivalCount from ";
                strSql += "(select count(1) as AllCount FROM SYS_LOG_APNEAR where " + isSuborg + isCurrent + sqlAPID + quanbu_keyi + ")t1,";

                strSql += "(select count(1) as WarningCount FROM SYS_LOG_APNEAR where  " + isSuborg + isCurrent + sqlAPID;
                strSql += " and G_SSID REGEXP (select GROUP_CONCAT(keyword SEPARATOR '|') from sys_log_alertkeyword where oid=@OID)";
                strSql += " and G_MAC not in(select MAC from sys_log_alertwhitelist where " + isSuborg + ")) t2,";

                strSql += "(select count(1) as BelievedCount FROM SYS_LOG_APNEAR where " + isSuborg + isCurrent + sqlAPID;
                strSql += " and G_MAC in(select MAC from sys_log_alertwhitelist where " + isSuborg + "))t3,";

                strSql += "(select count(1) as ChineseCount FROM SYS_LOG_APNEAR where " + isSuborg + isCurrent + sqlAPID;
                strSql += " and length(G_SSID)<>CHARACTER_LENGTH(G_SSID)) t4,";

                strSql += "(select count(1) as TodayCount FROM SYS_LOG_APNEAR where  " + isSuborg + isCurrent + sqlAPID;
                strSql += " and date(firsttime)=date(now()) )t5,";

                strSql += "(select count(1) as RivalCount FROM SYS_LOG_APNEAR where  " + isSuborg + isCurrent + sqlAPID;
                strSql += " and G_SSID REGEXP (select GROUP_CONCAT(rival SEPARATOR '|') from sys_log_alertrival where " + isSuborg + ")";
                strSql += " ) t6";

                //strSql += "(select count(1) as RivalCount from sys_log_alertrival where " + isSuborg + ")t6 ";


                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@OID",OID)
                };

                DataTable dt = mySql.GetDataTable(strSql, "M_SECURITY_SSID", parms);
                if (dt.Rows.Count > 0)
                {
                    list.Add(Convert.ToInt64(dt.Rows[0]["AllCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["WarningCount"]));
                    list.Add(Convert.ToInt16(dt.Rows[0]["BelievedCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["ChineseCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["TodayCount"]));
                    list.Add(Convert.ToInt64(dt.Rows[0]["RivalCount"]));
                }
                return list;
            }
        }

        public List<StatisticalAP> GetAPNearStatistical(Int64 oid, DateTime date)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<StatisticalAP> list = new List<StatisticalAP>();
                StatisticalAP sap;
                string strSql = "SELECT ALIAS,IFNULL(`KEY`,0) `KEY`,IFNULL(XINR,0) XINR,IFNULL(ZHONGW,0) ZHONGW,IFNULL(TONGY,0) TONGY,IFNULL(XINZ,0) XINZ FROM";
                strSql += "(SELECT ID,ALIAS,MAC FROM sys_apdevice WHERE ID IN (SELECT APID FROM sys_aporg WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + "))";
                strSql += " AND POWERDATETIME <> '2000-01-01 00:00:00' AND DEVICESTATE != 5) t LEFT JOIN";
                strSql += "(SELECT AP_MAC,COUNT(0) `KEY` FROM `sys_log_alert` WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " AND DATE_FORMAT(G_TIME,'%y%m') >= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND DATE_FORMAT(FIRSTTIME,'%y%m') <= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND G_MAC NOT IN (SELECT MAC FROM sys_log_alertwhitelist WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + "))";
                strSql += " GROUP BY AP_MAC) a ON t.MAC = a.AP_MAC LEFT JOIN";
                strSql += "(SELECT AP_MAC,COUNT(0) `XINR` FROM `sys_log_alert` WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " AND DATE_FORMAT(G_TIME,'%y%m') >= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND DATE_FORMAT(FIRSTTIME,'%y%m') <= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND G_MAC IN (SELECT MAC FROM sys_log_alertwhitelist WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + "))";
                strSql += " GROUP BY AP_MAC) b ON t.MAC = b.AP_MAC LEFT JOIN";
                strSql += "(SELECT AP_MAC,COUNT(0) `ZHONGW` FROM `sys_log_apnear` WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " AND DATE_FORMAT(G_TIME,'%y%m') >= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND DATE_FORMAT(FIRSTTIME,'%y%m') <= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND LENGTH(G_SSID)<>CHARACTER_LENGTH(G_SSID)";
                strSql += " GROUP BY AP_MAC) c ON t.MAC = c.AP_MAC LEFT JOIN";
                strSql += "(SELECT AP_MAC,COUNT(0) `TONGY` FROM `sys_log_apnear` WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " AND DATE_FORMAT(G_TIME,'%y%m') >= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND DATE_FORMAT(FIRSTTIME,'%y%m') <= DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " AND G_SSID REGEXP (SELECT GROUP_CONCAT(RIVAL SEPARATOR '|') FROM sys_log_alertrival where oid=10049)";
                strSql += " GROUP BY AP_MAC) d ON t.MAC = d.AP_MAC LEFT JOIN";
                strSql += "(SELECT AP_MAC,COUNT(0) `XINZ` FROM `sys_log_apnear` WHERE OID IN (SELECT id FROM luobo.SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + oid + "$%' OR id = " + oid + ")";
                strSql += " AND DATE_FORMAT(FIRSTTIME,'%y%m') = DATE_FORMAT(@DateTime,'%y%m')";
                strSql += " GROUP BY AP_MAC) e ON t.MAC = e.AP_MAC";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@DateTime",date.ToString("yyyy-MM")+"-1")
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_SECURITY_SSID", parms);
                foreach (DataRow row in dt.Rows)
                {
                    sap = new StatisticalAP();
                    sap.NAME = row[0].ToString();
                    sap.NUM = new List<Int64>();
                    sap.NUM.Add(Convert.ToInt64(row[1]));
                    sap.NUM.Add(Convert.ToInt64(row[2]));
                    sap.NUM.Add(Convert.ToInt64(row[3]));
                    sap.NUM.Add(Convert.ToInt64(row[4]));
                    sap.NUM.Add(Convert.ToInt64(row[5]));
                    list.Add(sap);
                }

                return list;
            }
        }
    }
}
