using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_DICT
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	ID	bigint			TRUE	FALSE	TRUE
        //类别	CATEGORY	varchar(100)	100		FALSE	FALSE	FALSE
        //名称	NAME	varchar(256)	256		FALSE	FALSE	FALSE
        //值	VALUE	int			FALSE	FALSE	FALSE
        /// <summary>
        /// 编号
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string CATEGORY { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public Int32 VALUE { get; set; }
    }
}
