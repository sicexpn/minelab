using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using System.Data;
using MySql.Data.MySqlClient;
using LUOBO.Model;

namespace LUOBO.DAL
{
    public class DAL_SYS_APDEVICE
    {
        public Int64 Insert(SYS_APDEVICE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_APDEVICE(MAC,ALIAS,MODEL,MANUFACTURER,PURCHASER,SERIAL,DESCRIPTION,MAXSSIDNUM,FIRMWAREVERSION,DEVICESTATE, SUPPORT3G, REGDATE, APCTID,HBINTERVAL,DATAINTERVAL,ISUPDATE,ISREBOOT,LON,LAT,LASTHB,ADDRESS,APCHANNEL,POWER,AERIALTYPE,ISSSIDON) VALUES (@MAC,@ALIAS, @MODEL, @MANUFACTURER, @PURCHASER, @SERIAL, @DESCRIPTION, @MAXSSIDNUM, @FIRMWAREVERSION, @DEVICESTATE, @SUPPORT3G, @REGDATE, @APCTID,@HBINTERVAL,@DATAINTERVAL,@ISUPDATE,@ISREBOOT,@LON,@LAT,@LASTHB,@ADDRESS,@CHANNEL,@POWER,@AERIALTYPE,@ISSSIDON)";
                strSql += ";SELECT LAST_INSERT_ID()";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@MAC", data.MAC),
                    new MySqlParameter("@ALIAS", data.ALIAS),
                    new MySqlParameter("@MODEL", data.MODEL),
                    new MySqlParameter("@MANUFACTURER", data.MANUFACTURER),
                    new MySqlParameter("@PURCHASER", data.PURCHASER),
                    new MySqlParameter("@SERIAL", data.SERIAL),
                    new MySqlParameter("@DESCRIPTION", data.DESCRIPTION),
                    new MySqlParameter("@MAXSSIDNUM", data.MAXSSIDNUM),
                    new MySqlParameter("@FIRMWAREVERSION", data.FIRMWAREVERSION),
                    new MySqlParameter("@DEVICESTATE", data.DEVICESTATE),
                    new MySqlParameter("@SUPPORT3G", data.SUPPORT3G),
                    new MySqlParameter("@REGDATE", data.REGDATE),
                    new MySqlParameter("@APCTID", data.APCTID),
                    new MySqlParameter("@HBINTERVAL", data.HBINTERVAL),
                    new MySqlParameter("@DATAINTERVAL", data.DATAINTERVAL),
                    new MySqlParameter("@ISUPDATE", data.ISUPDATE),
                    new MySqlParameter("@ISREBOOT", data.ISREBOOT),
                    new MySqlParameter("@LON", data.LON),
                    new MySqlParameter("@LAT", data.LAT),
                    new MySqlParameter("@LASTHB", data.LASTHB),
                    new MySqlParameter("@ADDRESS", data.ADDRESS),
                    new MySqlParameter("@CHANNEL", data.APCHANNEL),
                    new MySqlParameter("@POWER", data.POWER),
                    new MySqlParameter("@AERIALTYPE", data.AERIALTYPE),
                    new MySqlParameter("@ISSSIDON", data.ISSSIDON)
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public bool Update(Int64 ID, string MAC, Int64? SERIAL, string MODEL, string MANUFACTURER, string PURCHASER, string FIRMWAREVERSION, Int64? MAXSSIDNUM, Int32? DEVICESTATE, bool? SUPPORT3G, Int64? APCTID, string DESCRIPTION, DateTime? REGDATE, Int64? HBINTERVAL, Int64? DATAINTERVAL, bool? ISUPDATE, bool? ISREBOOT, double? lon, double? lat, DateTime? lasthb, string address, Int32? CHANNEL, Int32? POWER, Int32? AERIALTYPE, bool? ISSSIDON)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APDEVICE SET ID=ID";
                if (MAC != null)
                    strSql += ",MAC='" + MAC + "'";
                if (SERIAL != null)
                    strSql += ",SERIAL=" + SERIAL;
                if (MODEL != null)
                    strSql += ",MODEL='" + MODEL + "'";
                if (MANUFACTURER != null)
                    strSql += ",MANUFACTURER='" + MANUFACTURER + "'";
                if (PURCHASER != null)
                    strSql += ",PURCHASER='" + PURCHASER + "'";
                if (FIRMWAREVERSION != null)
                    strSql += ",FIRMWAREVERSION='" + FIRMWAREVERSION + "'";
                if (MAXSSIDNUM != null)
                    strSql += ",MAXSSIDNUM=" + MAXSSIDNUM;
                if (DEVICESTATE != null)
                    strSql += ",DEVICESTATE=" + DEVICESTATE;
                if (SUPPORT3G != null)
                    strSql += ",SUPPORT3G=" + SUPPORT3G;
                if (APCTID != null)
                    strSql += ",APCTID=" + APCTID;
                if (DESCRIPTION != null)
                    strSql += ",DESCRIPTION='" + DESCRIPTION + "'";
                if (REGDATE != null)
                    strSql += ",REGDATE=" + REGDATE;
                if (HBINTERVAL != null)
                    strSql += ",HBINTERVAL=" + HBINTERVAL;
                if (DATAINTERVAL != null)
                    strSql += ",DATAINTERVAL=" + DATAINTERVAL;
                if (ISUPDATE != null)
                    strSql += ",ISUPDATE=" + ISUPDATE;
                if (ISREBOOT != null)
                    strSql += ",ISREBOOT=" + ISREBOOT;
                if (lon != null)
                    strSql += ",LON=" + lon;
                if (lat != null)
                    strSql += ",LAT=" + lat;
                if (lasthb != null)
                    strSql += ",LASTHB=" + lasthb;
                if (!string.IsNullOrEmpty(address))
                    strSql += ",address=" + address;
                if (CHANNEL != null)
                    strSql += ",APCHANNEL=" + CHANNEL;
                if (POWER != null)
                    strSql += ",POWER=" + POWER;
                if (AERIALTYPE != null)
                    strSql += ",AERIALTYPE=" + AERIALTYPE;
                if (ISSSIDON != null)
                    strSql += ",ISSSIDON=" + ISSSIDON;
                strSql += " WHERE ID=" + ID;
                return mySql.ExecuteSQL(strSql);
            }
        }

        public bool Update(SYS_APDEVICE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_APDEVICE where 1<>1", "SYS_APDEVICE");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_APDEVICE>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_APDEVICE where ID=" + data.ID.ToString(), "SYS_APDEVICE");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SYS_APDEVICE>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APDEVICE WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Deletes(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APDEVICE WHERE ID in (" + ids + ")";
                //MySqlParameter[] parms = new MySqlParameter[] {
                //    new MySqlParameter("@ID", ids)
                //};
                return mySql.ExecuteSQL(strSql);
            }
        }

        public bool Disables(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APDEVICE SET STATE = 5 WHERE ID in (" + ids + ")";
                //MySqlParameter[] parms = new MySqlParameter[] {
                //    new MySqlParameter("@ID", ids)
                //};
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_APDEVICE> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
                string strSql = "SELECT * FROM SYS_APDEVICE";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE");
                list = DataChange<SYS_APDEVICE>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_APDEVICE> SelectByIDs(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
                string strSql = "SELECT * FROM SYS_APDEVICE WHERE ID in(" + ids + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE");
                list = DataChange<SYS_APDEVICE>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_APDEVICE> SelectAllApMac()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
                string strSql = "SELECT DISTINCT MAC FROM SYS_APDEVICE";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE");
                list = DataChange<SYS_APDEVICE>.FillModel(dt);
                return list;
            }
        }
        public Int64 GetOnLineUserNumsByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT IFNULL(SUM(ONLINEPEOPLENUM),0) FROM SYS_APDEVICE WHERE MAC=@MAC LIMIT 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@MAC",apMac)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }
        public Int64 GetOnLineUserNums()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT IFNULL(SUM(ONLINEPEOPLENUM),0) FROM SYS_APDEVICE ";

                return Int64.Parse(mySql.GetOnlyOneValue(strSql).ToString());
            }
        }
        public Int64 GetOnLineUserNumsByOID(Int64 orgID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT IFNULL(SUM(a.ONLINEPEOPLENUM),0) FROM SYS_APDEVICE AS a,SYS_APORG AS b WHERE a.ID = b.APID AND b.OID = @OID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@OID",orgID)
                };
                return Int64.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
            }
        }

        public Int32 GetSSIDNUMByMAC(string mac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT t2.SSIDNUM FROM sys_apdevice t1 INNER JOIN sys_aporg t2 ON t1.ID = t2.APID WHERE t2.ISCHILD = TRUE AND t1.MAC = @MAC";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@MAC",mac)
                };
                return Convert.ToInt32(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public DateTime? SelectPowerTimeByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT POWERDATETIME FROM SYS_APDEVICE WHERE MAC=@MAC LIMIT 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@MAC",apMac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", parms);
                SYS_APDEVICE data = null;
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_APDEVICE>.FillEntity(dt.Rows[0]);
                    return data.POWERDATETIME;
                }
                return null;
            }

        }

        public SYS_APDEVICE Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APDEVICE data = null;
                string strSql = "SELECT * FROM SYS_APDEVICE WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_APDEVICE>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        public SYS_APDEVICE SelectBySerial(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APDEVICE data = null;
                string strSql = "SELECT * FROM SYS_APDEVICE WHERE SERIAL = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_APDEVICE>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public SYS_APDEVICE SelectByMAC(string mac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APDEVICE data = null;
                string strSql = "SELECT * FROM SYS_APDEVICE WHERE MAC = @mac";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@mac", mac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_APDEVICE>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public List<SYS_AP_VIEW> SelectForManage(Int64 jgID, Int64 benJGID, Int64? startSerial, Int64? endSerial, string mac, string startDate, string endDate, int? FPState, int size, int curPage)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> datas = new List<SYS_AP_VIEW>();
                string strSql = "SELECT t1.*,t2.OID,t2.POID,t2.ISCHILD,t2.SDATE,t2.EDATE FROM SYS_APDEVICE t1,SYS_APORG t2";
                strSql += " WHERE t1.ID=t2.APID AND t2.ISCHILD=1 AND t1.ID NOT IN (SELECT ID FROM ({0}) t)";
                if (startSerial != null && startSerial > 0)
                    strSql += " AND t1.SERIAL >= " + startSerial;
                if (endSerial != null && endSerial > 0)
                    strSql += " AND t1.SERIAL <= " + endSerial;
                if (!string.IsNullOrEmpty(mac))
                    strSql += " AND t1.MAC like '" + mac + "%'";
                if (!string.IsNullOrEmpty(startDate))
                    strSql += " AND t1.REGDATE >= '" + startDate + "'";
                if (!string.IsNullOrEmpty(endDate))
                    strSql += " AND t1.REGDATE <= '" + endDate + "'";
                if (jgID != -99)
                    strSql += " AND (t2.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + jgID + "$%') OR t2.OID = " + jgID + ")";
                strSql += " ORDER BY t1.ID ASC LIMIT " + size;

                string strChildSql = "SELECT t1.ID FROM SYS_APDEVICE t1,SYS_APORG t2 WHERE t1.ID=t2.APID AND t2.ISCHILD=1";
                if (startSerial != null && startSerial > 0)
                    strChildSql += " AND t1.SERIAL >= " + startSerial;
                if (endSerial != null && endSerial > 0)
                    strChildSql += " AND t1.SERIAL <= " + endSerial;
                if (!string.IsNullOrEmpty(mac))
                    strChildSql += " AND t1.MAC like '" + mac + "%'";
                if (!string.IsNullOrEmpty(startDate))
                    strChildSql += " AND t1.REGDATE >= '" + startDate + "'";
                if (!string.IsNullOrEmpty(endDate))
                    strChildSql += " AND t1.REGDATE <= '" + endDate + "'";
                if (jgID != -99)
                    strChildSql += " AND (t2.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + jgID + "$%') OR t2.OID = " + jgID + ")";
                strChildSql += " ORDER BY t1.ID ASC LIMIT " + ((curPage - 1) * size);

                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "SYS_APDEVICE");
                datas = DataChange<SYS_AP_VIEW>.FillModel(dt);
                return datas;
            }
        }

        public int SelectCount(Int64 jgID, Int64 benJGID, Int64? startSerial, Int64? endSerial, string mac, string startDate, string endDate, int? FPState)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(1) FROM SYS_APDEVICE t1,SYS_APORG t2 WHERE t1.ID=t2.APID AND t2.ISCHILD=1";
                if (startSerial != null && startSerial > 0)
                    strSql += " AND t1.SERIAL >= " + startSerial;
                if (endSerial != null && endSerial > 0)
                    strSql += " AND t1.SERIAL <= " + endSerial;
                if (!string.IsNullOrEmpty(mac))
                    strSql += " AND t1.MAC like '" + mac + "%'";
                if (!string.IsNullOrEmpty(startDate))
                    strSql += " AND t1.REGDATE >= '" + startDate + "'";
                if (!string.IsNullOrEmpty(endDate))
                    strSql += " AND t1.REGDATE <= '" + endDate + "'";
                if (jgID != -99)
                    strSql += " AND (t2.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + jgID + "$%') OR t2.OID = " + jgID + ")";
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }

        public List<SYS_AP_VIEW> SelectAllAPByOID(Int64 OID, bool isInvalid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                string strSql = "SELECT t1.*,t2.ID APORGID,t2.POID,t2.OID,t2.SSIDNUM,t2.SDATE,t2.EDATE,t2.ISCHILD FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE (t2.OID = @OID OR t2.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%')) AND t1.DEVICESTATE != 5 AND t2.ISCHILD = 1";
                if (isInvalid)
                    strSql += " AND t2.EDATE < sysdate()";
                else
                    strSql += " AND t2.EDATE >= sysdate()";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                list = DataChange<SYS_AP_VIEW>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_AP_VIEW> SelectAPByOID(Int64 OID, bool isInvalid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                string strSql = "SELECT t1.*,t2.ID APORGID,t2.POID,t2.OID,t2.SSIDNUM,t2.SDATE,t2.EDATE,t2.ISCHILD FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE t2.OID = @OID AND t1.DEVICESTATE != 5 AND t2.ISCHILD = 1";
                if (isInvalid)
                    strSql += " AND t2.EDATE < sysdate()";
                else
                    strSql += " AND t2.EDATE >= sysdate()";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                list = DataChange<SYS_AP_VIEW>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_AP_VIEW> SelectInvalidAPByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                string strSql = "SELECT t1.*,t2.ID APORGID,t2.POID,t2.OID,t2.SSIDNUM,t2.SDATE,t2.EDATE,t2.ISCHILD,t3.NAME FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID INNER JOIN SYS_ORGANIZATION t3 ON t2.OID = t3.ID WHERE t2.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE ID = @OID OR PIDHELP LIKE CONCAT('%$', @OID, '$%')) AND t1.DEVICESTATE != 5 AND t2.ISCHILD = 1 AND t2.EDATE < sysdate()";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                list = DataChange<SYS_AP_VIEW>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_APDEVICE> SelectAPByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
                string strSql = "SELECT t1.* FROM SYS_APDEVICE t1, SYS_APORG t2 where t1.ID = t2.APID and t2.OID = @OID AND t1.DEVICESTATE !=5 AND t2.ISCHILD = 1";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                list = DataChange<SYS_APDEVICE>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_AP_VIEW> SelectAPViewByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                string strSql = "SELECT t1.*,t2.ID APORGID,t2.POID,t2.OID,t2.SSIDNUM,t2.SDATE,t2.EDATE,t2.ISCHILD FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE t2.OID = @OID AND t1.DEVICESTATE != 5 AND t2.ISCHILD = 1";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                list = DataChange<SYS_AP_VIEW>.FillModel(dt);
                return list;
            }
        }

        public SYS_AP_VIEW SelectAPViewByAPID(Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_AP_VIEW item = new SYS_AP_VIEW();
                //string strSql = "SELECT * FROM SYS_APDEVICE WHERE ID=@ID";
                string strSql = "SELECT a.*,b.POID,b.OID,b.SDATE,b.EDATE,b.ISCHILD FROM SYS_APDEVICE a,SYS_APORG b WHERE a.ID=b.APID and b.ischild=1 and a.ID = @ID";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@ID", APID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                if (dt.Rows.Count > 0)
                    item = DataChange<SYS_AP_VIEW>.FillEntity(dt.Rows[0]);
                return item;
            }
        }

        public List<Int64> SelectOnlinePeopleNumByOID(Int64 OID, Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_AP_VIEW item = new SYS_AP_VIEW();
                //string strSql = "SELECT * FROM SYS_APDEVICE WHERE ID=@ID";
                //string strSql = "select ifnull(sum(t1.onlinepeoplenum), 0) COUNT, IFNULL(t3.NUM, 0) NUM from sys_apdevice t1 left join sys_aporg t2 on t1.id = t2.apid LEFT JOIN (SELECT CONVERT(CalledStationId USING utf8) COLLATE utf8_unicode_ci CalledStationId,COUNT(1) NUM FROM statistical.openssid WHERE CurrentTime > adddate(now(), INTERVAL - 1 HOUR) GROUP BY CalledStationId) t3 ON SUBSTR(t1.MAC,4,12) = SUBSTR(t3.CalledStationId,4,12) where t2.oid in (select id from SYS_ORGANIZATION where PIDHELP like '%$" + OID + "$%' or id=" + OID + ") and t1.lasthb > adddate(now(), INTERVAL -1 HOUR)";


                string strSql = "SELECT IFNULL(COUNT(DISTINCT t3.CallingStationId), 0) COUNT, IFNULL(COUNT(0), 0) NUM FROM sys_apdevice t1";
                strSql += " LEFT JOIN sys_aporg t2 ON t1.id = t2.apid";
                strSql += " LEFT JOIN statistical.openssid t3 ON SUBSTR(t1.MAC, 4, 13)= SUBSTR(CONVERT(t3.CalledStationId USING utf8)COLLATE utf8_unicode_ci, 4, 13)";
                strSql += " WHERE t2.oid IN(SELECT id FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%' or id=" + OID + ")";
                strSql += " AND t3.CurrentTime > adddate(now(), INTERVAL - (t1.HBINTERVAL * 2) SECOND) AND t3.oid = " + OID;
                if (APID != -99)
                    strSql += " and t1.ID = " + APID;
                strSql += " GROUP BY t1.MAC";

                DataTable dt = mySql.GetDataTable(strSql, "sys_apdevice");
                List<Int64> result = new List<Int64>();
                if (dt.Rows.Count > 0)
                {
                    result.Add(Convert.ToInt64(dt.Rows[0][0]));
                    result.Add(Convert.ToInt64(dt.Rows[0][1]));
                }
                return result;
            }
        }

        public bool DeletesAp(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDADTE SYS_APORG SET ISDELETE=1 WHERE APID IN (" + ids + ")"; ;
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_APDEVICE> CheckMac(List<SYS_APDEVICE> datas)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
                string macs = "";
                foreach (var item in datas)
                    macs += "'" + item.MAC + "',";

                string strSql = "SELECT MAC FROM SYS_APDEVICE where MAC IN(" + macs.Substring(0, macs.Length - 1) + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE");
                list = DataChange<SYS_APDEVICE>.FillModel(dt);
                return list;
            }
        }

        public Int64 getSequence()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                object sec = mySql.GetOnlyOneValue("select _nextval('ID')");
                return Convert.ToInt64(sec);
            }
        }

        public List<SYS_AP_VIEW> SelectBackApByOrgId(string orgIds, int curPage, int size)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                if (orgIds.Length > 0)
                {
                    string strSql = "SELECT t1.*,t2.POID,t2.OID,t2.SDATE,t2.EDATE,t2.ISCHILD FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE t2.OID IN(" + orgIds + ") AND t2.ISCHILD=1 AND t1.DEVICESTATE != 5";

                    //string strSql = "SELECT ap.* FROM SYS_APDEVICE ap,SYS_APORG aporg WHERE ap.ID=aporg.APID AND aporg.OID IN (" + orgIds + ") AND aporg.ISCHILD=1 ";
                    strSql += " LIMIT " + ((curPage - 1) * size).ToString() + "," + size.ToString();
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG");
                    List<SYS_AP_VIEW> apList = DataChange<SYS_AP_VIEW>.FillModel(dt);
                    int count = apList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (1 == apList[i].EDATE.CompareTo(DateTime.Today))
                        {
                            apList[i].STATE = "已分配";
                        }
                        else
                            apList[i].STATE = "过期";
                    }
                    return apList;
                }
                else
                    return null;
            }
        }

        public Int32 CountsBackApByOrgId(string orgIds)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                if (orgIds.Length > 0)
                {
                    string strSql = "SELECT COUNT(*) FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE t2.OID IN(" + orgIds + ") AND t2.ISCHILD=1 AND t1.DEVICESTATE != 5";
                    Int32 counts = Int32.Parse(mySql.GetOnlyOneValue(strSql).ToString());
                    return counts;
                }
                else
                    return 0;
            }
        }

        public bool CheckIsRebootByAPID(Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT ISREBOOT FROM SYS_APDEVICE WHERE ID=" + APID;
                return Convert.ToBoolean(mySql.GetOnlyOneValue(strSql));
            }
        }

        public bool RebootComplete(Int64 serial)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APDEVICE SET ISREBOOT=0";
                strSql += " WHERE SERIAL=" + serial;
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_AP_GIS> SelectAPListForGIS(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_GIS> list = new List<SYS_AP_GIS>();
                string strSql = "SELECT t1.ID,t1.ALIAS, t1.MAC, t1.SERIAL, t1.HBINTERVAL, t1.LASTHB, t1.LAT, t1.LON, t1.ADDRESS,t1.POWERDATETIME,t1.FREETIME,t1.POWERTIME,t1.MEMFREE,t1.CPU,t1.NETWORKTOTAL,t1.NETWORKRATE,t1.ONLINEPEOPLENUM FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE t2.OID IN (SELECT ID FROM SYS_ORGANIZATION WHERE ID = @OID OR PIDHELP LIKE CONCAT('%$', @OID, '$%')) AND t1.DEVICESTATE != 5 AND t2.ISCHILD = 1 AND t2.EDATE > sysdate()";

                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                if (dt.Rows.Count > 0)
                    list = DataChange<SYS_AP_GIS>.FillModel(dt);
                return list;
            }
        }

        //public bool UpdateHeart(string deviceid, DateTime dateTime)
        //{

        //}

        /*public bool UpdateHeart(string devicemac, DateTime dateTime, string cpu, string memfree, string powertime, string freetime, string networktotal, string networkrate)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APDEVIC SET LASTHB=@LASTHB,CPU=@CPU,MEMFREE=@MEMFREE,POWERTIME=@POWERTIME,FREETIME=@FREETIME,NETWORKTOTAL=@NETWORKTOTAL,NETWORKRATE=@NETWORKRATE WHERE MAC=@MAC ";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@LASTHB",dateTime),
                new MySqlParameter("@CPU",cpu),
                new MySqlParameter("@MEMFREE",memfree),
                new MySqlParameter("@POWERTIME",powertime),
                new MySqlParameter("@FREETIME",freetime),
                new MySqlParameter("@NETWORKTOTAL",networktotal),
                new MySqlParameter("@NETWORKRATE",networkrate),
                new MySqlParameter("@MAC",devicemac)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
         * */

        public bool UpdateHeart(string devicemac, DateTime dateTime, string cpu, string memfree, string powertime, string freetime, string networktotal, string networkrate, string curdatetime, string ONLINEPEOPLENUM)
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
                string strSql = "UPDATE SYS_APDEVICE SET LASTHB=@LASTHB,CPU=@CPU,MEMFREE=@MEMFREE,POWERTIME=(CASE WHEN POWERTIME<@POWERTIME THEN @POWERTIME ELSE POWERTIME END),FREETIME=@FREETIME,NETWORKTOTAL=@NETWORKTOTAL,NETWORKRATE=@NETWORKRATE,POWERDATETIME=@POWERDATETIME,ONLINEPEOPLENUM=@ONLINEPEOPLENUM WHERE MAC=@MAC ";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@LASTHB",dateTime),
                new MySqlParameter("@CPU",cpu),
                new MySqlParameter("@MEMFREE",memfree),
                new MySqlParameter("@POWERTIME",powertime),
                new MySqlParameter("@FREETIME",freetime),
                new MySqlParameter("@NETWORKTOTAL",networktotal),
                new MySqlParameter("@NETWORKRATE",networkrate),
                new MySqlParameter("@MAC",devicemac),
                new MySqlParameter("@POWERDATETIME",POWERDATETIME),
                new MySqlParameter("@ONLINEPEOPLENUM",ONLINEPEOPLENUM)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public List<SYS_APDEVICE> SelectApStateListByOID(Int64 orgId, string apname, string column, string orderby)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "select a.ID,a.MAC,a.ALIAS,a.MODEL,a.MANUFACTURER,a.PURCHASER,a.SERIAL,a.DESCRIPTION,a.MAXSSIDNUM,a.FIRMWAREVERSION,a.DEVICESTATE";
                strSql += ",a.SUPPORT3G,a.REGDATE,a.APCTID,a.HBINTERVAL,a.DATAINTERVAL,a.ISUPDATE,a.ISREBOOT,a.LON,a.LAT,a.LASTHB,a.ADDRESS,a.CPU,a.MEMFREE";
                strSql += ",(TIMESTAMPDIFF(SECOND,a.POWERDATETIME,a.LASTHB)) POWERTIME,a.FREETIME,a.NETWORKTOTAL,a.NETWORKRATE,a.POWERDATETIME,a.HISTORYTIME,a.ONLINEPEOPLENUM";
                strSql += " from sys_apdevice a,sys_aporg b where a.ID=b.APID and b.ISCHILD = 1 and b.OID in (select c.ID from sys_organization c where c.PIDHELP like '%$" + orgId + "$%' or c.ID=" + orgId + ")";
                if (apname.Trim() != "")
                    strSql += " and a.ALIAS like '%" + apname.Trim() + "%'";
                if (orderby.Trim() != "")
                {
                    if (column.Trim().ToUpper() == "MEMFREE")
                        strSql += " order by SUBSTR(a.MEMFREE,1,LENGTH(a.MEMFREE)-1)+0 " + orderby;
                    else if (column.Trim().ToUpper() == "NETWORKTOTAL")
                        strSql += " order by (" + column + "+0) " + orderby;
                    else
                        strSql += " order by " + column + " " + orderby;
                }

                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE");
                List<SYS_APDEVICE> list = new List<SYS_APDEVICE>();
                if (dt.Rows.Count > 0)
                {
                    list = DataChange<SYS_APDEVICE>.FillModel(dt);
                }
                return list;
            }
        }

        //public Model.M_DeviceStatiscal SelectDeviceStatiscalByOID(long orgId)
        //{
        //    using (MySQLDataAccess mySql = new MySQLDataAccess())
        //    {
        //        string strSql = "SELECT a. FROM SYS_APDEVICE AS a ,SYS_APORG AS b ON a.ID=b.APID AND b.OID =@OID";
        //        return null;
        //    }
        //}

        public SYS_APDEVICE GetOrgNameByLatLon(M_LOCATION Location)
        {
            using (MySQLDataAccess mysql = new MySQLDataAccess())
            {
                //String strSql = "select c.NAME from ";
                //strSql += " (select *,min(power(abs( lat-@LAT),2)+power(abs(lon-@LON),2)) as location ";
                //strSql += " from sys_apdevice) a ,sys_aporg b,sys_organization c where a.ID=b.APID and b.ISCHILD=1 and b.OID=c.ID";
                String strSql = "SELECT * FROM sys_apdevice ORDER BY power(abs(lat - @LAT), 2)+ power(abs(lon - @LON), 2) LIMIT 1";
                MySqlParameter[] parms = new MySqlParameter[]{
                new MySqlParameter("@LAT",Location.LAT),
                new MySqlParameter("@LON",Location.LON)
                };
                DataTable dt = mysql.GetDataTable(strSql, "", parms);
                SYS_APDEVICE apd = null;
                if (dt.Rows.Count > 0)
                    apd = DataChange<SYS_APDEVICE>.FillEntity(dt.Rows[0]);
                return apd;
            }
        }

        public List<SYS_AP_VIEW> SelectAPAndORG()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                string strSql = "SELECT t1.*,t2.ID APORGID,t2.POID,t2.OID,t2.SSIDNUM,t2.SDATE,t2.EDATE,t2.ISCHILD FROM SYS_APDEVICE t1 INNER JOIN SYS_APORG t2 ON t1.ID = t2.APID WHERE t1.DEVICESTATE != 5 AND t2.ISCHILD = 1";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE");
                list = DataChange<SYS_AP_VIEW>.FillModel(dt);
                return list;
            }
        }

        public List<M_EXCEPTDEVICE> SelectExceptDevice(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_EXCEPTDEVICE> list = new List<M_EXCEPTDEVICE>();
                string strSql = " select a.ID AS SSID, a.`NAME` AS SSIDNAME, b.ID AS APID, b.ALIAS AS APNAME,c.ID AS OID,c.`NAME` AS ONAME from sys_ssid a, sys_apdevice b, sys_organization c WHERE";
                strSql += "  a.APID=b.ID and a.OID = c.ID";
                strSql += " and a.ISON = 1 and b.DEVICESTATE=1";
                strSql += " and APID in (" + ids + ")";
                DataTable dt = mySql.GetDataTable(strSql, "M_EXCEPTDEVICE");
                list = DataChange<M_EXCEPTDEVICE>.FillModel(dt);
                return list;
            }
        }

        public string SelectALIASByID(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                string strSql = "SELECT ALIAS FROM SYS_APDEVICE WHERE ID=@ID";
                MySqlParameter[] parms = new MySqlParameter[]{
                    new MySqlParameter("@ID",id)
                };
                return mySql.GetOnlyOneValue(strSql, parms).ToString();
            }
        }

        public SYS_AP_VIEW SelectAPViewBySSID(Int64 SSID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_AP_VIEW item = new SYS_AP_VIEW();
                //string strSql = "SELECT * FROM SYS_APDEVICE WHERE ID=@ID";
                string strSql = "SELECT a.*,b.POID,b.OID,b.SDATE,b.EDATE,b.ISCHILD,c.`NAME` FROM SYS_APDEVICE a,SYS_APORG b,sys_organization c WHERE a.ID=b.APID and b.OID = c.ID and b.ischild=1 and a.ID = (select APID from sys_ssid where id=@ssid)";
                MySqlParameter[] paras = new MySqlParameter[]
                {
                    new MySqlParameter("@ssid", SSID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APDEVICE", paras);
                if (dt.Rows.Count > 0)
                    item = DataChange<SYS_AP_VIEW>.FillEntity(dt.Rows[0]);
                return item;
            }
        }
    }
}
