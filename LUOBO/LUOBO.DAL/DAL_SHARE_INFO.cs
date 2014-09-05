using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using LUOBO.Access;
using LUOBO.Entity;

namespace LUOBO.DAL
{
    public class DAL_SHARE_INFO
    {
        public bool Update(SHARE_INFO data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                DataTable dt = mySql.GetDataTable("Select * from SHARE_INFO where 1<>1", "SHARE_INFO");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SHARE_INFO>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SHARE_INFO where ID=" + data.ID.ToString(), "SHARE_INFO");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SHARE_INFO>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "DELETE FROM SHARE_INFO WHERE ID = @ID ";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Deletes(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "DELETE FROM SHARE_INFO WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public SHARE_INFO Select(string SESSION, Int64 SSID, Int64 OID, Int64 ADID, string PATH, string SHARETYPE)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                SHARE_INFO data = null;
                string strSql = "SELECT * FROM SHARE_INFO WHERE SESSION = @SESSION AND SSID = @SSID AND OID = @OID AND ADID = @ADID AND PATH = @PATH AND SHARETYPE = @SHARETYPE";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@SESSION", SESSION),
                    new MySqlParameter("@SSID", SSID),
                    new MySqlParameter("@OID", OID),
                    new MySqlParameter("@ADID", ADID),
                    new MySqlParameter("@PATH", PATH),
                    new MySqlParameter("@SHARETYPE", SHARETYPE)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SHARE_INFO", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SHARE_INFO>.FillEntity(dt.Rows[0]);

                return data;
            }
        }
    }
}
