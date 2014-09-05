using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_DEVICESTATE
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// SSIDID
        /// </summary>
        public Int64 SSID { get; set; }
        /// <summary>
        /// 单位时间SSID广告访问次数
        /// </summary>
        public Int64 VCOUNT { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime CURRENTTIME { get; set; }
    }
}
