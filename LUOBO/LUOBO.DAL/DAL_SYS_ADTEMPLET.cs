using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using System.Data;
using LUOBO.Access;

namespace LUOBO.DAL
{
    public class DAL_SYS_ADTEMPLET
    {
        MySQLDataAccess mySql = new MySQLDataAccess();

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<SYS_ADTEMPLET> SelectAllPub()
        {
            List<SYS_ADTEMPLET> list = new List<SYS_ADTEMPLET>();
            string strSql = "SELECT * FROM SYS_ADTEMPLET where SADT_STATU = 1";
            DataTable dt = mySql.GetDataTable(strSql, "SYS_ADTEMPLET");
            list = DataChange<SYS_ADTEMPLET>.FillModel(dt);
            return list;
        }

        public SYS_ADTEMPLET SelectPubTemplet(int ADT_ID)
        {
            List<SYS_ADTEMPLET> list = new List<SYS_ADTEMPLET>();
            string strSql = "SELECT * FROM SYS_ADTEMPLET where SADT_STATU = 1 and SADT_ID = " + ADT_ID;
            DataTable dt = mySql.GetDataTable(strSql, "SYS_ADTEMPLET");
            list = DataChange<SYS_ADTEMPLET>.FillModel(dt);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
    }
}
