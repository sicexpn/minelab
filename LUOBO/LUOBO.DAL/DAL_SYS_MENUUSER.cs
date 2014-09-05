using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using System.Data;
using LUOBO.Entity;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_MENUUSER
    {
        public bool Update(SYS_MENUUSER data)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "";
                MySqlParameter[] parms = new MySqlParameter[] {
                        new MySqlParameter("@ID",data.ID),
                        new MySqlParameter("@M_ID",data.M_ID),
                        new MySqlParameter("@U_ID",data.U_ID)
                    };
                string[] tags = { "", "" };
                if (data.ID < 0)
                {
                    strSql = "INSERT INTO SYS_MENUUSER(M_ID, U_ID) VALUES (@M_ID,@U_ID);SELECT LAST_INSERT_ID()";
                    data.ID = Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
                    tags[0] = "M_ID" + data.M_ID;
                    tags[1] = "U_ID" + data.U_ID;
                    Helper.AppFabricCacheHelper.Instance().AddCache(data.ID.ToString(), "SYS_MENUUSER", data, tags);
                    flag = true;
                }
                else
                {
                    strSql = "UPDATE SYS_MENUUSER SET M_ID=@M_ID, U_ID=@U_ID WHERE ID=@ID";
                    flag = mySql.ExecuteSQL(strSql, parms);
                    if (flag)
                        Helper.AppFabricCacheHelper.Instance().SetCache(data.ID.ToString(), "SYS_MENUUSER", data);
                }
            }
            return flag;
        }

        public bool Delete(Int64 id)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_MENUUSER WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                flag = mySql.ExecuteSQL(strSql, parms);
            }
            if (flag)
            {
                Helper.AppFabricCacheHelper.Instance().RemoveOneCache(id.ToString(), "SYS_MENUUSER");
            }
            return flag;
        }

        public bool Deletes(string ids)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_MENUUSER WHERE ID in (" + ids + ")";
                flag = mySql.ExecuteSQL(strSql);
            }
            if (flag)
            {
                List<string> id_list = ids.Split(',').ToList();
                foreach (var id in id_list)
                    Helper.AppFabricCacheHelper.Instance().RemoveOneCache(id, "SYS_MENUUSER");
            }

            return flag;
        }

        public bool DeletesByUIDs(string uids)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_MENUUSER WHERE U_ID in (" + uids + ")";
                flag = mySql.ExecuteSQL(strSql);
            }
            if (flag)
            {
                List<SYS_MENUUSER> list = null;
                List<Int64> uid_list = uids.Split(',').Select(c => Convert.ToInt64(c)).ToList();
                if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENUUSER", "Table") == null)
                {
                    list = FillCache();
                    list = list.Where(c => uid_list.Contains(c.U_ID)).ToList();
                }
                else
                    list = Helper.AppFabricCacheHelper.Instance().GetCacheByAnyTag<SYS_MENUUSER>("SYS_MENUUSER", uids.Split(','));

                foreach (var item in list)
                    Helper.AppFabricCacheHelper.Instance().RemoveOneCache(item.ID.ToString(), "SYS_MENUUSER");
            }
            return flag;

        }

        /// <summary>
        /// 将数据库中的数据全部读取到缓存中
        /// </summary>
        /// <returns></returns>
        public static List<SYS_MENUUSER> FillCache()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_MENUUSER> list = null;
                string strSql = "Select * FROM SYS_MENUUSER";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_MENUUSER");
                string[] tags = { "", "" };
                if (dt.Rows.Count > 0)
                {
                    list = DataChange<SYS_MENUUSER>.FillModel(dt);
                    foreach (var item in list)
                    {
                        tags[0] = "M_ID" + item.M_ID;
                        tags[1] = "U_ID" + item.U_ID;
                        Helper.AppFabricCacheHelper.Instance().AddCache(item.ID.ToString(), "SYS_MENUUSER", item, tags);
                    }
                }
                return list;
            }
        }
    }
}
