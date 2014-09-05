using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace LUOBO.Access
{

    /// <summary>
    /// 数据库操作基础类
    /// 用于DAO访问数据库时统一的数据操作
    /// 提供数据添加、删除、修改、查询等功能
    /// </summary>
    public class MySQLDataAccess : IDisposable
    {
        #region 数据库参数
        private string connectionstr = null;
        private MySqlConnection conn = null;
        private MySqlDataAdapter adpt = null;
        #endregion

        public MySQLDataAccess()
        {
            connectionstr = ConfigurationSettings.AppSettings["ConnLUOBO"];
            conn = new MySqlConnection(connectionstr);
            adpt = new MySqlDataAdapter();
        }

        public MySQLDataAccess(LUOBO.Helper.CustomEnum.ENUM_SqlConn sqlConn)
        {
            if (sqlConn == LUOBO.Helper.CustomEnum.ENUM_SqlConn.LUOBO)
            {
                connectionstr = ConfigurationSettings.AppSettings["ConnLUOBO"];
            }
            else if (sqlConn == LUOBO.Helper.CustomEnum.ENUM_SqlConn.Radius)
            {
                connectionstr = ConfigurationSettings.AppSettings["ConnRadius"];
            }
            else if (sqlConn == LUOBO.Helper.CustomEnum.ENUM_SqlConn.Statistical)
            {
                connectionstr = ConfigurationSettings.AppSettings["ConnStatistical"];
            }

            conn = new MySqlConnection(connectionstr);
            adpt = new MySqlDataAdapter();
        }

        /// <summary>
        /// 注销对象时调用
        /// </summary>
        public void Dispose()
        {
            conn.Dispose();
            adpt.Dispose();
            conn = null;
            adpt = null;
        }
        /// <summary>
        /// 保存数据，调用前需要先调用BeginTransaction，如果在调用时不调用BeginTransaction，数据将不能保存到数据库中
        /// </summary>
        /// <param name="str">获取要保存的数据库结构的SQL语句，如只需要保存id字段，可以采用Select id from table1 </param>
        /// <param name="updateDt">需要保存的数据</param>
        /// <returns>调用成功返回true，失败返回false</returns>
        public bool Update(string str, DataTable updateDt)
        {
            MySqlCommandBuilder custCB = null;
            try
            {
                //				BeginTransaction();
                adpt.SelectCommand.CommandText = str;
                custCB = new MySqlCommandBuilder(adpt);
                //custCB.RefreshSchema();
                DataTable dt = updateDt.GetChanges();
                if (null == dt)
                {
                    if (custCB != null)
                    {
                        custCB.Dispose();
                    }
                    return true;
                }
                if (dt.Select(null, null, DataViewRowState.Deleted).Length > 0)
                {
                    adpt.Update(dt.Select(null, null, DataViewRowState.Deleted));
                }
                if (dt.Select(null, null, DataViewRowState.ModifiedCurrent).Length > 0)
                {

                    adpt.Update(dt.Select(null, null, DataViewRowState.ModifiedCurrent));
                }
                if (dt.Select(null, null, DataViewRowState.Added).Length > 0)
                {
                    adpt.Update(dt.Select(null, null, DataViewRowState.Added));
                }
                //				adpt.Update(dt);
                updateDt.AcceptChanges();
                return true;
            }
            catch (System.Exception ex)
            {
                if (!(ex is System.Data.DBConcurrencyException))
                {
                    throw ex;
                }
                updateDt.AcceptChanges();
                return false;
            }
            //				finally 
            //				{
            //					if(custCB != null) 
            //					{                  
            //						custCB.Dispose();
            //					}
            //				}

        }
        /// <summary>
        /// 保存数据，调用前需要先调用BeginTransaction，如果在调用时不调用BeginTransaction，数据将不能保存到数据库中
        /// 如果updateDt表名称与数据库中真实的表名称一致的情况下可以这样调用，本函数将在内部根据updateDt表名构建一个"Select * from "+updateDt.TableName
        /// </summary>
        /// <param name="updateDt">需要保存的数据</param>
        /// <returns>调用成功返回true，失败返回false</returns>
        public bool Update(DataTable updateDt)
        {
            MySqlCommandBuilder custCB = null;
            try
            {
                //				BeginTransaction();
                string sql = "Select * from " + updateDt.TableName + " where 1<>1";
                if (adpt.SelectCommand == null)
                {
                    adpt.SelectCommand = new MySqlCommand(sql, conn);
                }
                else
                {
                    adpt.SelectCommand.CommandText = sql;
                }
                adpt.SelectCommand.CommandText = "Select * from " + updateDt.TableName + " where 1<>1";
                custCB = new MySqlCommandBuilder(adpt);
                //custCB.RefreshSchema();
                DataTable dt = updateDt.GetChanges();
                if (null == dt)
                {
                    if (custCB != null)
                    {
                        custCB.Dispose();
                    }
                    //					if(myTrans != null && singleTrans) 
                    //					{
                    //						myTrans.Dispose();
                    //						myTrans = null;
                    //					}
                    return true;
                }
                //if (dt.Select(null, null, DataViewRowState.Deleted).Length > 0)
                //{
                //    adpt.Update(dt.Select(null, null, DataViewRowState.Deleted));
                //}
                //if (dt.Select(null, null, DataViewRowState.ModifiedCurrent).Length > 0)
                //{
                //    adpt.Update(dt.Select(null, null, DataViewRowState.ModifiedCurrent));
                //}
                //if (dt.Select(null, null, DataViewRowState.Added).Length > 0)
                //{
                //    adpt.Update(dt.Select(null, null, DataViewRowState.Added));
                //}
                adpt.Update(dt);
                //				adpt.Update(dt);
                updateDt.AcceptChanges();
                //				if(singleTrans) 
                //				{                    
                //					myTrans.Commit();       
                //				}
                return true;
            }
            catch (System.Exception ex)
            {
                //				if(myTrans != null) 
                //				{
                //					myTrans.Rollback();
                //				}              

                if (!(ex is System.Data.DBConcurrencyException))
                {
                    throw ex;
                }
                return false;
            }
            finally
            {
                if (custCB != null)
                {
                    custCB.Dispose();
                }
                //				if(myTrans != null && singleTrans) 
                //				{
                //					myTrans.Dispose();
                //					myTrans = null;
                //				}
            }
        }
        /// <summary>
        ///  保存多个数据表数据，调用前需要先调用BeginTransaction，如果在调用时不调用BeginTransaction，数据将不能保存到数据库中
        /// </summary>
        /// <param name="str">检索Sql语句数组,如只需要保存id字段，可以采用Select id from table1 </param>
        /// <param name="updatedset">要保存的数据集</param>
        /// <param name="Table_name">要保存的数据表数组</param>
        public bool Update(string[] str, DataSet updatedset, string[] Table_name)
        {
            MySqlCommandBuilder custCB = new MySqlCommandBuilder(adpt);
            string exstr = "";
            try
            {
                //				BeginTransaction();              
                DataSet dset = null;
                if (null != updatedset)
                {
                    dset = updatedset.GetChanges();
                    if (null == dset)
                    {
                        if (custCB != null)
                        {
                            custCB.Dispose();
                        }
                        //							if(myTrans != null && singleTrans) 
                        //							{
                        //								myTrans.Dispose();
                        //								myTrans = null;
                        //							}
                        return true;
                    }
                }
                for (int i = 0; i < str.Length; i++)
                {
                    if (dset != null)
                    {
                        if (dset.Tables.Contains(Table_name[i]))
                        {
                            adpt.SelectCommand.CommandText = str[i];
                            custCB.RefreshSchema();
                            if (dset.Tables[Table_name[i]].Select(null, null, DataViewRowState.Deleted).Length > 0)
                            {
                                adpt.Update(dset.Tables[Table_name[i]].Select(null, null, DataViewRowState.Deleted));
                            }
                            if (dset.Tables[Table_name[i]].Select(null, null, DataViewRowState.ModifiedCurrent).Length > 0)
                            {
                                adpt.Update(dset.Tables[Table_name[i]].Select(null, null, DataViewRowState.ModifiedCurrent));
                            }
                            if (dset.Tables[Table_name[i]].Select(null, null, DataViewRowState.Added).Length > 0)
                            {
                                adpt.Update(dset.Tables[Table_name[i]].Select(null, null, DataViewRowState.Added));
                            }
                            //							adpt.Update(dset, Table_name[i]);
                        }
                    }
                }
                //				if(singleTrans) 
                //				{
                //					myTrans.Commit();
                //				}              
                updatedset.AcceptChanges();
                return true;
            }
            catch (Exception ex)
            {
                //				if(myTrans != null) 
                //				{
                //					myTrans.Rollback();
                //				}               
                throw new Exception("保存数据表:" + exstr + "出错，原因是：" + ex.ToString());
                //					updateDt.AcceptChanges();

            }
            finally
            {
                if (custCB != null)
                {
                    custCB.Dispose();
                }
                //				if(myTrans != null && singleTrans) 
                //				{
                //					myTrans.Dispose();
                //					myTrans = null;
                //				}
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public void ExecuteProcedure(string sqlstr, DbParameter[] parameters)
        {
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                myCommand.Connection = conn;
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.CommandText = sqlstr;
                foreach (DbParameter parameter in parameters)
                {
                    myCommand.Parameters.Add(parameter);
                }
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr">Sql语句</param>
        /// <returns>执行成功返回true,否则抛出失败异常</returns>
        public bool ExecuteSQL(string sqlstr)
        {
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                using (MySqlConnection oraclConn = new MySqlConnection(connectionstr))
                {
                    if (oraclConn.State != ConnectionState.Open)
                    {
                        oraclConn.Close();
                        oraclConn.Open();
                    }
                    myCommand.Connection = oraclConn;
                    myCommand.CommandText = sqlstr;
                    myCommand.ExecuteNonQuery();
                }
                //				BeginTransaction();

                //				if(singleTrans) 
                //				{
                //					myTrans.Commit();
                //				}              
                //				myCommand.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //				if(myTrans != null) 
                //				{
                //					myTrans.Rollback();
                //				}
                throw ex;
            }
            finally
            {
                //				myCommand.Dispose();
                //				if(myTrans != null && singleTrans) 
                //				{
                //					myTrans.Dispose();
                //					myTrans = null;
                //				}
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr">检索Sql语句数组</param>
        /// <returns>执行成功返回true,否则抛出失败异常</returns>
        public bool ExecuteSQL(string[] sqlstr)
        {
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                //				BeginTransaction();
                using (MySqlConnection oraclConn = new MySqlConnection(connectionstr))
                {
                    if (oraclConn.State != ConnectionState.Open)
                    {
                        oraclConn.Close();
                        oraclConn.Open();
                    }
                    myCommand.Connection = oraclConn;
                    for (int i = 0; i < sqlstr.Length; i++)
                    {
                        myCommand.CommandText = sqlstr[i];
                        myCommand.ExecuteNonQuery();
                    }
                }
                //				if(singleTrans) 
                //				{
                //					myTrans.Commit();
                //				}     
                myCommand.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //				myTrans.Rollback();
                throw ex;
            }
            finally
            {
                myCommand.Dispose();
                //				if(myTrans != null && singleTrans) 
                //				{
                //					myTrans.Dispose();
                //					myTrans = null;
                //				}
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr">Sql语句</param>
        /// <returns>执行成功返回true,否则抛出失败异常</returns>
        public bool ExecuteSQL(string sqlstr, DbParameter[] parameters)
        {
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                //using (MySqlConnection oraclConn = new MySqlConnection(connectionstr))
                //{
                if (conn.State != ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                myCommand.Connection = conn;
                myCommand.CommandText = sqlstr;
                if (parameters != null)
                {
                    foreach (DbParameter parameter in parameters)
                    {
                        myCommand.Parameters.Add(parameter);
                    }
                }
                myCommand.ExecuteNonQuery();
                //}

                return true;
            }
            catch (Exception ex)
            {
                //				if(myTrans != null) 
                //				{
                //					myTrans.Rollback();
                //				}
                throw ex;
            }
            finally
            {
                //				myCommand.Dispose();
                //				if(myTrans != null && singleTrans) 
                //				{
                //					myTrans.Dispose();
                //					myTrans = null;
                //				}
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr">检索Sql语句数组</param>
        /// <returns>执行成功返回true,否则抛出失败异常</returns>
        public bool ExecuteProcedureSQL(string sqlstr)
        {
            MySqlCommand myCommand = new MySqlCommand();
            try
            {
                //				BeginTransaction();
                myCommand.Connection = conn;
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(null);
                myCommand.CommandText = sqlstr;
                myCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        /// <summary>
        /// 执行SQL语句, 返回数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表名称</param>
        /// <returns>返回数据表，失败抛出异常</returns>
        public DataTable GetDataTable(string sql, string tableName)
        {
            //if (conn.State != ConnectionState.Open)
            //{
            //    conn.Close();
            //    conn.Open();
            //}
            //if (adpt.TableMappings.IndexOf(tableName) < 0)
            //{
            //    adpt.TableMappings.Add(tableName, tableName);
            //}
            //Log.SaveLog(sql, "");
            //adpt = new MySqlDataAdapter();
            if (adpt.SelectCommand == null)
            {
                adpt.SelectCommand = new MySqlCommand(sql, conn);
            }
            else
            {
                adpt.SelectCommand.CommandText = sql;
            }

            DataTable dt = new DataTable(tableName);
            adpt.Fill(dt);
            //adpt.TableMappings.Remove(adpt.TableMappings[tableName]);
            return dt;
        }

        /// <summary>
        /// 执行SQL语句, 返回数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">表名称</param>
        /// <returns>返回数据表，失败抛出异常</returns>
        public DataTable GetDataTable(string sql, string tableName, DbParameter[] parameters)
        {
            //if (conn.State != ConnectionState.Open)
            //{
            //    conn.Close();
            //    conn.Open();
            //}
            //if (adpt.TableMappings.IndexOf(tableName) < 0)
            //{
            //    adpt.TableMappings.Add(tableName, tableName);
            //}
            //Log.SaveLog(sql, "");
            adpt = new MySqlDataAdapter();
            using (MySqlConnection oraclConn = new MySqlConnection(connectionstr))
            {
                if (adpt.SelectCommand == null)
                {
                    adpt.SelectCommand = new MySqlCommand(sql, oraclConn);
                }
                else
                {
                    adpt.SelectCommand.CommandText = sql;
                }
                foreach (DbParameter parameter in parameters)
                {
                    adpt.SelectCommand.Parameters.Add(parameter);
                }
                DataTable dt = new DataTable(tableName);
                adpt.Fill(dt);
                //adpt.TableMappings.Remove(adpt.TableMappings[tableName]);
                return dt;
            }

        }
        /// <summary>
        /// 根据SQL语句获取单个值对象
        /// </summary>
        /// <param name="sql">要查询单个数据的SQL语句</param>
        /// <returns>正确返回object类型对象，需要根据情况进行转换，错误抛出异常</returns>
        public object GetOnlyOneValue(string sql)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            //adpt = new MySqlDataAdapter();
            if (adpt.SelectCommand == null)
            {
                adpt.SelectCommand = new MySqlCommand(sql, conn);
            }
            else
            {
                adpt.SelectCommand.CommandText = sql;
            }
            object ob = adpt.SelectCommand.ExecuteScalar();
            if (ob == Convert.DBNull)
            {
                return null;
            }
            return ob;
        }
        /// <summary>
        /// 根据SQL语句获取单个值对象
        /// </summary>
        /// <param name="sql">要查询单个数据的SQL语句</param>
        /// <returns>正确返回object类型对象，需要根据情况进行转换，错误抛出异常</returns>
        public object GetOnlyOneValue(string sql, DbParameter[] parameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            //adpt = new MySqlDataAdapter();
            if (adpt.SelectCommand == null)
            {
                adpt.SelectCommand = new MySqlCommand(sql, conn);
            }
            else
            {
                adpt.SelectCommand.CommandText = sql;
            }
            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    adpt.SelectCommand.Parameters.Add(parameter);
                }
            }
            object ob = adpt.SelectCommand.ExecuteScalar();
            if (ob == Convert.DBNull)
            {
                return null;
            }
            return ob;
        }

        public DataTable SetTableZero(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                for (int j = 0; j < dt.Columns.Count; ++j)
                {
                    if (row[j] == DBNull.Value)
                    {
                        row[j] = 0;
                    }
                }
            }
            return dt;
        }


    }
}
