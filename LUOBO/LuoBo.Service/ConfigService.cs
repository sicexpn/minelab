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
using System.IO;
using LUOBO.Helper;
using System.Runtime.Serialization.Json;

namespace LUOBO.Service
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class ConfigService
    {
        BLL_Radius bll_radius = new BLL_Radius();
        [WebInvoke(UriTemplate = "/ControlConfig/", Method = "POST")]
        [Description("配置SSID限流、限时接口")]
        public bool ControlConfig(List<M_SSIDConfig> list)
        {
            return bll_radius.SSIDConfig(list);
        }
        [WebInvoke(UriTemplate = "/GetLoginInfo/", Method = "POST")]
        [Description("得到处理之后的用户名和密码")]
        public M_RadiusLoginInfo GetLoginInfo(M_RadiusUser user)
        {
            return bll_radius.GetLoginInfo(user);
        }
        [WebInvoke(UriTemplate = "/AddBlackList/{USERMAC}", Method = "GET")]
        [Description("将用户加入到黑名单接口")]
        public bool AddBlackList(string userMac)
        {
            return bll_radius.AddBlackList(userMac);
        }
        [WebInvoke(UriTemplate = "/DeleteBlackList/{USERMAC}", Method = "GET")]
        [Description("将用户从黑名单中删除接口")]
        public bool DeleteBlackList(string userMac)
        {
            return bll_radius.DisableBlackList(userMac);
        }
        [WebInvoke(UriTemplate = "/CheckBlackList/{USERMAC}", Method = "GET")]
        [Description("检查用户Mac是否存在于黑名单中接口")]
        public bool CheckIsBlackList(string userMac)
        {
            return bll_radius.CheckIsBlackList(userMac);
        }
        [WebInvoke(UriTemplate = "/test?json={STR}", Method = "GET")]
        [Description("")]
        public String Test(string str)
        {
            var ser = new DataContractJsonSerializer(typeof(UserLogin));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
            UserLogin myClass1 = (UserLogin)ser.ReadObject(ms);
            int i = 0;
            return myClass1.UserName;
            //return false;
        }

    }
}