using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.Entity;
using LUOBO.Helper;
using LUOBO.Public;
using LUOBO.Model;
using System.Configuration;

namespace LUOBO.Controllers
{
    public class ADManageController : Controller
    {
        private long org_id;            //=10035
        private String user_name;       //= "萝卜"
        //private String UserADPath = "ADUserFile";
        //private String AD_TempPath = "ADTemplet";
        private String ADTempletPath_WEB = ConfigurationSettings.AppSettings["ADTempletPath_WEB"];
        private String ADTempletPath_File = ConfigurationSettings.AppSettings["ADTempletPath_File"];
        private String UserADPath_WEB = ConfigurationSettings.AppSettings["UserADPath_WEB"];
        private String UserADPath_File = ConfigurationSettings.AppSettings["UserADPath_File"];

        BLL.BLL_ADManage adBll = new BLL.BLL_ADManage();
        //
        // GET: /ADManage/

        private void init()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            org_id = Convert.ToInt64(cookie.Values["oid"]);
            user_name = user_name = cookie.Values["username"];
        }

        public ActionResult Default()
        {
            return View();
        }

        #region 广告列表页

        /// <summary>
        /// AD列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ADList()
        {
            int aud_id = -1;
            try
            {
                aud_id = int.Parse(Request.QueryString["aud_id"]);
            }catch(Exception e){

            }

            ViewData["aud_id"] = aud_id;
            return View();
        }

        /// <summary>
        /// 查询AD列表
        /// </summary>
        /// <returns></returns>
        public JsonResult FindADList(int audit, int size, int curPage, String keystr)
        {
            //return Json(adBll.SelectADByPage(org_id, audit, size, curPage, keystr.Replace("'","").Trim()));
            try
            {
                init();

                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = adBll.SelectADByPage(org_id, audit, size, curPage, keystr.Replace("'", "").Trim());
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
        #endregion

        /// <summary>
        /// AD列表加编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ADListEdit()
        {
            int aud_id = -1;
            try
            {
                aud_id = int.Parse(Request.QueryString["aud_id"]);
            }
            catch (Exception e)
            {

            }

            ViewData["aud_id"] = aud_id;

            List<SYS_DICT> hList = Library.Instance().GetDicByCategory("行业");
            List<SelectListItem> hangye = new List<SelectListItem>();
            for (int i = 0; i < hList.Count; ++i)
            {
                hangye.Add(new SelectListItem
                {
                    Text = hList[i].NAME,
                    Value = hList[i].VALUE.ToString()
                });
            }
            //ViewData["ad_id"] = ad_id;
            ViewData["ad_type"] = hangye;
            List<SYS_ADTEMPLET> templet = adBll.SelectADTemplet();
            List<SelectListItem> ad_model = new List<SelectListItem>(); for (int i = 0; i < hList.Count; ++i)
            {
                ad_model.Add(new SelectListItem
                {
                    Text = templet[i].SADT_NAME,
                    Value = templet[i].SADT_ID.ToString()
                });
            }
            ViewData["ad_model"] = ad_model;
            return View();
        }


        #region 广告编辑页
        /// <summary>
        /// AD编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ADEdit(long ad_id)
        {
            List<SYS_DICT> hList = Library.Instance().GetDicByCategory("行业");
            List<SelectListItem> hangye = new List<SelectListItem>();
            for (int i = 0; i < hList.Count; ++i)
            {
                hangye.Add(new SelectListItem
                {
                    Text = hList[i].NAME,
                    Value = hList[i].VALUE.ToString()
                });
            }
            ViewData["ad_id"] = ad_id;
            ViewData["ad_type"] = hangye;

            List<SYS_ADTEMPLET> templet = adBll.SelectADTemplet();
            List<SelectListItem> ad_model = new List<SelectListItem>();
            for (int i = 0; i < templet.Count; ++i)
            {
                ad_model.Add(new SelectListItem
                {
                    Text = templet[i].SADT_NAME,
                    Value = templet[i].SADT_ID.ToString()
                });
            }
            ViewData["ad_model"] = ad_model;

            return View();
        }

        /// <summary>
        /// 模版文件列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTempletFiles(int ADT_ID)
        {
            try
            {
                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = adBll.GetTempletFiles(ADT_ID);
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
        /// 广告文件列表
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public JsonResult GetADFiles(long ad_id)
        {
            try
            {
                init();

                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = adBll.GetADFiles(ad_id, org_id);
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
        /// 获取广告详细信息
        /// </summary>
        /// <param name="AD_ID"></param>
        /// <returns></returns>
        public JsonResult GetADInfo(long AD_ID)
        {
            try
            {
                init();

                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = adBll.get_ADInfo(AD_ID);
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
        /// 获取广告发布方案分页内容
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult GetCaseFromPage(int page, int psize)
        {
            try
            {
                init();
                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = adBll.GetCaseFromPage(org_id, psize, page);
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
        /// 获取已上传图片列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetServerPics()
        {
            try
            {
                init();
                //String tmpPath = AppDomain.CurrentDomain.BaseDirectory + UserADPath + "/" + org_id;
                //String tmpPath = UserADPath_File + org_id;
                //if (!System.IO.Directory.Exists(tmpPath))
                //{
                //    System.IO.Directory.CreateDirectory(tmpPath);
                //    System.IO.Directory.CreateDirectory(tmpPath + "/UserPic");
                //}

                //String[] pics = System.IO.Directory.GetFiles(tmpPath + "/UserPic");
                //List<M_File> flist = new List<M_File>();
                //M_File tmpf;
                //for (int i = 0; i < pics.Length; ++i)
                //{
                //    tmpf = new M_File();
                //    tmpf.FileURL = pics[i].Replace(AppDomain.CurrentDomain.BaseDirectory, "/");
                //    tmpf.FileName = pics[i].Substring(pics[i].Replace("\\", "/").LastIndexOf("/") + 1);
                //    flist.Add(tmpf);
                //}
                List<M_File> flist = adBll.GetServerPics(org_id);

                M_Result result = new M_Result();
                result.ResultCode = 0;
                result.ResultOBJ = flist;
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
        /// 提交广告发布申请
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult PostAudit(long AD_ID, int AuditType, String IDS, int ascase, int iscopy)
        {
            try
            {
                init();
                M_Result result = new M_Result();
                String msg = adBll.PostAudit(AD_ID, org_id, AuditType, IDS, user_name, ascase, iscopy);
                if (msg == "ok")
                {
                    result.ResultCode = 0;
                }
                else
                {
                    result.ResultCode = 1;
                    result.ResultMsg = msg;
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

        #endregion

        #region 广告页保存
        public ActionResult ADSave()
        {
            init();
            AD_INFO adinfo = adBll.adModify(long.Parse(Request.Form["ad_id"]), org_id, Request.Form["ad_title"], Request.Form["ad_ssid"], int.Parse(Request.Form["ad_model"]), Request.Form["homepage"], int.Parse(Request.Form["ad_type"]), int.Parse(Request.Form["pubcount"]), Request.Form["ad_pubpath"]);
            //String tmpPath = AppDomain.CurrentDomain.BaseDirectory + UserADPath + "/" + org_id;
            String tmpPath = UserADPath_File + org_id;
            if (!System.IO.Directory.Exists(tmpPath))
            {
                System.IO.Directory.CreateDirectory(tmpPath);
                System.IO.Directory.CreateDirectory(tmpPath + "/UserPic");
            }
            String tpage = Request.Form["temppage"];
            if (tpage.Length > 0)
            {
                List<M_ADContentItem> templetitems = new List<M_ADContentItem>();
                M_ADContentItem item;
                String tmpS;
                for (int i = 0; i < Request.Files.Count; ++i)
                {
                    if (Request.Files[i].FileName.Length > 0)
                    {
                        tmpS = adinfo.AD_ID + "_" + i + "_" + DateTime.Now.ToString("yyMMddHHmmssfff") + Request.Files[i].FileName.Substring(Request.Files[i].FileName.LastIndexOf('.')).ToLower();
                        item = new M_ADContentItem();
                        item.TKey = Request.Files.AllKeys[i];
                        item.TValue = tmpS;
                        templetitems.Add(item);
                        Request.Files[i].SaveAs(tmpPath + "/UserPic/" + tmpS);
                    }
                }

                if (!System.IO.Directory.Exists(tmpPath + "/" + adinfo.AD_ID))
                {
                    //PubFun.CopyDirectory(AppDomain.CurrentDomain.BaseDirectory + AD_TempPath + "/" + adinfo.AD_Model, tmpPath + "/" + adinfo.AD_ID);
                    PubFun.CopyDirectory(ADTempletPath_File + adinfo.AD_Model, tmpPath + "/" + adinfo.AD_ID);
                }

                for (int i = 0; i < Request.Form.Count; ++i)
                {
                    if (Request.Form.AllKeys[i].StartsWith("Templet_"))
                    {
                        item = new M_ADContentItem();
                        item.TKey = Request.Form.AllKeys[i];
                        item.TValue = Request.Form[i];
                        templetitems.Add(item);
                    }
                }
                AD_Templet.WriteADFile(tmpPath + "/" + adinfo.AD_ID + "/" + tpage, templetitems, adinfo.AD_Title);
            }

            ViewData["upload"] = adinfo.AD_ID;
            return View();
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ADTempletPage()
        {
            return View();
        }

        /// <summary>
        /// AD审核进度页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ADAuditView()
        {
            return View();
        }

        /// <summary>
        /// AD发布历史页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ADPubHistory()
        {
            return View();
        }
    }
}
