using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class APCONFIGTEMPLATE
    {
//        Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
//ID	ID	bigint			TRUE	FALSE	TRUE
//固件名称	FIRMWARE	varchar(100)	100		FALSE	FALSE	FALSE
//固件版本号	VERSION	varchar(100)	100		FALSE	FALSE	FALSE
//AP配置描述	CONTENT	text			FALSE	FALSE	FALSE

        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 模版名称
        /// </summary>
        public string TNAME { get; set; }
        /// <summary>
        /// 模版描述
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 固件名称
        /// </summary>
        public string FIRMWARE { get; set; }
        /// <summary>
        /// 固件版本号
        /// </summary>
        public string VERSION { get; set; }
        /// <summary>
        /// AP配置模版
        /// </summary>
        public string CONTENT { get; set; }
        /// <summary>
        /// AP配置模版
        /// </summary>
        public DateTime UPDATETIME { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool ISDELETE { get; set; }

    }
}
