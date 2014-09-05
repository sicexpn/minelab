using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.BusinessService
{
    public class StatisticalPeople
    {
        public Int64 ID { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
    }

    public class StatisticalParam
    {
        private Int64 _id = -99;
        private string _type = "";
        private string _mode = "";
        private string _startTime = "";
        private string _endTime = "";

        /// <summary>
        /// 编号
        /// </summary>
        public Int64 ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 类型 AP、SSID
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 模式
        /// Week=按周
        /// Month=按月
        /// Year=按年
        /// Date=时间区间
        /// </summary>
        public string Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        /// <summary>
        /// 起始时间
        /// </summary>
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
    }

    public class PieData
    {
        public Int64 ID { get; set; }
        public string NAME { get; set; }
        public Int64 NUM { get; set; }
    }
}