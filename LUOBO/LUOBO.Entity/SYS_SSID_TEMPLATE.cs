using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SSID_TEMPLATE
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 模版名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 所属机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// 行业ID
        /// </summary>
        public Int32 INDUSTRY { get; set; }
    }
}
