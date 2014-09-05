using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using System.ServiceModel.Channels;
using LUOBO.Model;
using System.Web.Script.Serialization;
using LUOBO.Helper;

namespace LUOBO.BusinessService
{
    public class SecureWebServiceHostFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = base.CreateServiceHost(serviceType, baseAddresses);
            host.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager();
            return host;
        }

        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            var host = base.CreateServiceHost(constructorString, baseAddresses);
            host.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager();
            return host;
        }
    }

    public class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        List<string> filterList = new List<string> {
            "/BusinessService/help","/BusinessService/IgnoreDeviceAlarm", "/BusinessService/ReportStatistical",
            "/BusinessService/ShareAD", "/BusinessService/ShareCount","BusinessService/CheckInstallPerson",
            "/BusinessService/LoginInstall","/BusinessService/GetLoginProperty","/BusinessService/TripartiteAPI"
        };
        PubFun pubFun = new PubFun();
        string _ServicePath = "/BusinessService/{0}/{1}";

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var ctx = WebOperationContext.Current;
            if (ctx.IncomingRequest.Method == "GET" && ctx.IncomingRequest.UriTemplateMatch == null)
            {
                ctx.OutgoingResponse.SuppressEntityBody = true;
                M_Result result = new M_Result();
                result.ResultCode = (int)CustomEnum.ENUM_Result_Code.Fail;
                result.ResultMsg = "请求错误，应用POST请求！";
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                HttpContext.Current.Response.ContentType = "text/json";
                JavaScriptSerializer jss = new JavaScriptSerializer();
                HttpContext.Current.Response.Write(jss.Serialize(result));
                return false;
            }
            var auth = ctx.IncomingRequest.UriTemplateMatch.RequestUri.LocalPath;
            if (string.IsNullOrEmpty(auth) || filterList.Where(c => auth.IndexOf(c) > -1).Count() > 0)
                return true;
            if (string.IsNullOrEmpty(auth) || auth.IndexOf("/BusinessService/Login") == -1)
            {
                string[] arr = auth.Split('/');
                if (!pubFun.ValidationTokenTimeout(arr[arr.Length - 1], auth.Substring(0, auth.LastIndexOf('/'))))
                {
                    ctx.OutgoingResponse.SuppressEntityBody = true;
                    M_Result result = new M_Result();
                    result.ResultCode = (int)CustomEnum.ENUM_Result_Code.LoginTimeout;
                    result.ResultMsg = "登录超时，请重新登陆";
                    ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    HttpContext.Current.Response.ContentType = "text/json";
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    HttpContext.Current.Response.Write(jss.Serialize(result));
                    return false;
                }
                //权限部分代码
                if (false)
                {
                    
                }
            }
            return true;
        }
    }  
}