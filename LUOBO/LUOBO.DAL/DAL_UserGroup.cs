using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    public class DAL_UserGroup
    {
        public bool Insert(UserGroup userGroup)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into usergroup(UserName,GroupName,Priority) value(@UserName,@GroupName,@Priority)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userGroup.UserName),
                new MySqlParameter("@GroupName",userGroup.GroupName),
                new MySqlParameter("@Priority",userGroup.Priority)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }


        public bool Update(UserGroup userGroup)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into usergroup(UserName,GroupName,Priority) value(@UserName,@GroupName,@Priority)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userGroup.UserName),
                new MySqlParameter("@GroupName",userGroup.GroupName),
                new MySqlParameter("@Priority",userGroup.Priority)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool UpdateUserGroup(UserGroup userGroup)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select count(1) from usergroup where UserName=@UserName and GroupName=@GroupName and Priority=@Priority";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userGroup.UserName),
                new MySqlParameter("@GroupName",userGroup.GroupName),
                new MySqlParameter("@Priority",userGroup.Priority)
                };
                if (Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms)) > 0)
                {
                    return true;
                }
                else
                {
                    string strSqlInsert = "insert into usergroup(UserName,GroupName,Priority) value(@UserName,@GroupName,@Priority)";
                    return mySql.ExecuteSQL(strSqlInsert, parms);
                }
            }
        }
    }
}
