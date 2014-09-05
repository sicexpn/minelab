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
    public class DAL_SYS_APCONTACT
    {
        public bool Insert(SYS_APCONTACT data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "insert into sys_apcontact(`NAME`, OID, CONTACT, EMAIL, NOTICETYPE, CREATETIME) VALUES(@NAME, @OID, @CONTACT, @EMAIL, @NOTICETYPE, @CREATETIME)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@NAME",data.NAME),
                    new MySqlParameter("@OID",data.OID),
                    new MySqlParameter("@CONTACT",data.CONTACT),
                    new MySqlParameter("@EMAIL",data.EMAIL),
                    new MySqlParameter("@NOTICETYPE",data.NOTICETYPE),
                    new MySqlParameter("@CREATETIME",data.CREATETIME)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Update(SYS_APCONTACT data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_APCONTACT where 1<>1", "SYS_APCONTACT");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_APCONTACT>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_APCONTACT where ID=" + data.ID.ToString(), "SYS_APCONTACT");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.SYS_APCONTACT>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_APCONTACT WHERE ID = @ID";
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
                string strSql = "DELETE FROM SYS_APCONTACT WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_APCONTACT> SelectByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APCONTACT> list = new List<SYS_APCONTACT>();
                string strSql = "";
                strSql += "select REPLACE(PIDHELP,'$','') INTO @sql1 from sys_organization where id = "+OID+";";
                strSql += "set @sql1 = CONCAT('select a.*,if(OID=" + OID + ",true,false)as ISOWNORG,b.NAME AS ONAME from sys_apcontact a, sys_organization b where a.OID=b.ID and (oid in (',@sql1,') or oid = " + OID + ")');";
                strSql += "prepare s1 from @sql1;";
                strSql += "execute s1;";
                strSql += "deallocate prepare s1;";

                DataTable dt = mySql.GetDataTable(strSql, "SYS_APCONTACT");
                list = DataChange<SYS_APCONTACT>.FillModel(dt);
                return list;
            }
        }

        public List<SYS_APCONTACT> SelectByLogID(Int64 LogID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_APCONTACT> list = new List<SYS_APCONTACT>();
                string strSql = "select * from sys_apcontact where apid = (select ID from sys_apdevice where mac=(select AP_MAC from sys_log_alert where LOG_ID = @LOGID))";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@LOGID",LogID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APCONTACT", parms);
                if (dt.Rows != null && dt.Rows.Count > 0)
                    list = DataChange<SYS_APCONTACT>.FillModel(dt);
                return list;
            }
        }
    }
}
