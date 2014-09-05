using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_SYS_USER
    {
        public bool Insert(SYS_USER data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_USER (USERNAME, ACCOUNT, PWD, CREATETIME, OID, CONTACT, USERTYPE, STATE,MAC) VALUES (@USERNAME, @ACCOUNT, @PWD, @CREATETIME, @OID, @CONTACT, @USERTYPE, @STATE, @MAC)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@USERNAME", data.USERNAME),
                    new MySqlParameter("@ACCOUNT", data.ACCOUNT),
                    new MySqlParameter("@PWD", data.PWD),
                    new MySqlParameter("@CREATETIME", data.CREATETIME),
                    new MySqlParameter("@OID", data.OID),
                    new MySqlParameter("@CONTACT", data.CONTACT),
                    new MySqlParameter("@USERTYPE", data.USERTYPE),
                    new MySqlParameter("@STATE", data.STATE),
                    new MySqlParameter("@MAC", data.MAC)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Update(SYS_USER data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_USER SET USERNAME = @USERNAME, ACCOUNT = @ACCOUNT{0}, CONTACT = @CONTACT, USERTYPE = @USERTYPE, TOKEN = @TOKEN, TOKENTIMESTAMP = @TOKENTIMESTAMP, MAC=@MAC WHERE ID = @ID";
                List<MySqlParameter> parms = new List<MySqlParameter>() {
                    new MySqlParameter("@ID", data.ID),
                    new MySqlParameter("@USERNAME", data.USERNAME),
                    new MySqlParameter("@ACCOUNT", data.ACCOUNT),
                    new MySqlParameter("@CONTACT", data.CONTACT),
                    new MySqlParameter("@USERTYPE", data.USERTYPE),
                    new MySqlParameter("@TOKEN", data.TOKEN),
                    new MySqlParameter("@TOKENTIMESTAMP", data.TOKENTIMESTAMP),
                    new MySqlParameter("@MAC", data.MAC)
                };
                if (data.PWD != "" && data.PWD != null)
                {
                    strSql = string.Format(strSql, ", PWD = @PWD");
                    parms.Add(new MySqlParameter("@PWD", data.PWD));
                }
                else
                    strSql = string.Format(strSql, "");
                return mySql.ExecuteSQL(strSql, parms.ToArray());
            }
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_USER WHERE ID = @ID";
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
                string strSql = "DELETE FROM SYS_USER WHERE ID in (" + ids + ")";
                //MySqlParameter[] parms = new MySqlParameter[] {
                //    new MySqlParameter("@ID", ids)
                //};
                return mySql.ExecuteSQL(strSql);
            }
        }

        public bool Disables(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_USER SET STATE = 0 WHERE ID in (" + ids + ")";
                //MySqlParameter[] parms = new MySqlParameter[] {
                //    new MySqlParameter("@ID", ids)
                //};
                return mySql.ExecuteSQL(strSql);
            }
        }

        public List<SYS_USER> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_USER> list = new List<SYS_USER>();
                string strSql = "SELECT * FROM SYS_USER";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER");
                list = DataChange<SYS_USER>.FillModel(dt);
                return list;
            }
        }

        public SYS_USER Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USER data = null;
                string strSql = "SELECT * FROM SYS_USER WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USER>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public bool Select(string ACCOUNT)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT * FROM SYS_USER WHERE ACCOUNT = @ACCOUNT";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ACCOUNT", ACCOUNT)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public List<SYS_USER> SelectByOID(Int64 OID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT * FROM SYS_USER WHERE OID = @OID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@OID", OID)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                return DataChange<SYS_USER>.FillModel(dt);
            }
        }

        public List<SYS_USER> SelectByACCOUNTs(string ACCOUNTs)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT * FROM SYS_USER WHERE ACCOUNT in (" + ACCOUNTs + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER");
                return DataChange<SYS_USER>.FillModel(dt);
            }
        }

        public SYS_USER Select(string ACCOUNT, string PWD)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USER data = null;
                string strSql = "SELECT * FROM SYS_USER WHERE ACCOUNT = @ACCOUNT AND PWD = @PWD";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ACCOUNT", ACCOUNT),
                    new MySqlParameter("@PWD", PWD)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USER>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public SYS_USER Select(string ACCOUNT, string PWD, string MAC)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USER data = null;
                string strSql = "SELECT * FROM SYS_USER WHERE ACCOUNT = @ACCOUNT AND PWD = @PWD AND @MAC=MAC and USERTYPE=@USERTYPE";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ACCOUNT", ACCOUNT),
                    new MySqlParameter("@PWD", PWD),
                    new MySqlParameter("@MAC", MAC),
                    new MySqlParameter("@USERTYPE", (Int32)Helper.CustomEnum.ENUM_User_Type.Install)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USER>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public List<SYS_USER_VIEW> Select(int size, Int64 curPage, string jgName, string userName, Int32 userType)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_USER_VIEW> datas = new List<SYS_USER_VIEW>();
                string strSql = "SELECT t1.ID, t1.USERNAME, t1.ACCOUNT, t1.CONTACT,t1.MAC, t2.NAME ONAME, t3.NAME DNAME FROM SYS_USER t1 LEFT JOIN SYS_ORGANIZATION t2 ON t1.OID = t2.ID";
                strSql += " LEFT JOIN SYS_DICT t3 ON t1.USERTYPE = t3.VALUE";
                strSql += " WHERE t1.STATE = 1 AND t1.ID NOT IN (SELECT ID FROM ({0}) t) AND t3.CATEGORY = '用户类别'";
                if (jgName.Trim() != "")
                    strSql += " AND t2.NAME like '%" + jgName + "%'";
                if (userName.Trim() != "")
                    strSql += " AND t1.USERNAME like '%" + userName + "%'";
                if (userType != -99)
                    strSql += " AND t1.USERTYPE = " + userType;
                strSql += " ORDER BY t1.ID ASC LIMIT " + size;

                string strChildSql = "SELECT t1.ID FROM SYS_USER t1 LEFT JOIN SYS_ORGANIZATION t2 ON t1.OID = t2.ID WHERE t1.STATE = 1";
                if (jgName.Trim() != "")
                    strChildSql += " AND t2.NAME like '%" + jgName + "%'";
                if (userName.Trim() != "")
                    strChildSql += " AND t1.USERNAME like '%" + userName + "%'";
                if (userType != -99)
                    strChildSql += " AND t1.USERTYPE = " + userType;
                strChildSql += " ORDER BY t1.ID ASC LIMIT " + ((curPage - 1) * size);

                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "SYS_USER");
                datas = DataChange<SYS_USER_VIEW>.FillModel(dt);
                return datas;
            }
        }

        public int SelectCount(string jgName, string userName, Int32 userType)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(1) FROM SYS_USER t1 LEFT JOIN SYS_ORGANIZATION t2 ON t1.OID = t2.ID WHERE t1.STATE = 1";
                if (jgName.Trim() != "")
                    strSql += " AND t2.NAME like '%" + jgName + "%'";
                if (userName.Trim() != "")
                    strSql += " AND t1.USERNAME like '%" + userName + "%'";
                if (userType != -99)
                    strSql += " AND t1.USERTYPE = " + userType;
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }

        public SYS_USER SelectByToken(string token)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USER data = null;
                string strSql = "SELECT * FROM SYS_USER WHERE TOKEN = @TOKEN";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@TOKEN", token)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USER>.FillEntity(dt.Rows[0]);

                return data;
            }
        }

        public bool CheckAccount(string account)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT count(1) FROM SYS_USER WHERE ACCOUNT='" + account.Trim() + "'";
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                if (count > 0)
                    return false;
                else
                    return true;
            }
        }



        /// <summary>
        /// 根据MAC返回安装人员信息，无则返回null
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public SYS_USER CheckInstall(string mac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_USER data = null;
                string strSql = "SELECT * FROM SYS_USER WHERE MAC = @MAC AND (UserType=@USERTYPE or UserType=@USERTYPE2)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@MAC", mac),
                    new MySqlParameter("@USERTYPE", (Int32)Helper.CustomEnum.ENUM_User_Type.Install),
                    new MySqlParameter("@USERTYPE2", (Int32)Helper.CustomEnum.ENUM_User_Type.Admin)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_USER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_USER>.FillEntity(dt.Rows[0]);
                return data;
            }
        }
    }
}
