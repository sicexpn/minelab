using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_WCF_SSID_VIEW
    {
        /// <summary>
        /// SSID编号
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// 广告编号
        /// </summary>
        public Int64 ADID { get; set; }
        /// <summary>
        /// APID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// AP名称
        /// </summary>
        public string ALIAS { get; set; }
        /// <summary>
        /// SSID名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// SSID新名称(审核中时)
        /// </summary>
        public string NEWNAME { get; set; }
        /// <summary>
        /// 入口页
        /// </summary>
        public string PORTAL { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string PATH { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string DOWNPATH { get; set; }
        /// <summary>
        /// 最大连接数
        /// </summary>
        public Int64 MAXLINKCOUNT { get; set; }
        /// <summary>
        /// 最大上行速率
        /// </summary>
        public Int64 MAXUS { get; set; }
        /// <summary>
        /// 最大下行速率
        /// </summary>
        public Int64 MAXDS { get; set; }
        /// <summary>
        /// 最大流量
        /// </summary>
        public Int64 MAXFLOW { get; set; }
        /// <summary>
        /// 访客上网时长
        /// </summary>
        public Int64 VONLINETIME { get; set; }
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool ISPWD { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PWD { get; set; }
        /// <summary>
        /// 审核状态(枚举)
        /// </summary>
        public Int32 STATE { get; set; }
    }
}