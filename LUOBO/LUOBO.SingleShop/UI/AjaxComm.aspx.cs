using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace LUOBO.SingleShop.UI
{
    public partial class AjaxComm : System.Web.UI.Page
    {
        string serverpath = ConfigurationSettings.AppSettings["serverpath"];
        string serverRadiusPath = ConfigurationSettings.AppSettings["serverRadiusPath"];
        string weixinPath = ConfigurationSettings.AppSettings["weixinPath"];

        protected void Page_Load(object sender, EventArgs e)
        {
            string urlStr = "";

            switch (Request.Form["type"])
            {
                case "Login": //登陆
                    urlStr = serverpath + "/BusinessService/Login";
                    break;
                case "Logout": //登出
                    urlStr = serverpath + "/BusinessService/Logout/" + Request.Form["token"];
                    break;
                case "HasAccount": //登出
                    urlStr = serverpath + "/BusinessService/UserManage/HasAccount/" + Request.Form["token"];
                    break;
                case "GetPassword": //获取随机密码
                    urlStr = serverRadiusPath + "/LUOBOService/UserLogin/GetPassword";
                    break;
                case "APManage/GetDeviceList": //获取AP列表
                    urlStr = serverpath + "/BusinessService/APManage/GetDeviceList/" + Request.Form["token"];
                    break;
                case "APManage/GetAPListByOID": //获取AP列表，不包含子机构设备
                    urlStr = serverpath + "/BusinessService/APManage/GetAPListByOID/" + Request.Form["token"];
                    break;
                case "APManage/GetDeviceListByAdid": //获取AP列表
                    urlStr = serverpath + "/BusinessService/APManage/GetDeviceListByAdid/" + Request.Form["ID"] + "/" + Request.Form["token"];
                    break;
                case "APManage/GetAPInfoByID": //获取AP信息
                    urlStr = serverpath + "/BusinessService/APManage/GetAPInfoByID/" + Request.Form["token"];
                    break;
                case "APManage/GetSSIDNumByMAC": //获取AP的SSID数量
                    urlStr = serverpath + "/BusinessService/APManage/GetSSIDNumByMAC/" + Request.Form["token"];
                    break;
                case "SSIDManage/GetSSIDListByAPID": //根据APID获取SSID列表
                    urlStr = serverpath + "/BusinessService/SSIDManage/GetSSIDListByAPID/" + Request.Form["token"];
                    break;
                case "SSIDManage/SaveSSID": //保存SSID信息
                    urlStr = serverpath + "/BusinessService/SSIDManage/SaveSSID/" + Request.Form["token"];
                    break;
                case "SSIDManage/DisableSSID": //禁用SSID
                    urlStr = serverpath + "/BusinessService/SSIDManage/DisableSSID/" + Request.Form["token"];
                    break;
                case "SSIDManage/AddSSID": //添加SSID
                    urlStr = serverpath + "/BusinessService/SSIDManage/AddSSID/" + Request.Form["token"];
                    break;
                case "GetADInfo": //获取广告信息
                    urlStr = serverpath + "/BusinessService/ADManage/GetADInfo/" + Request.Form["ad_id"] + "/" + Request.Form["token"];
                    break;
                case "GetADInfoAndFreeHost": //获取广告信息
                    urlStr = serverpath + "/BusinessService/ADManage/GetADInfoAndFreeHost/" + Request.Form["ad_id"] + "/" + Request.Form["token"];
                    break;
                case "GetADFiles": //获取广告文件内容
                    urlStr = serverpath + "/BusinessService/ADManage/GetADFiles/" + Request.Form["ad_id"] + "/" + Request.Form["token"];
                    break;
                case "GetTempletList": //获取广告模版列表
                    urlStr = serverpath + "/BusinessService/ADManage/GetTempletList/" + Request.Form["token"];
                    break;
                case "GetTempletFiles": //获取广告模版文件内容
                    urlStr = serverpath + "/BusinessService/ADManage/GetTempletFiles/" + Request.Form["temp_ID"] + "/" + Request.Form["token"];
                    break;
                case "GetADList": //获取用户广告列表
                    urlStr = serverpath + "/BusinessService/ADManage/GetADList/" + Request.Form["token"];
                    break;
                case "GetADPubList": //获取用户广告列表，包含发布点数
                    urlStr = serverpath + "/BusinessService/ADManage/GetADPubList/" + Request.Form["token"];
                    break;
                case "GetServerPics": //获取用户已上传图片列表
                    urlStr = serverpath + "/BusinessService/ADManage/GetServerPics/" + Request.Form["token"];
                    break;
                case "PostADAudit": //提交广告审核
                    urlStr = serverpath + "/BusinessService/ADManage/PostADAudit/" + Request.Form["token"];
                    break;
                case "GetExtProperty": //获取机构扩展信息
                    urlStr = serverpath + "/BusinessService/GetExtProperty/" + Request.Form["token"];
                    break;
                case "GetLoginProperty": //获取机构扩展信息
                    urlStr = serverpath + "/BusinessService/GetLoginProperty/" + Request.Form["token"];
                    break;
                case "SaveExtProperty": //修改机构扩展信息
                    urlStr = serverpath + "/BusinessService/SaveExtProperty/" + Request.Form["token"];
                    break;
                #region 字典信息
                case "GetDicByHangYe": //获取行业信息
                    urlStr = serverpath + "/BusinessService/GetDicByHangYe/" + Request.Form["token"];
                    break;
                case "GetDicByMianJi": //获取面积信息
                    urlStr = serverpath + "/BusinessService/GetDicByMianJi/" + Request.Form["token"];
                    break;
                case "GetDicByAuditState": //获取审核申请状态
                    urlStr = serverpath + "/BusinessService/GetDicByAuditState/" + Request.Form["token"];
                    break;
                case "GetDicByFreeHost": //获取默认放行域名
                    urlStr = serverpath + "/BusinessService/GetDicByFreeHost/" + Request.Form["token"];
                    break;
                #endregion
                case "ADManage/TarAD": //广告打包
                    urlStr = serverpath + "/BusinessService/ADManage/TarAD/" + Request.Form["token"];
                    break;
                #region 统计
                case "ReportStatistical": //记录统计信息
                    urlStr = serverpath + "/BusinessService/ReportStatistical";
                    break;
                case "GetPeopleCount": //获取首页统计数据
                    urlStr = serverpath + "/BusinessService/Statistical/GetPeopleCount/" + Request.Form["token"];
                    break;
                case "StatisticalPeople_Map": //人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/People_Map/" + Request.Form["token"];
                    break;
                case "StatisticalOnline": //在线人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/Online/" + Request.Form["token"];
                    break;
                case "StatisticalPeople": //在线人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/People/" + Request.Form["token"];
                    break;
                case "StatisticalSSID": //在线人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/SSID/" + Request.Form["token"];
                    break;
                case "StatisticalAD": //在线人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/AD/" + Request.Form["token"];
                    break;
                case "GetSSIDPeopleStatistical": //SSID人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/GetSSIDPeopleStatistical/" + Request.Form["token"];
                    break;
                case "GetSSIDUseTimeStatistical": //SSID使用时长统计
                    urlStr = serverpath + "/BusinessService/Statistical/GetSSIDUseTimeStatistical/" + Request.Form["token"];
                    break;
                case "GetSSIDTrafficStatistical": //SSID流量统计
                    urlStr = serverpath + "/BusinessService/Statistical/GetSSIDTrafficStatistical/" + Request.Form["token"];
                    break;
                case "GetAPOfADStatistical": //广告访问人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/GetAPOfADStatistical/" + Request.Form["token"];
                    break;
                case "GetAuthenticationPeopleStatistical": //认证类型人数统计
                    urlStr = serverpath + "/BusinessService/Statistical/GetAuthenticationPeopleStatistical/" + Request.Form["token"];
                    break;
                case "SelectTowHourIntervalPeopleCount": //每两小时的访问人次统计
                    urlStr = serverpath + "/BusinessService/Statistical/SelectTowHourIntervalPeopleCount/" + Request.Form["token"];
                    break;
                case "SelectTowHourIntervalModelCount": //每两小时的机型访问人次统计
                    urlStr = serverpath + "/BusinessService/Statistical/SelectTowHourIntervalModelCount/" + Request.Form["token"];
                    break;
                case "SelectTowHourIntervalSSIDCount": //每两小时的SSID访问人次统计
                    urlStr = serverpath + "/BusinessService/Statistical/SelectTowHourIntervalSSIDCount/" + Request.Form["token"];
                    break;
                #endregion
                #region 分享
                case "ShareCount":
                    urlStr = serverpath + "/BusinessService/ShareCount";
                    break;
                case "ShareAD":
                    urlStr = serverpath + "/BusinessService/ShareAD";
                    break;
                #endregion
                #region 状态页
                case "GetAPListForState"://状态页设备列表
                    urlStr = serverpath + "/BusinessService/APManage/GetAPListForState/" + Request.Form["token"];
                    break;
                case "GetUserForState"://状态页用户列表
                    urlStr = serverpath + "/BusinessService/APManage/GetUserForState/" + Request.Form["token"];
                    break;
                case "GetAdInfoByCallingID"://用户状态页广告信息
                    urlStr = serverpath + "/BusinessService/Statistical/GetAdInfoByCallingID/" + Request.Form["token"];
                    break;
                case "GetVisitOLInfoByCalingID"://用户状态页用户上网信息
                    urlStr = serverpath + "/BusinessService/Statistical/GetVisitOLInfoByCalingID/" + Request.Form["token"];
                    break;
                case "GetCalingAuthentication"://用户状态页用户上网信息
                    urlStr = serverpath + "/BusinessService/Statistical/GetCalingAuthentication/" + Request.Form["token"];
                    break;
                #endregion
                #region 安全页
                case "GetWarnInfo": //获取警告列表
                    urlStr = serverpath + "/BusinessService/GetWarnInfo/" + Request.Form["token"];
                    break;
                case "GetWarnGraph": //获取警告列表
                    urlStr = serverpath + "/BusinessService/GetWarnGraph/" + Request.Form["token"];
                    break;
                case "GetAlertListByMAC": //获取警告列表
                    urlStr = serverpath + "/BusinessService/GetAlertListByMAC/" + Request.Form["mac"] + "/" + Request.Form["token"];
                    break;
                case "GetAlertCount": //获取警告条数
                    urlStr = serverpath + "/BusinessService/GetAlertCount/" + Request.Form["token"];
                    break;
                case "GetAlertListNotHandle": //获取未处理的警告列表
                    urlStr = serverpath + "/BusinessService/GetAlertListNotHandle/" + Request.Form["token"];
                    break;
                case "AddWhiteList": //添加白名单
                    urlStr = serverpath + "/BusinessService/AddWhiteList/" + Request.Form["token"];
                    break;
                case "DelWhiteList": //删除白名单
                    urlStr = serverpath + "/BusinessService/DelWhiteList/" + Request.Form["token"];
                    break;
                case "GetWhiteList": //获取白名单列表
                    urlStr = serverpath + "/BusinessService/GetWhiteList/" + Request.Form["token"];
                    break;
                case "AddAlertKeyWord": //添加告警关键词
                    urlStr = serverpath + "/BusinessService/AddAlertKeyWord/" + Request.Form["token"];
                    break;
                case "DelAlertKeyWord": //删除告警关键词
                    urlStr = serverpath + "/BusinessService/DelAlertKeyWord/" + Request.Form["token"];
                    break;
                case "GetAlertKeyWord": //获取告警关键词列表
                    urlStr = serverpath + "/BusinessService/GetAlertKeyWord/" + Request.Form["token"];
                    break;
                case "GetFilterSSIDInfo": //获取过滤排序之后的SSID信息列表
                    urlStr = serverpath + "/BusinessService/GetFilterSSIDInfo/" + Request.Form["page"] + "/" + Request.Form["size"] + "/" + Request.Form["token"];
                    break;
                case "GetFilterSSIDInfoByAPID": //获取过滤排序之后的SSID信息列表根据APID
                    urlStr = serverpath + "/BusinessService/GetFilterSSIDInfoByAPID/" + Request.Form["page"] + "/" + Request.Form["size"] + "/" + Request.Form["token"];
                    break;
                case "GetSameSSIDInfo": //获取同名SSID信息列表
                    urlStr = serverpath + "/BusinessService/GetSameSSIDInfo/" + Request.Form["token"];
                    break;
                case "GetAPContactByOID": //根据机构获取AP联系人
                    urlStr = serverpath + "/BusinessService/GetAPContactByOID/" + Request.Form["token"];
                    break;
                case "DelAPContact": //删除A联系人
                    urlStr = serverpath + "/BusinessService/DelAPContact/" + Request.Form["token"];
                    break;
                case "AddAPContact": //添加AP联系人
                    urlStr = serverpath + "/BusinessService/AddAPContact/" + Request.Form["token"];
                    break;
                case "SaveAPContact": //保存AP联系人
                    urlStr = serverpath + "/BusinessService/SaveAPContact/" + Request.Form["token"];
                    break;
                case "GetAPNearCountByOID"://获取告警列表统计  元素0:可疑,元素1:中文,元素:2新增
                    urlStr = serverpath + "/BusinessService/GetAPNearCountByOID/" + Request.Form["token"];
                    break;
                case "GetAPNearCountByAPID"://获取告警列表统计  元素0:可疑,元素1:中文,元素:2新增
                    urlStr = serverpath + "/BusinessService/GetAPNearCountByAPID/" + Request.Form["token"];
                    break;
                case "ProcessForWhiteList":
                    urlStr = serverpath + "/BusinessService/ProcessForWhiteList/" + Request.Form["token"];
                    break;
                case "ProcessForNotice":
                    urlStr = serverpath + "/BusinessService/ProcessForNotice/" + Request.Form["token"];
                    break;
                #endregion
                case "GetAPListForGIS": //获取地图上显示的设备信息
                    urlStr = serverpath + "/BusinessService/APManage/GetAPListForGIS/" + Request.Form["token"];
                    break;
                case "GetOrganizationInfoByName"://获取机构信息,根据名称
                    urlStr = serverpath + "/BusinessService/OrganizationManage/GetOrganizationInfoByName/" + Request.Form["token"];
                    break;
                case "GetOrganizationList"://获取机构和子机构信息
                    urlStr = serverpath + "/BusinessService/OrganizationManage/GetOrganizationList/" + Request.Form["token"];
                    break;
                #region 安装
                case "GetChainList"://获取连锁店信息列表
                    urlStr = serverpath + "/BusinessService/OrganizationManage/GetChainList/" + Request.Form["token"];
                    break;
                case "GetSingleList"://获取单店信息列表
                    urlStr = serverpath + "/BusinessService/OrganizationManage/GetSingleList/" + Request.Form["token"];
                    break;
                case "GetOrgType"://获取机构类别
                    urlStr = serverpath + "/BusinessService/OrganizationManage/GetOrgType/" + Request.Form["token"];
                    break;
                case "Install": //安装提交
                    urlStr = serverpath + "/BusinessService/Install/" + Request.Form["token"];
                    break;
                case "CheckInstallPerson": //检查安装人设备
                    urlStr = serverpath + "/BusinessService/CheckInstallPerson/" + Request.Form["ssid"] + "/" + Request.Form["mac"];
                    break;
                case "LoginInstall": // 登录安装页
                    urlStr = serverpath + "/BusinessService/LoginInstall";
                    break;
                #endregion
                #region 月统计
                case "StatisticalMonth/Promotion": //业务推广分析
                    urlStr = serverpath + "/BusinessService/StatisticalMonth/Promotion/" + Request.Form["token"];
                    break;
                case "StatisticalMonth/Total": //总体情况
                    urlStr = serverpath + "/BusinessService/StatisticalMonth/Total/" + Request.Form["token"];
                    break;
                case "StatisticalMonth/UserBehavior": //用户行为与构成
                    urlStr = serverpath + "/BusinessService/StatisticalMonth/UserBehavior/" + Request.Form["token"];
                    break;
                case "StatisticalMonth/AppDownload": //App下载分析
                    urlStr = serverpath + "/BusinessService/StatisticalMonth/AppDownload/" + Request.Form["token"];
                    break;
                case "StatisticalMonth/YingYeTing":
                    urlStr = serverpath + "/BusinessService/StatisticalMonth/YingYeTing/" + Request.Form["token"];
                    break;
                case "StatisticalMonth/AnQuan":
                    urlStr = serverpath + "/BusinessService/StatisticalMonth/AnQuan/" + Request.Form["token"];
                    break;
                #endregion
                #region Radius认证
                /*case "RadiusAuth"://OID,Type,UserName(QQ,WeiBo)
                    urlStr = serverpath + "/BusinessService/Radius/RadiusAuth/";
                    break;*/
                #endregion
                #region 菜单
                case "GetMenuByToken"://获取机构和子机构信息
                    urlStr = serverpath + "/BusinessService/Menu/GetMenuByToken/" + Request.Form["token"];
                    break;
                #endregion
                #region 审核页
                case "GetAuditList"://获取待审核单列表
                    urlStr = serverpath + "/BusinessService/GetAuditList/" + Request.Form["size"] + "/" + Request.Form["curPage"] + "/" + Request.Form["token"];
                    break;
                case "GetAuditHistoryList"://获取历史审核列表
                    urlStr = serverpath + "/BusinessService/GetAuditHistoryList/" + Request.Form["size"] + "/" + Request.Form["curPage"] + "/" + Request.Form["token"];
                    break;
                case "GetAuditProgress"://获取审核单审核进度
                    urlStr = serverpath + "/BusinessService/GetAuditProgress/" + Request.Form["token"];
                    break;
                case "GetAuditHistoryProgress"://获取审核单审核进度
                    urlStr = serverpath + "/BusinessService/GetAuditHistoryProgress/" + Request.Form["token"];
                    break;
                case "APManage/GetSSIDListByIDs"://获取审核单审核进度
                    urlStr = serverpath + "/BusinessService/APManage/GetSSIDListByIDs/" + Request.Form["token"];
                    break;
                case "HandleAudit"://获取审核单审核进度
                    urlStr = serverpath + "/BusinessService/HandleAudit/" + Request.Form["token"];
                    break;
                #endregion
                #region 三方接口调用
                case "GetYouKuShowsCategory"://获取优酷视频列表
                    urlStr = serverpath + "/BusinessService/TripartiteAPI/GetYouKuShowsCategory/" + Request.Form["category"] + "/" + Request.Form["count"] + "/" + Request.Form["page"];
                    break;
                case "GetDianPingFindDealsList"://获取大众点评网团购信息
                    urlStr = serverpath + "/BusinessService/TripartiteAPI/GetDianPingFindDealsList/" + Request.Form["city"] + "/" + Request.Form["latitude"] + "/" + Request.Form["longitude"] + "/" + Request.Form["limit"] + "/" + Request.Form["page"];
                    break;
                #endregion
                case "Weixin/Auth":// 
                    urlStr = weixinPath + "/weixin/auth";
                    break;
            }
            if (urlStr.Length > 1)
            {
                HttpWebResponse response = GetResponse(urlStr);
                StreamReader sr = new StreamReader(response.GetResponseStream());
                Response.ContentType = response.ContentType;
                Response.Write(sr.ReadToEnd());
            }
        }

        private HttpWebResponse GetResponse(string url)
        {
            string param = Request.Form["param"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = Request.HttpMethod;

            request.ContentLength = 0;
            request.ContentType = Request.AcceptTypes[0];

            if (param != null)
            {
                string regexStr = @"Date\(\d+ 0800\)";
                foreach (Match mc in Regex.Matches(param, regexStr))
                    param = param.Replace(mc.Value, mc.Value.Replace(" ", "+"));

                request.ContentLength = System.Text.Encoding.UTF8.GetBytes(param).Length;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(param);
                sw.Flush();
            }

            return (HttpWebResponse)request.GetResponse();
        }
    }
}
