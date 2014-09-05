using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_SSIDConfig
    {
        /// <summary>
        /// 配置类型0:freeUser 1:微博 2：微信 3：qq 4：短信认证
        /// </summary>
        public int ConfigType { get; set; }
        public String APMac { get; set; }
        public int SSID { get; set; }
        /// <summary>
        /// 用户最大时常（秒）
        /// </summary>
        public Int64 TimeLimit { get; set; }
        /// <summary>
        /// 最大流量
        /// </summary>
        public Int64 MaxTraffic { get; set; }
        /// <summary>
        /// 上行带宽
        /// </summary>
        public Int64 MaxUpRate { get; set; }
        /// <summary>
        /// 下行带宽
        /// </summary>
        public Int64 MaxDownRate { get; set; }
    }
}
