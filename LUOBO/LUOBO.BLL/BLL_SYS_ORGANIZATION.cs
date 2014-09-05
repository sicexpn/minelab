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
    public class BLL_SYS_ORGANIZATION
    {
        DAL_SYS_ORGANIZATION orgDAL = new DAL_SYS_ORGANIZATION();
        DAL_SYS_DICT_ZONE zoneDAL = new DAL_SYS_DICT_ZONE();
        DAL_SYS_ORG_PROPERTY orgPropDAL = new DAL_SYS_ORG_PROPERTY();
        DAL_SYS_SSID_TEMPLATE stDAL = new DAL_SYS_SSID_TEMPLATE();
        DAL_SYS_DICT dicDAL = new DAL_SYS_DICT();

        public bool Insert(SYS_ORGANIZATION data)
        {
            return orgDAL.Insert(data);
        }
        public bool Inserts(List<SYS_ORGANIZATION> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_ORGANIZATION data in datas)
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

        public bool Update(SYS_ORGANIZATION data)
        {
            return orgDAL.Update(data);
        }

        public bool Updates(List<SYS_ORGANIZATION> datas)
        {
            return orgDAL.Updates(datas);
        }

        public bool Delete(Int64 id)
        {
            return orgDAL.Delete(id);
        }

        public M_SYS_ORGANIZATION Select()
        {
            M_SYS_ORGANIZATION m_org = new M_SYS_ORGANIZATION();
            m_org.OrgList = orgDAL.Select();
            m_org.AllCount = orgDAL.SelectCounts("", "", "", "");
            return m_org;
        }
        public List<SYS_ORGANIZATION> Select(int size, Int64 curPage)
        {
            return orgDAL.Select(size, curPage);
        }
        public SYS_ORGANIZATION Select(int size, Int64 curPage, Int64 id)
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
        public M_SYS_ORGANIZATION Select(int size, Int64 curPage, string orgName, string province, string city, string country)
        {
            M_SYS_ORGANIZATION m_org = new M_SYS_ORGANIZATION();
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

        public SYS_ORGANIZATION Select(Int64 id)
        {
            return orgDAL.Select(id);
        }
        public int Insert(SYS_ORGANIZATION org, int p)
        {
            if (org.PID == 0)
            {
                org.PIDHELP = "$0$";
            }
            else
            {
                string pidHelp = orgDAL.Select(org.PID).PIDHELP;

                org.PIDHELP = pidHelp + "," + "$" + org.PID + "$";
            }

            return orgDAL.Insert(org, p);
        }

        public bool Deletes(string ids)
        {
            return orgDAL.Deletes(ids);
        }

        public List<SYS_ORGANIZATION> SelectByOrgType(LUOBO.Helper.CustomEnum.ENUM_Org_Type type)
        {
            return orgDAL.SelectByOrgType(type);
        }

        public List<SYS_ORGANIZATION> SelectSub(Int64 jgID)
        {
            return orgDAL.SelectParent(jgID);
        }

        public Int64 GetPrentAuditOID(Int64 oid, bool isSelf)
        {
            return orgDAL.SelectPrentAuditOID(oid, isSelf);
        }

        public List<SYS_DICT_ZONE> GetAllProvices()
        {
            return zoneDAL.SelectAllProvices();
        }

        public SYS_ORGANIZATION SelectByName(string name)
        {
            return orgDAL.SelectByName(name);
        }

        public bool UpdateOrgProp(SYS_ORG_PROPERTY orgProp)
        {

            if (orgPropDAL.IsExist(orgProp))
            {
                return orgPropDAL.Update(orgProp);
            }
            else
            {
                return orgPropDAL.Insert(orgProp);
            }

        }

        public bool UpdateOrgPropList(List<SYS_ORG_PROPERTY> list)
        {
            bool flag = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (SYS_ORG_PROPERTY data in list)
                        flag = UpdateOrgProp(data);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }
            return flag;
        }

        public List<SYS_ORG_PROPERTY> SelectAllPropsByOID(long id)
        {
            return orgPropDAL.SelectAllPropsByOID(id);
        }

        public bool IsSSIDTemplateByOID(Int64 oid)
        {
            if (stDAL.SelectDefaultByOID(oid).Count > 0)
                return true;
            return false;
        }

        public List<SYS_SSID_TEMPLATE> GetSSIDTemplateByOID(Int64 oid)
        {
            return stDAL.SelectByOID(oid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SYS_ORG_PROPERTY> GetLoginPropsByOID(long id)
        {
            return orgPropDAL.SelectLoginPropsByOID(id);
        }

        /// <summary>
        /// 获取用户扩展属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<M_ORG_EXT_PROPERTY> GetOrgExtProperty(long id)
        {
            List<M_ORG_EXT_PROPERTY> tmpList = new List<M_ORG_EXT_PROPERTY>();
            List<SYS_ORG_PROPERTY> userdata = SelectAllPropsByOID(id);
            List<SYS_ORG_PROPERTY> tmpuserdata;

            List<SYS_DICT> dicinfo = dicDAL.Select().Where(c => c.CATEGORY == "机构扩展属性").ToList();
            List<SYS_DICT_EXTPROP> propertyinfo = dicDAL.SelectExtProperty();
            if (dicinfo != null)
            {
                M_ORG_EXT_PROPERTY tmp1;
                M_ORG_EXT_PROPERTY_ITEM tmp2;
                List<SYS_DICT_EXTPROP> dictmp;

                foreach (SYS_DICT data1 in dicinfo)
                {
                    tmp1 = new M_ORG_EXT_PROPERTY();
                    tmp1.ORG_ID = id;
                    tmp1.PRO_ID = data1.VALUE;
                    tmp1.PRO_NAME = data1.NAME;
                    tmp1.PRO_ITEM = new List<M_ORG_EXT_PROPERTY_ITEM>();

                    dictmp = propertyinfo.Where(c => c.PROP_TYPE == data1.VALUE).OrderBy(c=>c.PROP_SORT).ToList();
                    if (dictmp != null)
                    {
                        foreach (SYS_DICT_EXTPROP data2 in dictmp)
                        {
                            tmp2 = new M_ORG_EXT_PROPERTY_ITEM();
                            tmp2.PropertyInfo = data2;
                            tmp2.ID = 0;
                            tmp2.ProValue = data2.PROP_DEFAULTVALUE.Replace("[%ORG_ID%]", id.ToString());

                            if (userdata != null && userdata.Count > 0)
                            {
                                tmpuserdata = userdata.Where(c => c.OID == id && c.PTYPE == data1.VALUE.ToString() && c.PNAME == data2.PROP_ID.ToString()).ToList();
                                if (tmpuserdata != null && tmpuserdata.Count > 0)
                                {
                                    tmp2.ID = tmpuserdata[0].ID;
                                    tmp2.ProValue = tmpuserdata[0].PVALUE;
                                }
                            }
                            tmp1.PRO_ITEM.Add(tmp2);
                        }
                    }
                    tmpList.Add(tmp1);
                }
            }

            return tmpList;
        }

        /// <summary>
        /// 获取微信公众名称
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public List<SYS_ORG_PROPERTY> GetWXNameByOID(Int64 oid)
        {
            return orgPropDAL.SelectWXNameByOID(oid);
        }
        public SYS_ORG_PROPERTY GetValueByOIDAndName(Int64 oid, string name)
        {
            return orgPropDAL.SelectValueByOIDAndName(oid, name);
        }
    }
}
