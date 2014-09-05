using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.Model;
using LUOBO.Public;
using System.Xml;
using LUOBO.Entity;

namespace LUOBO.Controllers
{
    public class AppManageController : Controller
    {
        BLL.BLL_SYS_APPCOMPETENC acBll = new BLL.BLL_SYS_APPCOMPETENC();
        BLL.BLL_SYS_APPLICATION appBll = new BLL.BLL_SYS_APPLICATION();
        //
        // GET: /Shared/

        public ActionResult Default()
        {
            return View();
        }

        public JsonResult FindAppcompetencList(int size, Int64 curPage, string name, Int64 appID)
        {
            M_SYS_APPCOMPETENC mAC = acBll.Select(size, curPage, name, appID);
            return Json(mAC);
        }

        public ActionResult AppCompetenc(int type, Int64 id)
        {
            ViewData["type"] = type;
            SYS_APPCOMPETENC appcompetenc = new SYS_APPCOMPETENC();
            if (type == 1)
                appcompetenc = acBll.Select(id);
            ViewData["appcompetenc"] = appcompetenc;
            return View();
        }

        public JsonResult FindAPPList()
        {
            List<SYS_APPLICATION> list = appBll.Select();
            return Json(list);
        }

        public JsonResult AddAppCompetenc(SYS_APPCOMPETENC appcompetenc)
        {
            M_Result result = new M_Result();

            if (acBll.Insert(appcompetenc))
            {
                result.ResultCode = 0;
                result.ResultMsg = "添加成功!";
            }
            else
            {
                result.ResultCode = 1;
                result.ResultMsg = "添加失败!";
            }
            return Json(result);
        }

        public JsonResult UpdateAppCompetenc(SYS_APPCOMPETENC appcompetenc)
        {
            bool flag = acBll.Update(appcompetenc);
            return Json(flag);
        }

        public JsonResult ImportAppCompetenc(HttpPostedFileBase fileToUpload,Int64 AppID)
        {
            M_Result result = new M_Result();
            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 poid = Convert.ToInt64(cookie.Values["oid"]);

            if (fileToUpload == null)
            {
                result.ResultCode = 1;
                result.ResultMsg = "请选择文件后提交";
            }
            else
            {
                string fileType = Library.Instance().CheckTrueFileName(fileToUpload.FileName);
                if (fileType != ".xml")
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "未知的文件类型，请选择XML文件";
                }
                else
                {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(fileToUpload.InputStream);

                    List<SYS_APPCOMPETENC> list = new List<SYS_APPCOMPETENC>();
                    SYS_APPCOMPETENC item = null;
                    foreach (XmlNode node in xd["Root"].ChildNodes)
                    {
                        item = new SYS_APPCOMPETENC();
                        item.APPID = AppID;
                        item.NAME = node["NAME"].InnerText;
                        item.CONTROLLER = node["CONTROLLER"].InnerText;
                        item.ACTION = node["ACTION"].InnerText;
                        list.Add(item);
                    }

                    bool flag = false;
                    try
                    {
                        flag = acBll.Inserts(list);
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        result.ResultMsg = "\n" + ex.Message;
                    }
                    finally { }

                    if (flag)
                    {
                        result.ResultCode = 0;
                        result.ResultMsg = "注册成功，共成功导入" + list.Count + "应用权限！";
                    }
                    else
                    {
                        result.ResultCode = 1;
                        result.ResultMsg = "注册失败！\n" + result.ResultMsg;
                    }
                }
            }

            return Json(result);
        }

        public ActionResult Authorize()
        {
            return View();
        }
    }
}
