using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Mvc;
using LUOBO.Helper;

namespace LUOBO.Entity
{
    [DataContract]
    [ModelBinder(typeof(JsonModelBinder))]
    public class SYS_AP_VIEW
    {
        [DataMember]
        public Int64 ID { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        [DataMember]
        public string MAC { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ALIAS { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        [DataMember]
        public string MODEL { get; set; }
        /// <summary>
        /// 生产商
        /// </summary>
        [DataMember]
        public string MANUFACTURER { get; set; }
        /// <summary>
        /// 购买人
        /// </summary>
        [DataMember]
        public string PURCHASER { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        [DataMember]
        public Int64 SERIAL { get; set; }
        /// <summary>
        /// 简要说明
        /// </summary>
        [DataMember]
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 最大的ssid数量
        /// </summary>
        [DataMember]
        public int MAXSSIDNUM { get; set; }
        /// <summary>
        /// 固件版本
        /// </summary>
        [DataMember]
        public string FIRMWAREVERSION { get; set; }
        /// <summary>
        /// 设备状态
        /// 1正常
        /// 5报废
        /// </summary>
        [DataMember]
        public int DEVICESTATE { get; set; }
        /// <summary>
        /// 支持3G
        /// </summary>
        [DataMember]
        public bool SUPPORT3G { get; set; }
        /// <summary>
        /// 心跳间隔
        /// </summary>
        [DataMember]
        public Int64 HBINTERVAL { get; set; }
        /// <summary>
        /// 数据间隔
        /// </summary>
        [DataMember]
        public Int64 DATAINTERVAL { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [DataMember]
        public DateTime REGDATE { get; set; }
        /// <summary>
        /// 配置模版ID
        /// </summary>
        [DataMember]
        public Int64 APCTID { get; set; }
        /// <summary>
        /// 是否更新
        /// </summary>
        [DataMember]
        public bool ISUPDATE { get; set; }
        /// <summary>
        /// 是否重启
        /// </summary>
        [DataMember]
        public bool? ISREBOOT { get; set; }
        /// <summary>
        /// APORGID
        /// </summary>
        [DataMember]
        public Int64 APORGID { get; set; }
        /// <summary>
        /// 上级机构ID
        /// </summary>
        [DataMember]
        public Int64 POID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        [DataMember]
        public Int64 OID { get; set; }
        /// <summary>
        /// ssid数量
        /// </summary>
        [DataMember]
        public int SSIDNUM { get; set; }
        /// <summary>
        /// 起始有效时间
        /// </summary>
        [DataMember]
        public DateTime SDATE { get; set; }
        public string SDATEStr { get { return SDATE.ToShortDateString(); } }
        /// <summary>
        /// 截至有效时间
        /// </summary>
        [DataMember]
        public DateTime EDATE { get; set; }
        public string EDATEStr { get { return EDATE.ToShortDateString(); } }
        /// <summary>
        /// 是否归属当前机构
        /// </summary>
        [DataMember]
        public bool ISCHILD { get; set; }
        /// <summary>
        /// 当前Ap的状态：1代理商（分配、未分配、过期）; 2 多店:分配,未分配,过期
        /// </summary>
        public string STATE { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string NAME { get; set; }
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

    [DataContract]
    [ModelBinder(typeof(JsonModelBinder))]
    public class SYS_AP_VIEW_ARR
    {
        [DataMember]
        public SYS_AP_VIEW[] Items { get; set; }
    }
}
