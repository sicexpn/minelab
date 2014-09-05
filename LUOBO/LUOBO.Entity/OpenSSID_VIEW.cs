using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class OpenSSID_VIEW
    {
        public Int64 ID { get; set; }
        public String SSID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public String OID { get; set; }
        /// <summary>
        /// 广告ID
        /// </summary>
        public String AdId { get; set; }
        /// <summary>
        /// SessionId
        /// </summary>
        public String AcctSessionId { get; set; }
        /// <summary>
        /// 用户设备连接到的设备（比如路由器）Mac地址
        /// </summary>
        public String CalledStationId { get; set; }
        /// <summary>
        /// 用户当前使用的设备Mac地址
        /// </summary>
        public String CallingStationId { get; set; }
        public String Title { get; set; }
        public String PageUrl { get; set; }
        /// <summary>
        /// UA信息
        /// </summary>
        public String UserAgent { get; set; }
        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public String ALIAS { get; set; }
        /// <summary>
        /// SSID名称
        /// </summary>
        public String SSIDNAME { get; set; }
    }
}
