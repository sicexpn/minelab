using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_PLAN
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`Operators` int(11) DEFAULT NULL,
        //`PlanType` int(11) DEFAULT NULL COMMENT '1 固定套餐\r\n            \r\n            2 可选套餐',
        //`Name` varchar(100) DEFAULT NULL,
        //`Content` varchar(10000) DEFAULT NULL,
        //`Rule` varchar(10000) DEFAULT NULL,
        //`DataOfPlan` bigint(20) DEFAULT NULL,
        //`PlanPrice` decimal(10,2) DEFAULT NULL,
        //`STATE` tinyint(1) DEFAULT NULL COMMENT '状态\r\n(0:作废\r\n1:正常)',
        //`CREATETIME` datetime DEFAULT NULL COMMENT '创建时间',
        //`UPDATETIME` datetime DEFAULT NULL COMMENT '更新时间',
        //`InvalidDate` datetime DEFAULT NULL COMMENT '作废时间',
        //`Aging` int(11) DEFAULT NULL,
        //`IsMonthPay` tinyint(1) DEFAULT NULL,
        public Int64 ID { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public Int32 Operators { get; set; }
        /// <summary>
        /// 套餐类型
        /// </summary>
        public Int32 PlanType { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 套餐内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 套餐规则
        /// </summary>
        public string Rule { get; set; }
        /// <summary>
        /// 套餐流量
        /// </summary>
        public Int64 DataOfPlan { get; set; }
        /// <summary>
        /// 套餐价格
        /// </summary>
        public decimal PlanPrice { get; set; }
        /// <summary>
        /// 状态
        /// 0:作废
        /// 1:正常
        /// </summary>
        public bool STATE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATETIME { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UPDATETIME { get; set; }
        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime InvalidDate { get; set; }
        /// <summary>
        /// 套餐时效
        /// </summary>
        public Int32 Aging { get; set; }
        /// <summary>
        /// 是否月结
        /// </summary>
        public bool IsMonthPay { get; set; }
    }
}
