using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Mvc;
using LUOBO.Helper;

namespace LUOBO.Entity
{
    /*
     * Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
ID	ID	bigint			TRUE	FALSE	TRUE
设备ID	APID	bigint			FALSE	TRUE	FALSE
父机构ID	POID	bigint			FALSE	FALSE	FALSE
机构ID	OID	bigint			FALSE	TRUE	FALSE
起始有效时间	SDATE	datetime			FALSE	FALSE	FALSE
截至有效时间	EDATE	datetime			FALSE	FALSE	FALSE
是否归属机构	ISCHILD	tinyint			FALSE	FALSE	FALSE
     */
    [DataContract]
    [ModelBinder(typeof(JsonModelBinder))]
    public class SYS_APORG
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public Int64 ID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        [DataMember]
        public Int64 APID { get; set; }

        /// <summary>
        /// 上级机构ID
        /// </summary>
        [DataMember]
        public Int64 POID { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [DataMember]
        public Int64 OID { get; set; }

        /// <summary>
        /// ssid数量
        /// </summary>
        [DataMember]
        public int SSIDNUM { get; set; }

        /// <summary>
        /// 起始有效时间
        /// </summary>
        [DataMember]
        public DateTime SDATE { get; set; }

        /// <summary>
        /// 截至有效时间
        /// </summary>
        [DataMember]
        public DateTime EDATE { get; set; }

        /// <summary>
        /// 是否归属当前机构
        /// </summary>
        [DataMember]
        public bool ISCHILD { get; set; }
    }
}
