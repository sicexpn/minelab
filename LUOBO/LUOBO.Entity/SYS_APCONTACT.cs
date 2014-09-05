using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_APCONTACT
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        public string ONAME { get; set; }

        public Int64 ISOWNORG { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string CONTACT { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string EMAIL { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        public Int32 REFRESHTIME { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public Int32 NOTICETYPE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATETIME { get; set; }
    }
}
