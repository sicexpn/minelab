using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    ///机构表
    /// </summary>
    public class SYS_ORGANIZATION
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	    ID	    bigint			TRUE	FALSE	TRUE
        //名称	NAME	varchar(100)	100		FALSE	FALSE	FALSE  
        //机构描述	DESCRIPTION	varchar(100)	100		FALSE	FALSE	FALSE
        //父机构ID	PID	bigint			FALSE	FALSE	FALSE    
        //父机构IDHelp IDHELP text
        //是否审核权	ISVERIFY	bit			FALSE	FALSE	FALSE   
        //城市	CITY	varchar(50)	50		FALSE	FALSE	FALSE   
        //区县	COUNTIES	varchar(50)	50		FALSE	FALSE	FALSE    
        //联系方式	CONTACT	varchar(16)	16		FALSE	FALSE	FALSE    
        //联系人	CONTACTER	varchar(50)	50		FALSE	FALSE	FALSE    
        //机构状态	STATE	bit			FALSE	FALSE	FALSE
        //机构类别  CATEGORY varchar(20) 20 
        //是否有最终审核权 ISVERIFY_END 1 0
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 机构描述
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 父机构ID
        /// </summary>
        public Int64 PID { get; set; }
        public string PIDHELP { get; set; }
        /// <summary>
        /// 机构类别
        /// </summary>
        public string CATEGORY { get; set; }
        public string CATEGORYNAME { get; set; }
        /// <summary>
        /// 是否审核权
        /// </summary>
        public bool ISVERIFY { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string PROVINCE { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CITY { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string COUNTIES { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string CONTACT { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string CONTACTER { get; set; }
        /// <summary>
        /// 机构状态
        ///  0 停用
        ///  1 正常
        /// </summary>
        public bool STATE { get; set; }
        /// <summary>
        /// 最终审核权（只有顶级机构有）
        /// </summary>
        public bool ISVERIFY_END { get; set; }
        /// <summary>
        /// 登陆方式名称
        /// </summary>
        public String PNAME { get; set; }

        /// <summary>
        /// 行业-字典
        /// </summary>
        public Int32 INDUSTRY { get; set; }
        /// <summary>
        /// 面积-字典
        /// </summary>
        public Int32 AREA { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 公众微信
        /// </summary>
        public string WEIXIN { get; set; }
        /// <summary>
        /// 公众微博
        /// </summary>
        public string WEIBO { get; set; }
    }
}
