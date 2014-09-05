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
    public class DAL_SHARE
    {
        public bool Update(SHARE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                DataTable dt = mySql.GetDataTable("Select * from SHARE where 1<>1", "SHARE");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SHARE>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SHARE where ID=" + data.ID.ToString(), "SHARE");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SHARE>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                string strSql = "DELETE FROM SHARE WHERE ID = @ID ";
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
                string strSql = "DELETE FROM SHARE WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public SHARE Select(Int64 SSID, Int64 OID, Int64 ADID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                SHARE data = null;
                string strSql = "SELECT * FROM SHARE WHERE SSID = @SSID AND OID = @OID AND ADID = @ADID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@SSID", SSID),
                    new MySqlParameter("@OID", OID),
                    new MySqlParameter("@ADID", ADID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SHARE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SHARE>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public SHARE SelectByID(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(Helper.CustomEnum.ENUM_SqlConn.Statistical))
            {
                SHARE data = null;
                string strSql = "SELECT * FROM SHARE WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SHARE", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SHARE>.FillEntity(dt.Rows[0]);

                return data;
            }
        }
    }
}
