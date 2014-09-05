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
    public class DAL_SYS_MENU
    {
        public bool Update(SYS_MENU data)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "";
                MySqlParameter[] parms = new MySqlParameter[] {
                        new MySqlParameter("@M_ID",data.M_ID),
                        new MySqlParameter("@APP_ID", data.APP_ID),
                        new MySqlParameter("@M_NAME", data.M_NAME),
                        new MySqlParameter("@M_PID", data.M_PID),
                        new MySqlParameter("@M_LEVEL", data.M_LEVEL),
                        new MySqlParameter("@M_ORDER", data.M_ORDER),
                        new MySqlParameter("@M_TYPE", data.M_TYPE),
                        new MySqlParameter("@M_URL", data.M_URL),
                        new MySqlParameter("@M_REMARK", data.M_REMARK),
                        new MySqlParameter("@M_ICON", data.M_ICON),
                        new MySqlParameter("@M_ICONTYPE", data.M_ICONTYPE),
                        new MySqlParameter("@M_ISON", data.M_ISON)
                    };
                if (data.M_ID < 0)
                {
                    strSql = "INSERT INTO SYS_MENU(APP_ID, M_NAME, M_PID, M_LEVEL, M_ORDER, M_TYPE, M_URL, M_REMARK, M_ICON, M_ICONTYPE,M_ISON) VALUES(@APP_ID,@M_NAME,@M_PID,@M_LEVEL,@M_ORDER,@M_TYPE,@M_URL,@M_REMARK,@M_ICON,@M_ICONTYPE,@M_ISON);SELECT LAST_INSERT_ID()";
                    
                    //MySqlParameter[] parms = new MySqlParameter[] {
                    //    new MySqlParameter("@APP_ID", data.APP_ID),
                    //    new MySqlParameter("@M_NAME", data.M_NAME),
                    //    new MySqlParameter("@M_PID", data.M_PID),
                    //    new MySqlParameter("@M_LEVEL", data.M_LEVEL),
                    //    new MySqlParameter("@M_ORDER", data.M_ORDER),
                    //    new MySqlParameter("@M_TYPE", data.M_TYPE),
                    //    new MySqlParameter("@M_URL", data.M_URL),
                    //    new MySqlParameter("@M_REMARK", data.M_REMARK),
                    //    new MySqlParameter("@M_ICON", data.M_ICON),
                    //    new MySqlParameter("@M_ICONTYPE", data.M_ICONTYPE),
                    //    new MySqlParameter("@M_ISON", data.M_ISON)
                    //};
                    data.M_ID = Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
                    Helper.AppFabricCacheHelper.Instance().AddCache(data.M_ID.ToString(), "SYS_MENU", data);
                    flag = true;
                }
                else
                {
                    strSql = "UPDATE SYS_MENU SET APP_ID=@APP_ID, M_NAME=@M_NAME, M_PID=@M_PID, M_LEVEL=@M_LEVEL, M_ORDER=@M_ORDER, M_TYPE=@M_TYPE, M_URL=@M_URL, M_REMARK=@M_REMARK, M_ICON=@M_ICON, M_ICONTYPE=@M_ICONTYPE,M_ISON=@M_ISON WHERE M_ID=@M_ID";
                    flag = mySql.ExecuteSQL(strSql, parms);
                    if(flag)
                        Helper.AppFabricCacheHelper.Instance().SetCache(data.M_ID.ToString(), "SYS_MENU", data);
                }
            }
            return flag;
        }

        public bool UpdateForIsOn(Int64 id, bool ison)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_MENU SET M_ISON = @ISON WHERE M_ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id),
                    new MySqlParameter("@ISON", ison)
                };
                flag = mySql.ExecuteSQL(strSql, parms);
            }
            if (flag)
            {
                SYS_MENU data = (SYS_MENU)Helper.AppFabricCacheHelper.Instance().GetOneCache(id.ToString(), "SYS_MENU");
                data.M_ISON = ison;
                Helper.AppFabricCacheHelper.Instance().SetCache(id.ToString(), "SYS_MENU", data);
            }
            return flag;
        }

        public bool Delete(Int64 id)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_MENU WHERE M_ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                flag = mySql.ExecuteSQL(strSql, parms);
            }
            if (flag)
                Helper.AppFabricCacheHelper.Instance().RemoveOneCache(id.ToString(), "SYS_MENU");

            return flag;
        }

        public bool Deletes(string ids)
        {
            bool flag = false;
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_MENU WHERE M_ID in (" + ids + ")";
                flag = mySql.ExecuteSQL(strSql);
            }
            if (flag)
            {
                List<string> ids_list= ids.Split(',').ToList();
                foreach (var id in ids_list)
                    Helper.AppFabricCacheHelper.Instance().RemoveOneCache(id, "SYS_MENU");
            }
            return flag;
        }

        public List<SYS_MENU> Select()
        {
            List<SYS_MENU> list = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENU", "Table") == null)
                list = FillCache_SYS_MENU();
            else
                list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_MENU>("SYS_MENU");

            return list.Where(c => c.M_ISON == true).OrderBy(c => c.M_ORDER).ToList();

            //using (MySQLDataAccess mySql = new MySQLDataAccess())
            //{
            //    List<SYS_MENU> list = new List<SYS_MENU>();
            //    string strSql = "SELECT * FROM SYS_MENU WHERE M_ISON = 1";
            //    DataTable dt = mySql.GetDataTable(strSql, "SYS_MENU");
            //    if (dt.Rows.Count > 0)
            //        list = DataChange<SYS_MENU>.FillModel(dt);
            //
            //    return list;
            //}
        }

        public List<SYS_MENU> SelectAll()
        {
            List<SYS_MENU> list = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENU", "Table") == null)
                list = FillCache_SYS_MENU();
            else
                list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_MENU>("SYS_MENU");
            return list.OrderBy(c=>c.M_ORDER).ToList();

            //using (MySQLDataAccess mySql = new MySQLDataAccess())
            //{
            //    List<SYS_MENU> list = new List<SYS_MENU>();
            //    string strSql = "SELECT * FROM SYS_MENU";
            //    DataTable dt = mySql.GetDataTable(strSql, "SYS_MENU");
            //    if (dt.Rows.Count > 0)
            //        list = DataChange<SYS_MENU>.FillModel(dt);
            //
            //    return list;
            //}
        }

        public List<SYS_MENU> SelectByUID(Int64 uid)
        {
            List<SYS_MENU> menu_list = new List<SYS_MENU>();
            List<SYS_MENUUSER> menuuser_list = new List<SYS_MENUUSER>();

            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENU", "Table") == null)
                menu_list = FillCache_SYS_MENU();
            else
                menu_list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_MENU>("SYS_MENU");

            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENUUSER", "Table") == null)
                menuuser_list = DAL_SYS_MENUUSER.FillCache();
            else
                menuuser_list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_MENUUSER>("SYS_MENUUSER");

            menuuser_list = menuuser_list.Where(c => c.U_ID == uid).ToList();
            List<Int64> muid = menuuser_list.Select(c => c.M_ID).ToList();
            menu_list = menu_list.Where(c => muid.Contains(c.M_ID) && c.M_ISON == true).ToList();
            return menu_list.OrderBy(c => c.M_ORDER).ToList();

            //using (MySQLDataAccess mySql = new MySQLDataAccess())
            //{
            //    List<SYS_MENU> list = new List<SYS_MENU>();
            //    string strSql = "SELECT t1.* FROM SYS_MENU t1 INNER JOIN SYS_MENUUSER t2 ON t1.M_ID = t2.M_ID WHERE t1.M_ISON = 1 AND t2.U_ID = @UID ORDER BY M_ORDER";
            //    MySqlParameter[] parms = new MySqlParameter[] {
            //        new MySqlParameter("@UID", uid)
            //    };
            //    DataTable dt = mySql.GetDataTable(strSql, "SYS_MENU", parms);
            //    if (dt.Rows.Count > 0)
            //        list = DataChange<SYS_MENU>.FillModel(dt);
            //
            //    return list;
            //}
        }

        public List<SYS_MENU> SelectByUIDAll(Int64 uid)
        {

            List<SYS_MENU> menu_list = new List<SYS_MENU>();
            List<SYS_MENUUSER> menuuser_list = new List<SYS_MENUUSER>();

            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENU", "Table") == null)
                menu_list = FillCache_SYS_MENU();
            else
                menu_list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_MENU>("SYS_MENU");

            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENUUSER", "Table") == null)
                menuuser_list = DAL_SYS_MENUUSER.FillCache();
            else
                menuuser_list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_MENUUSER>("SYS_MENUUSER");

            menuuser_list = menuuser_list.Where(c => c.U_ID == uid).ToList();
            List<Int64> muid = menuuser_list.Select(c => c.M_ID).ToList();
            menu_list = menu_list.Where(c => muid.Contains(c.M_ID)).ToList();
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                //string strSql = "SELECT t1.* FROM SYS_MENU t1 INNER JOIN SYS_MENUUSER t2 ON t1.M_ID = t2.M_ID WHERE t2.U_ID = @UID ORDER BY M_ORDER";
                //MySqlParameter[] parms = new MySqlParameter[] {
                //    new MySqlParameter("@UID", uid)
                //};
                //DataTable dt = mySql.GetDataTable(strSql, "SYS_MENU", parms);
                //if (dt.Rows.Count > 0)
                //    list = DataChange<SYS_MENU>.FillModel(dt);
            }
            return menu_list.OrderBy(c =>c.M_ORDER).ToList();

        }

        public SYS_MENU SelectByID(Int64 id)
        {
            SYS_MENU data = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_MENU", "Table") == null)
                data = FillCache_SYS_MENU().Where(c => c.M_ID == id).FirstOrDefault();
            else
                data = (SYS_MENU)Helper.AppFabricCacheHelper.Instance().GetOneCache(id.ToString(), "SYS_MENU");

            return data;
        }

        /// <summary>
        /// 将数据库中的数据全部读取到缓存中
        /// </summary>
        /// <returns></returns>
        public static List<SYS_MENU> FillCache_SYS_MENU()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_MENU> list = null;
                string strSql = "Select * FROM SYS_MENU ORDER BY M_ORDER";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_MENU");
                if (dt.Rows.Count > 0)
                {
                    list = DataChange<SYS_MENU>.FillModel(dt);
                    list.ForEach(c => Helper.AppFabricCacheHelper.Instance().AddCache(c.M_ID.ToString(), "SYS_MENU", c));
                }
                return list;
            }
        }
    }
}
