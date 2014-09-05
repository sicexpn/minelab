using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace LUOBO.Entity
{
    /// <summary>
    /// OpenSSID表
    /// </summary>
    public class OpenSSID
    {
        //public Int64 RadAcctId { get; set; }
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

        public DateTime CurrentTime { get; set; }
    }
}
