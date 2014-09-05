using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.Model;
using LUOBO.Entity;
using LUOBO.Helper;

namespace LUOBO.Controllers
{
    public class AuditManageController : Controller
    {
        private Int64 org_id;       // = 10035
        private String user_name;
        BLL.BLL_AuditManage auditBll = new BLL.BLL_AuditManage();
        BLL.BLL_DeviceService dsBll = new BLL.BLL_DeviceService();
        BLL.BLL_SYS_DICT dicBll = new BLL.BLL_SYS_DICT();

        private void init()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            org_id = Convert.ToInt64(cookie.Values["oid"]);
            user_name = cookie.Values["username"];
        }

        #region 广告审核列表页
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditList()
        {
            return View();
        }

        /// <summary>
        /// 查询AD待审核列表
        /// </summary>
        /// <returns></returns>
        public JsonResult FindAuditList(int size, int curPage, int statu)
        {
            //return Json(auditBll.SelectAuditByPage(org_id, size, curPage, statu));
            try
            {
                init();
                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = auditBll.SelectAuditByPage(org_id, size, curPage, statu);
                return Json(result);
            }
            catch (Exception e)
            {
                M_Result result = new M_Result();
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
                return Json(result);
            }
        }

        /// <summary>
        /// 获取审核进度
        /// </summary>
        /// <param name="audid"></param>
        /// <returns></returns>
        public JsonResult GetAuditProgress(long audid)
        {
            //return Json(auditBll.GetAuditProgress(audid));
            try
            {
                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = auditBll.GetAuditProgress(audid);
                return Json(result);
            }
            catch (Exception e)
            {
                M_Result result = new M_Result();
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
                return Json(result);
            }
        }

        /// <summary>
        /// 审核广告申请
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="audid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public JsonResult HandleAudit(int handle, Int64 audid, String auditstr, String freehost, Int64 adId, string defaultfree)
        {
            try
            {
                init();
                //bool flag=auditBll.
                //bool flag = dsBll.UpdateFreeHost(adId, freehost, "");
                bool flag = dsBll.UpdateFreeHost(adId, freehost, defaultfree);

                String resultstr = auditBll.HandleAudit(org_id, user_name, audid, handle, auditstr);
                M_Result result = new M_Result();
                result.ResultMsg = resultstr;
                if (resultstr == "ok" && flag)
                {
                    result.ResultCode = 0;
                }
                else
                {
                    result.ResultCode = 1;
                }
                return Json(result);
            }
            catch (Exception e)
            {
                M_Result result = new M_Result();
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
                return Json(result);
            }
        }

        public JsonResult GetDicByFreeHost()
        {
            M_Result result = new M_Result();

            try
            {
                List<SYS_DICT> dicinfo = dicBll.Select().Where(c => c.CATEGORY == "放行域名").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "默认放行域名信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "默认放行域名信息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "默认放行域名信息读取失败！\n" + ex.Message;
            }

            return Json(result);
        }
        #endregion

        #region 已审核列表页
        public ActionResult AuditHistory()
        {
            return View();
        }
        #endregion
    }
}
