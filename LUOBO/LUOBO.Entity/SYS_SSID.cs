using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SSID
    {
//ID	bigint	20	0	0	0	0	0	0		0					-1	0
//NAME	varchar	100	0	-1	0	0	0	0		0		utf8	utf8_general_ci		0	0
//ISON	tinyint	1	0	-1	0	0	0	0		0					0	0
//ISINTERNET	tinyint	1	0	-1	0	0	0	0		0					0	0
//MAXLINKCOUNT	int	11	0	-1	0	0	0	0		0					0	0
//PORTAL	varchar	4096	0	-1	0	0	0	0		0		utf8	utf8_general_ci		0	0
//MAXUS	bigint	20	0	-1	0	0	0	0		0					0	0
//MAXDS	bigint	20	0	-1	0	0	0	0		0					0	0
//VONLINETIME	bigint	20	0	-1	0	0	0	0		0					0	0
//VMAXDS	bigint	20	0	-1	0	0	0	0		0					0	0
//VMAXUS	bigint	20	0	-1	0	0	0	0		0					0	0
//ISUPDATE	tinyint	1	0	-1	0	0	0	0		0					0	0
//ISPWD	tinyint	1	0	-1	0	0	0	0		0					0	0
//PWD	varchar	256	0	-1	0	0	0	0		0		utf8	utf8_general_ci		0	0
//OID	bigint	20	0	-1	0	0	0	0		0					0	0
//APID	bigint	20	0	-1	0	0	0	0		0					0	0
//ACID	bigint	20	0	-1	0	0	0	0		0					0	0


        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool ISON { get; set; }
        /// <summary>
        /// 外网开关
        /// </summary>
        public bool ISINTERNET { get; set; }
        /// <summary>
        /// 最大连接数
        /// </summary>
        public Int64 MAXLINKCOUNT { get; set; }
        /// <summary>
        /// 入口地址
        /// </summary>
        public string PORTAL { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string PATH { get; set; }
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
        /// 访客最大上行速率
        /// </summary>
        public Int64 VMAXUS { get; set; }
        /// <summary>
        /// 访客最大下行速率
        /// </summary>
        public Int64 VMAXDS { get; set; }
        /// <summary>
        /// 是否更新
        /// </summary>
        public bool ISUPDATE { get; set; }
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool ISPWD { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PWD { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// AP设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// 广告方案ID
        /// </summary>
        public Int64 ACID { get; set; }
        /// <summary>
        /// 广告ID
        /// </summary>
        public Int64 ADID { get; set; }
    }
}
