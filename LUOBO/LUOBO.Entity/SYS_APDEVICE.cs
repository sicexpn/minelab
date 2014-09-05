using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// AP设备的基本信息
    /// </summary>
    public class SYS_APDEVICE
    {
        public Int64 ID { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string MODEL { get; set; }
        /// <summary>
        /// 生产商
        /// </summary>
        public string MANUFACTURER { get; set; }
        /// <summary>
        /// 购买人
        /// </summary>
        public string PURCHASER { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string ALIAS { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public Int64 SERIAL { get; set; }
        /// <summary>
        /// 简要说明
        /// </summary>
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 最大的ssid数量
        /// </summary>
        public int MAXSSIDNUM { get; set; }
        /// <summary>
        /// 固件版本
        /// </summary>
        public string FIRMWAREVERSION { get; set; }
        /// <summary>
        /// 设备状态
        /// 1正常
        /// 2演示机
        /// 5报废
        /// </summary>
        public int DEVICESTATE { get; set; }
        /// <summary>
        /// 支持3G
        /// </summary>
        public bool SUPPORT3G { get; set; }
        /// <summary>
        /// 心跳间隔
        /// </summary>
        public Int64 HBINTERVAL { get; set; }
        /// <summary>
        /// 数据间隔
        /// </summary>
        public Int64 DATAINTERVAL { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime REGDATE { get; set; }
        /// <summary>
        /// 配置模版ID
        /// </summary>
        public Int64 APCTID { get; set; }
        /// <summary>
        /// 是否更新
        /// </summary>
        public bool ISUPDATE { get; set; }
        /// <summary>
        /// 是否重启
        /// </summary>
        public bool ISREBOOT { get; set; }
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
        /// 设备所在地址
        /// </summary>
        public string ADDRESS { get; set; }
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
        /// <summary>
        /// 在线人数
        /// </summary>
        public Int64 ONLINEPEOPLENUM { get; set; }
        /// <summary>
        /// 信道(1-13)
        /// </summary>
        public Int32 APCHANNEL { get; set; }
        /// <summary>
        /// 功率(1-100 默认为17)
        /// </summary>
        public Int32 POWER { get; set; }
        /// <summary>
        /// 天线类型(0:全向 1:定向)
        /// </summary>
        public Int32 AERIALTYPE { get; set; }
        /// <summary>
        /// 是否开启SSID
        /// </summary>
        public bool ISSSIDON { get; set; }
    }
}
