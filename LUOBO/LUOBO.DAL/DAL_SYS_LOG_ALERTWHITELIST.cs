using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_LOG_ALERTWHITELIST
    {
        MySQLDataAccess mySql = new MySQLDataAccess();
        public bool Insert(SYS_LOG_ALERTWHITELIST data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_LOG_ALERTWHITELIST(OID,MAC) VALUES(@OID,@MAC)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", data.OID),
                    new MySqlParameter("@MAC", data.MAC)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Delete(SYS_LOG_ALERTWHITELIST data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_LOG_ALERTWHITELIST WHERE MAC=@MAC AND OID=@OID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", data.OID),
                    new MySqlParameter("@MAC", data.MAC)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public List<SYS_LOG_ALERTWHITELIST> getWhiteList(Int64 oid)
        {
            //if (Helper.CacheHelper.Instance().GetCache("SYS_LOG_ALERTWHITELIST") == null)
            //{

            //    DataTable dt = mySql.GetDataTable("Select * FROM SYS_LOG_ALERTWHITELIST order by ID desc", "SYS_LOG_ALERTWHITELIST");
            //    List<SYS_LOG_ALERTWHITELIST> datas = DataChange<SYS_LOG_ALERTWHITELIST>.FillModel(dt);
            //    Helper.CacheHelper.Instance().SetCache("SYS_LOG_ALERTWHITELIST", datas);
            //}
            //List<SYS_LOG_ALERTWHITELIST> list = Helper.CacheHelper.Instance().GetCache("SYS_LOG_ALERTWHITELIST") as List<SYS_LOG_ALERTWHITELIST>;
            //return list.Where(c=>c.OID==oid).ToList();

            string strSql = "Select * FROM SYS_LOG_ALERTWHITELIST where OID = @OID order by ID desc";
            MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", oid)
                };
            DataTable dt = mySql.GetDataTable(strSql, "SYS_LOG_ALERTWHITELIST", parms);
            List<SYS_LOG_ALERTWHITELIST> datas = DataChange<SYS_LOG_ALERTWHITELIST>.FillModel(dt);
            return datas;
        }
    }
}
