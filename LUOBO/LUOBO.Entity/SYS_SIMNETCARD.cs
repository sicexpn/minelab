using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SIMNETCARD
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`SIMID` bigint(20) DEFAULT NULL,
        //`NCID` bigint(20) DEFAULT NULL,
        //`ISUSE` tinyint(1) DEFAULT NULL,
        public Int64 ID { get; set; }
        /// <summary>
        /// 手机卡ID
        /// </summary>
        public Int64 SIMID { get; set; }
        /// <summary>
        /// 上网设备ID
        /// </summary>
        public Int64 NCID { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool ISUSE { get; set; }
    }
}
