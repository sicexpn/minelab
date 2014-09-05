using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_PROBEORG
    {
        public bool Insert(SYS_PROBEORG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_PROBEORG(PROVINCE,CITY, CATEGORY, CONTACT, CONTACTER,COUNTIES, ISVERIFY, NAME,DESCRIPTION, PID,PIDHELP, STATE,ISVERIFY_END) VALUES(@PROVINCE, @CITY, @CATEGORY, @CONTACT, @CONTACTER, @COUNTIES, @ISVERIFY, @NAME,@DESCRIPTION, @PID,@PIDHELP, @STATE, @ISVERIFY_END)";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@PROVINCE",data.PROVINCE),
                    new MySqlParameter("@CITY",data.CITY),
                    new MySqlParameter("@CATEGORY",data.CATEGORY),
                    new MySqlParameter("@CONTACT",data.CONTACT),
                    new MySqlParameter("@CONTACTER",data.CONTACTER),
                    new MySqlParameter("@COUNTIES",data.COUNTIES),
                    new MySqlParameter("@ISVERIFY",data.ISVERIFY),
                    new MySqlParameter("@NAME",data.NAME),
                    new MySqlParameter("@DESCRIPTION",data.DESCRIPTION),
                    new MySqlParameter("@PID",data.PID),
                    new MySqlParameter("@PIDHELP",data.PIDHELP),
                    new MySqlParameter("@STATE",data.STATE),
                    new MySqlParameter("@ISVERIFY_END",data.ISVERIFY_END)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Inserts(List<SYS_PROBEORG> datas)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                bool flag = false;
                try
                {
                    foreach (SYS_PROBEORG data in datas)
                    {
                        flag = Insert(data);
                    }
                }
                catch (Exception ex)
                {
                }

                return flag;
            }
        }

        public bool Update(SYS_PROBEORG data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE SYS_PROBEORG SET PROVINCE = @PROVINCE,CITY = @CITY, CATEGORY = @CATEGORY,CONTACT = @CONTACT, CONTACTER = @CONTACTER, COUNTIES = @COUNTIES,  NAME = @NAME,DESCRIPTION = @DESCRIPTION, PID = @PID, ISVERIFY_END=@ISVERIFY_END WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",data.ID),
                    new MySqlParameter("@PROVINCE",data.PROVINCE),
                    new MySqlParameter("@CITY",data.CITY),
                    new MySqlParameter("@CATEGORY",data.CATEGORY),
                    new MySqlParameter("@CONTACT",data.CONTACT),
                    new MySqlParameter("@CONTACTER",data.CONTACTER),
                    new MySqlParameter("@COUNTIES",data.COUNTIES),
                    new MySqlParameter("@NAME",data.NAME),
                    new MySqlParameter("@DESCRIPTION",data.DESCRIPTION),
                    new MySqlParameter("@ISVERIFY_END",data.ISVERIFY_END)
                    
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Updates(List<SYS_PROBEORG> datas)
        {
            return false;
        }

        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM SYS_PROBEORG WHERE ID = @ID";
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
                string strSql = "DELETE FROM SYS_PROBEORG WHERE ID IN (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }
        public List<SYS_PROBEORG> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_PROBEORG> list = new List<SYS_PROBEORG>();
                string strSql = "SELECT * FROM SYS_PROBEORG";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG");
                list = DataChange<SYS_PROBEORG>.FillModel(dt);
                return list;
            }
        }

        public SYS_PROBEORG Select(int size, Int64 curPage, Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_PROBEORG data = null;
                string strSql = "SELECT * FROM SYS_PROBEORG WHERE ID = @ID LIMIT " + ((curPage - 1) * size).ToString() + "," + size.ToString();
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_PROBEORG>.FillEntity(dt.Rows[0]);
                return data;
            }
        }
        public List<SYS_PROBEORG> Select(int size, Int64 curPage, string orgName, string province, string city, string country)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_PROBEORG> datas = new List<SYS_PROBEORG>();

                //string strSql = "SELECT org.ID, org.NAME, org.DESCRIPTION, org.CITY, org.COUNTIES,dict.NAME CATEGORYNAME FROM SYS_PROBEORG org,SYS_DICT dict WHERE  dict.VALUE = org.CATEGORY AND dict.CATEGORY='机构类别' ";
                string strSql = "SELECT org.*,dict.NAME CATEGORYNAME FROM SYS_PROBEORG org,SYS_DICT dict WHERE  dict.VALUE = org.CATEGORY AND dict.CATEGORY='机构类别' ";

                if (orgName.Trim() != "" && orgName.Trim() != "null")
                    strSql += " AND NAME LIKE '%" + orgName + "%'";
                if (province.Trim() != "" && province.Trim() != "null")
                    strSql += " AND PROVINCE LIKE '%" + province + "%'";
                if (city.Trim() != "" && city.Trim() != "null")
                    strSql += " AND CITY LIKE '%" + city + "%'";
                if (country.Trim() != "" && country.Trim() != "null")
                    strSql += " AND COUNTIES LIKE '%" + country + "%'";

                strSql += " LIMIT " + ((curPage - 1) * size).ToString() + "," + size.ToString();
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG");
                datas = DataChange<SYS_PROBEORG>.FillModel(dt);
                return datas;
            }
        }
        #region 根据机构ID选择授权的应用
        public List<SYS_APPLICATION> SelectAppsAuth(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT app.ID,app.APPLICATIONNAME FROM SYS_APPLICATION app WHERE app.ID IN ( SELECT APPID FROM SYS_ORGAPP WHERE ORGID=" + id + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPLICATION");

                List<SYS_APPLICATION> appAuths = new List<SYS_APPLICATION>();
                appAuths = DataChange<SYS_APPLICATION>.FillModel(dt);
                return appAuths;
            }
        }
        public List<SYS_APPLICATION> SelectAppsNoAuth(int id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT app.ID,app.APPLICATIONNAME FROM SYS_APPLICATION app WHERE app.ID NOT IN ( SELECT APPID FROM SYS_ORGAPP WHERE ORGID=" + id + ")";
                DataTable dt = mySql.GetDataTable(strSql, "SYS_APPLICATION");

                List<SYS_APPLICATION> appAuths = new List<SYS_APPLICATION>();
                appAuths = DataChange<SYS_APPLICATION>.FillModel(dt);
                return appAuths;
            }
        }
        #endregion



        public List<SYS_PROBEORG> Select(int size, long curPage)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_PROBEORG> list = new List<SYS_PROBEORG>();
                string strSql = "SELECT * FROM SYS_PROBEORG LIMIT " + ((curPage - 1) * size).ToString() + "," + size.ToString();
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG");
                list = DataChange<SYS_PROBEORG>.FillModel(dt);
                return list;
            }
        }

        public SYS_PROBEORG Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_PROBEORG data = null;
                string strSql = "SELECT * FROM SYS_PROBEORG WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ID",id)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_PROBEORG>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        public int Insert(SYS_PROBEORG data, int p)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO SYS_PROBEORG(PROVINCE,CITY, CATEGORY, CONTACT, CONTACTER,COUNTIES, ISVERIFY, NAME,DESCRIPTION, PID,PIDHELP, STATE, ISVERIFY_END) VALUES(@PROVINCE,@CITY, @CATEGORY, @CONTACT, @CONTACTER, @COUNTIES, @ISVERIFY, @NAME,@DESCRIPTION, @PID,@PIDHELP, @STATE, @ISVERIFY_END)";
                strSql += ";SELECT LAST_INSERT_ID()";
                string result;
                MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@PROVINCE",data.PROVINCE),
                    new MySqlParameter("@CITY",data.CITY),
                    new MySqlParameter("@CATEGORY",data.CATEGORY),
                    new MySqlParameter("@CONTACT",data.CONTACT),
                    new MySqlParameter("@CONTACTER",data.CONTACTER),
                    new MySqlParameter("@COUNTIES",data.COUNTIES),
                    new MySqlParameter("@ISVERIFY",data.ISVERIFY),
                    new MySqlParameter("@NAME",data.NAME),
                    new MySqlParameter("@DESCRIPTION",data.DESCRIPTION),
                    new MySqlParameter("@PID",data.PID),
                    new MySqlParameter("@PIDHELP",data.PIDHELP),
                    new MySqlParameter("@STATE",data.STATE),
                    new MySqlParameter("@ISVERIFY_END",data.ISVERIFY_END)
                };
                result = mySql.GetOnlyOneValue(strSql, parms).ToString();
                //DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG", parms);
                //if (dt.Rows.Count > 0)
                //    result = dt.Rows[0].ToString();//to be test
                return Int32.Parse(result);
            }
        }


        public List<SYS_PROBEORG> SelectSub(Int64 jgID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<SYS_PROBEORG> list = new List<SYS_PROBEORG>();
                string strSql = "select * from SYS_PROBEORG where PIDHELP like '%$" + jgID + "$%' or id=" + jgID;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG");
                list = DataChange<SYS_PROBEORG>.FillModel(dt);
                return list;
            }
        }
        public List<Int64> SelectSubOrgIDs(Int64 jgID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "select ID from SYS_PROBEORG where PIDHELP like '%$" + jgID + "$%' or id=" + jgID;
                DataTable dt = mySql.GetDataTable(strSql, "SYS_PROBEORG");
                List<Int64> list = new List<Int64>();
                if (dt.Rows.Count > 0)
                {
                    var dslp = from d in dt.AsEnumerable() select d;
                    foreach (var res in dslp)
                    {
                        list.Add(Convert.ToInt64(res.Field<Int64>("ID")));
                    }
                }
                return list;
            }
        }

        public int SelectCounts(string orgName, string province, string city, string country)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT IFNULL(COUNT(*),0) FROM SYS_PROBEORG WHERE 1=1 ";

                if (orgName.Trim() != "" && orgName.Trim() != "null")
                    strSql += " AND NAME LIKE '%" + orgName + "%'";
                if (province.Trim() != "" && province.Trim() != "null")
                    strSql += " AND PROVINCE LIKE '%" + province + "%'";
                if (city.Trim() != "" && city.Trim() != "null")
                    strSql += " AND CITY LIKE '%" + city + "%'";
                if (country.Trim() != "" && country.Trim() != "null")
                    strSql += " AND COUNTIES LIKE '%" + country + "%'";
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }
    }
}
