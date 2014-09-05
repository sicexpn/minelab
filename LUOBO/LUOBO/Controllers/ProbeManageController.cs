using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.IO;
using LUOBO.Public;
using LUOBO.Model;
using LUOBO.Entity;
using LUOBO.Helper;

namespace LUOBO.Controllers
{
    public class ProbeManageController : Controller
    {
        BLL.BLL_PROBEAPManage apBll = new BLL.BLL_PROBEAPManage();
        BLL.BLL_SYS_PROBEORG orgBll = new BLL.BLL_SYS_PROBEORG();
        XmlDocument xd = new XmlDocument();

        #region 探测设备管理页面
        [SupportFilter]
        public ActionResult ProbeManage()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 oid = Convert.ToInt64(cookie.Values["oid"]);
            ViewData["OID"] = oid;
            return View();
        }


        /// <summary>
        /// AP管理的查询列表
        /// </summary>
        /// <param name="jgID"></param>
        /// <param name="benJGID"></param>
        /// <param name="startSerial"></param>
        /// <param name="endSerial"></param>
        /// <param name="mac"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="FPState"></param>
        /// <param name="curPage"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public JsonResult FindProbeList(Int64 jgID, Int64 benJGID, Int64? startSerial, Int64? endSerial, string mac, string startDate, string endDate, int? FPState, int curPage, int size)
        {
            //return Json(apBll.SelectAllAPByPage(jgID, benJGID, startSerial, endSerial, mac, startDate, endDate, FPState, curPage, size));
            return null;
        }

        /// <summary>
        /// AP设备废弃
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [SupportFilter]
        public JsonResult DisablesProbes(string ids)
        {
            //return Json(apBll.DisablesAP(ids));
            return null;
        }

        /// <summary>
        /// 查询子机构列表
        /// </summary>
        /// <param name="jgID"></param>
        /// <returns></returns>
        public JsonResult FindJGList(Int64 jgID)
        {
            return Json(orgBll.SelectSub(jgID));
        }
        #endregion

        #region 探测设备注册页面
        [SupportFilter]
        public ActionResult ProbeRegister()
        {
            return View();
        }


        /// <summary>
        /// 单条插入AP设备
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="serial"></param>
        /// <param name="model"></param>
        /// <param name="firmVersion"></param>
        /// <param name="manuf"></param>
        /// <param name="maxssid"></param>
        /// <param name="purchaser"></param>
        /// <param name="apctID"></param>
        /// <param name="support3g"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [SupportFilter]
        public JsonResult InsertProbe(string alias, string mac, Int64 serial, string model, string firmVersion, string manuf, int maxssid, string purchaser, Int64 apctID, string support3g, string description)
        {
            M_Result result = new M_Result();
            xd.Load(HttpContext.Server.MapPath("~") + "DefaultValue.xml");

            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 oid = Convert.ToInt64(cookie.Values["oid"]);

            Entity.SYS_APDEVICE ap = new Entity.SYS_APDEVICE();
            ap.ID = -1;
            ap.MAC = mac;
            ap.SERIAL = serial;
            ap.MODEL = model;
            ap.ALIAS = alias;
            ap.FIRMWAREVERSION = firmVersion;
            ap.MANUFACTURER = manuf;
            ap.MAXSSIDNUM = maxssid;
            ap.PURCHASER = purchaser;
            ap.APCTID = apctID;
            ap.DESCRIPTION = description;
            ap.DEVICESTATE = 1;
            ap.SUPPORT3G = support3g == "true" ? true : false;
            ap.REGDATE = DateTime.Now;
            ap.HBINTERVAL = Convert.ToInt64(xd["Root"]["AP"]["HBInterval"].InnerText);
            ap.DATAINTERVAL = Convert.ToInt64(xd["Root"]["AP"]["DataInterval"].InnerText);
            ap.ISREBOOT = false;
            ap.ISUPDATE = false;
            try
            {
                //List<Entity.SYS_APDEVICE> checkedList = apBll.CheckMac(ap);
                List<Entity.SYS_APDEVICE> checkedList = new List<SYS_APDEVICE>();
                if (checkedList.Count == 1)
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "该Mac已经注册了，请重新输入";
                }
                //else if (apBll.RegAPDvice(ap, oid))
                //    result.ResultCode = 0;
                else
                    result.ResultCode = 1;
            }
            catch (Exception e)
            {
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// 批量插入AP设备
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="model"></param>
        /// <param name="firmVersion"></param>
        /// <param name="manuf"></param>
        /// <param name="maxssid"></param>
        /// <param name="purchaser"></param>
        /// <param name="apctID"></param>
        /// <param name="description"></param>
        /// <param name="support3g"></param>
        /// <returns></returns>
        [SupportFilter]
        public JsonResult InsertProbes(HttpPostedFileBase fileToUpload, string model, string firmVersion, string manuf, int maxssid, string purchaser, Int64 apctID, string description, string support3g)
        {
            M_Result result = new M_Result();

            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 oid = Convert.ToInt64(cookie.Values["oid"]);

            if (fileToUpload == null)
            {
                result.ResultCode = 1;
                result.ResultMsg = "请选择文件后提交";
            }
            else
            {
                List<Entity.SYS_APDEVICE> apList = new List<Entity.SYS_APDEVICE>();
                Entity.SYS_APDEVICE item = null;
                string fileType = Library.Instance().CheckTrueFileName(fileToUpload.FileName);
                StreamReader sr = new StreamReader(fileToUpload.InputStream);
                if (fileType != ".txt")
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "未知的文件类型，请选择txt文本文件";
                }
                else// if(fileType == CustomEnum.FileExtension.TXT)
                {
                    xd.Load(HttpContext.Server.MapPath("~") + "DefaultValue.xml");


                    Int64 HBInterval = Convert.ToInt64(xd["Root"]["AP"]["HBInterval"].InnerText);
                    Int64 dataInterval = Convert.ToInt64(xd["Root"]["AP"]["DataInterval"].InnerText);
                    string[] strs = sr.ReadToEnd().Split('\n');
                    string[] sTmp;
                    foreach (string s in strs)
                    {
                        sTmp = s.Split('\t');
                        if (sTmp.Count() == 0)
                            continue;
                        item = new Entity.SYS_APDEVICE();
                        item.ID = -1;
                        item.MAC = sTmp[0];
                        item.SERIAL = Convert.ToInt64(sTmp[1]);
                        item.APCTID = apctID;
                        item.DATAINTERVAL = dataInterval;
                        item.DESCRIPTION = description;
                        item.DEVICESTATE = 1;
                        item.FIRMWAREVERSION = firmVersion;
                        item.HBINTERVAL = HBInterval;
                        item.ISREBOOT = false;
                        item.ISUPDATE = false;
                        item.MANUFACTURER = manuf;
                        item.MAXSSIDNUM = maxssid;
                        item.MODEL = model;
                        item.PURCHASER = purchaser;
                        item.REGDATE = DateTime.Now;
                        item.SUPPORT3G = support3g == "true" ? true : false;

                        if (sTmp.Count() >= 3)
                        {
                            item.ALIAS = sTmp[2];
                            if (sTmp.Count() >= 4)
                            {
                                item.LAT = Convert.ToDouble(sTmp[3].Split(',')[0]);
                                item.LON = Convert.ToDouble(sTmp[3].Split(',')[1]);
                            }
                        }
                        apList.Add(item);
                    }
                    //去掉重复数据的MAC结果
                    var dist = apList.Distinct(c => c.MAC).ToList();

                    //List<Entity.SYS_APDEVICE> checkedList = apBll.CheckMacs(dist);
                    List<Entity.SYS_APDEVICE> checkedList = new List<SYS_APDEVICE>();

                    if (checkedList.Count > 0)
                    {
                        result.ResultCode = 1;
                        result.ResultMsg = "注册失败，如下的Mac已经注册，请处理后再导入。\n";

                        result.ResultMsg += checkedList.ToString("MAC");
                    }
                    else
                    {
                        //if (apBll.RegAPDvices(dist, oid))
                        if (true)
                        {
                            result.ResultCode = 0;
                            if (dist.Count < apList.Count)
                            {
                                result.ResultMsg = "注册成功，其中有" + (apList.Count - dist.Count) + "个Mac重复，已被过滤，共成功导入" + dist.Count + "个设备";
                            }
                            else
                            {
                                result.ResultMsg = "注册成功，共成功导入" + dist.Count + "个设备";
                            }
                        }
                        else
                        {
                            result.ResultCode = 1;
                            result.ResultMsg = "注册失败";
                        }
                    }
                }
            }
            return Json(result);
        }
        #endregion

        #region 探测设备分配页面
        [SupportFilter]
        public ActionResult ProbeDistribution()
        {
            return View();
        }

        public JsonResult GetProbeListByOID(Int64 OID, bool isInvalid)
        {
            M_Result result = new M_Result();

            result.ResultCode = 0;
            //result.ResultOBJ = apBll.SelectAPByOID(OID, isInvalid);

            return Json(result);
        }

        public JsonResult AllotProbe(SYS_AP_VIEW_ARR list)
        {
            bool flag = false;

            //是否创建SSID
            if (Request.Cookies["LUOBO"].Values["oid"] == "0")
                flag = true;
            //flag = apBll.AllotAP(list.Items.ToList(), flag);

            return Json(flag);
        }

        public JsonResult ImportProbe(HttpPostedFileBase fileToUpload, Int64 OID)
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
                if (fileType != ".txt")
                {
                    result.ResultCode = 1;
                    result.ResultMsg = "未知的文件类型，请选择txt文本文件";
                }
                else// if(fileType == CustomEnum.FileExtension.TXT)
                {
                    //List<string[]> strList = new List<string[]>();
                    StreamReader sr = new StreamReader(fileToUpload.InputStream);
                    List<string[]> strList = sr.ReadToEnd().Split('\n').Select(c => c.Split('\t')).ToList();
                    List<string[]> dist = strList.Distinct(c => c[0]).ToList();

                    //List<SYS_AP_VIEW> apList = apBll.SelectAPByOID(poid);//所有拥有的AP
                    List<SYS_AP_VIEW> apList = new List<SYS_AP_VIEW>();//所有拥有的AP
                    List<SYS_AP_VIEW> invalidAPList = apList.Where(c => c.EDATE < DateTime.Now).ToList();//过期的AP
                    List<SYS_AP_VIEW> availableAPList = apList.Where(c => c.EDATE >= DateTime.Now).ToList();//可用的AP

                    string invalidMAC = "";
                    foreach (string[] arr in dist)
                    {
                        if (invalidAPList.Where(c => c.MAC == arr[0]).Count() > 0)
                        {
                            if (invalidMAC != "")
                                invalidMAC += ",";
                            invalidMAC += arr[0];
                        }
                    }
                    string nohaveMAC = "";
                    foreach (string[] arr in dist)
                    {
                        if (availableAPList.Where(c => c.MAC == arr[0]).Count() == 0)
                        {
                            if (nohaveMAC != "")
                                nohaveMAC += ",";
                            nohaveMAC += arr[0];
                        }
                    }

                    if (invalidMAC != "" || nohaveMAC != "")
                    {
                        result.ResultCode = 1;
                        result.ResultMsg = "注册失败！\n";
                        if (nohaveMAC != "")
                            result.ResultMsg += "如下的Mac无效或已分配，请处理后再导入。\n" + nohaveMAC + "\n";
                        if (invalidMAC != "")
                            result.ResultMsg += "如下的Mac已经过期，请处理后再导入。\n" + invalidMAC + "\n";
                    }
                    else
                    {
                        List<SYS_AP_VIEW> list = new List<SYS_AP_VIEW>();
                        SYS_AP_VIEW item = null;
                        foreach (string[] arr in dist)
                        {
                            item = availableAPList.Where(c => c.MAC == arr[0]).FirstOrDefault();
                            item.POID = item.OID;
                            item.OID = OID;
                            item.SDATE = DateTime.Parse(arr[2]);
                            item.EDATE = DateTime.Parse(arr[3]);
                            item.SSIDNUM = Convert.ToInt32(arr[4]);
                            list.Add(item);
                        }

                        //是否创建SSID
                        bool flag = false;
                        if (poid == 0)
                            flag = true;
                        try
                        {
                            //flag = apBll.AllotAP(list, flag);
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
                            {
                                result.ResultMsg = "注册成功，其中有" + (strList.Count - dist.Count) + "个Mac重复，已被过滤，共成功导入" + dist.Count + "个设备";
                            }
                            else
                            {
                                result.ResultMsg = "注册成功，共成功导入" + dist.Count + "个设备";
                            }
                        }
                        else
                        {
                            result.ResultCode = 1;
                            result.ResultMsg = "注册失败！" + result.ResultMsg;
                        }
                    }
                }
            }

            return Json(result);
        }
        #endregion

        #region 探测设备回收页面
        [SupportFilter]
        public ActionResult ProbeBack()
        {
            HttpCookie cookie = Request.Cookies["LUOBO"];
            Int64 oid = Convert.ToInt64(cookie.Values["oid"]);
            ViewData["OID"] = oid;
            return View();
        }

        /// <summary>
        /// AP回收的查询列表
        /// </summary>
        public JsonResult FindProbeListForBack(Int64 jgID, int curPage, int size)
        {
            //return Json(apBll.SelectBackAPByPage(jgID, curPage, size));
            return null;
        }

        /// <summary>
        /// 回收AP设备
        /// </summary>
        [SupportFilter]
        public JsonResult BackProbe(int jgID, string ids)
        {
            //return Json(apBll.BackAp(jgID, ids));
            return null;
        }
        #endregion

        #region 探测设备设置页面
        [SupportFilter]
        public ActionResult ProbeSetting()
        {
            //Int64 id = -1;
            //Int64.TryParse(Request.QueryString["APID"], out id);
            //var tmp = apBll.SelectAPViewByAPID(id);
            //if (tmp.ID != 0)
            //{
            //    ViewData["APDEVICE"] = tmp;
            //}
            //else
            //    return Redirect("/APManage/APmanage");
            return View();
        }

        public JsonResult FindProbeViewByAPID(Int64 apID)
        {
            //return Json(apBll.SelectAPViewByAPID(apID));
            return null;
        }

        public JsonResult SaveProbeBase(Int64 ID, Int64 SERIAL, string MODEL, string MANUFACTURER, string PURCHASER, string FIRMWAREVERSION, Int64 MAXSSIDNUM, bool SUPPORT3G, Int64 APCTID, string DESCRIPTION, Int64 HBINTERVAL, Int64 DATAINTERVAL, bool? ISREBOOT)
        {
            M_Result result = new M_Result();
            try
            {
                //if (apBll.UpdateAP(ID, SERIAL, MODEL, MANUFACTURER, PURCHASER, FIRMWAREVERSION, MAXSSIDNUM, SUPPORT3G, APCTID, DESCRIPTION, HBINTERVAL, DATAINTERVAL, ISREBOOT))
                if (true)
                    result.ResultCode = 0;
                else
                    result.ResultCode = 1;
            }
            catch (Exception e)
            {
                result.ResultCode = 1;
                result.ResultMsg = e.Message;
            }
            return Json(result);
        }

        public JsonResult SubmitSetting(Int64 apid)
        {
            M_Result result = new M_Result();
            try
            {
                //SYS_AP_VIEW ap = apBll.SelectAPViewByAPID(apid);
                //List<SYS_SSID> ssid = apBll.SelectSSIDByAPID(apid);
                SYS_AP_VIEW ap = new SYS_AP_VIEW();
                List<SYS_SSID> ssid = new List<SYS_SSID>();
                if (ap.ISUPDATE || ssid.Select(c => c.ISUPDATE).Contains(true))
                {
                    //if (apBll.CreateSettingFile(ap, ssid))
                    if (true)
                    {
                        result.ResultCode = 0;
                        result.ResultMsg = "已生成配置文件";
                    }
                    else
                    {
                        result.ResultCode = 1;
                        result.ResultMsg = "生成中遇到错误";
                    }
                }
                else
                {
                    result.ResultMsg = "没有需要发布到设备的更新";
                }
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
