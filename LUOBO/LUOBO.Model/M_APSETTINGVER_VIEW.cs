using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_APSETTINGVER_VIEW
    {
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public Int64 SERIAL { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public Int32 DEVICESTATE { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        private string _GUID = "";
        public string GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime DATETIME { get; set; }
        /// <summary>
        /// 版本类型 0配置文件 1广告
        /// </summary>
        public int TYPE { get; set; }
        /// <summary>
        /// 信道(1-13)
        /// </summary>
        public Int32 APCHANNEL { get; set; }
        /// <summary>
        /// 功率(1-100)
        /// </summary>
        public Int32 POWER { get; set; }
        /// <summary>
        /// 是否开启SSID
        /// </summary>
        public bool ISSSIDON { get; set; }
    }
}
