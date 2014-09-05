using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SIMPLAN
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`SIMID` bigint(20) DEFAULT NULL,
        //`PlanID` bigint(20) DEFAULT NULL,
        public Int64 ID { get; set; }
        /// <summary>
        /// 手机卡ID
        /// </summary>
        public Int64 SIMID { get; set; }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public Int64 PlanID { get; set; }
    }
}
