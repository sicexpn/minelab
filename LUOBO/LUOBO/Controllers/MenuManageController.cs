using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LUOBO.Entity;
using LUOBO.Model;
using LUOBO.BLL;
using LUOBO.Helper;

namespace LUOBO.Controllers
{
    public class MenuManageController : Controller
    {
        BLL_SYS_MENU mBll = new BLL_SYS_MENU();
        BLL_SYS_USER uBll = new BLL_SYS_USER();
        BLL_SYS_DICT dBll = new BLL_SYS_DICT();
        BLL_SYS_ORGANIZATION oBll = new BLL_SYS_ORGANIZATION();

        #region 菜单列表
        //
        // GET: /MenuManage/
        [SupportFilter]
        public ActionResult MenuList()
        {
            return View();
        }

        public List<M_SYS_MENU> CreateMenuTree(List<SYS_MENU> sList, Int64 pid, Int32 level)
        {
            List<M_SYS_MENU> list = new List<M_SYS_MENU>();
            M_SYS_MENU listItem = null;

            List<SYS_MENU> tmpList = PubFun.ChangeNewList<SYS_MENU, SYS_MENU>(sList.Where(c => c.M_LEVEL == level && c.M_PID == pid).ToList());
            foreach (SYS_MENU item in tmpList)
            {
                listItem = PubFun.ChangeNewItem<M_SYS_MENU, SYS_MENU>(item);
                listItem.SubMenuList = new List<M_SYS_MENU>();
                listItem.SubMenuList.AddRange(CreateMenuTree(sList, listItem.M_ID, level + 1));
                list.Add(listItem);
            }

            return list;
        }

        [SupportFilter]
        public JsonResult GetUserMenuList(string type)
        {
            M_Result result = new M_Result();
            List<SYS_MENU> list = new List<SYS_MENU>();
            try
            {
                HttpCookie cookie = Request.Cookies["LUOBO"];
                Int64 uid = Convert.ToInt64(cookie.Values["userid"]);
                if (uid == 0)
                {
                    if (type == "all")
                        list = mBll.GetAllMenuList();
                    else
                        list = mBll.GetMenuList();
                }
                else
                {
                    if (type == "all")
                        list = mBll.GetMenuByUIDAll(uid);
                    else
                        list = mBll.GetMenuByUID(uid);
                }
                result.ResultCode = 0;
                result.ResultOBJ = CreateMenuTree(list, -1, 1);
                if (list.Count == 0)
                    result.ResultMsg = "获取成功!无菜单信息.";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = "获取失败!:\r\n" + ex.Message;
            }
            finally { }

            return Json(result);
        }

        [SupportFilter]
        public JsonResult SetMenuIsOn(Int64 id,bool ison)
        {
            M_Result result = new M_Result();

            try
            {
                bool flag = mBll.SetMenuIsOn(id, ison);
                result.ResultCode = 0;
                if (!flag)
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "更新失败!:\r\n系统错误,情稍微再试!";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = "更新失败!:\r\n" + ex.Message;
            }
            finally { }

            return Json(result);
        }
        #endregion

        #region 菜单权限分配
        [SupportFilter]
        public ActionResult MenuAllocation()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 uid = Convert.ToInt64(cookie.Values["userid"]);
            ViewData["uid"] = uid;
            return View();
        }

        public List<SYS_ORGANIZATION> CreateOrgTree(List<SYS_ORGANIZATION> oList, Int64 pid)
        {
            List<SYS_ORGANIZATION> list = new List<SYS_ORGANIZATION>();
            List<SYS_ORGANIZATION> tmpList = oList.Where(c => c.PID == pid).ToList();
            foreach (SYS_ORGANIZATION item in tmpList)
            {
                list.Add(item);
                list.AddRange(CreateOrgTree(oList, item.ID));
            }
            return list;
        }

        public JsonResult GetOrgList()
        {
            M_Result result = new M_Result();
            List<SYS_ORGANIZATION> list = new List<SYS_ORGANIZATION>();

            try
            {
                list = CreateOrgTree(oBll.Select().OrgList, 0);
                result.ResultCode = 0;
                result.ResultOBJ = list;
                result.ResultMsg = "获取成功!";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = "获取失败!:\r\n" + ex.Message;
            }
            finally { }

            return Json(result);
        }

        [SupportFilter]
        public JsonResult GetUserListByOID()
        {
            M_Result result = new M_Result();
            List<SYS_USER> list = new List<SYS_USER>();

            try
            {
                HttpCookie cookie = Request.Cookies["LUOBO"];
                Int64 oid = Convert.ToInt64(cookie.Values["oid"]);
                Int64 uid = Convert.ToInt64(cookie.Values["userid"]);
                if (uid == 0)
                    list = uBll.Select();
                else
                    list = uBll.SelectByOID(oid);
                result.ResultCode = 0;
                result.ResultOBJ = list.Where(c => c.ID != uid).ToList();
                result.ResultMsg = "获取成功!";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = "获取失败!:\r\n" + ex.Message;
            }
            finally { }

            return Json(result);
        }

        [SupportFilter]
        public JsonResult SetUserMenuPermissions(string menuids, string userids)
        {
            M_Result result = new M_Result();
            try
            {
                bool flag = mBll.SetUserMenuPermissions(menuids, userids);
                result.ResultCode = 0;
                if (!flag)
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "更新失败!:\r\n系统错误,情稍微再试!";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultMsg = "更新失败!:\r\n" + ex.Message;
            }
            finally { }
            return Json(result);
        }
        #endregion

        #region 菜单添加编辑
        //[SupportFilter]
        public ActionResult MenuItem()
        {
            return View();
        }

        public JsonResult GetDicByMenuType()
        {
            M_Result result = new M_Result();

            try
            {
                List<SYS_DICT> dicinfo = dBll.Select().Where(c => c.CATEGORY == "菜单类型").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "菜单类型信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "菜单类型息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "菜单类型信息读取失败！\n" + ex.Message;
            }

            return Json(result);
        }

        public JsonResult GetDicByMenuICONType()
        {
            M_Result result = new M_Result();

            try
            {
                List<SYS_DICT> dicinfo = dBll.Select().Where(c => c.CATEGORY == "菜单图标类型").ToList();
                if (dicinfo != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "菜单图标类型信息读取成功！";
                    result.ResultOBJ = dicinfo;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "菜单图标类型息读取失败！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "菜单图标类型信息读取失败！\n" + ex.Message;
            }

            return Json(result);
        }

        public JsonResult GetMenuByID(Int64 mid)
        {
            M_Result result = new M_Result();

            try
            {
                SYS_MENU data = mBll.GetMenuByID(mid);
                if (data != null)
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "读取成功！";
                    result.ResultOBJ = data;
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "读取失败，无菜单信息！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "读取失败！\n" + ex.Message;
            }

            return Json(result);
        }

        public JsonResult SaveMenu(SYS_MENU menu)
        {
            M_Result result = new M_Result();

            try
            {
                if (mBll.SaveMenu(menu))
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.OK;
                    result.ResultMsg = "菜单保存成功！";
                }
                else
                {
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                    result.ResultMsg = "菜单保存失败！\n系统错误,情稍微再试！";
                }
            }
            catch (Exception ex)
            {
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "菜单保存失败！\n" + ex.Message;
            }

            return Json(result);
        }
        #endregion
    }
}
