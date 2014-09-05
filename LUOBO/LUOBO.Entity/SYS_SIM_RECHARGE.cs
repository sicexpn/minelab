using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SIM_RECHARGE
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`SIMID` bigint(20) DEFAULT NULL,
        //`Amount` decimal(10,2) DEFAULT NULL,
        //`RepaidDate` datetime DEFAULT NULL,
        //`CardNumber` varchar(100) DEFAULT NULL,
        //`CardPwd` varchar(100) DEFAULT NULL,
        //`Operator` varchar(100) DEFAULT NULL,
        //`STATE` tinyint(1) DEFAULT NULL COMMENT '状态\r\n(0:作废\r\n1:正常)',
        //`InvalidDate` datetime DEFAULT NULL COMMENT '作废时间',
        //`Remark` varchar(1000) DEFAULT NULL,
        public Int64 ID { get; set; }
        /// <summary>
        /// SIMID
        /// </summary>
        public Int64 SIMID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 充值时间
        /// </summary>
        public DateTime RepaidDate { get; set; }
        /// <summary>
        /// 充值卡号
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// 充值密码
        /// </summary>
        public string CardPwd { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 状态
        /// 0:作废
        /// 1:正常
        /// </summary>
        public bool STATE { get; set; }
        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime InvalidDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
