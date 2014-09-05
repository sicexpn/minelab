using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.Access;
using LUOBO.Model;
using System.Data;
using MySql.Data.MySqlClient;

namespace LUOBO.DAL
{
    public class DAL_SYS_SETTINGVER
    {

        public bool Update(SYS_SETTINGVER data)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                DataTable dt = mySql.GetDataTable("Select * from SYS_SETTINGVER where 1<>1", "SYS_SETTINGVER");
                if (data.ID < 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(DataChange<Entity.SYS_SETTINGVER>.FillRow(data, dr));
                }
                else
                {
                    dt = mySql.GetDataTable("Select * from SYS_SETTINGVER where ID=" + data.ID.ToString(), "SYS_SETTINGVER");
                    if (dt.Rows.Count == 0)
                    {
                        throw new Exception("没有找到相关的数据，无法保存");
                    }
                    DataChange<SYS_SETTINGVER>.FillRow(data, dt.Rows[0]);
                }

                return mySql.Update(dt);
            }
        }

        public SYS_SETTINGVER SelectNewByAPSerial(string deviceSerial)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                SYS_SETTINGVER data = new SYS_SETTINGVER();
                string strSql = "SELECT * FROM SYS_SETTINGVER WHERE APID=(SELECT ID FROM SYS_APDEVICE WHERE MAC=@deviceSerial) ORDER BY DATETIME desc limit 1";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@deviceSerial", deviceSerial)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SETTINGVER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<SYS_SETTINGVER>.FillEntity(dt.Rows[0]);
                return data;
            }
        }

        public Model.M_APSETTINGVER_VIEW SelectByApMac(string apMac)
        {
            using (MySQLDataAccess mySql = new MySQLDataAccess())
            {
                M_APSETTINGVER_VIEW data = null;
                string strSql = "SELECT b.MAC,b.ID as APID,a.GUID,b.SERIAL,b.DEVICESTATE,b.APCHANNEL,b.POWER,b.ISSSIDON FROM SYS_SETTINGVER AS a,SYS_APDEVICE AS b WHERE a.APID=b.ID and b.MAC = @MAC  ORDER BY a.DATETIME desc limit 1";
                MySqlParameter[] parms = new MySqlParameter[] {
                    new MySqlParameter("@MAC", apMac)
                };
                DataTable dt = mySql.GetDataTable(strSql, "SYS_SETTINGVER", parms);
                if (dt.Rows.Count > 0)
                    data = DataChange<M_APSETTINGVER_VIEW>.FillEntity(dt.Rows[0]);
                return data;
            }
        }
    }
}
