using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_SECURITY_SSID
    {
        public Int64 APID { get; set; }
        public String CapturerName { get; set; }
        public String G_SSID { get; set; }
        public String G_MAC { get; set; }
        public Decimal G_STRONG { get; set; }
        public Int32 CHANNEL { get; set; }
        public DateTime G_TIME { get; set; }
        public String KEYWORD { get; set; }
        public Decimal Similarity { get; set; }
        public Int64 GroupCount { get; set; }
        /// <summary>
        /// 1，可疑
        /// 2，可信
        /// 3，中文
        /// 4，今日新增
        /// 0,其他
        /// </summary>
        public String LevelFlag { get; set; }
        public DateTime FirstTime { get; set; }
    }
}
