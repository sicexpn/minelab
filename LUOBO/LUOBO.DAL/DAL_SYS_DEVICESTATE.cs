using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using System.Data;
using MySql.Data.MySqlClient;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    class DAL_SYS_DEVICESTATE
    {
        /// <summary>
        /// 单条更新数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(SYS_DEVICESTATE data)
        {
            List<SYS_DEVICESTATE> list = Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") as List<SYS_DEVICESTATE>;
            if (list == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {
                    string strSql = "select * from SYS_DEVICESTATE";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_DEVICESTATE");
                    if (dt.Rows.Count > 0)
                        list = DataChange<SYS_DEVICESTATE>.FillModel(dt);
                    Helper.CacheHelper.Instance().SetCache("SYS_DEVICESTATE", list);
                }
            }
            
            list.Add(data);

            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_DEVICESTATE where 1<>1", "SYS_DEVICESTATE");
                if (data.ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<SYS_DEVICESTATE>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_DEVICESTATE where ID=" + data.ID.ToString(), "SYS_DEVICESTATE");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<SYS_DEVICESTATE>.FillRow(data, dt.Rows[0]);
                }
                return mySql.Update(dt);
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool Inserts(List<SYS_DEVICESTATE> datas)
        {
            List<SYS_DEVICESTATE> list = new List<SYS_DEVICESTATE>();
            if (Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {

                    string strSql = "select * from SYS_DEVICESTATE";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_DEVICESTATE");
                    if (dt.Rows.Count > 0)
                        list = DataChange<SYS_DEVICESTATE>.FillModel(dt);
                    Helper.CacheHelper.Instance().SetCache("SYS_DEVICESTATE", list);
                }
            }
            else
                list = Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") as List<SYS_DEVICESTATE>;
            list.AddRange(datas);

            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_DEVICESTATE where 1<>1", "AD_FREEHOST");
                foreach (var data in datas)
                {
                    if (data.ID < 0)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(DataChange<SYS_DEVICESTATE>.FillRow(data, dr));
                    }
                }
                return mySql.Update(dt);
            }
        }

        /// <summary>
        /// 删除全部数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_DEVICESTATE";
                return mySql.ExecuteSQL(strSql);
            }
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        public List<SYS_DEVICESTATE> SelectAll()
        {
            List<SYS_DEVICESTATE> list = Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") as List<SYS_DEVICESTATE>;
            if (list == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {
                    string strSql = "select * from SYS_DEVICESTATE";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_DEVICESTATE");
                    if (dt.Rows.Count > 0)
                        list = DataChange<SYS_DEVICESTATE>.FillModel(dt);
                    Helper.CacheHelper.Instance().SetCache("SYS_DEVICESTATE", list);
                    return list;
                }
            }
            else
                list = Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") as List<SYS_DEVICESTATE>;
            return list;
        }

        /// <summary>
        /// 更新VCOUNT和访问时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateCountAddOne(OpenSSID data)
        {
            List<SYS_DEVICESTATE> list = Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") as List<SYS_DEVICESTATE>;
            if (list == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {
                    string strSql = "select * from SYS_DEVICESTATE";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_DEVICESTATE");
                    if (dt.Rows.Count > 0)
                        list = DataChange<SYS_DEVICESTATE>.FillModel(dt);
                    Helper.CacheHelper.Instance().SetCache("SYS_DEVICESTATE", list);
                }
            }
            SYS_DEVICESTATE tmp = list.Where(c => c.SSID == Convert.ToInt64(data.SSID)).FirstOrDefault();
            if (tmp != null)
            {
                tmp.VCOUNT += 1;
                tmp.CURRENTTIME = data.CurrentTime;
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {
                    string strSql = "UPDATE SYS_DEVICESTATE SET VCOUNT=VCOUNT+1,CURRENTTIME=@CURTIME WHERE SSID=@SSID";
                    MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@CURTIME",data.CurrentTime),
                    new MySqlParameter("@SSID",Convert.ToInt64(data.SSID))
                    };
                    return mySql.ExecuteSQL(strSql, parms);
                }
            }
            return false;
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        public SYS_DEVICESTATE SelectBySSID(Int64 SSID)
        {
            List<SYS_DEVICESTATE> list = Helper.CacheHelper.Instance().GetCache("SYS_DEVICESTATE") as List<SYS_DEVICESTATE>;
            if (list == null)
            {
                using (MySQLDataAccess mySql = new MySQLDataAccess())
                {

                    string strSql = "select * from SYS_DEVICESTATE";
                    DataTable dt = mySql.GetDataTable(strSql, "SYS_DEVICESTATE");
                    if (dt.Rows.Count > 0)
                        list = DataChange<SYS_DEVICESTATE>.FillModel(dt);
                    Helper.CacheHelper.Instance().SetCache("SYS_DEVICESTATE", list);
                    return list.Where(c => c.SSID == SSID).ToList().FirstOrDefault();
                }
            }
            return list.Where(c => c.SSID == SSID).ToList().FirstOrDefault();
        }

        public bool ResetVCount(List<SYS_DEVICESTATE> datas)
        {
            datas.ForEach(c => c.VCOUNT = 0);

            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_DEVICESTATE SET VCOUNT=0 WHERE ID in(" + datas.ToString("ID", ",") + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }


        public List<SYS_DEVICESTATE> SelectBySSID(string ssids)
        {
            List<SYS_DEVICESTATE> list = new List<SYS_DEVICESTATE>();
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "select * from SYS_DEVICESTATE where SSID in (" + ssids + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_DEVICESTATE");
                if (dt.Rows.Count > 0)
                    list = DataChange<SYS_DEVICESTATE>.FillModel(dt);
            }
            return list;
        }
    }

}
