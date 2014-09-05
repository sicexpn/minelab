using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using LUOBO.Entity;
using LUOBO.BLL;
using System.Web;
using System.IO;
using System.Configuration;
using LUOBO.Model;

namespace LUOBO.Service
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class DeviceService
    {
        BLL_DeviceService bll_device = new BLL_DeviceService();
        BLL_WarnManage bll_warnmanage = new BLL_WarnManage();
        //[WebGet(UriTemplate = "/Device/{deviceid}/{lastseq}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //public string Device(string deviceid, string lastseq)
        //{
        //    // TODO: Replace the current implementation to return a collection of SampleItem instances
        //    //string str = p.Generate();
        //    try
        //    {
        //        StringBuilder strs = new StringBuilder();
        //        //strs.Append("config luobocloud 'config'\n");
        //        //strs.Append("option updt_srv_bak 'http://10.10.20.14:8080/hawkcloud/index.php/Update/'\n");
        //        //strs.Append("option cfgver '1.0'\n");
        //        //strs.Append("option wl_disabled '0'\n");
        //        //strs.Append("option wl_channel '1'\n");
        //        //strs.Append("option privwl_on '1'\n");
        //        //strs.Append("option privwl_ssid 'HawkCloud-Mgmt'\n");
        //        //strs.Append("option privwl_key 'hawkcloud404'\n");
        //        //strs.Append("option privwl_hidden '1'\n");
        //        //strs.Append("option wcver '1'\n");
        //        //strs.Append("option wsver '1'\n");
        //        //strs.Append("option wson '0'\n");
        //        //strs.Append("option macaddr '38:83:45:65:C5:4C'\n");
        //        //strs.Append("option key 'D4CE8871A622D1B9'\n");
        //        //strs.Append("option devid '17'\n");
        //        //strs.Append("option wl_txpower '21'\n");
        //        //strs.Append("option fwver 'FD563F644DCD58B8EBA6697513B0FD02'\n");
        //        //strs.Append("option updt_srv 'http://192.168.9.14:8080/hawkcloud/index.php/Update/get'\n");
        //        //strs.Append("option wccnt '3'\n");
        //        //strs.Append("option cfgmd5 '391B080809F2B6FF157B0FA3C8270F6C'\n");
        //        //strs.Append("\n");
        //        //strs.Append("config wiclick\n");
        //        //strs.Append("option ssid '129'\n");
        //        //strs.Append("option name '............-............'\n");
        //        //strs.Append("option radiusnasid '130'\n");
        //        //strs.Append("option uamsecret '9b8619251a19057cff70779273e95aa6'\n");
        //        //strs.Append("option dns1 '10.1.0.1'\n");
        //        //strs.Append("option dns2 '10.1.0.1'\n");
        //        //strs.Append("option uamurl 'http://10.1.0.1:4992/www/usb/demos/www/130.html'\n");
        //        //strs.Append("option uamallow '182.50.9.129,192.168.1.1'\n");
        //        //strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        //strs.Append("option aaa 'http'\n");
        //        //strs.Append("option uamaaaurl 'http://10.1.0.1:4992/www/usb/demos/www/'\n");
        //        //strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        //strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        //strs.Append("option radiussecret 'testing123'\n");
        //        //strs.Append("\n");
        //        //strs.Append("config wiclick\n");
        //        //strs.Append("option ssid '130'\n");
        //        //strs.Append("option name '............-............'\n");
        //        //strs.Append("option radiusnasid '130'\n");
        //        //strs.Append("option uamsecret '9b8619251a19057cff70779273e95aa6'\n");
        //        //strs.Append("option dns1 '10.2.0.1'\n");
        //        //strs.Append("option dns2 '10.2.0.1'\n");
        //        //strs.Append("option uamurl 'http://10.2.0.1:4992/www/usb/demos/www/130.html'\n");
        //        //strs.Append("option uamallow '182.50.9.129,192.168.90.3'\n");
        //        //strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        //strs.Append("option aaa 'http'\n");
        //        //strs.Append("option uamaaaurl 'http://10.2.0.1:4992/www/usb/demos/www/'\n");
        //        //strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        //strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        //strs.Append("option radiussecret 'testing123'\n");
        //        //strs.Append("\n");
        //        //strs.Append("config wiclick\n");
        //        //strs.Append("option ssid '131'\n");
        //        //strs.Append("option name '............-............'\n");
        //        //strs.Append("option radiusnasid '131'\n");
        //        //strs.Append("option uamsecret '1afa34a7f984eeabdbb0a7d494132ee5'\n");
        //        //strs.Append("option dns1 '10.3.0.1'\n");
        //        //strs.Append("option dns2 '10.3.0.1'\n");
        //        //strs.Append("option uamurl 'http://10.3.0.1:4993/www/usb/demos/www/131.html'\n");
        //        //strs.Append("option uamallow '182.50.9.129,192.168.90.3'\n");
        //        //strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        //strs.Append("option aaa 'http'\n");
        //        //strs.Append("option uamaaaurl 'http://10.3.0.1:4993/www/usb/demos/www/'\n");
        //        //strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        //strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        //strs.Append("option radiussecret 'testing123'\n");
        //        //strs.Append("\n");
        //        //strs.Append("config wiclick\n");
        //        //strs.Append("option ssid '132'\n");
        //        //strs.Append("option name '............-............'\n");
        //        //strs.Append("option radiusnasid '132'\n");
        //        //strs.Append("option uamsecret '65ded5353c5ee48d0b7d48c591b8f430'\n");
        //        //strs.Append("option dns1 '10.4.0.1'\n");
        //        //strs.Append("option dns2 '10.4.0.1'\n");
        //        //strs.Append("option uamurl 'http://10.4.0.1:4994/www/usb/demos/www/132.html'\n");
        //        //strs.Append("option uamallow '182.50.9.129,192.168.90.3'\n");
        //        //strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        //strs.Append("option aaa 'http'\n");
        //        //strs.Append("option uamaaaurl 'http://10.4.0.1:4994/www/usb/demos/www/'\n");
        //        //strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        //strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        //strs.Append("option radiussecret 'testing123'\n");
        //        strs.Append("config luobocloud 'config'\n");
        //        strs.Append("option updt_srv 'http://192.168.1.11/ds/DeviceService/Device/'\n");
        //        strs.Append("option cfgver '1.0'\n");
        //        strs.Append("option macaddr '38:83:45:65:C5:4C'\n");
        //        strs.Append("option devid '17'\n");
        //        strs.Append("option wificnt '4'\n");
        //        strs.Append("\n");

        //        strs.Append("config wifi-iface\n");
        //        strs.Append("option ssid '129'\n");
        //        strs.Append("option name '............-............'\n");//SSID name
        //        strs.Append("option radiusnasid '130'\n");
        //        strs.Append("option uamsecret '9b8619251a19057cff70779273e95aa6'\n");
        //        strs.Append("option dns1 '10.1.0.1'\n");
        //        strs.Append("option dns2 '10.1.0.1'\n");
        //        strs.Append("option uamurl 'http://10.1.0.1:4992/www/usb/demos/www/130.html'\n");
        //        strs.Append("option uamallow '182.50.9.129,192.168.1.1'\n");
        //        strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        strs.Append("option aaa 'http'\n");
        //        strs.Append("option uamaaaurl 'http://10.1.0.1:4992/www/usb/demos/www/'\n");
        //        strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        strs.Append("option radiussecret 'testing123'\n");
        //        strs.Append("option encryption 'psk2'\n");
        //        strs.Append("option key 'xiaoluobo'\n");
        //        strs.Append("\n");

        //        strs.Append("config wifi-iface\n");
        //        strs.Append("option ssid '130'\n");
        //        strs.Append("option name '............-............'\n");
        //        strs.Append("option radiusnasid '130'\n");
        //        strs.Append("option uamsecret '9b8619251a19057cff70779273e95aa6'\n");
        //        strs.Append("option dns1 '10.2.0.1'\n");
        //        strs.Append("option dns2 '10.2.0.1'\n");
        //        strs.Append("option uamurl 'http://10.2.0.1:4992/www/usb/demos/www/130.html'\n");
        //        strs.Append("option uamallow '182.50.9.129,192.168.90.3'\n");
        //        strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        strs.Append("option aaa 'http'\n");
        //        strs.Append("option uamaaaurl 'http://10.2.0.1:4992/www/usb/demos/www/'\n");
        //        strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        strs.Append("option radiussecret 'testing123'\n");
        //        strs.Append("option encryption 'psk2'\n");
        //        strs.Append("option key 'xiaoluobo'\n");
        //        strs.Append("\n");

        //        strs.Append("config wifi-iface\n");
        //        strs.Append("option ssid '131'\n");
        //        strs.Append("option name '............-............'\n");
        //        strs.Append("option radiusnasid '131'\n");
        //        strs.Append("option uamsecret '1afa34a7f984eeabdbb0a7d494132ee5'\n");
        //        strs.Append("option dns1 '10.3.0.1'\n");
        //        strs.Append("option dns2 '10.3.0.1'\n");
        //        strs.Append("option uamurl 'http://10.3.0.1:4993/www/usb/demos/www/131.html'\n");
        //        strs.Append("option uamallow '182.50.9.129,192.168.90.3'\n");
        //        strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        strs.Append("option aaa 'http'\n");
        //        strs.Append("option uamaaaurl 'http://10.3.0.1:4993/www/usb/demos/www/'\n");
        //        strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        strs.Append("option radiussecret 'testing123'\n");
        //        strs.Append("option encryption 'psk2'\n");
        //        strs.Append("option key 'xiaoluobo'\n");
        //        strs.Append("\n");

        //        strs.Append("config wifi-iface\n");
        //        strs.Append("option ssid '132'\n");
        //        strs.Append("option name '............-............'\n");
        //        strs.Append("option radiusnasid '132'\n");
        //        strs.Append("option uamsecret '65ded5353c5ee48d0b7d48c591b8f430'\n");
        //        strs.Append("option dns1 '10.4.0.1'\n");
        //        strs.Append("option dns2 '10.4.0.1'\n");
        //        strs.Append("option uamurl 'http://10.4.0.1:4994/www/usb/demos/www/132.html'\n");
        //        strs.Append("option uamallow '182.50.9.129,192.168.90.3'\n");
        //        strs.Append("option uamdomain '.wifind.com.cn,.nextwifi.cn,.nextwifi.com.cn'\n");
        //        strs.Append("option aaa 'http'\n");
        //        strs.Append("option uamaaaurl 'http://10.4.0.1:4994/www/usb/demos/www/'\n");
        //        strs.Append("option radiusserver1 '127.0.0.1'\n");
        //        strs.Append("option radiusserver2 '182.50.9.129'\n");
        //        strs.Append("option radiussecret 'testing123'\n");
        //        strs.Append("option encryption 'psk2'\n");
        //        strs.Append("option key 'xiaoluobo'\n");
        //        strs.Append("\n");
        //        HttpContext.Current.Response.Write(strs.ToString());
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }


        //}

        /// <summary>
        /// 设备心跳处理
        /// 当设备心跳到达后，采集设备信息，并判断是否存在bin或者设备版本更新
        /// </summary>
        /// <param name="deviceid">设备mac地址</param>
        /// <param name="lastseq">最后一次更新的版本号</param>
        /// <returns>如果有更新，返回该设备的配置信息</returns>
        [WebGet(UriTemplate = "/HeartBeat/{devicemac}/{firmwareseq}/{lastseq}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]

        public Stream HeartBeat(string devicemac, string firmwareseq, string lastseq)
        {
            ///cpu百分比
            string cpu = HttpContext.Current.Request.QueryString["cpu"];
            //空闲内存
            string memfree = HttpContext.Current.Request.QueryString["memfree"];
            //开机时长，秒为单位
            string powertime = HttpContext.Current.Request.QueryString["powertime"];
            //空闲时长
            string freetime = HttpContext.Current.Request.QueryString["freetime"];
            //网络总流量
            string networktotal = HttpContext.Current.Request.QueryString["networktotal"];
            //网络速率
            string networkrate = HttpContext.Current.Request.QueryString["networkrate"];
            //设备当前时间
            string curtime = HttpContext.Current.Request.QueryString["curtime"];
            //在线人数
            string ONLINEPEOPLENUM = HttpContext.Current.Request.QueryString["ONLINEPEOPLENUM"];
            string returnstr = bll_device.IsUpdate(devicemac, firmwareseq, lastseq, cpu, memfree, powertime, freetime, networktotal, networkrate, curtime, ONLINEPEOPLENUM);

            Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(returnstr));
            return filestream;
        }
        ///// <summary>
        ///// 获取设备是否存在bin更新
        ///// </summary>
        ///// <param name="deviceid">设备mac地址</param>
        ///// <param name="lastseq">最后一次更新的版本号</param>
        ///// <returns>如果有更新，返回该设备的配置信息</returns>
        //[WebGet(UriTemplate = "/UpdateBin/{devicemac}/{firmwarever}/{lastseq}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //public Stream UpdateBin(string devicemac, string firmwarever, string lastseq)
        //{
        //    ///判断是否有错误
        //    string upgradeerror = HttpContext.Current.Request.QueryString["upgradeerror"];
        //    //Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(""));
        //    //return filestream;

        //    string runDir = ConfigurationSettings.AppSettings["UpdateBinPath"];
        //    string FilePath = System.IO.Path.Combine(runDir, firmwarever);
        //    SYS_FIRMWARE firmware = bll_device.CheckFirmWareVersion(devicemac, firmwarever, lastseq);
        //    if (firmware == null)
        //    {
        //        string strs = "";
        //        Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strs));
        //        return filestream;
        //    }
        //    System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open);
        //    System.Threading.Thread.Sleep(2000);
        //    WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
        //    return fs;
        //}

        /// <summary>
        /// 获取设备是否存在bin更新
        /// </summary>
        /// <param name="deviceid">设备mac地址</param>
        /// <param name="lastseq">最后一次更新的版本号</param>
        /// <returns>如果有更新，返回该设备的配置信息</returns>
        [WebGet(UriTemplate = "/UpdateBin/{devicemac}/{lastseq}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Stream UpdateBin(string devicemac, string lastseq)
        {
            ///判断是否有错误
            string upgradeerror = HttpContext.Current.Request.QueryString["upgradeerror"];
            //Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(""));
            //return filestream;

            string runDir = ConfigurationSettings.AppSettings["UpdateBinPath"];
            SYS_FIRMWARE firmware = bll_device.CheckFirmWareVersion(devicemac, lastseq);
            if (firmware == null||firmware.VERNO.ToUpper()==lastseq.ToUpper())
            {
                string strs = "";
                Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strs));
                return filestream;
            }
            try
            {
                string FilePath = System.IO.Path.Combine(runDir, firmware.FIREWARENAME);
                System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                System.Threading.Thread.Sleep(1000);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
                MemoryStream storeStream = new MemoryStream();
                storeStream.SetLength(fs.Length);
                fs.Read(storeStream.GetBuffer(), 0, (int)fs.Length);

                storeStream.Flush();
                fs.Close();

                return storeStream;
            }
            catch (Exception ex)
            {
                string strs = ex.ToString();
                Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strs));
                return filestream;
            }
        }

        //}
        /// <summary>
        /// 获取设备配置信息
        /// </summary>
        /// <param name="deviceid">设备mac地址</param>
        /// <param name="lastseq">最后一次更新的版本号</param>
        /// <returns>如果有更新，返回该设备的配置信息</returns>
        [WebGet(UriTemplate = "/Device/{devicemac}/{lastseq}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]

        public Stream Device(string devicemac, string lastseq)
        {
            // TODO: Replace the current implementation to return a collection of SampleItem instances
            //string str = p.Generate();
            try
            {//是否更新判断
                string strs = "";
                //http://182.50.9.129:8006/DeviceService/Device/5C63BFCE126B/d152ed78-9e5e-4cff-8a4c-00269b359f0c?cpu=2.8149%&&memfree=32.2269%&&powertime=100482.09&&freetime=97653.57&&networktotal=22135539&&networkrate=0
                ///cpu百分比
                string cpu = HttpContext.Current.Request.QueryString["cpu"];
                //空闲内存
                string memfree = HttpContext.Current.Request.QueryString["memfree"];
                //开机时长，秒为单位
                string powertime = HttpContext.Current.Request.QueryString["powertime"];
                //空闲时长
                string freetime = HttpContext.Current.Request.QueryString["freetime"];
                //网络总流量
                string networktotal = HttpContext.Current.Request.QueryString["networktotal"];
                //网络速率
                string networkrate = HttpContext.Current.Request.QueryString["networkrate"];
                //设备当前时间
                string curtime = HttpContext.Current.Request.QueryString["curtime"];
                //设备当前登录人数
                string ONLINEPEOPLENUM = HttpContext.Current.Request.QueryString["ONLINEPEOPLENUM"];
                string IsUpdateBin = ConfigurationSettings.AppSettings["IsUpdateBin"];
                if (IsUpdateBin == "false")
                {
                    string returnstr = bll_device.IsUpdate(devicemac, "", lastseq, cpu, memfree, powertime, freetime, networktotal, networkrate, curtime, ONLINEPEOPLENUM);
                    if (returnstr == "true")
                    {
                        strs = bll_device.UpdateDevice(devicemac, lastseq);
                        if (strs == null)
                        {
                            strs = "";
                        }
                    }
                    else
                    {
                        strs = "";
                    }
                }
              
                else
                {
                    strs = bll_device.UpdateDevice(devicemac, lastseq);
                }
                //bll_device.IsUpdate(devicemac, lastseq, cpu, memfree, powertime, freetime, networktotal, networkrate, curtime, ONLINEPEOPLENUM);
                //if (bll_device.IsUpdate(devicemac, lastseq, cpu, memfree, powertime, freetime, networktotal, networkrate, curtime))
                //{

                //string cpu = HttpContext.Current.Request.QueryString["cpu"];
 
                //}
                Stream filestream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(strs));
                return filestream;
                //HttpContext.Current.Response.Write(strs);
                //return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [WebGet(UriTemplate = "/upload/{devicemac}")]
        public string upload(string devicemac)
        {
            string data = HttpContext.Current.Request.QueryString["data"];
            //data = "4253532032303a64633a65363a65373a66623a643020286f6e20776c616e30290a097369676e616c3a202d38322e30302064426d0a09535349443a204452433531300a09445320506172616d65746572207365743a206368616e6e656c20360a4253532062323a34383a37613a36383a38363a656220286f6e20776c616e30290a097369676e616c3a202d38342e30302064426d0a09535349443a205c7832305b5c7865395c7838325c7861655c7865355c7838325c7861385c7865395c7839335c7862365c7865385c7861315c7838635c7865355c7838355c7838645c7865385c7862345c786239576946695d0a09445320506172616d65746572207365743a206368616e6e656c2031310a4253532062323a34383a37613a36383a38363a656420286f6e20776c616e30290a097369676e616c3a202d38322e30302064426d0a09535349443a205c7832305b5c7865395c7838325c7861655c7865355c7838325c7861385c7865365c7838395c7838625c7865365c7839635c7862615c7865395c7839335c7862365c7865385c7861315c7838635c7865345c7862385c7838625c7865385c7862645c7862645d320a09445320506172616d65746572207365743a206368616e6e656c2031310a4253532062303a34383a37613a36383a38363a656120286f6e20776c616e30290a097369676e616c3a202d38352e30302064426d0a09535349443a200a09445320506172616d65746572207365743a206368616e6e656c2031310a4253532062323a34383a37613a36383a38363a656320286f6e20776c616e30290a097369676e616c3a202d38342e30302064426d0a09535349443a205c7832305b5c7865395c7838325c7861655c7865355c7838325c7861385c7865365c7838395c7838625c7865365c7839635c7862615c7865395c7839335c7862365c7865385c7861315c7838635c7865345c7862385c7838625c7865385c7862645c7862645d0a09445320506172616d65746572207365743a206368616e6e656c2031310a";
            //string str = "";
            //for (int i = 0; i < data.Length; i = i + 2)
            //{
            //    str = str + char.Parse(data.Substring(i, 2));
            //}
            try
            {
                string apMac = devicemac.Substring(0, 2) + "-" + devicemac.Substring(2, 2) + "-" + devicemac.Substring(4, 2) + "-" + devicemac.Substring(6, 2) + "-" + devicemac.Substring(8, 2) + "-" + devicemac.Substring(10, 2);

                if (apMac == "40-16-9F-5C-B0-CD")
                {
                    StreamWriter sw = new StreamWriter("D:/LUOBOFile/Log/apnear.txt", true);
                    sw.Write("原文:\r\n" + data);
                    sw.Flush();
                    sw.Close();
                }
                data = Get16ByteStrToString(data);
                if (apMac == "40-16-9F-5C-B0-CD")
                {
                    StreamWriter sw = new StreamWriter("D:/LUOBOFile/Log/apnear.txt", true);
                    sw.Write("翻译:\r\n" + data);
                    sw.Write("\r\n\r\n-----------------------------------------------\r\n\r\n");
                    sw.Flush();
                    sw.Close();
                }


                string[] datas = data.Split('\n');
                List<Upload> list = new List<Upload>();
                Upload upload;
                for (int i = 0; i < datas.Length / 4; i++)
                {
                    upload = new Upload();
                    upload.AP_MAC = apMac;
                    upload.MAC = datas[i * 4].Split(' ')[1];
                    upload.Signal = datas[i * 4 + 1].Split(':')[1].Trim().Split(' ')[0];
                    upload.SSID = Get16ByteStrToEncodingUTF8(datas[i * 4 + 2].Split(':')[1].Trim());
                    upload.DS = datas[i * 4 + 3].Split(':')[1].Trim().Split(' ')[1];
                    list.Add(upload);
                }

                bll_warnmanage.SSIDScand(list);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Get16ByteStrToString(string hexValues)
        {
            string result = "";
            string hex = "";
            for (int i = 0; i < hexValues.Length; i += 2)
            {
                if (i + 2 <= hexValues.Length)
                {
                    hex = hexValues.Substring(i, 2);
                    // Convert the number expressed in base-16 to an integer. 
                    int value = Convert.ToInt32(hex, 16);
                    // Get the character corresponding to the integral value. 
                    result += Char.ConvertFromUtf32(value);
                }
            }
            return result;
        }

        public string Get16ByteStrToEncodingUTF8(string value)
        {
            List<byte> list = new List<byte>();
            string strHex = value;
            string strTmp = "";
            char charTmp;
            int strCount = strHex.Length;
            int i = 0;
            while (strCount > i)
            {
                if (strHex.IndexOf("\\x") == 0)
                {
                    strTmp = strHex.Substring(0, 4);
                    strHex = strHex.Substring(4);
                    list.Add(Convert.ToByte(strTmp.Substring(2), 16));
                    i += 4;
                }
                else
                {
                    charTmp = strHex[0];
                    strHex = strHex.Substring(1);
                    list.Add((byte)charTmp);
                    i++;
                }
            }
            return System.Text.Encoding.UTF8.GetString(list.ToArray());
        }
    }
}
;