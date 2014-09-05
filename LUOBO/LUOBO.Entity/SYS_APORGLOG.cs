using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /*
     * Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
ID	ID	bigint			TRUE	FALSE	TRUE
设备ID	APID	bigint			FALSE	FALSE	FALSE
从机构	FOID	bigint			FALSE	FALSE	FALSE
到机构	TOID	bigint			FALSE	FALSE	FALSE
起始有效时间	SDATE	datetime			FALSE	FALSE	FALSE
截至有效时间	EDATE	datetime			FALSE	FALSE	FALSE
创建时间	CREATETIME	datetime			FALSE	FALSE	FALSE
操作名称	OPNAME	varchar(100)	100		FALSE	FALSE	FALSE
     */
    public class SYS_APORGLOG
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 APID { get; set; }

        /// <summary>
        /// 从机构
        /// </summary>
        public Int64 FOID { get; set; }

        /// <summary>
        /// 到机构
        /// </summary>
        public Int64 TOID { get; set; }

        /// <summary>
        /// ssid数量
        /// </summary>
        public int SSIDNUM { get; set; }

        /// <summary>
        /// 起始有效时间
        /// </summary>
        public DateTime SDATE { get; set; }

        /// <summary>
        /// 截至有效时间
        /// </summary>
        public DateTime EDATE { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATETIME { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string OPNAME { get; set; }
    }
}
