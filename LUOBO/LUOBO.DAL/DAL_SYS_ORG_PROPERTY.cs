using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using LUOBO.Access;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_SYS_ORG_PROPERTY
    {
        public bool IsExist(Entity.SYS_ORG_PROPERTY orgProp)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "Select * from sys_org_property where PTYPE=@PTYPE and OID=@OID and PNAME=@PNAME";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("PTYPE",orgProp.PTYPE),
                    new MySqlParameter("OID",orgProp.OID),
                    new MySqlParameter("PNAME",orgProp.PNAME)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ORG_PROPTY", parms);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }

        }

        public bool Update(Entity.SYS_ORG_PROPERTY orgLogin)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "Update sys_org_property set PVALUE=@PVALUE where PTYPE=@PTYPE and OID=@OID and PNAME=@PNAME";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("PTYPE",orgLogin.PTYPE),
                    new MySqlParameter("OID",orgLogin.OID),
                    new MySqlParameter("PNAME",orgLogin.PNAME),
                    new MySqlParameter("PVALUE",orgLogin.PVALUE)
                };
                return mySql.ExecuteSQL(strSql, parms);

            }
        }

        public bool Insert(Entity.SYS_ORG_PROPERTY orgLogin)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "Insert into sys_org_property(OID,PTYPE,PNAME,PVALUE) Values(@OID,@PTYPE,@PNAME,@PVALUE)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("PTYPE",orgLogin.PTYPE),
                    new MySqlParameter("OID",orgLogin.OID),
                    new MySqlParameter("PNAME",orgLogin.PNAME),
                    new MySqlParameter("PVALUE",orgLogin.PVALUE)
                };
                return mySql.ExecuteSQL(strSql, parms);

            }
        }

        public List<SYS_ORG_PROPERTY> SelectAllPropsByOID(long id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "select * from sys_org_property where OID=@OID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("OID",id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ORG_PROPERTY", parms);
                List<SYS_ORG_PROPERTY> result = null;
                if (dt.Rows.Count > 0)
                {
                    result = DataChange<SYS_ORG_PROPERTY>.FillModel(dt);
                }
                return result;

            }
        }

        public List<SYS_ORG_PROPERTY> SelectLoginPropsByOID(long id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "SELECT a.ID,a.OID,c.`NAME` as PTYPE,b.PROP_NAME as PNAME,a.PVALUE FROM `sys_org_property` a, sys_dict_extprop b,sys_dict c WHERE a.PTYPE = b.PROP_TYPE and a.PNAME = b.PROP_ID and b.PROP_TYPE = c.`VALUE` and a.OID = @OID and b.PROP_NAME in ('Radius用户名','Radius密码','是否启用','上网时长（分钟）')";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("OID",id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ORG_PROPERTY", parms);
                List<SYS_ORG_PROPERTY> result = null;
                if (dt.Rows.Count > 0)
                {
                    result = DataChange<SYS_ORG_PROPERTY>.FillModel(dt);
                }
                return result;

            }
        }

        public List<SYS_ORG_PROPERTY> SelectWXNameByOID(long id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "SELECT a.ID,a.OID,c.`NAME` as PTYPE,b.PROP_NAME as PNAME,a.PVALUE FROM `sys_org_property` a, sys_dict_extprop b,sys_dict c WHERE a.PTYPE = b.PROP_TYPE and a.PNAME = b.PROP_ID and b.PROP_TYPE = c.`VALUE` and a.OID = @OID and b.PROP_NAME in ('公众号名称')";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("OID",id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ORG_PROPERTY", parms);
                List<SYS_ORG_PROPERTY> result = null;
                if (dt.Rows.Count > 0)
                {
                    result = DataChange<SYS_ORG_PROPERTY>.FillModel(dt);
                }
                return result;

            }
        }

        public SYS_ORG_PROPERTY SelectValueByOIDAndName(long oid, string name)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                String strSql = "SELECT a.ID,a.OID,c.`NAME` as PTYPE,b.PROP_NAME as PNAME,a.PVALUE FROM `sys_org_property` a, sys_dict_extprop b,sys_dict c WHERE a.PTYPE = b.PROP_TYPE and a.PNAME = b.PROP_ID and b.PROP_TYPE = c.`VALUE` and a.OID = @OID and b.PROP_NAME =@NAME";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("OID",oid),
                    new MySqlParameter("NAME",name)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_ORG_PROPERTY", parms);
                SYS_ORG_PROPERTY result = null;
                if (dt.Rows.Count > 0)
                {
                    result = DataChange<SYS_ORG_PROPERTY>.FillEntity(dt.Rows[0]);
                }
                return result;
            }
        }

    }
}
