using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class APBySSIDCount
    {
        /// <summary>
        /// AP设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// SSID数量
        /// </summary>
        public Int64 SSIDCount { get; set; }
    }
}
