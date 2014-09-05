using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using System.Data;
using LUOBO.Helper;

namespace LUOBO.DAL
{
    public class DAL_BlackList
    {
        public bool Insert(BlackList data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "INSERT INTO BLACKLIST(USERMAC,ENABLE) VALUES(@USERMAC,@ENABLE)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@USERMAC",data.UserMac),
                    new MySqlParameter("@ENABLE",data.Enable)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        /// <summary>
        /// 修改enable字段，enable=1，为黑名单，enable=0，删除黑名单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool ToogleBlackList(BlackList data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "UPDATE BLACKLIST SET ENABLE = @ENABLE WHERE USERMAC = @USERMAC";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@USERMAC",data.UserMac),
                    new MySqlParameter("@ENABLE",data.Enable)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool CheckBlackList(BlackList data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "SELECT COUNT(*) FROM BLACKLIST WHERE USERMAC = @USERMAC";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@USERMAC",data.UserMac)
                };
                Int16 count = Int16.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
                if (count > 0)
                    return true;
                return false;
            }
        }

        public bool CheckIsBlackList(string userMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "SELECT COUNT(*) FROM BLACKLIST WHERE USERMAC = @USERMAC AND ENABLE=@ENABLE";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@USERMAC",userMac),
                    new MySqlParameter("@ENABLE",1)
                };
                Int16 count = Int16.Parse(mySql.GetOnlyOneValue(strSql, parms).ToString());
                if (count > 0)
                    return true;
                return false;
            }
        }
    }
}
