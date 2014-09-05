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
    public class DAL_AD_FREEHOST
    {
        public bool Update(AD_FREEHOST data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from AD_FREEHOST where 1<>1", "AD_FREEHOST");
                if (data.AF_ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<AD_FREEHOST>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from AD_FREEHOST where ID=" + data.AF_ID.ToString(), "AD_FREEHOST");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<AD_FREEHOST>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public List<AD_FREEHOST> SelectByADID(Int64 ADID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<AD_FREEHOST> list = new List<AD_FREEHOST>();
                AD_FREEHOST data = null;
                string strSql = "select * from ad_freehost where ad_id = @ADID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ADID", ADID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "ad_freehost", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<AD_FREEHOST>.FillEntity(dt.Rows[0]);
                if (data != null )
                {
                    if (!string.IsNullOrEmpty(data.F_Default))
                    {
                        strSql = "select * from ad_freehost where AF_ID in (" + data.F_Default + ")";
                        dt = mySql.GetDataTable(strSql, "ad_freehost");
                        if (dt.Rows.Count > 0)
                            list = DataChange<AD_FREEHOST>.FillModel(dt);
                    }
                    list.Add(data);
                }
                return list;
            }
        }

        public bool Delete(Int64 AF_ID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM AD_FREEHOST WHERE AF_ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", AF_ID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool DeleteByADID(Int64 AD_ID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM AD_FREEHOST WHERE AD_ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", AD_ID)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool UpdateFreeHost(AD_FREEHOST data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("select * from ad_freehost where AD_ID=" + data.AD_ID, "AD_FREEHOST");
                if (dt.Rows.Count > 0)
                {
                    string strSql = "update ad_freehost set F_Host=@F_Host,F_Default=@F_Default where AD_ID=@AD_ID";
                    MySqlParameter[] parms = new MySqlParameter[]{
                        new MySqlParameter("@AD_ID",data.AD_ID),
                        new MySqlParameter("@F_Host",data.F_Host),
                        new MySqlParameter("@F_Domain",data.F_Domain),
                        new MySqlParameter("@F_Default",data.F_Default)
                    };

                    return mySql.ExecuteSQL(strSql, parms);
                }
                else
                {
                    return Insert(data);
                }
            }
        }
        public bool Insert(AD_FREEHOST data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "insert into ad_freehost(AD_ID,F_Host,F_Domain,F_Default) values(@AD_ID,@F_Host,@F_Domain,@F_Default)";
                MySqlParameter[] parms = new MySqlParameter[]{
                    new MySqlParameter("@AD_ID",data.AD_ID),
                    new MySqlParameter("@F_Host",data.F_Host),
                    new MySqlParameter("@F_Domain",data.F_Domain),
                    new MySqlParameter("@F_Default",data.F_Default)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
    }

}
