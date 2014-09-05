using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using System.Data;
using LUOBO.Model;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    public class DAL_SYS_SSID
    {
        DAL_SYS_SETTINGVER setverDAL = new DAL_SYS_SETTINGVER();

        public bool Update(SYS_SSID data)
        {
            data.NAME = "☑️" + data.NAME.Trim().Replace("☑️", "");
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_SSID where 1<>1", "SYS_SSID");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_SSID>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_SSID where ID=" + data.ID.ToString(), "SYS_SSID");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SYS_SSID>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public bool Update(string oids,string apids,Int64 oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_SSID SET OID = " + oid + " WHERE OID IN (" + oids + ") AND APID IN (" + apids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_SSID WHERE ID = @ID ";
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
                string strSql = "DELETE FROM SYS_SSID WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_SSID> SelectByAPID(Int64 APID, Int64? oID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> list = new List<SYS_SSID>();
                string strSql = "SELECT * FROM SYS_SSID WHERE ISON=1 and APID = @APID ";
                if (oID != null)
                    strSql += " AND OID=" + oID;
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@APID", APID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID", parms);
                list = DataChange<SYS_SSID>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_SSID> SelectByAPIDs(string APIDs)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> list = new List<SYS_SSID>();
                string strSql = "SELECT * FROM SYS_SSID WHERE APID in (" + APIDs + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID");
                list = DataChange<SYS_SSID>.FillModel(dt);
                return list;
            }
        }

        public SYS_SSID SelectByID(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_SSID data = null;
                //string strSql = "SELECT * FROM SYS_SSID WHERE ID = @ID AND ISON = 1";
                string strSql = "SELECT * FROM SYS_SSID WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public List<SYS_SSID> SelectByIDs(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> data = null;
                string strSql = "SELECT * FROM SYS_SSID WHERE ID in(" + ids + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID");
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SSID>.FillModel(dt);

                return data;
            }
        }

        public int SelectCountByACID(Int64 ACID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(1) FROM SYS_SSID t1 WHERE ACID = @ACID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ACID", ACID)
                };
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql, parms));
                return count;
            }
        }

        public List<APBySSIDCount> SelectCountByAPID(string APIDs)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<APBySSIDCount> list = new List<APBySSIDCount>();
                string strSql = "select t1.ID APID,MAXSSIDNUM - ifnull(SSIDNUM,0) SSIDCount from sys_apdevice t1 left join (select APID,count(1) SSIDNUM from sys_ssid group by APID) t2 on t1.id = t2.APID WHERE t1.ID in (" + APIDs + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SSIDCount");
                if (dt.Rows.Count > 0)
                    list = DataChange<APBySSIDCount>.FillModel(dt);

                return list;
            }
        }

        public List<SYS_SSID> SelectByOID(Int64 oid, bool ison)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> datas = new List<SYS_SSID>();
                string strSql = "SELECT t1.* FROM SYS_SSID t1 INNER JOIN SYS_APORG t2 ON t1.APID = t2.APID WHERE EDATE >= sysdate() AND t1.OID = " + oid;
                if (ison)
                    strSql += " AND ISON = 1";
                else
                    strSql += " AND ISON = 0";
                strSql += " ORDER BY t1.ID ASC";

                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID");
                datas = DataChange<SYS_SSID>.FillModel(dt);
                return datas;
            }
        }

        public List<SYS_SSID> SelectByOID(int size, int curPage, Int64 oid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> datas = new List<SYS_SSID>();
                string strSql = "SELECT t1.* FROM SYS_SSID t1 INNER JOIN SYS_APORG t2 ON t1.APID = t2.APID WHERE EDATE >= sysdate() AND t1.ID ON () AND t1.OID = " + oid + " ORDER BY t1.ID ASC LIMIT " + size;
                string strChildSql = "SELECT t1.ID FROM SYS_SSID t1 INNER JOIN SYS_APORG t2 ON t1.APID = t2.APID WHERE EDATE >= sysdate() AND t1.OID = " + oid + " ORDER BY t1.ID ASC LIMIT " + ((curPage - 1) * size);

                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "SYS_SSID");
                datas = DataChange<SYS_SSID>.FillModel(dt);
                return datas;
            }
        }

        public int SelectCountByOID(Int64 oid, bool ison)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> datas = new List<SYS_SSID>();
                string strSql = "SELECT COUNT(1) FROM SYS_SSID t1 INNER JOIN SYS_APORG t2 ON t1.APID = t2.APID WHERE EDATE >= sysdate() AND t1.OID = " + oid;
                if (ison)
                    strSql += " AND ISON = 1";
                else
                    strSql += " AND ISON = 0";
                return Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
            }
        }

        public List<M_WCF_SSID_VIEW> SelectWcfSSIDViewByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_WCF_SSID_VIEW> datas = new List<M_WCF_SSID_VIEW>();
                string strSql = "SELECT t1.ID, t1.ADID, t1.APID,TRIM(t1.NAME) AS NAME, t1.PORTAL, t1.PATH, t1.MAXFLOW, t1.MAXLINKCOUNT, t1.MAXUS,t1.MAXDS,t1.VONLINETIME,t1.ISPWD,t1.PWD, t1.OID, t3.SSIDNAME AS NEWNAME, IFNULL(t3.STATE,2) AS STATE FROM sys_ssid t1 LEFT JOIN (select * from sys_ssid_audit where ID in(select max(ID) from sys_ssid_audit group by SSIDID)) t3 ON t1.ID=t3.SSIDID WHERE (t1.OID=@OID OR t1.OID IN(SELECT ID FROM SYS_ORGANIZATION WHERE PIDHELP LIKE '%$" + OID + "$%')) AND ISON=1 ORDER BY t1.APID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID", parms);
                DataTable tmp_dt = dt.Clone();
                tmp_dt.Columns["STATE"].DataType = typeof(Int32);

                foreach (DataRow row in dt.Rows)
                    tmp_dt.Rows.Add(row.ItemArray);

                datas = DataChange<M_WCF_SSID_VIEW>.FillModel(tmp_dt);
                return datas;
            }
        }

        public List<M_WCF_SSID_VIEW> SelectWcfSSIDViewByAPID(Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_WCF_SSID_VIEW> datas = new List<M_WCF_SSID_VIEW>();
                string strSql = "SELECT t1.ID, t1.ADID, t1.APID,TRIM(t1.NAME) AS NAME, t1.PORTAL, t1.PATH, t1.MAXFLOW, t1.MAXLINKCOUNT, t1.MAXUS,t1.MAXDS,t1.VONLINETIME,t1.ISPWD,t1.PWD, t1.OID, t3.SSID_NAME AS NEWNAME, IFNULL(t3.AUD_STAT,2) AS STATE FROM sys_ssid t1 LEFT JOIN (select PUB_LIST, SSID_NAME, max(AUD_STAT)AUD_STAT from ad_audit where (PUB_TYPE=3 or PUB_TYPE=4) group by PUB_LIST) t3 ON INSTR(t3.PUB_LIST,+t1.ID)  WHERE t1.APID = @APID AND ISON=1";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@APID", APID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID", parms);
                DataTable tmp_dt = dt.Clone();
                tmp_dt.Columns["STATE"].DataType = typeof(Int32);

                foreach (DataRow row in dt.Rows)
                    tmp_dt.Rows.Add(row.ItemArray);

                datas = DataChange<M_WCF_SSID_VIEW>.FillModel(tmp_dt);
                return datas;
            }
        }

        public List<SYS_SSID> SelectByOwnerAndIsOn(Int64 OID, Int64 APID, bool ISON)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_SSID> datas = new List<SYS_SSID>();
                string strSql = "SELECT * FROM SYS_SSID WHERE APID=@APID AND OID=@OID AND ISON=@ISON";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", OID),
                    new MySqlParameter("@APID", APID),
                    new MySqlParameter("@ISON", ISON)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID", parms);
                datas = DataChange<SYS_SSID>.FillModel(dt);
                return datas;
            }
        }

        public List<M_Statistical> SelectStatisticalSSIDByOID(Int64 OID, Int64 APID, DateTime startTime, DateTime endTime)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_Statistical> datas = new List<M_Statistical>();
                string strSql = "select TRIM(t1.NAME) NAME,ifnull(SUM(t2.ssidcount), 0)NUM from sys_ssid t1 left join (select ssid,count(0) ssidcount from statistical.openssid where date_format(CurrentTime, '%Y%m%d') >= date_format('" + startTime.ToShortDateString() + "', '%Y%m%d') AND date_format(CurrentTime, '%Y%m%d') <= date_format('" + endTime.ToShortDateString() + "', '%Y%m%d')";
                if (APID != -99)
                    strSql += " AND SUBSTR(CalledStationId,4,12) in (select SUBSTR(CONVERT(MAC USING utf8)COLLATE utf8_unicode_ci,4,12) FROM sys_apdevice WHERE ID = " + APID + ")";
                strSql += " group by ssid) t2 on t1.id = t2.ssid where t1.oid in (select id from SYS_ORGANIZATION where PIDHELP like '%$" + OID + "$%' or id=" + OID + ") AND t1.ison = 1 group by TRIM(t1.NAME) order by NUM desc limit 5";
                DataTable dt = mySql.GetDataTable(strSql, "M_Statistical");
                DataTable tmpDT = dt.Clone();

                tmpDT.Columns["NUM"].DataType = typeof(Int64);
                foreach (DataRow item in dt.Rows)
                    tmpDT.Rows.Add(item.ItemArray);

                datas = DataChange<M_Statistical>.FillModel(tmpDT);
                return datas;
            }
        }

        /// <summary>
        /// 广告发布审核通过后SSID信息更新，并备份
        /// </summary>
        /// <param name="PubType"></param>
        /// <param name="IDS"></param>
        /// <param name="AD_ID"></param>
        /// <param name="AD_Name"></param>
        /// <param name="isSameName"></param>
        /// <returns></returns>
        public Boolean PubSSIDFromAD(int PubType, String IDS, Int64 AD_ID, String AD_Path, Boolean isSameName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strWhere = "";
                switch (PubType)
                {
                    case (int)CustomEnum.ENUM_ADC_Type.ToCase:
                        strWhere = "ACID in (" + IDS + ")";
                        break;
                    case (int)CustomEnum.ENUM_ADC_Type.ToSSID:
                        strWhere = "id in (" + IDS + ")";
                        break;
                    default:
                        return false;
                        break;
                }
                String cSql = " from ad_info where ad_info.AD_ID = " + AD_ID;
                String uName = "";
                if (isSameName)
                {
                    uName = "NAME = (select AD_Title " + cSql + "),";
                }
                String sql = "update sys_ssid set " + uName + "ADID =(select AD_ID " + cSql + "),PORTAL =(select AD_HomePage " + cSql + "),PATH = '" + AD_Path + "' WHERE " + strWhere;
                mySql.ExecuteSQL(sql);
                sql = "INSERT INTO sys_ssid_history select 0 as a,id,NAME,PORTAL,PATH,oid,APID,ACID,now() as b,ADID,b.AD_Title,'广告编辑' as c from sys_ssid a ,ad_info b WHERE a.ADID = b.AD_ID and " + strWhere;
                mySql.ExecuteSQL(sql);

                sql = "select DISTINCT(APID) as APID from sys_ssid WHERE " + strWhere;
                DataTable dt = mySql.GetDataTable(sql, "SYS_APIDLIST");
                SYS_SETTINGVER setver = null;
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    setver = new SYS_SETTINGVER();
                    setver.ID = -1;
                    setver.APID = Int64.Parse(dt.Rows[i]["APID"].ToString());
                    setver.TYPE = (int)CustomEnum.ENUM_Setting_Type.Ad;
                    setver.GUID = Guid.NewGuid().ToString();
                    setver.DATETIME = DateTime.Now;
                    setverDAL.Update(setver);
                }

                sql = "INSERT INTO sys_ziptask SELECT 0 as id,a.*,1 as kind,0 as statu,NOW() as ti from ( select DISTINCT(APID) as APID from sys_ssid WHERE " + strWhere + " ) a";
                mySql.ExecuteSQL(sql);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Dictionary<Int64, Int32> getPubCountBYIds(String ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT adid,count(0) as pubcount FROM sys_ssid where adid in (" + ids + ") and ison = 1 group by adid";
                DataTable dt = mySql.GetDataTable(strSql, "AD_INFO");
                Dictionary<Int64, Int32> result = new Dictionary<long, int>();
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    result.Add(Int64.Parse(dt.Rows[i]["adid"].ToString()),Int32.Parse(dt.Rows[i]["pubcount"].ToString()));
                }
                return result;
            }
        }

        /// <summary>
        /// 根据SSID获取SSID所属信息，包含机构和AP名称
        /// </summary>
        /// <param name="ssids"></param>
        /// <returns></returns>
        public List<M_EXCEPTDEVICE> GetSSIDBelong(string ssids)
        {
            List<M_EXCEPTDEVICE> list = new List<M_EXCEPTDEVICE>();
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "select a.ID AS SSID, a.`NAME` AS SSIDNAME, b.ID AS APID, b.ALIAS AS APNAME,c.ID AS OID,c.`NAME` AS ONAME from sys_ssid a, sys_apdevice b, sys_organization c WHERE";
                strSql += " a.APID=b.ID and a.OID = c.ID and a.ISON = 1 and b.DEVICESTATE=1";
                strSql += " and a.ID in (" + ssids + ")";
                DataTable dt = mySql.GetDataTable(strSql, "AD_INFO");
                if (dt.Rows != null && dt.Rows.Count > 0)
                    list = DataChange<M_EXCEPTDEVICE>.FillModel(dt);
                return list;
            }
        }

        public Int64 GetAPIDBySSID(Int64 SSID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT APID FROM SYS_SSID where ID=@SSID";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID");
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt64(dt.Rows[0][0]);
                }
                return -1;
            }
        }

        public List<M_SSID_AP> GetSSIDInfoByIDs(string IDs)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<M_SSID_AP> list = new List<M_SSID_AP>();
                string strSql = "select a.ID, a.NAME SSIDNAME,b.ID APID, b.ALIAS APNAME,c.ID OID, c.`NAME` ONAME from sys_ssid a,sys_apdevice b,sys_organization c where a.APID=b.ID and a.OID=c.ID and a.ID in (" + IDs + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID");
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    list = DataChange<M_SSID_AP>.FillModel(dt);
                }
                return list;
            }
        }
    }
}
