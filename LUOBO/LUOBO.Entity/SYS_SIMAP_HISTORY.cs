using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SIMAP_HISTORY
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`ASID` bigint(20) DEFAULT NULL,
        //`APID` bigint(20) DEFAULT NULL,
        //`SNCID` bigint(20) DEFAULT NULL,
        //`STARTTIME` datetime DEFAULT NULL,
        //`ENDTIME` datetime DEFAULT NULL,
        public Int64 ID { get; set; }
        /// <summary>
        /// AP手机关系表ID
        /// </summary>
        public Int64 ASID { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// 手机卡上网设备关系表ID
        /// </summary>
        public Int64 SNCID { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime STARTTIME { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ENDTIME { get; set; }
    }
}
