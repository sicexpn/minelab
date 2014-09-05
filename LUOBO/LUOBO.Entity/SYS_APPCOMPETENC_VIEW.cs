using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_APPCOMPETENC_VIEW
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Int64 APPID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// Controller名称
        /// </summary>
        public string CONTROLLER { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        public string ACTION { get; set; }
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
