using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Access;
using MySql.Data.MySqlClient;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_SYS_SIM
    {
        public Int64 SelectDailyLimitByPAID(Int64 APID)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                string strSql = "select ifnull(sum(DataOfDailyLimit),-1) from sys_sim where ID =(select SIMID from sys_simnetcard where ID=(select SNCID from sys_simap where APID=@APID))";

                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@APID", APID)
                };
                return Convert.ToInt64(mySql.GetOnlyOneValue(strSql, parms));
            }
        }
    }
}
