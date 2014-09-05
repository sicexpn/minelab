using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.DAL;
using LUOBO.Entity;
using LUOBO.Model;
using LUOBO.Helper;

namespace LUOBO.BLL
{
    public class BLL_Statistics
    {
        public DAL_RadAcct dal_radAcct = new DAL_RadAcct();
        public DAL_RadGroupReply dal_radGroupReply = new DAL_RadGroupReply();
        public DAL_SYS_APSTATE dal_apState = new DAL_SYS_APSTATE();
        public DAL_SYS_APORG dal_apOrg = new DAL_SYS_APORG();
        public DAL_SYS_APDEVICE dal_apDevice = new DAL_SYS_APDEVICE();
        public DAL_OpenSSID_Statistical dal_openSSID_Statical = new DAL_OpenSSID_Statistical();
        public DAL_SYS_USER uDAL = new DAL_SYS_USER();
        public DAL_SYS_ORGANIZATION orgDAL = new DAL_SYS_ORGANIZATION();
        public DAL_SYS_LOG_APNEAR apnDAL = new DAL_SYS_LOG_APNEAR();

        public string GetTrafficByApMac(string apMac, DateTime startTime, DateTime endTime)
        {
            return dal_radAcct.GetTrafficByApMac(apMac, startTime, endTime);
        }

        public string GetSessionTimeByApMac(string apMac, DateTime startTime, DateTime endTime)
        {
            return dal_radAcct.GetSessionTimeByApMac(apMac, startTime, endTime);
        }

        public string GetSessionTimeBySSID(string ssid, DateTime startTime, DateTime endTime)
        {
            List<string> acctSessionList = dal_openSSID_Statical.SelectSessionsbySSID(ssid, startTime);
            string sessionStr = string.Join(",", acctSessionList.ToArray());
            return dal_radAcct.GetSessionTimeBySSID(sessionStr, startTime, endTime);
        }

        public string GetTrafficBySSID(string ssid, DateTime startTime, DateTime endTime)
        {
            return dal_radAcct.GetTrafficBySSID(ssid, startTime, endTime);
        }

        public string GetAvailableTrafficByUser(string userName)
        {
            Int64 result = dal_radGroupReply.GetTopTrafficByUser(userName) - dal_radAcct.GetUsedTrafficByUser(userName);
            return result.ToString();
        }

        public string GetAvailableSessionTimeByUser(string userName)
        {
            Int64 result = dal_radGroupReply.GetTopSessionTimeByUser(userName) - dal_radAcct.GetUsedSessionTimeByUser(userName);
            return result.ToString();
        }

        public Int64 GetOnLineLoginUserCountsByApMac(string apMac)
        {
            return dal_radAcct.GetOnLineLoginUserCountsByApMac(apMac);
        }
        public Int64 GetOnLineLoginUserCounts()
        {
            return dal_radAcct.GetOnLineLoginUserCounts();
        }
        //public Model.M_PeopleCount GetPeopleCountByApMac(string apMac)
        //{
        //    DateTime? apPowerTime = dal_apDevice.SelectPowerTimeByApMac(apMac);
        //    DateTime? apStartTime = dal_apOrg.SelectApStartTimeByApMac(apMac);
        //    M_PeopleCount result = new M_PeopleCount();
        //    result.OnlinePeopleNum = dal_apDevice.GetOnLineUserNumsByApMac(apMac);// 连接当前ap的在线人数
        //    apMac = apMac.Substring(0, apMac.Length - 1);
        //    apMac += "_";

        //    result.InstalPeopleCount = dal_openSSID_Statical.GetInstalPeopleCount(apMac, apStartTime);//ap安装之后的打开广告业的人次
        //    result.InstalPeopleNum = dal_openSSID_Statical.GetInstalPeopleNum(apMac, apStartTime);//ap安装之后的打开广告业的人数
        //    result.StartPeopleCount = dal_openSSID_Statical.GetStartPeopleCount(apMac, apPowerTime);//ap开机之后的打开广告业的人次
        //    result.StartPeopleNum = dal_openSSID_Statical.GetStartPeopleNum(apMac, apPowerTime);// ap开机之后的打开广告业的人数


        //    return result;
        //}

        public M_PeopleCount GetPeopleCountByOID(string oID)
        {
            Int64 orgID = Int64.Parse(oID);
            return dal_openSSID_Statical.SelectCountByOrg(orgID);
            //List<M_OrgApTime> listOrgApTime = dal_apOrg.SelectOrgApTimeByOID(oID);//获取对应ap的启动时间/安装时间等待信息
            //if (listOrgApTime == null)
            //    return null;
            //M_PeopleCount result = new M_PeopleCount();
            //M_PeopleCount peopleCount = new M_PeopleCount();
            //foreach (M_OrgApTime data in listOrgApTime)
            //{
            //    peopleCount = dal_openSSID_Statical.SelectPeopleCount(data);
            //    result.InstalPeopleCount += peopleCount.InstalPeopleCount;
            //    result.InstalPeopleNum += peopleCount.InstalPeopleNum;
            //    result.StartPeopleCount += peopleCount.StartPeopleCount;
            //    result.StartPeopleNum += peopleCount.StartPeopleNum;
            //}
            //result.OnlinePeopleNum = dal_apDevice.GetOnLineUserNumsByOID(orgID);
            //return result;
        }

        public List<List<Int64>> GetOLPeopleByDateAndOID(DateTime start, DateTime end, string token, Int64 apid, CustomEnum.ENUM_Statistical_Type type)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            List<SYS_ORGANIZATION> org = orgDAL.SelectParent(user.OID);
            org.Add(new SYS_ORGANIZATION() { ID = user.OID });
            return dal_openSSID_Statical.SelectByDateAndOID(start, end, org.ToString("ID", ","), apid, type);
        }

        public List<M_Statistical> SelectStatisticalADByToken(string token, Int64 apid, DateTime startTime, DateTime endTime)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            List<M_Statistical> list = dal_openSSID_Statical.SelectStatisticalADByOID(user.OID, apid, startTime, endTime);
            for (int i = 0; i < list.Count; i++)
                list[i].ID = i + 1;
            return list;
        }

        public List<List<Int64>> SelectStatisticalWIFIByToken(string token, Int64 apid, DateTime startTime, DateTime endTime, CustomEnum.ENUM_Statistical_Type type)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return dal_openSSID_Statical.SelectStatisticalWIFIByOID(user.OID, apid, startTime, endTime, type);
        }

        public List<List<Int64>> SelectOnlinePeopleNum_MapByToken(string token, Int64 apid, DateTime startTime, DateTime endTime, CustomEnum.ENUM_Statistical_Type type)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return dal_openSSID_Statical.SelectOnlinePeopleNum_MapByOID(user.OID, apid, startTime, endTime, type);
        }

        public Int64 SelectAvgVisitNumByToken(string token)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return dal_openSSID_Statical.SelectAvgVisitNumByOID(user.OID);
        }

        public List<Int64> SelectUserForStateByToken(string token, string apMac)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return dal_openSSID_Statical.SelectUserForStateByOID(user.OID, apMac);
        }

        public List<StatisticalAP> GetAPNearStatistical(string token, DateTime date)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return apnDAL.GetAPNearStatistical(user.OID, date);
        }

        public List<Int32> GetCheckTypeListByMac(string token,string mac)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return dal_radAcct.GetCheckTypeListByMac(mac, user.OID);
        }
    }
}
