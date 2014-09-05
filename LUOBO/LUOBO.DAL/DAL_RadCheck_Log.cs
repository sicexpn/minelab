using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LUOBO.Access;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using System.Data;
using LUOBO.Helper;
namespace LUOBO.DAL
{
    public class DAL_RadCheck_Log
    {
        public bool Insert(RadCheck_Log log)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into radcheck_log(OID,OpenID,UserType,UserMac,CreateTime) value(@OID,@OpenID,@UserType,@UserMac,@CreateTime)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@OID",log.OID),
                    new MySqlParameter("@OpenID",log.OpenID),
                    new MySqlParameter("UserType",log.UserType),
                    new MySqlParameter("UserMac",log.UserMac),
                    new MySqlParameter("CreateTime",log.CreateTime)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
    }
}
