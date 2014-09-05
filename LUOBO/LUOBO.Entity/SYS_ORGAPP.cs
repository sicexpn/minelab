using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 机构应用关系
    /// </summary>
    public class SYS_ORGAPP
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	ID	bigint			TRUE	FALSE	TRUE
        //应用ID	APPID	bigint			FALSE	TRUE	FALSE
        //机构ID	ORGID	bigint			FALSE	TRUE	FALSE
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 ORGID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Int64 APPID { get; set; }
    }
}
