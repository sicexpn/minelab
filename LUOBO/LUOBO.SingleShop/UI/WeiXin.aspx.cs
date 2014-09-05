using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Xml;
using System.Web.Configuration;
using LUOBO.Helper;

namespace LUOBO.SingleShop.UI
{
    public partial class WeiXin : System.Web.UI.Page
    {
        private static readonly string token = "luoboNextWifi";        
        private static readonly string appid = "wx9058b5ebde9efe94";
        private static readonly string appsecret = "ab5b608196ca82f5f77341da9a1cbaa9";


        protected void Page_Load(object sender, EventArgs e)
        {
            // 首次认证
            if (HttpContext.Current.Request.HttpMethod == "GET")
            {
                Auth();
                
            }
            else if (HttpContext.Current.Request.HttpMethod == "POST")
            {
                string signature = HttpContext.Current.Request.QueryString["signature"];
                string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = HttpContext.Current.Request.QueryString["nonce"];

                try
                {

                    string postString = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();

                    MessageHelper helper = new MessageHelper();

                    string responseContent = helper.ReturnMessage(postString);

                    HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                    HttpContext.Current.Response.Write(responseContent);
                }
                catch (Exception ex)
                {

                }
            }

        }

        private void Auth()
        {
            try
            {
                string echoStr = HttpContext.Current.Request["echostr"];
                string signature = HttpContext.Current.Request["signature"];
                string timestamp = HttpContext.Current.Request["timestamp"];
                string nonce = HttpContext.Current.Request["nonce"];

                if (echoStr != "")
                {
                    if (CheckSignature(token, signature, timestamp, nonce))
                    {
                        HttpContext.Current.Response.ContentType = "text/plain";
                        HttpContext.Current.Response.Write(echoStr);
                    }
                }
            }
            catch (Exception ex)
            {
                string wrongText = string.Empty;
                wrongText = ex.Message;
                File.WriteAllText(Server.MapPath("~/") + @"\test.txt", wrongText);

            }
            finally
            {
                HttpContext.Current.Response.End();
            }
        }


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
    }
}