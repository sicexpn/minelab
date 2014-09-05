using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace LUOBO.Service
{
    /// <summary>
    /// DeviceService 的摘要说明
    /// </summary>
    public class DeviceAshx : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string deviceid = context.Request.QueryString["deviceid"];
            string lastseq = context.Request.QueryString["lastseq"];
            
            StringBuilder strs = new StringBuilder();
            context.Response.Write("config luobocloud 'config'\n");
            context.Response.Write("    option updt_srv_bak 'http://10.10.20.14:8080/hawkcloud/index.php/Update/'\n");
            context.Response.Write("    option cfgver '1.0'\n");
            context.Response.Write("    option wl_disabled '0'\n");
            context.Response.Write("    option wl_channel '1'\n");
            context.Response.Write("    option privwl_on '1'\n");
            context.Response.Write("    option privwl_ssid 'HawkCloud-Mgmt'\n");
            context.Response.Write("    option privwl_key 'hawkcloud404'\n");
            context.Response.Write("    option privwl_hidden '1'\n");
            context.Response.Write("    option wcver '1'\n");
            context.Response.Write("    option wsver '1'\n");
            context.Response.Write("    option wson '0'\n");
            context.Response.Write("    option macaddr '38:83:45:65:C5:4C'\n");
            context.Response.Write("    option key 'D4CE8871A622D1B9'\n");
            context.Response.Write("    option devid '17'\n");
            context.Response.Write("    option wl_txpower '21'\n");
            context.Response.Write("    option fwver 'FD563F644DCD58B8EBA6697513B0FD02'\n");
            context.Response.Write("    option updt_srv 'http://192.168.9.14:8080/hawkcloud/index.php/Update/get'\n");
            context.Response.Write("    option wccnt '3'\n");
            context.Response.Write("    option cfgmd5 '391B080809F2B6FF157B0FA3C8270F6C'\n");
            context.Response.Write("\n");
            context.Response.Write("config wiclick\n");
            context.Response.Write("    option wcid '130'\n");
            context.Response.Write("    option name '............-............'\n");
            context.Response.Write("    option radiusnasid '130'\n");
            context.Response.Write("    option uamsecret '9b8619251a19057cff70779273e95aa6'\n");
            context.Response.Write("    option dns1 '10.2.0.1'\n");
            context.Response.Write("    option dns2 '10.2.0.1'\n");
            context.Response.Write("    option uamurl 'http://10.2.0.1:4992/www/usb/demos/www/130.html'\n");
            context.Response.Write("    option uamallow '182.50.9.129,192.168.90.3'\n");
            context.Response.Write("    option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
            context.Response.Write("    option aaa 'http'\n");
            context.Response.Write("    option uamaaaurl 'http://10.2.0.1:4992/www/usb/demos/www/'\n");
            context.Response.Write("    option radiusserver1 '127.0.0.1'\n");
            context.Response.Write("    option radiusserver2 '182.50.9.129'\n");
            context.Response.Write("    option radiussecret 'testing123'\n");
            context.Response.Write("\n");
            context.Response.Write("config wiclick\n");
            context.Response.Write("    option wcid '131'\n");
            context.Response.Write("    option name '............-............'\n");
            context.Response.Write("    option radiusnasid '131'\n");
            context.Response.Write("    option uamsecret '1afa34a7f984eeabdbb0a7d494132ee5'\n");
            context.Response.Write("    option dns1 '10.3.0.1'\n");
            context.Response.Write("    option dns2 '10.3.0.1'\n");
            context.Response.Write("    option uamurl 'http://10.3.0.1:4993/www/usb/demos/www/131.html'\n");
            context.Response.Write("    option uamallow '182.50.9.129,192.168.90.3'\n");
            context.Response.Write("    option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
            context.Response.Write("    option aaa 'http'\n");
            context.Response.Write("    option uamaaaurl 'http://10.3.0.1:4993/www/usb/demos/www/'\n");
            context.Response.Write("    option radiusserver1 '127.0.0.1'\n");
            context.Response.Write("    option radiusserver2 '182.50.9.129'\n");
            context.Response.Write("    option radiussecret 'testing123'\n");
            context.Response.Write("\n");
            context.Response.Write("config wiclick\n");
            context.Response.Write("    option wcid '132'\n");
            context.Response.Write("    option name '............-............'\n");
            context.Response.Write("    option radiusnasid '132'\n");
            context.Response.Write("    option uamsecret '65ded5353c5ee48d0b7d48c591b8f430'\n");
            context.Response.Write("    option dns1 '10.4.0.1'\n");
            context.Response.Write("    option dns2 '10.4.0.1'\n");
            context.Response.Write("    option uamurl 'http://10.4.0.1:4994/www/usb/demos/www/132.html'\n");
            context.Response.Write("    option uamallow '182.50.9.129,192.168.90.3'\n");
            context.Response.Write("    option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
            context.Response.Write("    option aaa 'http'\n");
            context.Response.Write("    option uamaaaurl 'http://10.4.0.1:4994/www/usb/demos/www/'\n");
            context.Response.Write("    option radiusserver1 '127.0.0.1'\n");
            context.Response.Write("    option radiusserver2 '182.50.9.129'\n");
            context.Response.Write("    option radiussecret 'testing123'\n");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}