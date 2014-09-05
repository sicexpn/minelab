using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 用户表
    /// 用户表，存放所有用户，跟具体角色相关
    /// </summary>
    public class SYS_USER
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	ID	bigint			TRUE	FALSE	TRUE
        //用户名称	USERNAME	varchar(100)	100		FALSE	FALSE	FALSE
        //帐号	ACCOUNT	varchar(32)	32		FALSE	FALSE	FALSE
        //密码	PWD	varchar(64)	64		FALSE	FALSE	FALSE
        //创建时间	CREATETIME	datetime			FALSE	FALSE	FALSE
        //机构ID	OID	bigint			FALSE	FALSE	FALSE
        //联系方式	CONTACT	varchat(16)	16		FALSE	FALSE	FALSE
        //用户类别	USERTYPE	int			FALSE	FALSE	FALSE
        //用户状态	STATE	bit			FALSE	FALSE	FALSE
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
        /// 用户状态
        /// 0 停用
        /// 1 正常
        /// </summary>
        public bool STATE { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string TOKEN { get; set; }
        /// <summary>
        /// 令牌时长
        /// </summary>
        public DateTime TOKENTIMESTAMP { get; set; }
        /// <summary>
        /// 用户手机MAC
        /// </summary>
        public string MAC { get; set; }
    }
}
