using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_Passager
    {
        public Int64 ID { get; set; }
        /// <summary>
        /// 客户Mac
        /// </summary>
        public String MAC { get; set; }
        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
        /// <summary>
        /// 上网时长（秒）
        /// </summary>
        public Int64 OnLineTime { get; set; }
        /// <summary>
        /// 上网次数
        /// </summary>
        public Int64 OnLineCounts { get; set; }
        /// <summary>
        /// 连接次数
        /// </summary>
        public Int64 LineCounts { get; set; }
        /// <summary>
        /// 使用流量
        /// </summary>
        public Int64 UsedTraffic { get; set; }
        /// <summary>
        /// 认证方式号0：1：2：
        /// </summary>
        public string OnLineType { get; set; }
    }
}
