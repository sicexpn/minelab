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
    public class DAL_RadCheck
    {
        /*public bool Insert(OpenSSID data, string password)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "insert into radcheck(UserName,Attribute,Value,UserType) value(@UserName,@Attribute,@Attribute,@UserType)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@UserName",data.UserName),
                    new MySqlParameter("@Attribute","User-Password"),
                    new MySqlParameter("@Value",password),
                    new MySqlParameter("@UserType",data.UserType)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        */
        public int Select(string userName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select count(*) from radcheck where UserName='" + userName + "'";
                return Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
            }
        }

        public int Select(UserLogin userLogin)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select COUNT(*) from radcheck where UserName=@UserName and Attribute=@Attribute and UserType=@UserType";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userLogin.UserName),
                new MySqlParameter("@Attribute","User-Password"),
                new MySqlParameter("@UserType",userLogin.UserType)
                };
                object tmp = mySql.GetOnlyOneValue(strSql, parms);
                return Int32.Parse(tmp.ToString());
            }
        }
        /// <summary>
        /// 根据用户名、User-Password、UserType返回密码
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns>password</returns>
        public string SelectPassword(UserLogin userLogin)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                RadCheck data = null;
                string strSql = "select Value from radcheck where UserName=@UserName and Attribute=@Attribute and UserType=@UserType";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userLogin.UserName),
                new MySqlParameter("@Attribute","User-Password"),
                new MySqlParameter("@UserType",userLogin.UserType)
                };
                DataTable dt = mySql.GetDataTable(strSql, "radcheck", parms);
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<RadCheck>.FillEntity(dt.Rows[0]);
                }
                return data.Value;
            }
        }

        public bool Insert(UserLogin userLogin, string password)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into radcheck(UserName,Attribute,Value,UserType) value(@UserName,@Attribute,@Value,@UserType)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@UserName",userLogin.UserName),
                    new MySqlParameter("@Attribute","User-Password"),
                    new MySqlParameter("@Value",password),
                    new MySqlParameter("@UserType",userLogin.UserType)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public int Select(Model.M_RadiusUser user)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select COUNT(*) from radcheck where UserName=@UserName and Attribute=@Attribute and UserType=@UserType";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",user.UserName),
                new MySqlParameter("@Attribute","User-Password"),
                new MySqlParameter("@UserType",user.UserType)
                };
                object tmp = mySql.GetOnlyOneValue(strSql, parms);
                return Int32.Parse(tmp.ToString());
            }
        }

        public string SelectPassword(Model.M_RadiusUser user)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                RadCheck data = null;
                string strSql = "select Value from radcheck where UserName=@UserName and Attribute=@Attribute and UserType=@UserType";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",user.UserName),
                new MySqlParameter("@Attribute","User-Password"),
                new MySqlParameter("@UserType",user.UserType)
                };
                DataTable dt = mySql.GetDataTable(strSql, "radcheck", parms);
                if (dt.Rows.Count > 0)
                {
                    data = DataChange<RadCheck>.FillEntity(dt.Rows[0]);
                }
                return data.Value;
            }
        }

        public bool Insert(Model.M_RadiusUser user, string userName, string password)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into radcheck(UserName,Attribute,Value,UserType) value(@UserName,@Attribute,@Value,@UserType)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@UserName",userName),
                    new MySqlParameter("@Attribute","User-Password"),
                    new MySqlParameter("@Value",password),
                    new MySqlParameter("@UserType",user.UserType)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool UpdateFreeUser(RadCheck userCheck)
        {
            if (IsExist(userCheck))
            {
                return UpdateAuth(userCheck);
            }
            else
            {
                return InsertAuth(userCheck);
            }
        }

        private bool InsertAuth(RadCheck userCheck)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into radcheck(UserName,Attribute,Value,UserType) value(@UserName,@Attribute,@Value,@UserType)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@UserName",userCheck.UserName),
                    new MySqlParameter("@Attribute",userCheck.Attribute),
                    new MySqlParameter("@Value",userCheck.Value),
                    new MySqlParameter("@UserType",userCheck.UserType)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        private bool UpdateAuth(RadCheck userCheck)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "update radcheck set Value=@Value where UserName=@UserName and Attribute=@Attribute and UserType=@UserType";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@UserName",userCheck.UserName),
                    new MySqlParameter("@Attribute",userCheck.Attribute),
                    new MySqlParameter("@Value",userCheck.Value),
                    new MySqlParameter("@UserType",userCheck.UserType)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool IsExist(RadCheck userCheck)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {

                string strSql = "select count(1) from radcheck where UserName=@UserName and Attribute=@Attribute and UserType=@UserType";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userCheck.UserName),
                new MySqlParameter("@Attribute",userCheck.Attribute),
                new MySqlParameter("@UserType",userCheck.UserType)
                };
                if (Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms)) > 0)
                {
                    return true;
                }
                return false;
            }
        }
        //更新用户密码
        public bool UpdateAuthUser(RadCheck userCheck)
        {
            if (IsExist(userCheck))
            {

                return UpdateAuth(userCheck);
            }
            else
            {
                return InsertAuth(userCheck);
            }

        }
        public RadiusAuthResult GetAuthUser(RadCheck userCheck)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                RadiusAuthResult result = null;
                string strSql = "select UserName,Value as PassWord from radcheck where UserName=@UserName and Attribute=@Attribute and UserType=@UserType Limit 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userCheck.UserName),
                new MySqlParameter("@Attribute",userCheck.Attribute),
                new MySqlParameter("@UserType",userCheck.UserType)
                };
                DataTable dt = mySql.GetDataTable(strSql, "radcheck", parms);
                if (dt.Rows.Count > 0)
                {
                    result = DataChange<RadiusAuthResult>.FillEntity(dt.Rows[0]);
                }
                return result;
            }
        }

        public RadiusAuthResult GetAuthUser(string userName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                RadiusAuthResult result = new RadiusAuthResult();
                string strSql = "select UserName,Value as PassWord from radcheck where UserName=@UserName and Attribute=@Attribute Limit 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userName),
                new MySqlParameter("@Attribute","User-Password"),
                };
                DataTable dt = mySql.GetDataTable(strSql, "radcheck", parms);
                if (dt.Rows.Count > 0)
                {
                    result = DataChange<RadiusAuthResult>.FillEntity(dt.Rows[0]);
                }
                //result.UserName = "";
                //result.PassWord = "";
                return result;
            }
        }
    }
}
