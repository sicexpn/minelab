using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SSID_AUDIT
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
        public Int64 SSIDID { get; set; }
        /// <summary>
        /// SSID名称
        /// </summary>
        public string SSIDNAME { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime APPLYTIME { get; set; }
        /// <summary>
        /// 申请机构
        /// </summary>
        public Int64 APPLYOID { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string APPLICANT { get; set; }
        /// <summary>
        /// 审核机构
        /// </summary>
        public Int64 AUDITOID { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AUDITER { get; set; }
        /// <summary>
        /// 审核说明
        /// </summary>
        public string AUDITINTRO { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AUDITTIME { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int STATE { get; set; }
    }
}
