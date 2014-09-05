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
    public class DAL_SYS_APORG
    {
        public bool Insert(SYS_APORG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_APORG(APID, OID, POID, SDATE, EDATE, ISCHILD, SSIDNUM) VALUES(@APID, @OID, @POID, @SDATE, @EDATE, @ISCHILD, @SSIDNUM)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@APID",data.APID),
                    new MySqlParameter("@OID",data.OID),
                    new MySqlParameter("@POID",data.POID),
                    new MySqlParameter("@SDATE",data.SDATE),
                    new MySqlParameter("@EDATE",data.EDATE),
                    new MySqlParameter("@ISCHILD",data.ISCHILD),
                    new MySqlParameter("@SSIDNUM",data.SSIDNUM)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Update(SYS_APORG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APORG SET APID = @APID, OID = @OID, POID = @POID, SDATE = @SDATE, EDATE = @EDATE, ISCHILD = @ISCHILD,SSIDNUM = @SSIDNUM WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",data.ID),
                    new MySqlParameter("@APID",data.APID),
                    new MySqlParameter("@OID",data.OID),
                    new MySqlParameter("@POID",data.POID),
                    new MySqlParameter("@SDATE",data.SDATE),
                    new MySqlParameter("@EDATE",data.EDATE),
                    new MySqlParameter("@ISCHILD",data.ISCHILD),
                    new MySqlParameter("@SSIDNUM",data.SSIDNUM)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool UpdateCHILD(SYS_APORG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APORG SET ISCHILD = @ISCHILD WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",data.ID),
                    new MySqlParameter("@ISCHILD",data.ISCHILD)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APORG WHERE ID = " + id;
                return mySql.ExecuteSQL(strSql);
            }
        }
        public bool Deletes(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APORG WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }
        public SYS_APORG Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APORG data = null;
                string strSql = "SELECT * FROM SYS_APORG WHERE ID = " + id;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG");
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_APORG>.FillEntity(dt.Rows[0]);
                }
                return data;
            }
        }

        public List<SYS_APORG> SelectByApId(int apId)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT * FROM SYS_APORG WHERE APID = " + apId;
                List<SYS_APORG> list = null;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG");
                list = DataChange<SYS_APORG>.FillModel(dt);
                return list;
            }
        }

        public SYS_APORG SelectByApId(Int64 apId, bool ischild)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT * FROM SYS_APORG WHERE APID = @APID AND ISCHILD = @ISCHILD";
                SYS_APORG data = null;
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@APID",apId),
                    new MySqlParameter("@ISCHILD",ischild)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_APORG>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        public bool DeleteByApId(int apId)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE  FROM SYS_APORG WHERE APID = " + apId;
                return mySql.ExecuteSQL(strSql);
            }
        }
        /// <summary>
        /// xpn
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="apIds"></param>
        /// <returns></returns>
        public bool UpdateBackByOID(long oId, string apIds)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                //string strSql = "SELECT * FROM SYS_APORG WHERE APID IN( " + apIds + ") AND ISCHILD=1";
                string strSql = null;
                if (apIds.Length > 0)
                {
                    //strSql = "UPDATE SYS_APORG SET ISCHILD=0 WHERE APID IN(" + apIds + ") AND ISCHILD=1 AND OID!=" + oId;
                    //string strSql1 = "; UPDATE SYS_APORG SET ISCHILD=1 WHERE APID IN(" + apIds + ")AND OID=" + oId;
                    //strSql += strSql1;
                    strSql = "update sys_aporg set ischild=1 where OID = " + oId + " and apid in (" + apIds + ")";
                }
                return mySql.ExecuteSQL(strSql);
            }
        }

        /// <summary>
        /// xpn
        /// </summary>
        /// <param name="jgID"></param>
        /// <param name="apIds"></param>
        /// <returns></returns>
        public List<SYS_APORG> SelectByBackApId(int jgID, string apIds)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APORG> list = new List<SYS_APORG>();
                if (apIds.Length > 0)
                {
                    //string strSql = "SELECT * FROM SYS_APORG WHERE APID IN(" + apIds + ") AND ISCHILD=1 AND OID!=" + jgID;
                    string strSql = "select a.* from sys_aporg a, sys_organization b where a.OID = b.ID and PIDHELP like '%$"+jgID+"$%' and apid in("+apIds+")";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG");
                    list = DataChange<SYS_APORG>.FillModel(dt);
                }
                return list;
            }
        }
        public DateTime? SelectApStartTimeByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT SDATE FROM SYS_APORG WHERE APID = ( SELECT ID FROM SYS_APDEVICE WHERE MAC = @MAC LIMIT 0,1) LIMIT 0,1 ";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@MAC",apMac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG", parms);
                SYS_APORG data = new SYS_APORG();
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_APORG>.FillEntity(dt.Rows[0]);
                    return data.SDATE;
                }
                return null;
            }
        }

        public Int64? SelectOIDByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT OID FROM SYS_APORG WHERE APID = ( SELECT ID FROM SYS_APDEVICE WHERE MAC = @MAC LIMIT 0,1) AND ISCHILD=1 LIMIT 0,1 ";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@MAC",apMac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORG", parms);
                SYS_APORG data = new SYS_APORG();
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_APORG>.FillEntity(dt.Rows[0]);
                    return data.OID;
                }
                return null;
            }
        }

        public List<Model.M_OrgApTime> SelectOrgApTimeByOID(string oID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT a.OID AS OID,a.SDATE AS ApStartTime,b.POWERDATETIME AS ApPowerTime,b.MAC,b.ONLINEPEOPLENUM AS ONLINEPEOPLENUM FROM SYS_APORG AS a,SYS_APDEVICE AS b WHERE  a.APID = b.ID AND a.OID = @OID ";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@OID",oID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "M_OrgApTime", parms);
                List<Model.M_OrgApTime> list = null;
                if (dt.Rows.Count > 0)
                    list = DataChange<Model.M_OrgApTime>.FillModel(dt);
                return list;
            }
        }
    }
}
