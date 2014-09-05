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
    public class DAL_SYS_APORGLOG
    {
        public bool Insert(SYS_APORGLOG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_APORGLOG(APID, FOID, TOID,SSIDNUM, SDATE, EDATE, OPNAME, CREATETIME) VALUES(@APID, @FOID, @TOID, @SSIDNUM, @SDATE, @EDATE, @OPNAME, @CREATETIME)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@APID",data.APID),
                    new MySqlParameter("@FOID",data.FOID),
                    new MySqlParameter("@TOID",data.TOID),
                    new MySqlParameter("@SSIDNUM", data.SSIDNUM),
                    new MySqlParameter("@SDATE",data.SDATE),
                    new MySqlParameter("@EDATE",data.EDATE),
                    new MySqlParameter("@OPNAME",data.OPNAME),
                    new MySqlParameter("@CREATETIME",data.CREATETIME)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Update(SYS_APORGLOG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_APORGLOG SET APID = @APID, FOID = @FOID, TOID = @TOID,SSIDNUM = @SSIDNUM, SDATE = @SDATE, EDATE = @EDATE, OPNAME = @OPNAME, CREATETIME = @CREATETIME WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",data.ID),
                    new MySqlParameter("@APID",data.APID),
                    new MySqlParameter("@FOID",data.FOID),
                    new MySqlParameter("@TOID",data.TOID),
                    new MySqlParameter("@SSIDNUM", data.SSIDNUM),
                    new MySqlParameter("@SDATE",data.SDATE),
                    new MySqlParameter("@EDATE",data.EDATE),
                    new MySqlParameter("@OPNAME",data.OPNAME),
                    new MySqlParameter("@CREATETIME",data.CREATETIME)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APORGLOG WHERE ID = " + id;
                return mySql.ExecuteSQL(strSql);
            }
        }
        public SYS_APORGLOG Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_APORGLOG data = null;
                string strSql = "SELECT * FROM SYS_APORGLOG WHERE ID = " + id;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APORGLOG");
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<SYS_APORGLOG>.FillEntity(dt.Rows[0]);
                }
                return data;
            }
        }

        public bool Insert(int jgID, string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<string> apIds = new List<string>();
                apIds = ids.Split(',').ToList();
                int countIds = apIds.Count();

                throw new NotImplementedException();
            }
        }
    }
}
