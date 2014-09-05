using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 应用
    /// 应用管理表，用于存储所有现有应用
    /// </summary>
    public class SYS_APPLICATION
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	ID	bigint			TRUE	FALSE	TRUE
        //应用名称	APPLICATIONNAME	varchar(100)	100		FALSE	FALSE	FALSE
        //系统名称	SYSTEMNAME	varchar(100)	100		FALSE	FALSE	FALSE
        //程序集名称	ASSEMBLYNAME	varchar(100)	100		FALSE	FALSE	FALSE
        //类名称	CLASSNAME	varchar(100)	100		FALSE	FALSE	FALSE
        //是否公共	ISPUBLIC	bit			FALSE	FALSE	FALSE
        //是否默认	ISDEFAULT	bit			FALSE	FALSE	FALSE
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string APPLICATIONNAME { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SYSTEMNAME { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string ASSEMBLYNAME { get; set; }
        /// <summary>
        /// 类名称
        /// </summary>
        public string CLASSNAME { get; set; }
        /// <summary>
        /// 是否公共
        /// </summary>
        public bool ISPUBLIC { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool ISDEFAULT { get; set; }
    }
}
