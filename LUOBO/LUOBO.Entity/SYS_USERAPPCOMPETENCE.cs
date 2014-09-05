using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 用户应用表
    /// </summary>
    public class SYS_USERAPPCOMPETENCE
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	ID	bigint			TRUE	FALSE	TRUE
        //用户ID	USERID	bigint			FALSE	TRUE	TRUE
        //应用ID	APPID	bigint			FALSE	TRUE	TRUE
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Int64 UID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Int64 APPCID { get; set; }
    }
}
