using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.BLL;
using LUOBO.Entity;
using LUOBO.Model;
using LUOBO.Public;
using LUOBO.Helper;
using System.IO;

namespace LUOBO.Controllers
{
    public class UsersManageController : Controller
    {
        //
        // GET: /UsersManage/
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        BLL_SYS_APPCOMPETENC acBll = new BLL_SYS_APPCOMPETENC();
        BLL_SYS_USERAPPCOMPETENCE uacBll = new BLL_SYS_USERAPPCOMPETENCE();

        [SupportFilter]
        public ActionResult Default()
        {
            return View();
        }
        
        public JsonResult FindUserList(string jgName, string userName, int size, Int32 curPage, Int32 userType)
        {
            M_SYS_USER mUser = uBll.Select(size, curPage, jgName, userName, userType);
            return Json(mUser);
        }
        
        public JsonResult DisableUser(string ids)
        {
            return Json(uBll.Disables(ids));
        }
        [SupportFilter]
        public ActionResult Search()
        {
            return View();
        }
        
        public JsonResult GetUserTypeList()
        {
            List<SYS_DICT> list = Library.Instance().GetDicByCategory("用户类别");
            return Json(list);
        }
        [SupportFilter]
        public ActionResult User(int type, Int64 id)
        {
            ViewData["type"] = type;
            SYS_USER user = new SYS_USER();
            if (type == 1)
                user = uBll.Select(id);
            ViewData["user"] = user;
            return View();
        }

        public JsonResult AddUser(SYS_USER user)
        {
            M_Result result = new M_Result();

            if (uBll.Select(user.ACCOUNT))
            {
                result.ResultCode = 1;
                result.ResultMsg = "添加失败,用户名已存在!";
                return Json(result);
            }

            user.CREATETIME = DateTime.Now;
            user.STATE = true;
            if (uBll.Insert(user))
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

        public JsonResult UpdateUser(SYS_USER user)
        {
            bool flag = uBll.Update(user);
            return Json(flag);
        }

        public JsonResult ImportUser(HttpPostedFileBase fileToUpload, Int64 OID)
        {
            M_Result result = new M_Result();
            
            if (fileToUpload == null)
            {
                result.ResultCode = 1;
                result.ResultMsg = "请选择文件后提交";
            }
            else
            {
                string fileType = Library.Instance().CheckTrueFileName(fileToUpload.FileName);
                if (fileType != ".txt")
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "未知的文件类型，请选择txt文本文件";
                }
                else// if(fileType == CustomEnum.FileExtension.TXT)
                {
                    StreamReader sr = new StreamReader(fileToUpload.InputStream);
                    List<string[]> strList = sr.ReadToEnd().Replace("\r\n", "\n").Split('\n').Select(c => c.Split('\t')).ToList();
                    List<string[]> dist = strList.Distinct(c => c[0]).ToList();

                    List<SYS_USER> hasUserList = uBll.SelectByACCOUNTs(dist.Select(c => c[1]).ToList());
                    if (hasUserList.Count > 0)
                    {
                        result.ResultCode = 1;
                        result.ResultMsg = "添加失败！\n";
                        result.ResultMsg += "如下的用户名重复，请处理后再导入。\n" + hasUserList.ToString("ACCOUNT", ",") + "\n";
                    }
                    else
                    {
                        List<SYS_USER> list = new List<SYS_USER>();
                        SYS_USER user = null;

                        foreach (string[] item in dist)
                        {
                            user = new SYS_USER();
                            user.USERNAME = item[0];
                            user.ACCOUNT = item[1];
                            user.PWD = item[2];
                            user.CONTACT = item[3];
                            user.CREATETIME = DateTime.Now;
                            user.USERTYPE = 2;
                            user.STATE = true;
                            list.Add(user);
                        }

                        bool flag = false;
                        try
                        {
                            flag = uBll.Inserts(list);
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
                            if (dist.Count < strList.Count)
                                result.ResultMsg = "添加成功，其中有" + (strList.Count - dist.Count) + "个用户名重复，已被过滤，共成功导入" + dist.Count + "个用户";
                            else
                                result.ResultMsg = "添加成功，共成功导入" + dist.Count + "个用户";
                        }
                        else
                        {
                            result.ResultCode = 1;
                            result.ResultMsg = "添加失败！" + result.ResultMsg;
                        }
                    }
                }
            }

            return Json(result);
        }

        public ActionResult Authorize(Int64 id)
        {
            ViewData["uid"] = id;
            return View();
        }

        public JsonResult FindAppcompetencList()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 userid = Convert.ToInt64(cookie.Values["userid"]);

            List<SYS_APPCOMPETENC_VIEW> list = acBll.Select_view(userid);
            return Json(list);
        }

        public string GetUserAppCompetence(Int64 uid)
        {
            string result = "";

            List<SYS_USERAPPCOMPETENCE> list = uacBll.SelectByUID(uid);
            foreach (SYS_USERAPPCOMPETENCE item in list)
            {
                if (result != "")
                    result += ",";
                result += "$" + item.APPCID + "$";
            }

            return result;
        }

        public JsonResult AddUserAppCompetence(string appcids, Int64 uid)
        {
            M_Result result = new M_Result();

            List<SYS_USERAPPCOMPETENCE> list = new List<SYS_USERAPPCOMPETENCE>();
            SYS_USERAPPCOMPETENCE uac;
            foreach (string item in appcids.Split(','))
            {
                uac = new SYS_USERAPPCOMPETENCE();
                uac.APPCID = Convert.ToInt64(item);
                uac.UID = uid;
                list.Add(uac);
            }

            try
            {
                if (uacBll.Inserts(list))
                {
                    result.ResultCode = 0;
                    result.ResultMsg = "授权成功!";
                }
                else
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "授权失败!";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = "\n" + ex.Message;
            }
            finally { }

            return Json(result);
        }
    }
}
