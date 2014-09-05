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
    public class DAL_SYS_LOG_ALERTKEYWORD
    {
        public bool Insert(SYS_LOG_ALERTKEYWORD data)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_LOG_ALERTKEYWORD(OID,KEYWORD) VALUES(@OID,@KEYWORD);SELECT LAST_INSERT_ID()";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", data.OID),
                    new MySqlParameter("@KEYWORD", data.KEYWORD)
                };
                data.ID = Convert.ToInt32(mySql.GetOnlyOneValue(strSql, parms));
                flag = true;
            }
            if (flag)
            {
                Helper.AppFabricCacheHelper.Instance().AddCache(data.ID.ToString(), "SYS_LOG_ALERTKEYWORD", data, "OID" + data.OID);
            }
            return flag;
        }

        public bool Delete(Int64 ID)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_LOG_ALERTKEYWORD WHERE ID=@ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", ID)
                };
                flag = mySql.ExecuteSQL(strSql, parms);
            }
            if (flag)
            {
                Helper.AppFabricCacheHelper.Instance().RemoveOneCache(ID.ToString(), "SYS_LOG_ALERTKEYWORD");
            }
            return flag;
        }

        public List<SYS_LOG_ALERTKEYWORD> getKeyWord(Int64 oid)
        {
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_LOG_ALERTKEYWORD", "Table") == null)
            {
                List<SYS_LOG_ALERTKEYWORD> list = FillCache();
                return list.Where(c => c.OID == oid).ToList();
            }
            return Helper.AppFabricCacheHelper.Instance().GetCacheByTag<SYS_LOG_ALERTKEYWORD>("SYS_LOG_ALERTKEYWORD", "OID" + oid);
        }

        /// <summary>
        /// 将数据库中的数据全部读取到缓存中
        /// </summary>
        /// <returns></returns>
        public static List<SYS_LOG_ALERTKEYWORD> FillCache()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_LOG_ALERTKEYWORD> list = null;
                string strSql = "Select * FROM SYS_LOG_ALERTKEYWORD";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_LOG_ALERTKEYWORD");
                if (dt.Rows.Count > 0)
                {
                    list = DataChange<SYS_LOG_ALERTKEYWORD>.FillModel(dt);
                    list.ForEach(c => Helper.AppFabricCacheHelper.Instance().AddCache(c.ID.ToString(), "SYS_LOG_ALERTKEYWORD", c, "OID" + c.OID));
                }
                return list;
            }
        }
    }
}
