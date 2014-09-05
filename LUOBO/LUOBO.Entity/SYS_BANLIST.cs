using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_BANLIST
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public Int32 TYPE { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public string RES { get; set; }
        /// <summary>
        /// SSIDID
        /// </summary>
        public Int64 SSIDID { get; set; }
    }
}
