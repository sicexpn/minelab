using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using LUOBO.Entity;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace LUOBO.DAL
{
    public class DAL_APCONFIGTEMPLATE
    {
        public bool Insert(APCONFIGTEMPLATE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "INSERT INTO APCONFIGTEMPLATE VALUES (_nextval('ID'), @TNAME, @FIRMWARE, @VERSION, @DESCRIPTION, @UPDATETIME, @CONTENT,0)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@TNAME", data.TNAME),
                    new MySqlParameter("@DESCRIPTION", data.DESCRIPTION),
                    new MySqlParameter("@FIRMWARE", data.FIRMWARE),
                    new MySqlParameter("@VERSION", data.VERSION),
                    new MySqlParameter("@UPDATETIME", data.UPDATETIME),
                    new MySqlParameter("@CONTENT", data.CONTENT)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        public bool Update(APCONFIGTEMPLATE data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from APCONFIGTEMPLATE where 1<>1", "APCONFIGTEMPLATE");
                if (data.ID < 0)
                {
                    //data.ID = getSequence();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.APCONFIGTEMPLATE>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from APCONFIGTEMPLATE where ID=" + data.ID.ToString(), "APCONFIGTEMPLATE");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<Entity.APCONFIGTEMPLATE>.FillRow(data, dt.Rows[0]);
                }
                return mySql.Update(dt);
            }
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM APCONFIGTEMPLATE WHERE ID = @ID";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", id)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Deletes(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "DELETE FROM APCONFIGTEMPLATE WHERE ID in (@ID)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@ID", ids)
                };
                return mySql.ExecuteSQL(strSql, parms);
            }
        }

        /// <summary>
        /// 禁用多条记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Disables(string ids)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "UPDATE APCONFIGTEMPLATE SET ISDELETE = 1 WHERE ID in (" + ids + ")";
                return mySql.ExecuteSQL(strSql);
            }
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<APCONFIGTEMPLATE> Select()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<APCONFIGTEMPLATE> list = new List<APCONFIGTEMPLATE>();
                string strSql = "SELECT * FROM APCONFIGTEMPLATE";
                DataTable dt = mySql.GetDataTable(strSql, "APCONFIGTEMPLATE");
                list = DataChange<APCONFIGTEMPLATE>.FillModel(dt);
                return list;
            }
        }

        /// <summary>
        /// 查询APCTSimple列表
        /// </summary>
        /// <returns></returns>
        public List<APCONFIGTEMPLATE> SelectAPCTSimple()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<APCONFIGTEMPLATE> list = new List<APCONFIGTEMPLATE>();
                string strSql = "SELECT ID, TNAME FROM APCONFIGTEMPLATE";
                DataTable dt = mySql.GetDataTable(strSql, "APCONFIGTEMPLATE");
                list = DataChange<APCONFIGTEMPLATE>.FillModel(dt);
                return list;
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APCONFIGTEMPLATE Select(Int64 id)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                APCONFIGTEMPLATE data = null;
                string strSql = "SELECT * FROM APCONFIGTEMPLATE WHERE ID = " + id;
                DataTable dt = mySql.GetDataTable(strSql, "APCONFIGTEMPLATE");
                if (dt.Rows.Count > 0)
                    data = DataChange<APCONFIGTEMPLATE>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="maxID"></param>
        /// <param name="jgName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<APCONFIGTEMPLATE> Select(int size, int curPage)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                List<APCONFIGTEMPLATE> datas = new List<APCONFIGTEMPLATE>();
                string strSql = "SELECT * FROM APCONFIGTEMPLATE";
                strSql += " WHERE ID NOT IN (SELECT ID FROM ({0}) t) AND ISDELETE = 0";
                strSql += " ORDER BY ID ASC LIMIT " + size;

                string strChildSql = "SELECT ID FROM APCONFIGTEMPLATE WHERE 1 = 1";
                strChildSql += " LIMIT " + ((curPage - 1) * size);

                DataTable dt = mySql.GetDataTable(string.Format(strSql, strChildSql), "APCONFIGTEMPLATE");
                datas = DataChange<APCONFIGTEMPLATE>.FillModel(dt);
                return datas;
            }
        }

        /// <summary>
        /// 查询数据总量
        /// </summary>
        /// <param name="jgName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SelectCount()
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "SELECT COUNT(1) FROM APCONFIGTEMPLATE WHERE 1 = 1";
                int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
                return count;
            }
        }

        public string SelectConfigByAPID(Int64 apID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                APCONFIGTEMPLATE data = null;
                string strSql = "SELECT CONTENT FROM APCONFIGTEMPLATE WHERE ID = (select APCTID from sys_apdevice where id=@apID)";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@apID", apID)
                };

                return mySql.GetOnlyOneValue(strSql, parms).ToString();
            }
        }
    }
}
