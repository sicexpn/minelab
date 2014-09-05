using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using MySql.Data.MySqlClient;
using LUOBO.Access;
using System.Data;

namespace LUOBO.DAL
{
    public class DAL_AD_AUDIT
    {
        MySQLDataAccess mySql = new MySQLDataAccess();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        public AD_AUDIT Insert(AD_AUDIT audit)
        {
            string strSql = "INSERT INTO AD_AUDIT(ORG_ID, AD_ID, PUB_TYPE, PUB_LIST, isCopyName,SSID_NAME, FROM_ORG_ID, FROM_USER, FROM_DATE, FROM_TYPE, TO_ORG_ID, AUD_CONTENT, AUD_STAT, AUD_PARENTID) VALUES(@ORG_ID, @AD_ID, @PUB_TYPE, @PUB_LIST, @isCopyName,@SSID_NAME, @FROM_ORG_ID, @FROM_USER, @FROM_DATE, @FROM_TYPE, @TO_ORG_ID, @AUD_CONTENT, @AUD_STAT, @AUD_PARENTID)";
            MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@ORG_ID",audit.ORG_ID),
                    new MySqlParameter("@AD_ID",audit.AD_ID),
                    new MySqlParameter("@PUB_TYPE",audit.PUB_TYPE),
                    new MySqlParameter("@PUB_LIST",audit.PUB_LIST),
                    new MySqlParameter("@SSID_NAME",audit.SSID_NAME),
                    new MySqlParameter("@isCopyName",audit.ISCOPYNAME),
                    new MySqlParameter("@FROM_ORG_ID",audit.FROM_ORG_ID),
                    new MySqlParameter("@FROM_USER",audit.FROM_USER),
                    new MySqlParameter("@FROM_DATE",audit.FROM_DATE),
                    new MySqlParameter("@FROM_TYPE",audit.FROM_TYPE),
                    new MySqlParameter("@TO_ORG_ID",audit.TO_ORG_ID),
                    new MySqlParameter("@AUD_CONTENT",audit.AUD_CONTENT),
                    new MySqlParameter("@AUD_STAT",audit.AUD_STAT),
                    new MySqlParameter("@AUD_PARENTID",audit.AUD_PARENTID)
                };
            if (mySql.ExecuteSQL(strSql, parms))
            {
                return SelectNew(audit.ORG_ID, audit.AD_ID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audit"></param>
        /// <returns></returns>
        public AD_AUDIT Update(AD_AUDIT audit)
        {
            string strSql = "UPDATE AD_AUDIT SET ORG_ID = @ORG_ID, AD_ID = @AD_ID, PUB_TYPE = @PUB_TYPE, PUB_LIST = @PUB_LIST, SSID_NAME=@SSID_NAME, isCopyName = @isCopyName, FROM_ORG_ID = @FROM_ORG_ID, FROM_USER = @FROM_USER, FROM_DATE = @FROM_DATE, FROM_TYPE = @FROM_TYPE, TO_ORG_ID = @TO_ORG_ID, AUD_CONTENT = @AUD_CONTENT, AUD_STAT = @AUD_STAT, AUD_PARENTID = @AUD_PARENTID WHERE AUD_ID = @AUD_ID";
            MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@ORG_ID",audit.ORG_ID),
                new MySqlParameter("@AD_ID",audit.AD_ID),
                new MySqlParameter("@PUB_TYPE",audit.PUB_TYPE),
                new MySqlParameter("@PUB_LIST",audit.PUB_LIST),
                new MySqlParameter("@SSID_NAME",audit.SSID_NAME),
                new MySqlParameter("@isCopyName",audit.ISCOPYNAME),
                new MySqlParameter("@FROM_ORG_ID",audit.FROM_ORG_ID),
                new MySqlParameter("@FROM_USER",audit.FROM_USER),
                new MySqlParameter("@FROM_DATE",audit.FROM_DATE),
                new MySqlParameter("@FROM_TYPE",audit.FROM_TYPE),
                new MySqlParameter("@TO_ORG_ID",audit.TO_ORG_ID),
                new MySqlParameter("@AUD_CONTENT",audit.AUD_CONTENT),
                new MySqlParameter("@AUD_STAT",audit.AUD_STAT),
                new MySqlParameter("@AUD_PARENTID",audit.AUD_PARENTID),
                new MySqlParameter("@AUD_ID",audit.AUD_ID)
            };
            if (mySql.ExecuteSQL(strSql, parms))
            {
                return audit;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ORG_ID"></param>
        /// <param name="AD_ID"></param>
        /// <returns></returns>
        public AD_AUDIT SelectNew(Int64 ORG_ID, Int64 AD_ID)
        {
            List<AD_AUDIT> list = new List<AD_AUDIT>();
            string strSql = "SELECT * FROM AD_AUDIT where ORG_ID = " + ORG_ID + " and AD_ID = " + AD_ID + " and AUD_PARENTID = 0 order by AUD_ID desc limit 0,1 ";
            DataTable dt = mySql.GetDataTable(strSql, "AD_AUDIT");
            list = DataChange<AD_AUDIT>.FillModel(dt);
            return list[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <param name="pagesize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<AD_AUDIT> Select(long org_id, int pagesize, int page, int aud_statu)
        {
            string strw = "";
            if (aud_statu >= 0)
            {
                strw += " and a.AUD_STAT = " + aud_statu;
            }
            List<AD_AUDIT> datas = new List<AD_AUDIT>();
            string strSql = "SELECT a.*,b.AD_Title as AD_NAME_V,b.AD_SSID as AD_SSID_V,c.NAME as ORG_NAME_V,d.NAME as FROM_ORG_NAME_V FROM ad_audit a ";
            strSql += " LEFT JOIN ad_info b ON a.AD_ID = b.AD_ID ";
            strSql += " LEFT JOIN sys_organization c on a.ORG_ID = c.ID ";
            strSql += " LEFT JOIN sys_organization d on a.FROM_ORG_ID = d.ID ";
            strSql += " WHERE a.TO_ORG_ID = " + org_id + strw;
            strSql += " ORDER BY a.FROM_DATE DESC LIMIT " + ((page - 1) * pagesize) + "," + pagesize;
            DataTable dt = mySql.GetDataTable(strSql, "AD_AUDIT");
            datas = DataChange<AD_AUDIT>.FillModel(dt);
            return datas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public int SelectCount(long org_id, int aud_statu)
        {
            string strw = "";
            if (aud_statu >= 0)
            {
                strw += " and AUD_STAT = " + aud_statu;
            }
            string strSql = "SELECT COUNT(1) FROM AD_AUDIT WHERE TO_ORG_ID = " + org_id + strw;
            int count = Convert.ToInt32(mySql.GetOnlyOneValue(strSql));
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public List<AD_AUDIT> getAuditProgress(long aud_id)
        {
            List<AD_AUDIT> datas = new List<AD_AUDIT>();
            string strSql = "SELECT a.*,d.NAME as FROM_ORG_NAME_V FROM ad_audit a ";
            strSql += " LEFT JOIN sys_organization d on a.FROM_ORG_ID = d.ID ";
            strSql += " WHERE a.AUD_ID = " + aud_id + " or AUD_PARENTID = " + aud_id;
            strSql += " ORDER BY a.AUD_ID ASC ";
            DataTable dt = mySql.GetDataTable(strSql, "AD_AUDIT");
            datas = DataChange<AD_AUDIT>.FillModel(dt);
            return datas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public AD_AUDIT select(Int64 aud_id)
        {
            AD_AUDIT data = null;
            string strSql = "SELECT * FROM AD_AUDIT WHERE AUD_ID = @AUD_ID";
            MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@AUD_ID",aud_id)
            };
            DataTable dt = mySql.GetDataTable(strSql, "AD_AUDIT", parms);
            if (dt.Rows.Count > 0)
                data = DataChange<AD_AUDIT>.FillEntity(dt.Rows[0]);
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public Boolean ChangeSTAT(Int64 aud_id, int stat)
        {
            string strSql = "Update AD_AUDIT set AUD_STAT = @AUD_STAT where AUD_ID = @AUD_ID";
            MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@AUD_STAT",stat),
                new MySqlParameter("@AUD_ID",aud_id)
            };
            return mySql.ExecuteSQL(strSql, parms);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        public Boolean ChangeTreeStatu(Int64 aud_id, int stat)
        {
            string strSql = "Update AD_AUDIT set AUD_STAT = @AUD_STAT where AUD_ID = @AUD_ID or AUD_PARENTID = @AUD_PARENTID";
            MySqlParameter[] parms = new MySqlParameter[] { 
                new MySqlParameter("@AUD_STAT",stat),
                new MySqlParameter("@AUD_ID",aud_id),
                new MySqlParameter("@AUD_PARENTID",aud_id)
            };
            if (mySql.ExecuteSQL(strSql, parms))
            {
                //strSql = "Insert into AD_AUDIT_HISTORY select * from AD_AUDIT where AUD_ID = @AUD_ID or AUD_PARENTID = @AUD_PARENTID";
                //parms = new MySqlParameter[] { 
                //    new MySqlParameter("@AUD_ID",aud_id),
                //    new MySqlParameter("@AUD_PARENTID",aud_id)
                //};
                //if (mySql.ExecuteSQL(strSql, parms))
                //{
                //    strSql = "delete from AD_AUDIT where AUD_ID = @AUD_ID or AUD_PARENTID = @AUD_PARENTID";
                //    parms = new MySqlParameter[] { 
                //        new MySqlParameter("@AUD_ID",aud_id),
                //        new MySqlParameter("@AUD_PARENTID",aud_id)
                //    };
                //    return mySql.ExecuteSQL(strSql, parms);
                //}
                //else
                //{
                //    return false;
                //}
                return MoveTreeToHistory(aud_id);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aud_id"></param>
        /// <returns></returns>
        public Boolean MoveTreeToHistory(Int64 aud_id)
        {
            String strSql = "Insert into AD_AUDIT_HISTORY select * from AD_AUDIT where AUD_ID = @AUD_ID or AUD_PARENTID = @AUD_PARENTID";
            MySqlParameter[] parms = new MySqlParameter[] { 
                    new MySqlParameter("@AUD_ID",aud_id),
                    new MySqlParameter("@AUD_PARENTID",aud_id)
                };
            if (mySql.ExecuteSQL(strSql, parms))
            {
                strSql = "delete from AD_AUDIT where AUD_ID = @AUD_ID or AUD_PARENTID = @AUD_PARENTID";
                parms = new MySqlParameter[] { 
                        new MySqlParameter("@AUD_ID",aud_id),
                        new MySqlParameter("@AUD_PARENTID",aud_id)
                    };
                return mySql.ExecuteSQL(strSql, parms);
            }
            else
            {
                return false;
            }
        }
    }
}
