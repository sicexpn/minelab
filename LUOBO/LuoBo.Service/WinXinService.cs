using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Text;
using LUOBO.Model;
using System.Xml;
using System.Xml.Serialization;
using LUOBO.Helper;

namespace LUOBO.Service
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class WinXinService
    {
        private static readonly string token = "luoboNextWifi";
        //开发者ID
        private static readonly string appid = "wx9058b5ebde9efe94";
        private static readonly string appsecret = "ab5b608196ca82f5f77341da9a1cbaa9";
        
        //请求需带的令牌
        private static string access_token = "";

        #region 微信接入认证
        [WebGet(UriTemplate = "/ValidUrl/")]
        [Description("Auth 微信接入,一次即可")]
        public void ValidUrl()
        {
            Auth();
        }

        public void Auth()
        {
            if (string.IsNullOrEmpty(token))
            {
                // Console.write Error(string.Format("WeixinToken 配置项没有配置！"));
            }

            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];

            if (CheckSignature(token, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    HttpContext.Current.Response.AddHeader("Content-Length", System.Text.Encoding.UTF8.GetBytes(echoString).Length.ToString());
                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        [WebInvoke(UriTemplate = "/ValidUrl/", Method = "POST")]
        [Description("微信推送的消息处理")]
        public void GetMessgae(Stream msg)
        {
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            //if (!CheckSignature(token, signature, timestamp, nonce))
            //    return;
            string responseContent = "";

            try
            {
                //IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                //string postStr = OperationContext.Current.RequestContext.RequestMessage.ToString();

                string postString = new StreamReader(msg).ReadToEnd();

                MessageHelper helper = new MessageHelper();

                responseContent = helper.ReturnMessage(postString);
                //HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                HttpContext.Current.Response.ContentType = "text/xml";
                HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + responseContent);
            }
            catch (Exception ex)
            {

            }

            //byte[] resultBytes = Encoding.UTF8.GetBytes(responseContent);
            //return new MemoryStream(resultBytes);
        }
        
        [WebInvoke(UriTemplate = "/CreateAccessToken", Method = "GET")]
        [Description("生成access_token")]
        public void CreateAccessToken()
        {
            String url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();

                response.Close();
            }
            catch (Exception ex)
            {

            }
            
        }

        [WebInvoke(UriTemplate = "/CreateTicket", Method = "POST")]
        [Description("生成二维码 ticket")]
        public void CreateTicket()
        {
            String url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + token;
            WebClient webClient = new WebClient();

        }

        #region scan upload
        [WebInvoke(UriTemplate = "/ScanUpload/", Method = "GET")]
        [Description("客流检测上传")]
        public String ScanUpload()
        {
            String reqStr = HttpContext.Current.Request.QueryString["scan"];
            return reqStr;
        }

        [WebInvoke(UriTemplate = "/WgetPost/", Method = "POST")]
        public String WgetPost(String str)
        {
            return str;
        }

        [WebInvoke(Method = "PUT", UriTemplate = "/PUTFile/{mac}")]
        public void PUTFile(string mac,System.IO.Stream stream)
        {
            string MAC = mac;
        }

        #endregion
    }

}