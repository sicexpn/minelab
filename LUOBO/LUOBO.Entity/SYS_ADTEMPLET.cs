using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_ADTEMPLET
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 SADT_ID { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string SADT_NAME { get; set; }

        /// <summary>
        /// 模版说明
        /// </summary>
        public string SADT_CONTENT { get; set; }

        /// <summary>
        /// 模版入口页
        /// </summary>
        public string SADT_PORTALFILE { get; set; }

        /// <summary>
        /// 模版文件列表
        /// </summary>
        public string SADT_FILELIST { get; set; }

        /// <summary>
        /// 模版状态
        /// </summary>
        public Int64 SADT_STATU { get; set; }

        /// <summary>
        /// 模版适用范围
        /// </summary>
        public string SADT_SCOPE { get; set; }

    }
}
