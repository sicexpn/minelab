using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using System.Transactions;
using LUOBO.Helper;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_RadGroupReply
    {
        public bool Insert(RadGroupReply radGroupReply)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "insert into radGroupReply(GroupName,Attribute,Value) value(@GroupName,@Attribute,@Value)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@GroupName",radGroupReply.GroupName),
                new MySqlParameter("@Attribute",radGroupReply.Attribute),
                new MySqlParameter("@Value",radGroupReply.Value)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool DeleteByGroupName(string groupName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "DELETE FROM radGroupReply WHERE GroupName=@GroupName";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@GroupName",groupName)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }
        public bool Insert(List<RadGroupReply> lists)
        {

            using (TransactionScope scope = new TransactionScope())
            {
                bool flag = false;
                try
                {
                    foreach (RadGroupReply data in lists)
                    {
                        flag = Insert(data);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
                return flag;
            }
        }
        public Int64 GetTopTrafficByUser(string userName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select value from radgroupreply as t1,userGroup as t2 where t1.GroupName=t2.GroupName " +
                    "and Attribute=@Attribute and t2.userName=@UserName LIMIT 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userName),
                new MySqlParameter("@Attribute","ChilliSpot-Max-Total-Octets"),
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public Int64 GetTopSessionTimeByUser(string userName)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select  value from radgroupreply as t1,userGroup as t2 where t1.GroupName=t2.GroupName " +
                    "and Attribute=@Attribute and t2.userName=@UserName LIMIT 0,1";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@UserName",userName),
                new MySqlParameter("@Attribute","Session-Timeout"),
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }

        public bool UpdateGroupAttr(RadGroupReply radGroupReply)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess(CustomEnum.ENUM_SqlConn.Radius))
            {
                string strSql = "select count(1) from radGroupReply where Attribute=@Attribute and GroupName=@GroupName";
                MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@GroupName",radGroupReply.GroupName),
                new MySqlParameter("@Attribute",radGroupReply.Attribute),
                new MySqlParameter("@Value",radGroupReply.Value)
                };
                //DataTable dt = mySql.GetDataTable(strSql, "radGroupReply", parms);
                if (Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms)) > 0)
                {
                    string strSqlupdate = "update radGroupReply set Value=@Value where Attribute=@Attribute and GroupName=@GroupName";
                    return mySql.ExecuteSQL(strSqlupdate, parms);
                }
                else
                {
                    string strSqlinsert = "insert into radGroupReply(GroupName,Attribute,Value) value(@GroupName,@Attribute,@Value)";
                    return mySql.ExecuteSQL(strSqlinsert, parms);
                }
            }
        }
    }
}
