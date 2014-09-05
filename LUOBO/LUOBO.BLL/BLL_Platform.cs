using System;
using LUOBO.Entity;
using LUOBO.DAL;
using System.Collections;
using System.Text;
using System.IO;

namespace LUOBO.BLL
{
    public class BLL_Platform
    {
        DAL_SYS_SETTINGVER settingverDAL = new DAL_SYS_SETTINGVER();

        public string GetHBResponse(Int64 serial, string version)
        {
            StringBuilder sb = new StringBuilder();
            //    Entity.SYS_SETTINGVER settingver = settingverDAL.SelectNewByAPSerial(serial);

            //    if (settingver.ID <= 0)
            //    {
            //        return null;
            //    }

            //    sb.Append("0@@" + settingver.GUID + "\n");

            //    string baseUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APSettingFile", deviceSerial + "", settingver.GUID);

            //    ArrayList fileList = null;
            //    //没有版本更新时
            //    if (version == settingver.GUID)
            //    {
            //        if (apManageBll.CheckIsRebootByAPID(settingver.APID))
            //            result.Append("3@@reboot\n");
            //        apManageBll.RebootComplete(settingver.APID);
            //    }
            //    else
            //    {
            //        // 如果没有配置文件，则直接跳过
            //        if (!Directory.Exists(baseUrl))
            //        {
            //            return result.ToString();
            //        }

            //        fileList = fileHelper.GetAllFileName(baseUrl);
            //        string platform_url = System.Configuration.ConfigurationSettings.AppSettings["platform_url"];
            //        string tmp = "";
            //        foreach (string item in fileList)
            //        {
            //            tmp = item.Substring(baseUrl.Length).Replace('\\', '/');
            //            result.Append("1@@" + tmp + "@@" + "http://" + platform_url + "/luobo/df/" + deviceSerial + "/" + settingver.GUID + "/" + tmp.Substring(1).Replace('/', '_') + "\n");
            //        }
            //        result.Append("3@@reboot\n");
            //    }
            //}

            
            return sb.ToString();
        }

    }
}
