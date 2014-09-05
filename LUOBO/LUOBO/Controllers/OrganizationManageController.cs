using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.Entity;
using LUOBO.BLL;
using LUOBO.Public;
using LUOBO.Model;
using LUOBO.Helper;
namespace LUOBO.Controllers
{
    /// <summary>
    /// 机构管理-xpn
    /// </summary>
    public class OrganizationManageController : Controller
    {
        BLL_SYS_ORGANIZATION orgBLL = new BLL_SYS_ORGANIZATION();
        BLL_SYS_USER usrBll = new BLL_SYS_USER();
        BLL_SYS_APPLICATION appBll = new BLL_SYS_APPLICATION();
        BLL_SYS_ORGAPP orgAppBll = new BLL_SYS_ORGAPP();
        public ActionResult Default()
        {
            return View();
        }

        /// <summary>
        /// 机构管理
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult OrgManage()
        {
            //List<SYS_ORGANIZATION> orgs = new List<SYS_ORGANIZATION>() { new SYS_ORGANIZATION() { ID = 1, CITY = "bj", CONTACT = "123456", CONTACTER = "xpn", COUNTIES = "counties", NAME = "org1" } };

            M_SYS_ORGANIZATION m_orgs = new M_SYS_ORGANIZATION();
            m_orgs = orgBLL.Select();
            return View();
        }
        #region 机构删除
        [SupportFilter]
        public JsonResult DeleteOrg(string ids)
        {
            bool flag = orgBLL.Deletes(ids);
            return Json(flag);
        }
        #endregion
        #region 机构授权
        /// <summary>
        /// 机构授权
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult OrgAuthorize(int id)
        {
            ViewData["id"] = id;
            SYS_ORGANIZATION org = orgBLL.Select(id);
            ViewData["orgName"] = org.NAME;
            List<SYS_APPLICATION> appsAuth = orgBLL.SelectAppsAuth(id);
            ViewData["appsAuth"] = appsAuth;
            List<SYS_APPLICATION> appsNoAuth = orgBLL.SelectAppsNoAuth(id);
            ViewData["appsNoAuth"] = appsNoAuth;
            return View();
        }
        public JsonResult UnAuthApp(int id, string ids)
        {
            bool flag = orgAppBll.Deletes(id, ids);//删除orgid对应的应用
            //
            M_ORGAPP m_orgApp = new M_ORGAPP();
            if (flag)
            {

                SYS_ORGANIZATION org = orgBLL.Select(id);
                ViewData["orgName"] = org.NAME;
                m_orgApp.appsAuth = orgBLL.SelectAppsAuth(id);
                m_orgApp.appsNoAuth = orgBLL.SelectAppsNoAuth(id);
            }
            return Json(m_orgApp);
        }
        public JsonResult AuthApp(int id, string ids)
        {
            List<SYS_ORGAPP> orgApps = new List<SYS_ORGAPP>();

            List<string> appIds = new List<string>();
            appIds = ids.Split(',').ToList();
            int countIds = appIds.Count();
            for (int i = 0; i < countIds; i++)
            {
                SYS_ORGAPP orgApp = new SYS_ORGAPP();
                orgApp.ORGID = id;
                orgApp.APPID = Int64.Parse(appIds[i]);
                orgApps.Add(orgApp);
            }

            bool flag = orgAppBll.Inserts(orgApps);//添加orgid对应的应用
            M_ORGAPP m_orgApp = new M_ORGAPP();
            if (flag)
            {

                SYS_ORGANIZATION org = orgBLL.Select(id);
                ViewData["orgName"] = org.NAME;
                m_orgApp.appsAuth = orgBLL.SelectAppsAuth(id);
                m_orgApp.appsNoAuth = orgBLL.SelectAppsNoAuth(id);
            }
            return Json(m_orgApp);
        }
        #endregion
        #region 根据用户字段（机构名称、城市、区县）查找机构
        /// <summary>
        /// 根据用户字段（机构名称、城市、区县）查找机构
        /// </summary>
        /// <param name="orgName"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="size"></param>
        /// <param name="curPage"></param>
        /// <returns></returns>
        public JsonResult FindOrgList(string orgName, string province, string city, string country, int size, Int32 curPage)
        {
            M_SYS_ORGANIZATION orgs = orgBLL.Select(size, curPage, orgName, province, city, country);

            return Json(orgs);
        }
        #endregion

        #region 从字典中根据类别获取对应列表：机构类别、城市、区县
        static List<SYS_DICT_ZONE> allProvices = new List<SYS_DICT_ZONE>();
        [SupportFilter]
        public JsonResult GetOrgCategoryList()
        {
            List<SYS_DICT> categoryList = Library.Instance().GetDicByCategory("机构类别");
            return Json(categoryList);
        }
        public JsonResult GetCityList(string Provice)
        {
            //List<SYS_DICT> cityList = Library.Instance().GetDicByCategory("城市");
            var query = from s in allProvices
                        where s.Province == Provice
                        group s by s.City into g
                        select g.Key;
            return Json(query.ToList());
        }
        public JsonResult GetCountryList(string Provice, string City)
        {
            //List<SYS_DICT> countryList = Library.Instance().GetDicByCategory("区县类别");
            var query = from s in allProvices
                        where s.Province == Provice
                        && s.City == City
                        select s.Town;
            return Json(query.ToList());
        }
        public JsonResult GetAllProviceList()
        {
            allProvices = orgBLL.GetAllProvices();
            var query = from s in allProvices
                        where 1 == 1
                        group s by s.Province into citys
                        select citys.Key;
            //List<String> provices = (List<String>)query.ToList();
            return Json(query.ToList());
        }
        #endregion
        #region 机构注册：机构注册和管理员注册
        /// <summary>
        /// 机构注册
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult OrgRegister()
        {
            return View();
        }
        public static int orgID;
        [SupportFilter]
        public JsonResult AddOrg(SYS_ORGANIZATION org)
        {

            //bool flag = orgBLL.Insert(org, id);
            if (Request.Cookies["LUOBO"].Values["oid"] == "0")
            {
                org.ISVERIFY_END = true;
            }
            Int64 pId = Convert.ToInt64(Request.Cookies["LUOBO"].Values["oid"]);
            org.PID = pId;
            orgID = orgBLL.Insert(org, 1);//1:代表有返回对应的ID
            return Json(orgID);
        }
        public JsonResult AddUser(SYS_USER user)
        {
            if (usrBll.CheckAccount(user.ACCOUNT))//account unique check
            {
                user.OID = orgID;
                user.STATE = true;
                user.USERTYPE = 1;
                user.CREATETIME = DateTime.Now;
                bool flag = usrBll.Insert(user);
                return Json(flag);
            }
            else
            {
                orgBLL.Delete(orgID);
                return Json(false);
            }

        }
        #endregion
        #region 机构修改
        /// <summary>
        /// 机构修改
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult OrgEdit(int id)
        {
            SYS_ORGANIZATION org = orgBLL.Select(id);
            return View(org);
        }
        public JsonResult UpdateOrg(SYS_ORGANIZATION org)
        {
            //int i;
            if (Request.Cookies["LUOBO"].Values["oid"] == "0")
                org.ISVERIFY_END = true;
            Int64 pId = Convert.ToInt64(Request.Cookies["LUOBO"].Values["oid"]);
            org.PID = pId;
            bool flag = orgBLL.Update(org);
            return Json(flag);
        }
        #endregion

        /// <summary>
        /// 选点获取经纬度
        /// </summary>
        /// <returns></returns>
        [SupportFilter]
        public ActionResult GetLocation()
        {
            return View();
        }
        #region 机构扩展功能维护（登陆方式等）
        [SupportFilter]
        public ActionResult OrgPropExpend()
        {
            return View();
        }
        [SupportFilter]
        public ActionResult OrgPropConfig(Int64 id)
        {
            SYS_ORGANIZATION org = orgBLL.Select(id);
            //List<SYS_ORG_PROPERTY> orgPropList = orgBLL.SelectAllPropsByOID(id);
            return View(org);
        }
        [SupportFilter]
        public JsonResult GetOrgProps(Int64 oid)
        {
            List<SYS_ORG_PROPERTY> orgPropList = orgBLL.SelectAllPropsByOID(oid);
            return Json(orgPropList);
        }
        [SupportFilter]
        public JsonResult UpdateOrgProp(SYS_ORG_PROPERTY OrgProps)
        {
            bool flag = false;
            //flag = orgBLL.UpdateOrgPropList(OrgProps.Items.ToList());
            flag = orgBLL.UpdateOrgProp(OrgProps);
            return Json(flag);
        }
        [SupportFilter]
        public JsonResult GetOrgPropList()
        {
            List<SYS_DICT> orgPropList = Library.Instance().GetDicByCategory("机构扩展属性");
            return Json(orgPropList);
        }
        #endregion
    }
}