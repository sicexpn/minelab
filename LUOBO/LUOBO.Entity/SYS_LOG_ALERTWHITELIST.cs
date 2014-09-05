using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 机构对应设备白名单
    /// </summary>
    public class SYS_LOG_ALERTWHITELIST
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// 要排除的白名单
        /// </summary>
        public string MAC { get; set; }
    }
}
