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
    public class SYS_PROBEORG
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 机构描述
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 父机构ID
        /// </summary>
        public Int64 PID { get; set; }
        public string PIDHELP { get; set; }
        /// <summary>
        /// 机构类别
        /// </summary>
        public string CATEGORY { get; set; }
        public string CATEGORYNAME { get; set; }
        /// <summary>
        /// 是否审核权
        /// </summary>
        public bool ISVERIFY { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string PROVINCE { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CITY { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string COUNTIES { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string CONTACT { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string CONTACTER { get; set; }
        /// <summary>
        /// 机构状态
        ///  0 停用
        ///  1 正常
        /// </summary>
        public bool STATE { get; set; }
        /// <summary>
        /// 最终审核权（只有顶级机构有）
        /// </summary>
        public bool ISVERIFY_END { get; set; }
    }
}
