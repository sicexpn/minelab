using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_WEIXIN_AUTH
    {
        public bool Update(WEIXIN_AUTH data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from WEIXIN_AUTH where 1<>1", "WEIXIN_AUTH");
                if (data.ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.WEIXIN_AUTH>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from WEIXIN_AUTH where ID=" + data.ID.ToString(), "WEIXIN_AUTH");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.WEIXIN_AUTH>.FillRow(data, dt.Rows[0]);
                }
                return mySql.Update(dt);
            }
        }

        public WEIXIN_AUTH SelectByOpenID(Int64 oid, string openid)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                WEIXIN_AUTH data = null;
                string strSql = "Select * from WEIXIN_AUTH where oid=@oid and openid=@openid";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@oid",oid),
                    new MySqlParameter("@openid",openid)
                };
                DataTable dt = mySql.GetDataTable(strSql, "WEIXIN_AUTH", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<Entity.WEIXIN_AUTH>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        public bool CheckAuth(Int64 oid, string openid, string pwd)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT * FROM `weixin_auth` where date_add(createtime, interval 5 minute) > now() ";
                strSql += "and oid=@oid and openid=@openid and pwd=@pwd";

                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@oid",oid),
                    new MySqlParameter("@openid",openid),
                    new MySqlParameter("@pwd",pwd)
                };
                DataTable dt = mySql.GetDataTable(strSql, "WEIXIN_AUTH", parms);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
