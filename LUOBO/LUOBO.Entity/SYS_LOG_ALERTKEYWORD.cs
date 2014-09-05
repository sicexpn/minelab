using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_LOG_ALERTKEYWORD
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get;set;}
        /// <summary>
        /// 关键词
        /// </summary>
        public string KEYWORD { get; set; }
    }
}
