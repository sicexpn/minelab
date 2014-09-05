using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.Model;
using LUOBO.Entity;

namespace LUOBO.Controllers
{
    public class SSIDManageController : Controller
    {
        BLL.BLL_SYS_SSID ssidBll = new BLL.BLL_SYS_SSID();
        BLL.BLL_APManage apBll = new BLL.BLL_APManage();
        //
        // GET: /SSIDManage/

        public ActionResult Default()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 oid = Convert.ToInt64(cookie.Values["oid"]);
            ViewData["OID"] = oid;
            return View();
        }

        public JsonResult GetSSIDList(Int64 OID, int size, int curPage)
        {
            M_SSID_VIEW mSSID = ssidBll.GetSSIDByOID(size, curPage, OID);
            return Json(mSSID);
        }

        #region SSID审核
        public ActionResult SSIDAUDIT()
        {
            return View();
        }

        public JsonResult GetSSIDAudList(string keystr,int state, int curPage, int size)
        {
            M_Result result = new M_Result();
            try
            {
                if (!string.IsNullOrEmpty(keystr))
                    keystr = keystr.Trim();
                result.ResultOBJ = apBll.SelectSSIDAuditOnPage(keystr, state, curPage, size);
                result.ResultCode = 0;
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 审核SSID通过
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult AuditSSID(string ids)
        {
            M_Result result = new M_Result();
            try
            {
                HttpCookie cookie = Request.Cookies["LUOBO"];
                Int64 AudOID = Convert.ToInt64(cookie.Values["oid"]);
                string account = cookie.Values["account"].ToString();

                //TODO 审核说明
                string auditIntro = "";

                if (ssidBll.AuditSSID(ids, AudOID, account, auditIntro))
                    result.ResultCode = 0;
                else
                    result.ResultCode = 1;
                
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 审核SSID不通过
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult NoAuditSSID(string ids)
        {
            M_Result result = new M_Result();
            try
            {
                HttpCookie cookie = Request.Cookies["LUOBO"];
                Int64 AudOID = Convert.ToInt64(cookie.Values["oid"]);
                string account = cookie.Values["account"].ToString();

                //TODO 审核说明
                string auditIntro = "";



                if (ssidBll.NoAuditSSID(ids, AudOID, account, auditIntro))
                    result.ResultCode = 0;
                else
                    result.ResultCode = 1;
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult BackAuditSSID(string ids)
        {
            M_Result result = new M_Result();
            try
            {
                HttpCookie cookie = Request.Cookies["LUOBO"];
                Int64 AudOID = Convert.ToInt64(cookie.Values["oid"]);
                string account = cookie.Values["account"].ToString();

                //TODO 审核说明
                string auditIntro = "";

                if (ssidBll.BackAuditSSID(ids, AudOID, account, auditIntro))
                    result.ResultCode = 0;
                else
                    result.ResultCode = 1;
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 删除审核
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult RemoveAuditSSID(string ids)
        {
            M_Result result = new M_Result();
            try
            {


                result.ResultCode = 0;
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = ex.Message;
            }

            return Json(result);
        }
        #endregion
    }
}
