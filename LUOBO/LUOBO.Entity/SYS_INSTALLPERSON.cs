using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_INSTALLPERSON
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 IP_ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 IP_OID { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string IP_NAME { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string IP_CONTACT { get; set; }
        /// <summary>
        /// 设备MAC
        /// </summary>
        public string IP_MAC { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime IP_CREATETIME { get; set; }
    }
}
