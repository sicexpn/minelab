using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
using LUOBO.DAL;
using System.Transactions;
using LUOBO.Model;

namespace LUOBO.BLL
{
    public class BLL_OpenSSID_Statistical
    {
        DAL_OpenSSID_Statistical oDal = new DAL_OpenSSID_Statistical();
        DAL_SYS_USER uDAL = new DAL_SYS_USER();
        DAL_SYS_ALARMSCOPE scopeDAL = new DAL_SYS_ALARMSCOPE();
        DAL_SYS_SSID ssidDAL = new DAL_SYS_SSID();
        public List<OpenSSID> Select()
        {
            return oDal.Select();
        }
        public List<OpenSSID> SelectByUser(long userid)
        {
            return oDal.SelectByUser(userid);
        }
        public List<M_Passager> SelectPassagers(Int64 OID, string apMac, string column, string orderby, Int64 size, Int64 curPage)
        {
            switch (column)
            {
                case "ConnectTime":
                    column = "a.CurrentTime";
                    break;
                case "OnLineTime":
                    column = "b.AcctSessionTime";
                    break;
                case "OnLineCounts":
                    column = "Count(b.AcctSessionId)";
                    break;
                case "UsedTraffic":
                    column = "b.AcctInputOctets + b.AcctOutputOctets";
                    break;
                case "OnLineType":
                    column = "c.userType";
                    break;
                case "LineCounts":
                    column = "Count(DISTINCT a.AcctSessionId)";
                    break;
            }
            return oDal.SelectPassagers(OID, apMac, column, orderby, size, curPage);
        }
        public Int64 SelectPassagersCount(Int64 OID, string apMac)
        {
            return oDal.SelectPassagersCount(OID, apMac);
        }
        public bool Insert(OpenSSID data)
        {
            scopeDAL.UpdateCountAddOne(data);
            return oDal.Insert(data);
        }

        public bool Inserts(List<OpenSSID> datas)
        {
            bool flag = false;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (OpenSSID data in datas)
                        oDal.Insert(data);

                    flag = true;
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("错误原因是：" + ex.Message);
                }
            }

            return flag;
        }

        public List<OpenSSID_VIEW> SelectAdInfoByCallingID(Int64 OID, string callingID, string column, string orderby)
        {
            return oDal.SelectInfoByCallingID(OID, callingID, column, orderby);
        }

        public List<Int64> SelectOLInfoByCallingID(string callingID)
        {
            return oDal.SelectOLInfoByCallingID(callingID);
        }

        #region AP统计页
        public List<StatisticalAP> SelectSSIDPeopleStatistical(string token, Int64 APID, DateTime startDate, DateTime endDate)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectSSIDPeopleStatistical(user.OID, APID, startDate, endDate);
        }
        public List<M_Statistical> SelectAuthenticationPeopleStatistical(string token, Int64 APID, DateTime startDate, DateTime endDate)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectAuthenticationPeopleStatistical(user.OID, APID, startDate, endDate);
        }
        public List<StatisticalAP> SelectSSIDUseTimeStatistical(string token, Int64 APID, DateTime startDate, DateTime endDate)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectSSIDUseTimeStatistical(user.OID, APID, startDate, endDate);
        }
        public List<StatisticalAP> SelectSSIDTrafficStatistical(string token, Int64 APID, DateTime startDate, DateTime endDate)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectSSIDTrafficStatistical(user.OID, APID, startDate, endDate);
        }
        public List<StatisticalAP> SelectAPOfADStatistical(string token, Int64 APID, DateTime startDate, DateTime endDate)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectAPOfADStatistical(user.OID, APID, startDate, endDate);
        }
        public List<Int64> SelectTowHourIntervalPeopleCount(string token, Int64 APID)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectTowHourIntervalPeopleCount(user.OID, APID);
        }
        public List<StatisticalAP> SelectTowHourIntervalModelCount(string token, Int64 APID)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectTowHourIntervalModelCount(user.OID, APID);
        }
        public List<StatisticalAP> SelectTowHourIntervalSSIDCount(string token, Int64 APID)
        {
            SYS_USER user = uDAL.SelectByToken(token);
            return oDal.SelectTowHourIntervalSSIDCount(user.OID, APID);
        }
        #endregion

        #region 月统计
        
        /// <summary>
        /// 机构下根据AP不同时段的点击次数（6-12 12-18 18-6点）
        /// APID为0是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<Int64> GetClicksByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            return oDal.GetClicksByAPAndDate(oid, apid, date);
        }

        /// <summary>
        /// 机构下指定AP访问人次
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public Int64 GetVisitsByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            return oDal.GetVisitsByAPAndDate(oid, apid, date);
        }

        /// <summary>
        /// 机构下指定AP认证后总访问时长和平均时长
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<Int64> GetApproveTimeByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            return oDal.GetApproveTimeByAPAndDate(oid, apid, date);
        }

        /// <summary>
        /// 业务推广分析 -- 暂时被下边取代了
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <param name="isDown">是否下载</param>
        /// <returns></returns>
        public List<StatisticalAP> GetPromotionByAPAndDate(Int64 oid, Int64 apid, DateTime date, bool isDown)
        {
            return oDal.GetPromotionByAPAndDate(oid, apid, date, isDown);
        }

        /// <summary>
        /// 业务推广分析 下载分析 -- 不同手机操作系统，不同时段的广告或下载点击数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <param name="isDown">是否下载</param>
        /// <returns></returns>
        public List<StatisticalAP> GetOSADClicksByAPAndDate(Int64 oid, Int64 apid, DateTime date, bool isDown)
        {
            return oDal.GetOSADClicksByAPAndDate(oid, apid, date, isDown);
        }

        /// <summary>
        /// 业务推广分析 下载分析 -- 不同广告的点击次数
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<M_Statistical> GetADByAPAndDate(Int64 oid, Int64 apid, DateTime date, bool isDown)
        {
            return oDal.GetADByAPAndDate(oid, apid, date, isDown);
        }

        /// <summary>
        /// 业务推广分析 下载分析 -- 不同营业厅的不同广告或下载点击数
        /// APID为0是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP> GetOSADClicksByDate(Int64 oid, DateTime date, bool isDown)
        {
            return oDal.GetOSADClicksByDate(oid, date, isDown);
        }

        /// <summary>
        /// 用户行为与构成分析 -- 不同操作系统不同SSID的点击次数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <param name="isDown">是否下载</param>
        /// <returns></returns>
        public List<StatisticalAP<M_Statistical>> GetUserBehaviorByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            return oDal.GetUserBehaviorByAPAndDate(oid, apid, date);
        }

        /// <summary>
        /// 用户行为与构成分析 -- 不同操作系统的人群分布
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <param name="isDown">是否下载</param>
        /// <returns></returns>
        public List<StatisticalAP<M_Statistical>> GetDifferentOSPersonAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            return oDal.GetDifferentOSPersonAPAndDate(oid, apid, date);
        }

        /// <summary>
        /// 机构下，访问不同SSID的人群分布
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="apid">APID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<Int64> GetSSIDVisitByAPAndDate(Int64 oid, Int64 apid, DateTime date)
        {
            return oDal.GetSSIDVisitByAPAndDate(oid, apid, date);
        }

        /// <summary>
        /// 机构下所有营业厅总人次、总人数、下载人数、广告点击次数、下载点击次数、
        /// iphone下载次数、ipad下载次数、安卓下载次数、win phone下载次数、win nt下载次数、mac os下载次数、其他下载次数
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP> GetVisitsByDate(Int64 oid, DateTime date)
        {
            return oDal.GetVisitsByDate(oid, date);
        }

        public StatisticalAP GetTotalInfoByDate(Int64 oid, DateTime date)
        {
            return oDal.GetTotalInfoByDate(oid, date);
        }

        /// <summary>
        /// 机构下不同不同营业厅认证后总访问时长和平均时长
        /// APID为-99是全部AP
        /// </summary>
        /// <param name="oid">机构ID</param>
        /// <param name="date">统计年月</param>
        /// <returns></returns>
        public List<StatisticalAP<double>> GetApproveTimeByDate(Int64 oid, DateTime date)
        {
            return oDal.GetApproveTimeByDate(oid, date);
        }

        /// <summary>
        /// 机构下不同营业厅广告点击数和下载点击数
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<StatisticalAP> GetADorDownByDate(Int64 oid, DateTime date)
        {
            return oDal.GetADorDownByDate(oid, date);
        }
        #endregion
    }
}
