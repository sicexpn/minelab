using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Model;
using LUOBO.DAL;
using LUOBO.Entity;
using System.Timers;
using System.Configuration;
using LUOBO.Helper;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;

namespace LUOBO.BLL
{
    public class BLL_Esper
    {
        private static BLL_Esper _instance;
        private bool isDebug = Convert.ToBoolean(ConfigurationSettings.AppSettings["AlarmIsDebug"]);
        private string logPath = "D:\\LUOBOFile\\Log\\SafeService\\";
        StreamWriter logWriter = null;
        private Timer alarmTimer = new Timer();
        private int checkInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["CheckInterval"]);
        private string businessServicePath = ConfigurationSettings.AppSettings["BusinessServicePath"].ToString();
        private string sendToEmail = ConfigurationSettings.AppSettings["SendToEmail"].ToString();
        List<M_SYS_ALARMRULE> alarmRuleList = null;
        List<SYS_ALARMSCOPE> alarmScopeList = null;
        
        //List<SYS_DEVICESTATE> deviceStateList = null;
        List<SYS_ALARMSCOPE> haveAlarmList = null;

        List<SYS_APDEVICE> apList = null;
        //List<M_EXCEPTDEVICE> exceptDeviceList = null;
        Dictionary<string, string> ignoreDevice = new Dictionary<string, string>();

        private DAL_OpenSSID_Statistical openDAL = new DAL_OpenSSID_Statistical();
        private DAL_SYS_ALARMRULE arDAL = new DAL_SYS_ALARMRULE();
        private DAL_SYS_ALARMSCOPE scopeDAL = new DAL_SYS_ALARMSCOPE();
        //private DAL_SYS_DEVICESTATE dsDAL = new DAL_SYS_DEVICESTATE();
        private DAL_SYS_SSID ssidDAL = new DAL_SYS_SSID();
        private DAL_SYS_APDEVICE apDAL = new DAL_SYS_APDEVICE();

        public static BLL_Esper Instance()
        {
            if (_instance == null)
            {
                _instance = new BLL_Esper();
                _instance.ResetTimer();
            }
            return _instance;
        }

        BLL_Esper()
        {
            alarmTimer.Elapsed += new ElapsedEventHandler(TimerHandler);
        }

        ~BLL_Esper()
        {
            if (alarmTimer.Enabled)
            {
                alarmTimer.Stop();
                alarmTimer.Elapsed -= new ElapsedEventHandler(TimerHandler);
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 异常监测服务停止了...");
            }
        }

        /// <summary>
        /// 重置设备监测，重新载入配置
        /// </summary>
        public void ResetTimer()
        {
            if (!isDebug)
            {
                if (alarmTimer.Enabled)
                    alarmTimer.Stop();
                alarmRuleList = PubFun.ChangeNewList<M_SYS_ALARMRULE, SYS_ALARMRULE>(arDAL.Select());
                scopeDAL.ResetVCountAll(alarmRuleList.Where(c => c.AL_TYPE != (int)Helper.CustomEnum.ENUM_Alarm_Type.Close).Select(c => c.AL_ID).ToList());
                alarmScopeList = scopeDAL.SelectAll();

                checkInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["CheckInterval"]);
                sendToEmail = ConfigurationSettings.AppSettings["SendToEmail"].ToString();
                alarmTimer.Interval = checkInterval * 1000;

                alarmTimer.Start();
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")+" 异常监测服务启动...");
            }
        }

        /// <summary>
        /// 指定设备当天不再发送邮件
        /// </summary>
        /// <param name="APID"></param>
        /// <param name="type">忽略类型：1:不告警设备宕机（HB），2：不告警设备访问异常（EX）</param>
        /// <returns></returns>
        public bool IgnoreDevice(string APID, string type)
        {
            try
            {
                if (type == "1")
                {
                    if (ignoreDevice.ContainsKey("HB" + APID))
                        ignoreDevice["HB" + APID] = DateTime.Now.ToString("yyyyMMdd");
                    else
                        ignoreDevice.Add("HB" + APID, DateTime.Now.ToString("yyyyMMdd"));
                }
                else
                {
                    if (ignoreDevice.ContainsKey("EX" + APID))
                        ignoreDevice["EX" + APID] = DateTime.Now.ToString("yyyyMMdd");
                    else
                        ignoreDevice.Add("EX" + APID, DateTime.Now.ToString("yyyyMMdd"));
                }
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " APID为" + APID + "的设备今天不再发送" + (type == "1" ? "宕机" : "访问异常") + "邮件");
                return true;
                
            }
            catch (Exception ex)
            {
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 设备忽略时遇到异常..错误原因：" + ex.Message);
                return false;
            }
        }

        #region 私有函数
        /// <summary>
        /// Timer轮询事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void TimerHandler(object obj, ElapsedEventArgs e)
        {
            List<Int64> ids = null;
            WriterLog("----Timer轮询开始------------------------------------------------------------");
            foreach (var rule in alarmRuleList)
            {
                WriterLog("---------\r\n"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 开始检查规则(" + rule.AL_ID + ")：" + rule.AL_REMARK);
                // 是否在有效时间内
                if (!checkDateAndTime(rule))
                {
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + "不在有效时间内");
                    continue;
                }
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + "在有效时间内");
                // 是否到达检查时间
                if (!checkisRunEvent(rule))
                {
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + "未到达检查时间");
                    continue;
                }
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + "到达检查时间");

                #region 检查心跳
                if (rule.AL_MODE == (int)Helper.CustomEnum.ENUM_Alarm_Mode.All || rule.AL_MODE == (int)Helper.CustomEnum.ENUM_Alarm_Mode.HeartBeat)
                {
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + " 开始检查心跳");
                    apList = apDAL.SelectByIDs(alarmScopeList.Where(c => c.ALID == rule.AL_ID).ToString("APID", ","));
                    apList = apList.Where(c => c.LASTHB.AddSeconds(c.HBINTERVAL * 2) < DateTime.Now).ToList();
                    ids = ignoreDevice.Where(c => c.Key.Substring(0, 2) == "HB" && c.Value == DateTime.Now.ToString("yyyyMMdd")).Select(c => Convert.ToInt64(c.Key.Substring(2))).ToList();
                    apList = apList.Where(c => !ids.Contains(c.ID)).ToList();
                    if (apList.Count > 0)
                    {
                        sendHeartBeatEmail(apList, rule);
                    }
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + " 有" + apList.Count + "台设备没有没有心跳");
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + " 心跳检查结束");
                }
                #endregion

                #region 检查访问异常
                if (rule.AL_MODE == (int)Helper.CustomEnum.ENUM_Alarm_Mode.All || rule.AL_MODE == (int)Helper.CustomEnum.ENUM_Alarm_Mode.Except)
                {
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + "开始检查访问异常");
                    alarmScopeList = scopeDAL.SelectAll();
                    ids = ignoreDevice.Where(c => c.Key.ToString().Substring(0, 2) == "EX" && c.Value == DateTime.Now.ToString("yyyyMMdd")).Select(c => Convert.ToInt64(c.Key.Substring(2))).ToList();
                    haveAlarmList = alarmScopeList.Where(c => c.ALID == rule.AL_ID && !ids.Contains(c.APID) && (c.VCOUNT <= rule.AL_MIN || c.VCOUNT >= rule.AL_MAX)).ToList();
                    if (haveAlarmList.Count > 0)
                    {
                        sendExceptEmail(haveAlarmList, rule);
                    }
                    scopeDAL.ResetVCount(rule);
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + " 有" + haveAlarmList.Count + "访问异常");
                    WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")" + rule.AL_REMARK + "检查访问异常结束");
                }
                #endregion
                WriterLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 规则(" + rule.AL_ID + ")检查结束：" + rule.AL_REMARK+"\r\n---------");
            }
            WriterLog("----Timer轮询结束------------------------------------------------------------\r\n\r\n");
        }
        /// <summary>
        /// 检查是否在有效时段内
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private bool checkDateAndTime(M_SYS_ALARMRULE rule)
        {
            if (rule.AL_TYPE == (int)Helper.CustomEnum.ENUM_Alarm_Type.Close)
                return false;
            int week = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            if ((rule.AL_DATERULE & (1 << (week - 1))) != 0)
                if (DateTime.Now.TimeOfDay >= rule.AL_STIME && DateTime.Now.TimeOfDay <= rule.AL_ETIME)
                    return true;
            return false;
        }
        /// <summary>
        /// 根据轮询间隔判断是否需要执行检查事件
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private bool checkisRunEvent(M_SYS_ALARMRULE rule)
        {
            bool flag = false;
            switch (rule.AL_UNIT)
            {
                case "h":
                    rule.AL_CHECK += checkInterval / 3600.0;
                    break;
                case "m":
                    rule.AL_CHECK += checkInterval / 60.0;
                    break;
                case "d":
                    rule.AL_CHECK += checkInterval / 86400.0;
                    break;
            }
            if (rule.AL_CHECK >= rule.AL_TIME)
            {
                rule.AL_CHECK = 0;
                flag = true;
            }
            return flag;
        }
        /// <summary>
        /// 发送异常邮件
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="rule"></param>
        private void sendExceptEmail(List<SYS_ALARMSCOPE> devices, M_SYS_ALARMRULE rule)
        {
            Debug.WriteLine("SendEX-->" + devices.ToString("SSID", ","));

            List<M_EXCEPTDEVICE> exceptDeviceList = ssidDAL.GetSSIDBelong(devices.ToString("SSID", ","));
            NetworkCredential cred = new NetworkCredential("next_wifi@163.com", "wqyz@2014");
            MailMessage msg = new MailMessage();
            msg.Subject = rule.AL_REMARK;
            msg.From = new MailAddress("next_wifi@163.com");
            msg.To.Add(sendToEmail);
            string body = "<div style='font-size:13px;line-height: 1.8em'>";
            List<Int64> oids = exceptDeviceList.Select(c => c.OID).Distinct().ToList();
            List<M_EXCEPTDEVICE> devs;
            foreach (int oid in oids)
            {
                devs = exceptDeviceList.Where(c => c.OID == oid).ToList();
                body += "【" + devs.First().ONAME + "】机构：<br/>";
                foreach (var alarmDev in devs)
                {
                    body += "&nbsp;&nbsp;&nbsp;&nbsp;【" + alarmDev.APNAME + "】下的【" + alarmDev.SSIDNAME.Trim() + "】近 " + rule.AL_TIME + getUnit(rule.AL_UNIT) + " 内，用户访问量为 " + devices.Where(c => c.SSID == alarmDev.SSID).FirstOrDefault().VCOUNT + "。";
                    body += "  <a href='" + Path.Combine(businessServicePath, "BusinessService/IgnoreDeviceAlarm/" + alarmDev.APID + "/" + "2") + "'>【今天不再对该设备进行提醒】</a><br/>";
                }
            }
            body += "</div>";
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.163.com", 25);
            client.Credentials = cred;
            
            client.Send(msg);
        }
        /// <summary>
        /// 发送宕机邮件
        /// </summary>
        /// <param name="devices"></param>
        private void sendHeartBeatEmail(List<SYS_APDEVICE> devices, M_SYS_ALARMRULE rule)
        {
            Debug.WriteLine("SendHB-->" + devices.ToString("ALIAS", ","));

            List<M_EXCEPTDEVICE> exceptDeviceList = apDAL.SelectExceptDevice(devices.ToString("ID", ","));

            NetworkCredential cred = new NetworkCredential("next_wifi@163.com", "wqyz@2014");
            MailMessage msg = new MailMessage();
            msg.Subject = rule.AL_REMARK;
            msg.From = new MailAddress("next_wifi@163.com");
            msg.To.Add(sendToEmail);
            string body = "<div style='font-size:13px;line-height: 1.8em'>";
            List<Int64> apids = exceptDeviceList.Select(c => c.APID).Distinct().ToList();
            M_EXCEPTDEVICE devs;
            foreach (Int64 apid in apids)
            {
                devs = exceptDeviceList.Where(c => c.APID == apid).First();
                body += "【" + devs.ONAME + "】机构下，【" + devs.APNAME + "】设备没有心跳了，最后心跳时间【" + devices.Where(c => c.ID == apid).First().LASTHB.ToString("yyyy-MM-dd HH:mm:ss") + "】，请检查。";
                body += "  <a href='" + Path.Combine(businessServicePath, "BusinessService/IgnoreDeviceAlarm/" + apid + "/" + "1") + "'>【今天不再对该设备进行提醒】</a><br/>";
            }
            body += "</div>";
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.163.com", 25);
            client.Credentials = cred;
            
            client.Send(msg);
            
        }
        /// <summary>
        /// 根据缩写返回单位
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string getUnit(string str)
        {
            string tmp = "";
            switch (str)
            {
                case "M":
                    tmp = "月";
                    break;
                case "d":
                    tmp ="天";
                    break;
                case "h":
                    tmp = "小时";
                    break;
                case "m":
                    tmp = "分钟";
                    break;
                case "s":
                    tmp = "秒";
                    break;
                case "ms":
                    tmp = "毫秒";
                    break;
            }
            return tmp;
        }

        private void WriterLog(string text)
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            string filePath = logPath + "OP_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            logWriter = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write));
            logWriter.WriteLine(text);
            logWriter.Close();
        }
        #endregion
    }
}
