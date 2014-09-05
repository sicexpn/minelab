using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_AP_GIS
    {
        public Int64 ID { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string ALIAS { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public Int64 SERIAL { get; set; }
        /// <summary>
        /// 心跳间隔
        /// </summary>
        public Int64 HBINTERVAL { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double LAT { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double LON { get; set; }
        /// <summary>
        /// 最后心跳时间
        /// </summary>
        public DateTime LASTHB { get; set; }
        /// <summary>
        /// 是否存活
        /// </summary>
        public bool ISLIVE { get; set; }
        /// <summary>
        /// 设备所在地址
        /// </summary>
        public string ADDRESS { get; set; }
        /// <summary>
        /// 在线人数
        /// </summary>
        public Int64 ONLINEPEOPLENUM { get; set; }
        /// <summary>
        /// CPU占用率%
        /// </summary>
        public String CPU { get; set; }
        /// <summary>
        /// 空闲内存
        /// </summary>
        public String MEMFREE { get; set; }
        /// <summary>
        /// 开机时常秒
        /// </summary>
        public Int64 POWERTIME { get; set; }
        /// <summary>
        /// 待机时常秒
        /// </summary>
        public Int64 FREETIME { get; set; }
        /// <summary>
        /// 开机后总流量
        /// </summary>
        public String NETWORKTOTAL { get; set; }
        /// <summary>
        /// 网络速率
        /// </summary>
        public String NETWORKRATE { get; set; }
        /// <summary>
        /// 历史时常
        /// </summary>
        public Int64 HISTORYTIME { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime POWERDATETIME { get; set; }

    }
}
