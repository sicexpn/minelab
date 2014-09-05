using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_USER_VIEW
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string USERNAME { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PWD { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATETIME { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string ONAME { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string CONTACT { get; set; }
        /// <summary>
        /// 用户类别
        /// 1 管理员
        /// 2 普通用户
        /// </summary>
        public Int32 USERTYPE { get; set; }
        /// <summary>
        /// 用户类别文字
        /// </summary>
        public string DNAME { get; set; }
        /// <summary>
        /// 用户状态
        /// 0 停用
        /// 1 正常
        /// </summary>
        public bool STATE { get; set; }
        /// <summary>
        /// 用户手机MAC
        /// </summary>
        public string MAC { get; set; }
    }
}
