using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SSID_AUDIT_VIEW
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// SSIDID
        /// </summary>
        public Int64 SSIDID { get; set; }
        public Int64 APID { get; set; }
        /// <summary>
        /// SSID名称
        /// </summary>
        public string SSIDNAME { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime APPLYTIME { get; set; }
        /// <summary>
        /// 申请机构ID
        /// </summary>
        public Int64 APPLYOID { get; set; }
        /// <summary>
        /// 申请机构名称
        /// </summary>
        public string ONAME { get; set; }
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
