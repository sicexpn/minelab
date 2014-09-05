using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SIM
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`Operator` bigint(20) DEFAULT NULL COMMENT '运营商',
        //`Phone` varchar(20) DEFAULT NULL COMMENT '手机号',
        //`NetType` varchar(50) DEFAULT NULL COMMENT '网络类型\r\n(字典：2g 3g 4g)',
        //`Balances` decimal(10,2) DEFAULT NULL COMMENT '余额',
        //`ActualBalances` decimal(10,2) DEFAULT NULL,
        //`DataOfTotal` bigint(20) DEFAULT NULL COMMENT '总流量',
        //`DataOfRemaining` bigint(20) DEFAULT NULL COMMENT '剩余流量',
        //`DataOfDailyUsed` bigint(20) DEFAULT NULL COMMENT '日用流量',
        //`DataOfDailyLimit` bigint(20) DEFAULT NULL COMMENT '日限流量',
        //`DataOfUsed` bigint(20) DEFAULT NULL COMMENT '已使用流量',
        //`ActiveDate` datetime DEFAULT NULL COMMENT '激活时间',
        //`ExpirationDate` datetime DEFAULT NULL COMMENT '到期时间',
        //`PUK` varchar(50) DEFAULT NULL COMMENT 'PUK码',
        //`PIN` varchar(50) DEFAULT NULL COMMENT 'PIN码',
        //`SIMCapacity` bigint(20) DEFAULT NULL COMMENT 'SIM卡容量',
        //`Possessor` varchar(100) DEFAULT NULL COMMENT '实名认证人',
        //`IDCard` varchar(20) DEFAULT NULL COMMENT '身份证号',
        //`SIMNum` varchar(100) DEFAULT NULL COMMENT 'SIM卡号',
        //`STATE` int(11) DEFAULT NULL COMMENT '状态\r\n(-1:作废\r\n0:未激活\r\n1:正常\r\n2:欠费)',
        //`CREATETIME` datetime DEFAULT NULL COMMENT '创建时间',
        //`UPDATETIME` datetime DEFAULT NULL COMMENT '更新时间',
        //`InvalidDate` datetime DEFAULT NULL COMMENT '作废时间',
        //`Remark` varchar(1000) DEFAULT NULL COMMENT '备注',
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 网络类型.字典：1:2g 2:3g 3:4g
        /// </summary>
        public string NetType { get; set; }
        /// <summary>
        /// 余额 
        /// </summary>
        public decimal Balances { get; set; }
        /// <summary>
        /// 总流量 单位B
        /// </summary>
        public Int64 DataOfTotal { get; set; }
        /// <summary>
        /// 剩余流量 单位B
        /// </summary>
        public Int64 DataOfRemaining { get; set; }
        /// <summary>
        /// 已使用流量 单位B
        /// </summary>
        public Int64 DataOfDailyUsed { get; set; }
        /// <summary>
        /// 日限制流量 单位B
        /// </summary>
        public Int64 DataOfDailyLimit { get; set; }
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ActiveDate { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// PUK
        /// </summary>
        public string PUK { get; set; }
        /// <summary>
        /// PIN
        /// </summary>
        public string PIN { get; set; }
        /// <summary>
        /// SIM卡容量 单位KB
        /// </summary>
        public Int64 SIMCapacity { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 实名认证人
        /// </summary>
        public string Possessor { get; set; }
        /// <summary>
        /// SIM卡串号
        /// </summary>
        public string SIMum { get; set; }
        /// <summary>
        /// 状态
        /// -1:作废
        /// 0:未激活
        /// 1:正常
        /// 2:欠费
        /// </summary>
        public Int32 STATE { get; set; }
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
