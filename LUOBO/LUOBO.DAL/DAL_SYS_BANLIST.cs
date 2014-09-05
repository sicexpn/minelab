using System;
using System.Collections.Generic;
using System.Linq;
using LUOBO.Access;
using System.Text;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_BANLIST
    {
        public bool Update(SYS_BANLIST data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_BANLIST where 1<>1", "SYS_BANLIST");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_BANLIST>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_BANLIST where ID=" + data.ID.ToString(), "SYS_BANLIST");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SYS_BANLIST>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_BANLIST WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Deletes(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_BANLIST WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public bool DeleteBySSIDID(Int64 SSIDID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_BANLIST WHERE SSIDID = " + SSIDID;
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_BANLIST> SelectByAPID(Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_BANLIST> list = new List<SYS_BANLIST>();
                string strSql = "SELECT * FROM SYS_BANLIST WHERE SSIDID IN(SELECT ID FROM SYS_SSID WHERE APID = @APID)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@APID", APID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SSID", parms);
                list = DataChange<SYS_BANLIST>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_BANLIST> SelectBySSIDID(Int64 SSIDID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_BANLIST> list = new List<SYS_BANLIST>();
                string strSql = "SELECT * FROM SYS_BANLIST WHERE SSIDID = @SSIDID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@SSIDID", SSIDID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_BANLIST", parms);
                list = DataChange<SYS_BANLIST>.FillModel(dt);
                return list;
            }
        }
    }
}
