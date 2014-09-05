using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using System.Data;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using LUOBO.Model;

namespace LUOBO.DAL
{
    public class DAL_SYS_INSTALLPERSON
    {
        public bool Update(SYS_INSTALLPERSON data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_INSTALLPERSON where 1<>1", "SYS_INSTALLPERSON");
                if (data.IP_ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_INSTALLPERSON>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_INSTALLPERSON where ID=" + data.IP_ID.ToString(), "SYS_INSTALLPERSON");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SYS_INSTALLPERSON>.FillRow(data, dt.Rows[0]);
                }
                return mySql.Update(dt);
            }
        }

        /// <summary>
        /// 根据ID获取一条记录
        /// </summary>
        /// <param name="IP_ID"></param>
        /// <returns></returns>
        public SYS_INSTALLPERSON SelectByID(Int64 IP_ID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_INSTALLPERSON data = new SYS_INSTALLPERSON();
                string strSql = "SELECT * FROM SYS_INSTALLPERSON WHERE IP_ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", IP_ID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_INSTALLPERSON", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_INSTALLPERSON>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        /// <summary>
        /// 根据MAC获取一条记录，如果没有返回null
        /// </summary>
        /// <param name="IP_ID"></param>
        /// <returns></returns>
        public SYS_INSTALLPERSON SelectByMAC(Int64 IP_ID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_INSTALLPERSON data = null;
                string strSql = "SELECT * FROM SYS_INSTALLPERSON WHERE IP_ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", IP_ID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_INSTALLPERSON", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_INSTALLPERSON>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        /// <summary>
        /// 检查MAC是否是安装人员
        /// </summary>
        /// <param name="IP_ID"></param>
        /// <returns></returns>
        public SYS_INSTALLPERSON SelectByMAC(string mac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_INSTALLPERSON data = null;
                string strSql = "SELECT * FROM SYS_INSTALLPERSON WHERE IP_MAC = @MAC";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@MAC", mac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_INSTALLPERSON", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_INSTALLPERSON>.FillEntity(dt.Rows[0]);
                return data;
            }
        }
    }
}
