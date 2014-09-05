using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_SYS_ALARMRULE
    {
        public bool Update(SYS_ALARMRULE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_ALARMRULE where 1<>1", "SYS_APDEVICE");
                if (data.AL_ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_ALARMRULE>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_ALARMRULE where ID=" + data.AL_ID.ToString(), "SYS_APDEVICE");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SYS_ALARMRULE>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public List<SYS_ALARMRULE> Select()
        {
            List<SYS_ALARMRULE> list = null;
            if (Helper.AppFabricCacheHelper.Instance().GetOneCache("SYS_ALARMRULE", "Table") == null)
                list = FillCache();
            else
                list = Helper.AppFabricCacheHelper.Instance().GetRegionCache<SYS_ALARMRULE>("SYS_ALARMRULE");

            return list;
        }

        /// <summary>
        /// 将数据库中的数据全部读取到缓存中
        /// </summary>
        /// <returns></returns>
        public static List<SYS_ALARMRULE> FillCache()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_ALARMRULE> list = null;
                string strSql = "Select * FROM SYS_ALARMRULE";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ALARMRULE");
                if (dt.Rows.Count > 0)
                {
                    list = DataChange<SYS_ALARMRULE>.FillModel(dt);
                    list.ForEach(c => Helper.AppFabricCacheHelper.Instance().AddCache(c.AL_ID.ToString(), "SYS_ALARMRULE", c));
                }
                return list;
            }
        }
    }
}
