using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Model;
using System.Transactions;

namespace LUOBO.BLL
{
    public class BLL_SYS_PROBEORG
    {
        DAL_SYS_PROBEORG orgDAL = new DAL_SYS_PROBEORG();
        DAL_SYS_DICT_ZONE zoneDAL = new DAL_SYS_DICT_ZONE();

        public bool Insert(SYS_PROBEORG data)
        {
            return orgDAL.Insert(data);
        }
        public bool Inserts(List<SYS_PROBEORG> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_PROBEORG data in datas)
                    {
                        orgDAL.Insert(data);
                    }
                    scope.Complete();
                    flag = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }


            return flag;
        }

        public bool Update(SYS_PROBEORG data)
        {
            return orgDAL.Update(data);
        }

        public bool Updates(List<SYS_PROBEORG> datas)
        {
            return orgDAL.Updates(datas);
        }

        public bool Delete(Int64 id)
        {
            return orgDAL.Delete(id);
        }

        public M_SYS_PROBEORG Select()
        {
            M_SYS_PROBEORG m_org = new M_SYS_PROBEORG();
            m_org.OrgList = orgDAL.Select();
            m_org.AllCount = orgDAL.SelectCounts("", "", "", "");
            return m_org;
        }
        public List<SYS_PROBEORG> Select(int size, Int64 curPage)
        {
            return orgDAL.Select(size, curPage);
        }
        public SYS_PROBEORG Select(int size, Int64 curPage, Int64 id)
        {
            return orgDAL.Select(size, curPage, id);
        }
        /// <summary>
        /// 基于机构名称、城市、区县的查询
        /// </summary>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <param name="orgName"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public M_SYS_PROBEORG Select(int size, Int64 curPage, string orgName, string province, string city, string country)
        {
            M_SYS_PROBEORG m_org = new M_SYS_PROBEORG();
            m_org.OrgList = orgDAL.Select(size, curPage, orgName, province, city, country);
            m_org.AllCount = orgDAL.SelectCounts(orgName, province, city, country);
            return m_org;
        }

        public List<SYS_APPLICATION> SelectAppsAuth(int id)
        {
            return orgDAL.SelectAppsAuth(id);
        }

        public List<SYS_APPLICATION> SelectAppsNoAuth(int id)
        {
            return orgDAL.SelectAppsNoAuth(id);
        }

        public SYS_PROBEORG Select(Int64 id)
        {
            return orgDAL.Select(id);
        }
        public int Insert(SYS_PROBEORG org, int p)
        {
            string pidHelp = orgDAL.Select(org.PID).PIDHELP;
            org.PIDHELP = pidHelp + "," + "$" + org.PID + "$";
            return orgDAL.Insert(org, p);
        }

        public bool Deletes(string ids)
        {
            return orgDAL.Deletes(ids);
        }

        public List<SYS_PROBEORG> SelectSub(Int64 jgID)
        {
            return orgDAL.SelectSub(jgID);
        }

        public Int64 GetPrentAuditOID(Int64 oid)
        {
            return 0;
        }

        public List<SYS_DICT_ZONE> GetAllProvices()
        {
            return zoneDAL.SelectAllProvices();
        }
    }
}
