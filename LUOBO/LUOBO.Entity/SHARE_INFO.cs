using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 分享信息
    /// </summary>
    public class SHARE_INFO
    {
        //`ID` bigint(20) NOT NULL AUTO_INCREMENT,
        //`ADID` bigint(20) DEFAULT NULL,
        //`OID` bigint(20) DEFAULT NULL,
        //`SSID` bigint(20) DEFAULT NULL,
        //`SESSION` varchar(100) DEFAULT NULL,
        //`PSESSION` varchar(100) DEFAULT NULL,
        //`TITLE` varchar(100) DEFAULT NULL,
        //`PATH` varchar(200) DEFAULT NULL,
        //`VISITCOUNT` bigint(20) DEFAULT NULL,
        //`CREATETIME` datetime DEFAULT NULL,
        //`UPDATETIME` datetime DEFAULT NULL,
        /// <summary>
        /// 编号
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 广告ID
        /// </summary>
        public Int64 ADID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// SSID
        /// </summary>
        public Int64 SSID { get; set; }
        /// <summary>
        /// 转发人
        /// </summary>
        public string SESSION { get; set; }
        /// <summary>
        /// 分享人
        /// </summary>
        public string PSESSION { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string TITLE { get; set; }
        /// <summary>
        /// 分享路径
        /// </summary>
        public string PATH { get; set; }
        /// <summary>
        /// 分享次数
        /// </summary>
        public Int64 VISITCOUNT { get; set; }
        /// <summary>
        /// 分享类型
        /// QQ空间=qzone,新浪微博=tsina,微信=weixin
        /// </summary>
        public string SHARETYPE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATETIME { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UPDATETIME { get; set; }
    }
}
