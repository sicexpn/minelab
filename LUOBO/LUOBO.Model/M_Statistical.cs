using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.Model
{
    public class M_Statistical
    {
        public Int64 ID { get; set; }
        public string NAME { get; set; }
        public Int64 NUM { get; set; }
    }

    public class M_APListForState
    {
        public List<SYS_APDEVICE> APList = new List<SYS_APDEVICE>();
        public Int64 AvgVisitNum = 0;
    }

    public class M_UserForState
    {
        public List<M_Passager> UserList = new List<M_Passager>();
        public Int64 AllPeopleCount = 0;
        public Int64 OnlinePeopleNum = 0;
        public Int64 RZUserNum = 0;
        public Int64 AvgVisitNum = 0;
        public Int64 AvgVisitTime = 0;
    }
    #region 月统计
    /// <summary>
    /// 月统计总体情况
    /// </summary>
    public class M_Month_Total
    {
        public Int64 ZONGRS { get; set; }
        public Int64 ZONGSWRC { get; set; }
        public double PINGJSWCS { get; set; }
        public Int64 GUANGGDJRC { get; set; }
        public Int64 XIAZRC { get; set; }
        public Int64 XIAZRS { get; set; }
        public double PINGJSWSC { get; set; }
        public double LIANJRQXZB { get; set; }
        public List<M_Statistical> BUTCZXTXZB { get; set; }
        public double PINGJYYTXZB { get; set; }
    }

    public class M_Month_UserBehavior
    {
        public string SHOUJZD { get; set; }
        public Int64 SHUL { get; set; }
        public double BIL { get; set; }
        public List<M_Statistical> SSID { get; set; }
    }

    public class M_Month_ADOrDown
    {
        public string LEIX { get; set; }
        public List<StatisticalAP> VALUE { get; set; }
    }

    public class M_Month_YingYeTing
    {
        public string MINGC { get; set; }
        public Int64 RENC { get; set; }
        public Int64 RENS { get; set; }
        public double PINGJFWCS { get; set; }
        public double PINGJFWSC { get; set; }
        public Int64 XIAZCS { get; set; }
        public Int64 GUANGGDJCS { get; set; }
        public Int64 XIAZRS { get; set; }
        public List<M_Statistical> BUTCZXTXZB { get; set; }
    }

    public class M_Month_AnQuan
    {
        public string ALIAS { get; set; }
        public Int64 KEY { get; set; }
        public Int64 XINR { get; set; }
        public Int64 ZHONGW { get; set; }
        public Int64 TONGY { get; set; }
        public Int64 XINZ { get; set; }
        public Int64 ZONGS { get; set; }
    }
    #endregion
}
