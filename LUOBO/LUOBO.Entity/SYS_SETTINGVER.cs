using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_SETTINGVER
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        private string _GUID = "";
        public string GUID {
            get { return _GUID;}
            set { _GUID = value;}
        }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime DATETIME { get; set; }
        /// <summary>
        /// 版本类型 0配置文件 1广告
        /// </summary>
        public int TYPE { get; set; }
    }
}
