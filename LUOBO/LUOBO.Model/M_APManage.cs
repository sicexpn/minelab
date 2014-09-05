using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    /// <summary>
    /// 界面所需对象
    /// </summary>
    public class M_APManage
    {
        public bool Check
        {
            get;
            set;
        }
        public Int64 ID
        {
            get;
            set;
        }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC
        {
            get;
            set;
        }
        /// <summary>
        /// 型号
        /// </summary>
        public string MODEL
        {
            get;
            set;
        }
        /// <summary>
        /// 生产商
        /// </summary>
        public string MANUFACTURER
        {
            get;
            set;
        }
        /// <summary>
        /// 购买人
        /// </summary>
        public string PURCHASER
        {
            get;
            set;
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string SERIAL
        {
            get;
            set;
        }
        /// <summary>
        /// 简要说明
        /// </summary>
        public string DESCRIPTION
        {
            get;
            set;
        }
        /// <summary>
        /// 最大的ssid数量
        /// </summary>
        public int MAXSSIDNUM
        {
            get;
            set;
        }
        /// <summary>
        /// 固件版本
        /// </summary>
        public string FIRMWAREVERSION
        {
            get;
            set;
        }
        /// <summary>
        /// 设备状态
        /// 1正常
        /// 5报废
        /// </summary>
        public int DEVICESTATE
        {
            get;
            set;
        }
        /// <summary>
        /// 支持3G
        /// </summary>
        public bool SUPPORT3G
        {
            get;
            set;
        }
    }
}
