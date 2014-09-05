using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Model;
using System.Configuration;
using LUOBO.Helper;

namespace LUOBO.BLL
{
    public class BLL_DeviceService
    {
        DAL_SYS_SETTINGVER dal_settingver = new DAL_SYS_SETTINGVER();
        DAL_SYS_APDEVICE dal_apDevice = new DAL_SYS_APDEVICE();
        DAL_SYS_APSTATE dal_apState = new DAL_SYS_APSTATE();
        DAL_SYS_SSID dal_ssid = new DAL_SYS_SSID();
        DAL_SYS_FIRMWARE dal_firmware = new DAL_SYS_FIRMWARE();
        DAL_NAS dal_nas = new DAL_NAS();
        DAL_AD_FREEHOST dal_freehost = new DAL_AD_FREEHOST();
        DAL_SYS_SIM dal_sim = new DAL_SYS_SIM();

        string dns = "ad.next-wifi.com";
        int intcount = 0;
        /// <summary>
        /// 更新心跳
        /// </summary>
        /// <param name="devicemac">设备MAC</param>
        /// <param name="lastseq">系统最后版本号</param>
        /// <param name="cpu">cpu占用率，带%号</param>
        /// <param name="memfree">空闲内存byte</param>
        /// <param name="powertime">开机时长，单位秒</param>
        /// <param name="freetime">待机时长，单位秒</param>
        /// <param name="networktotal">开机后网络总流量</param>
        /// <param name="networkrate">网络速率</param>
        /// <param name="curdatetime">当前设备时间</param>
        /// <returns></returns>
        public string IsUpdate(string devicemac, string md5seq, string lastseq, string cpu, string memfree, string powertime, string freetime, string networktotal, string networkrate, string curdatetime, string ONLINEPEOPLENUM)
        {
            string apMac = devicemac.Substring(0, 2) + "-" + devicemac.Substring(2, 2) + "-" + devicemac.Substring(4, 2) + "-" + devicemac.Substring(6, 2) + "-" + devicemac.Substring(8, 2) + "-" + devicemac.Substring(10, 2);

            dal_apDevice.UpdateHeart(apMac, DateTime.Now, cpu, memfree, powertime, freetime, networktotal, networkrate, curdatetime, ONLINEPEOPLENUM);
            SYS_FIRMWARE firmwareVer = dal_firmware.GetCurrentVersion(apMac);
            string binupdt_srv = ConfigurationSettings.AppSettings["UpdateBinService"];
            string updt_srv = ConfigurationSettings.AppSettings["UpdateService"];
            string IsUpdateBin = ConfigurationSettings.AppSettings["IsUpdateBin"];
            if (IsUpdateBin == "true")
            {
                if (firmwareVer != null && firmwareVer.VERNO != md5seq)
                {
                    //mac@@cmd@@url@@md5
                    return devicemac + "@@1@@" + binupdt_srv + devicemac + "/" + firmwareVer.FIREWARENAME + "/" + firmwareVer.VERNO + "/";
                }
                SYS_SETTINGVER settingVer = dal_settingver.SelectNewByAPSerial(apMac);
                if (settingVer.GUID != lastseq)
                {
                    return devicemac + "@@2@@" + updt_srv + devicemac + "/" + settingVer.GUID + "/";
                }
            }
            else
            {
                SYS_SETTINGVER settingVer = dal_settingver.SelectNewByAPSerial(apMac);
                if (settingVer.GUID != lastseq)
                {
                    return "true";
                }
            }
            return "false";
            //if (settingVer.GUID.Equals(lastseq))
            //    return false;
            //return true;

        }
        /// <summary>
        /// 判断读取固件的mac地址是否正确
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="firmwarever"></param>
        /// <param name="verno"></param>
        /// <returns></returns>
        public SYS_FIRMWARE CheckFirmWareVersion(string devicemac, string firmwarever, string verno)
        {
            string apMac = devicemac.Substring(0, 2) + "-" + devicemac.Substring(2, 2) + "-" + devicemac.Substring(4, 2) + "-" + devicemac.Substring(6, 2) + "-" + devicemac.Substring(8, 2) + "-" + devicemac.Substring(10, 2);
            return dal_firmware.CheckVersion(apMac, firmwarever, verno);
        }

        public string UpdateDevice(string devicemac, string lastseq)
        {
            if (devicemac.Length != 12)
            {
                return "";
            }
            string apMac = devicemac.Substring(0, 2) + "-" + devicemac.Substring(2, 2) + "-" + devicemac.Substring(4, 2) + "-" + devicemac.Substring(6, 2) + "-" + devicemac.Substring(8, 2) + "-" + devicemac.Substring(10, 2);
            //此处有问题，可以将setting 和apdevice的对象合并为一个，这样只需要一条查询sql语句即可
            M_APSETTINGVER_VIEW apSetting = dal_settingver.SelectByApMac(apMac);

            //SYS_SETTINGVER settingVer = dal_settingver.SelectNewByAPSerial(apSerial);
            int nasId = 1;
            //SYS_APDEVICE apDevice = dal_apDevice.SelectByMAC(apSerial);//通过Serial查找AP设备
            if (apSetting == null)
            {
                return null;
            }
            string IsUpdateBin = ConfigurationSettings.AppSettings["IsUpdateBin"];
            if (IsUpdateBin == "true")
            {
                if (apSetting.GUID != lastseq)
                {
                    return null;
                }
            }
            else
            {
                if (apSetting.GUID == lastseq)
                {
                    return null;
                }
            }
            List<SYS_SSID> list_SSID = dal_ssid.SelectByAPID(apSetting.APID, null);
            NAS nas = new NAS();

            nas = dal_nas.Select(nasId);

            string updt_srv = ConfigurationSettings.AppSettings["UpdateService"];
            string binupdt_srv = ConfigurationSettings.AppSettings["UpdateBinService"];
            StringBuilder strs = new StringBuilder();
            strs.Append("config luobocloud 'config'\n");
            strs.Append("option updt_srv '"); strs.Append(updt_srv + "'\n");
            strs.Append("option binupdt_srv '"); strs.Append(binupdt_srv + "'\n");
            strs.Append("option cfgver '"); strs.Append(apSetting.GUID + "'\n");
            //strs.Append("option macaddr '"); strs.Append(apDevice.MAC + "'\n");
            strs.Append("option devid '"); strs.Append(apSetting.SERIAL + "'\n");
            strs.Append("option wificnt '"); strs.Append(list_SSID.Count.ToString() + "'\n");
            strs.Append("option channel '" + apSetting.APCHANNEL + "'\n");
            strs.Append("option txpower '" + apSetting.POWER + "'\n");
            strs.Append("option disabled '" + (apSetting.ISSSIDON ? 0 : 1) + "'\n");
            strs.Append("\n");
            intcount = 0;
            foreach (SYS_SSID ssid in list_SSID)
            {
                intcount = intcount + 1;
                strs.Append("config wifi-iface\n");
                strs.Append("option ssid '"); strs.Append(ssid.ID + "'\n");
                strs.Append("option name '"); strs.Append(ssid.NAME + "'\n");//SSID name
                strs.Append("option radiusnasid '" + ssid.ID + "'\n"); //strs.Append(nas.ID + "\n");//nas
                strs.Append("option uamsecret '9b8619251a19057cff70779273e95aa6'\n");
                strs.Append("option dns1 '114.114.114.114'\n");
                strs.Append("option dns2 '8.8.8.8'\n");
                if (apSetting.DEVICESTATE == (int)Helper.CustomEnum.ENUM_ApState.Demo)
                {
                    strs.Append("option uamurl '" + ssid.PATH + ssid.PORTAL + "'\n");
                }
                else
                {
                    if (ssid.PATH.IndexOf("http://") >= 0)
                    {
                        strs.Append("option uamurl '" + ssid.PATH + ssid.PORTAL + "'\n");
                    }
                    else
                    {
                        strs.Append("option uamurl 'http://" + dns + ssid.PATH + ssid.PORTAL + "'\n");
                    }
                }

                // 拼凑放行域名
                List<AD_FREEHOST> list_freehost = dal_freehost.SelectByADID(ssid.ADID);                
                if (list_freehost.Count == 0)
                    strs.Append("option uamallow '" + dns + "'\n");
                else
                    strs.Append("option uamallow '" + dns + "," + list_freehost.ToString("F_Host",",") + "'\n");
                //strs.Append("option uamallow 'next-wifi.com,ad.next-wifi.com,tjs.sjs.sinajs.cn,qzonestyle.gtimg.cn,www.ptweixin.com,static.ptweixin.com,img.ptweixin.com,js.t.sinajs.cn,img.t.sinajs.cn,api.weibo.com,graph.qq.com,timg.sjs.sinajs.cn,openapi.qzone.qq.com,qzonestyle.gtimg.cn,pub.idqqimg.com,itunes.apple.com,ssl.apple.com,securemetrics.apple.com,s.mzstatic.com,a2.mzstatic.com,a4.mzstatic.com,mobile.psbc.com,wap.psbc.com," + dns + "'\n");
                //strs.Append("option uamdomain '.next-wifi.com,.sinajs.cn,.gtimg.cn,.ptweixin.com,.weibo.com,.qq.com,.gtimg.cn,.idqqimg.com,.apple.com,.mzstatic.com,.mobile.psbc.com'\n");
                //strs.Append("option aaa 'http'\n");
                //strs.Append("option uamurl 'http://" + dns + ":5216" + ssid.PATH  + "'\n");
                //strs.Append("option uamaaaurl 'http://10." + intcount.ToString() + ".0.1:4992/www/usb/demos/www/'\n");
                strs.Append("option radiusserver1 '"); strs.Append(nas.Server + "'\n");
                strs.Append("option radiusserver2 '192.168.1.11'\n"); //strs.Append(nas.Server + "\n");//nas
                strs.Append("option radiussecret '"); strs.Append(nas.Secret + "'\n");//nas
                if (ssid.ISPWD)
                {
                    strs.Append("option encryption 'psk2'\n");
                    strs.Append("option key '"); strs.Append(ssid.PWD + "'\n");
                }
                else
                    strs.Append("option encryption 'none'\n");

                Int64 dayFlowLimit = dal_sim.SelectDailyLimitByPAID(apSetting.APID);
                if (dayFlowLimit > -1)
                    strs.Append("option dayflowlimit '" + dayFlowLimit + "'\n");
                strs.Append("\n");
            }
            return strs.ToString();
        }

        public SYS_FIRMWARE CheckFirmWareVersion(string devicemac, string lastseq)
        {
            string apMac = devicemac.Substring(0, 2) + "-" + devicemac.Substring(2, 2) + "-" + devicemac.Substring(4, 2) + "-" + devicemac.Substring(6, 2) + "-" + devicemac.Substring(8, 2) + "-" + devicemac.Substring(10, 2);
            return dal_firmware.CheckVersion(apMac, lastseq);
        }

        public bool UpdateFreeHost(long adId, string freehost, string defaultfree)
        {
            AD_FREEHOST data = new AD_FREEHOST();
            data.AD_ID = adId;
            data.F_Host = freehost;
            data.F_Default = defaultfree;
            return dal_freehost.UpdateFreeHost(data);
        }
    }
}
