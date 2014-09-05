using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using LUOBO.Entity;
using LUOBO.Model;
using LUOBO.BLL;
using System.Net;
using System.ComponentModel;

namespace LUOBO.Service
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class LUOBOService
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances
        //public DAL_OpenSSID dal_openSSID = new DAL_OpenSSID();
        //public DAL_RadAcct dal_radAcct = new DAL_RadAcct();
        public BLL_Radius bll_radius = new BLL_Radius();
        public BLL_DeviceService bll_ds = new BLL_DeviceService();
        public BLL_Statistics bll_statistics = new BLL_Statistics();
        [WebGet(UriTemplate = "/test/")]
        [Description("调试测试接口")]
        public string GetCollection()
        {
            // TODO: Replace the current implementation to return a collection of SampleItem instances
            //string str = p.Generate();
            try
            {

                return bll_radius.SelectAllOpenSSID().FirstOrDefault().CalledStationId.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [WebInvoke(UriTemplate = "OpenSSID", Method = "POST")]
        [Description("打开SSID广告页面接口")]
        public bool CreateOpenSSID(OpenSSID openSSID)
        {
            return bll_radius.Insert(openSSID);
        }
        [WebInvoke(UriTemplate = "UserLogin/GetPassword", Method = "POST")]
        [Description("用户登录接口")]
        public string CreateUser(UserLogin userLogin)
        {
            //string errorCode = "203";
            return bll_radius.GetPassword(userLogin);//返回密码
            //1 check sessionId
            //2 check userName
            //if (bll_radius.CheckSessionId(userLogin.AcctSessionId.Trim()))
            //{
            //    return bll_radius.GetPassword(userLogin);//返回密码
            //}
            //else
            //    return errorCode;
        }
        [WebInvoke(UriTemplate = "APControl", Method = "POST")]
        [Description("Ap组限流、限时配置接口")]
        public bool APControl(APControl apControl)
        {
            return bll_radius.Insert(apControl);
        }
        private bool CheckAuthorization()
        {
            var ctx = WebOperationContext.Current;
            var auth = ctx.IncomingRequest.Headers[HttpRequestHeader.Authorization];
            if (string.IsNullOrEmpty(auth) || auth != "fangxing/123")
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                return false;
            }
            return true;
        }
        //统计接口
        /// <summary>
        /// 根据apMac统计流量
        /// </summary>
        /// <param name="apMac"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/Statistics/TrafficApMac/{ApMac}?starttime={STARTTIME}&endtime={ENDTIME}", Method = "GET")]
        [Description("根据ApMac统计总流量接口")]
        public string GetTrafficByApMac(string apMac, DateTime startTime, DateTime endTime)
        {
            return bll_statistics.GetTrafficByApMac(apMac, startTime, endTime);
        }
        [WebInvoke(UriTemplate = "/Statistics/SessionTimeApMac/{ApMac}?starttime={STARTTIME}&endtime={ENDTIME}", Method = "GET")]
        [Description("根据ApMac统计总时间接口")]
        public string GetSessionTimeByApMac(string apMac, DateTime startTime, DateTime endTime)
        {
            return bll_statistics.GetSessionTimeByApMac(apMac, startTime, endTime);

        }
        [WebInvoke(UriTemplate = "/Statistics/OnLineLoginUsersCountsApMac/{ApMac}", Method = "GET")]
        [Description("统计单个ap的认证登陆在线人数接口")]
        public Int64 GetOnLineLoginUserCountsByApMac(string apMac)
        {
            return bll_statistics.GetOnLineLoginUserCountsByApMac(apMac);
        }
        [WebInvoke(UriTemplate = "/Statistics/OnLineLoginUsersCounts", Method = "GET")]
        [Description("统计认证登陆在线总人数接口")]
        public Int64 GetOnLineLoginUserCounts()
        {
            return bll_statistics.GetOnLineLoginUserCounts();
        }
        [WebInvoke(UriTemplate = "/Statistics/SessionTimeSSID/{SSID}?starttime={STARTTIME}&endtime={ENDTIME}", Method = "GET")]
        [Description("根据SSID统计总时间接口")]
        public string GetSessionTimeBySSID(string ssid, DateTime startTime, DateTime endTime)
        {
            return bll_statistics.GetSessionTimeBySSID(ssid, startTime, endTime);
        }

        [WebInvoke(UriTemplate = "/Statistics/TrafficSSID/{SSID}?starttime={STARTTIME}&endtime={ENDTIME}", Method = "GET")]
        [Description("根据SSID统计总流量接口")]
        public string GetTrafficBySSID(string ssid, DateTime startTime, DateTime endTime)
        {
            return bll_statistics.GetTrafficBySSID(ssid, startTime, endTime);
        }
        [WebInvoke(UriTemplate = "/Statistics/AvailableTrafficUser/{UserName}", Method = "GET")]
        [Description("用户剩余可用流量接口")]
        public string GetAvailableTrafficByUser(string userName)
        {
            return bll_statistics.GetAvailableTrafficByUser(userName);
        }
        [WebInvoke(UriTemplate = "/Statistics/AvailableSessionTimeUser/{UserName}", Method = "GET")]
        [Description("用户剩余可用时间接口")]
        public string GetAvailableSessionTimeyUser(string userName)
        {
            try
            {
                return bll_statistics.GetAvailableSessionTimeByUser(userName);
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        /// <summary>
        /// 获取用户人次人数信息
        /// </summary>
        /// <param name="apID">apID</param>
        /// <returns>安装:人次,人数  开机:人次,人数  实时:在线人数</returns>
        [WebInvoke(UriTemplate = "/Statistics/GetPeopleCount/{APMAC}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("统计ap设备的安装人次、人数；开机人次、人数；在线连接人数")]
        public M_PeopleCount GetPeopleCount(string apMac)
        {
            M_PeopleCount result = new M_PeopleCount();
            //M_PeopleCount result = bll_statistics.GetPeopleCountByApMac(apMac);
            return result;
        }
        [WebInvoke(UriTemplate = "/Statistics/GetPeopleCountAllAp/{OID}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("统计所有机构下所有ap设备的安装人次、人数；开机人次、人数；在线连接人数")]
        public M_PeopleCount GetPeopleCountAllApByOID(string oID)
        {
            //M_PeopleCount result = new M_PeopleCount();

            M_PeopleCount result = bll_statistics.GetPeopleCountByOID(oID);
            return result;
        }
        [WebInvoke(UriTemplate = "Radius/RadiusAuth/", Method = "POST")]
        [Description("Radius验证")]
        public RadiusAuthResult RadiusAuth(RadiusAuth radiusAuth)
        {
            return bll_radius.GetRadiusAuth(radiusAuth);
        }
    }
}
