using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_NETWORKCARD
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`Manufacturer` varchar(100) NOT NULL COMMENT '品牌',
        //`Model` varchar(100) DEFAULT NULL COMMENT '型号',
        //`IMEI` varchar(100) DEFAULT NULL COMMENT 'IMEI',
        //`SupportNetwork` varchar(100) DEFAULT NULL COMMENT '支持网络制式',
        //`SupportTFCard` tinyint(1) DEFAULT NULL COMMENT '支持扩展卡',
        //`TFCardCapacity` bigint(20) DEFAULT NULL COMMENT '扩展卡容量(单位:MB)',
        //`STATE` tinyint(1) DEFAULT NULL COMMENT '状态\r\n(0:作废\r\n1:正常)',
        //`CREATETIME` datetime DEFAULT NULL COMMENT '创建时间',
        //`UPDATETIME` datetime DEFAULT NULL COMMENT '更新时间',
        //`InvalidDate` datetime DEFAULT NULL COMMENT '作废时间',
        //`Remark` varchar(1000) DEFAULT NULL COMMENT '备注',
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 生产商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// IMEI
        /// </summary>
        public string IMEI { get; set; }
        /// <summary>
        /// 支持的网络制式
        /// </summary>
        public string SupportNetwork { get; set; }
        /// <summary>
        /// 支持扩展卡
        /// </summary>
        public bool SupportTFCard { get; set; }
        /// <summary>
        /// 扩展卡容量(单位:MB)
        /// </summary>
        public Int64 TFCardCapactity { get; set; }
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
