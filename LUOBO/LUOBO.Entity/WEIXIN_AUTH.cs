using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class WEIXIN_AUTH
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string OPENID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PWD { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool ISUSE { get; set; }
        /// <summary>
        /// 密码生成时间
        /// </summary>
        public DateTime CREATETIME { get; set; }

    }
}
