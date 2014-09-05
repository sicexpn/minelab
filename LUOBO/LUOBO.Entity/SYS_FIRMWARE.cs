using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_FIRMWARE
    {
        /*
           ID` bigint(20) NOT NULL,
          `FIREWARENAME` varchar(100) DEFAULT NULL COMMENT '与AP表的固件名称关联',
          `MODELTYLE` varchar(20) DEFAULT NULL,
          `VERNO` varchar(50) DEFAULT NULL,
         */
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 固件名称
        /// </summary>
        public string FIREWARENAME { get; set; }
        /// <summary>
        /// 适用机型
        /// </summary>
        public string MODELTYLE { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string VERNO
        {
            get;
            set;
        }
    }
}
