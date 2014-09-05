using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using System.Data;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using LUOBO.Model;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    public class DAL_SYS_ALARMSCOPE
    {
        /// <summary>
        /// 单条更新数据，插入无法获取自增ID，缓存目前有问题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(SYS_ALARMSCOPE data)
        {
            List<SYS_ALARMSCOPE> list = new List<SYS_ALARMSCOPE>();
            if (Helper.CacheHelper.Instance().GetCache("SYS_ALARMSCOPE") == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {
                    string strSql = "select * from SYS_ALARMSCOPE";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_ALARMSCOPE");
                    if (dt.Rows.Count > 0)
                        list = DataChange<SYS_ALARMSCOPE>.FillModel(dt);
                    Helper.CacheHelper.Instance().SetCache("SYS_ALARMSCOPE", list);
                }
            }

            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_ALARMSCOPE where 1<>1", "SYS_ALARMSCOPE");
                if (data.ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<SYS_ALARMSCOPE>.FillRow(data, dr));
                    list.Add(data);
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_ALARMSCOPE where ID=" + data.ID.ToString(), "SYS_ALARMSCOPE");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<SYS_ALARMSCOPE>.FillRow(data, dt.Rows[0]);
                    list.Remove(list.Find(c => c.ID == data.ID));
                    list.Add(data);
                }

                return mySql.Update(dt);
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool Inserts(List<SYS_ALARMSCOPE> datas)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_ALARMSCOPE where 1<>1", "SYS_ALARMSCOPE");
                DataRow dr = null;
                foreach (var data in datas)
                {
                    if (data.ID < 0)
                    {
                        dr = dt.NewRow();
                        dt.Rows.Add(DataChange<SYS_ALARMSCOPE>.FillRow(data, dr));
                    }
                }
                return mySql.Update(dt);
            }
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        public List<SYS_ALARMSCOPE> SelectAll()
        {
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_ALARMSCOPE", "Table") == null)
                return FillCache();
            else
                return Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_ALARMSCOPE>("SYS_ALARMSCOPE");
        }

        /// <summary>
        /// 根据规则ID查询数据
        /// </summary>
        /// <returns></returns>
        public List<SYS_ALARMSCOPE> SelectByALID(Int64 ALID)
        {
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_ALARMSCOPE", "Table") == null)
            {
                List<SYS_ALARMSCOPE> list = FillCache();
                return list.Where(c => c.ALID == ALID).ToList();
            }
            else
                return Helper.AppFabricCacheHelper.Instance().GetCacheByTag<SYS_ALARMSCOPE>("SYS_ALARMSCOPE", "ALID" + ALID);
        }

        /// <summary>
        /// 更新VCOUNT和访问时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateCountAddOne(OpenSSID data)
        {
            List<SYS_ALARMSCOPE> list = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_ALARMSCOPE", "Table") == null)
            {
                list = FillCache();
                list = list.Where(c => c.SSID == Convert.ToInt64(data.SSID)).ToList();
            }
            else
                list = Helper.AppFabricCacheHelper.Instance().GetCacheByTag<SYS_ALARMSCOPE>("SYS_ALARMSCOPE", "SSID" + data.SSID);

            // 规则里不需要检查SSID
            if (list == null)
                return true;

            string[] tags = { "", "" };
            foreach (var item in list)
            {
                item.VCOUNT += 1;
                item.CURRENTTIME = data.CurrentTime;
                tags[0] = "ALID" + item.ALID;
                tags[1] = "SSID" + item.SSID;
                Helper.AppFabricCacheHelper.Instance().SetCache(item.ID.ToString(), "SYS_ALARMSCOPE", item, tags);
            }

            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_ALARMSCOPE SET VCOUNT=VCOUNT+1,CURRENTTIME=@CURTIME WHERE SSID=@SSID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@CURTIME",data.CurrentTime),
                    new MySqlParameter("@SSID",Convert.ToInt64(data.SSID))
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        /// <summary>
        /// 重置某一规则的计数
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public bool ResetVCount(M_SYS_ALARMRULE rule)
        {
            List<SYS_ALARMSCOPE> list = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_ALARMSCOPE", "Table") == null)
            {
                list = FillCache();
                list = list.Where(c => c.ALID == rule.AL_ID).ToList();
            }
            else
                list = Helper.AppFabricCacheHelper.Instance().GetCacheByTag<SYS_ALARMSCOPE>("SYS_ALARMSCOPE", "ALID" + rule.AL_ID);

            // 规则没有需要检查的SSID
            if (list == null)
                return true;

            string[] tags = { "", "" };
            foreach (var item in list)
            {
                item.VCOUNT = 0;
                tags[0] = "ALID" + item.ALID;
                tags[1] = "SSID" + item.SSID;
                Helper.AppFabricCacheHelper.Instance().SetCache(item.ID.ToString(), "SYS_ALARMSCOPE", item, tags);
            }

            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_ALARMSCOPE SET VCOUNT=0 WHERE ALID =@ALID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ALID",rule.AL_ID)
                    };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        /// <summary>
        /// 重置所有规则的计数
        /// </summary>
        /// <param name="ruleIDs"></param>
        /// <returns></returns>
        public bool ResetVCountAll(List<Int64> ruleIDs)
        {
            bool flag = false;

            List<SYS_ALARMSCOPE> list = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_ALARMSCOPE", "Table") == null)
                list = FillCache();
            else
                list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_ALARMSCOPE>("SYS_ALARMSCOPE");

            list = list.Where(c => ruleIDs.Contains(c.ALID)).ToList();
            
            string[] tags = { "", "" };
            foreach (var item in list)
            {
                item.VCOUNT = 0;
                tags[0] = "ALID" + item.ALID;
                tags[1] = "SSID" + item.SSID;
                Helper.AppFabricCacheHelper.Instance().SetCache(item.ID.ToString(), "SYS_ALARMSCOPE", item, tags);
            }
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_ALARMSCOPE SET VCOUNT=0 WHERE ALID in (" + ruleIDs.ToString(null, ",") + ")";
                flag = mySql.ExecuteSQL(strSql);
            }
            return flag;
        }

        /// <summary>
        /// 将数据库中的数据全部读取到缓存中
        /// </summary>
        /// <returns></returns>
        public static List<SYS_ALARMSCOPE> FillCache()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_ALARMSCOPE> list = null;
                string[] tags = { "", "" };
                string strSql = "select * from SYS_ALARMSCOPE";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ALARMSCOPE");
                if (dt.Rows.Count > 0)
                {
                    list = DataChange<SYS_ALARMSCOPE>.FillModel(dt);
                    foreach (var item in list)
                    {
                        tags[0] = "ALID" + item.ALID;
                        tags[1] = "SSID" + item.SSID;
                        Helper.AppFabricCacheHelper.Instance().AddCache(item.ID.ToString(), "SYS_ALARMSCOPE", item, tags);
                    }
                }
                return list;
            }
        }
    }
}
