using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_SSID
    {
        //public Entity.SYS_SSID ssid { get; set; }
        //public List<Entity.SYS_BANLIST> banlist { get; set; }


        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 新名称
        /// </summary>
        public string NEWNAME { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool ISON { get; set; }
        public string PATH { get; set; }
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
        /// 最大上行速率
        /// </summary>
        public Int64 MAXUS { get; set; }
        /// <summary>
        /// 最大下行速率
        /// </summary>
        public Int64 MAXDS { get; set; }
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



        public string macs { get; set; }
        public string urls { get; set; }
        public string ports { get; set; }

        public bool ISAUDIT { get; set; }
    }
}
