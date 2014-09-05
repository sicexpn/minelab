using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_LOG_ALERT
    {
        public Int64 LOG_ID { get; set; }
        public Int64 OID { get; set; }
        public string AP_MAC { get; set; }
        public string G_SSID { get; set; }
        public string G_MAC { get; set; }
        public DateTime G_TIME { get; set; }
        public string G_TIME_S
        {
            get
            {
                return G_TIME.ToString("yyyy-MM-dd hh:mm");
            }
        }
        public Decimal G_STRONG { get; set; }
        public Int32 CHANNEL { get; set; }
        /// <summary>
        /// 是否已经处理
        /// </summary>
        public bool ISPROCESS { get; set; }

        /// <summary>
        /// 是否白名单
        /// </summary>
        public bool ISWHITELIST { get; set; }
        /// <summary>
        /// 报警包含的关键字，如果为空就说明不包含报警关键字
        /// </summary>
        public string KEYWORD { get; set; }
        /// <summary>
        /// 相似度
        /// </summary>
        public decimal Similarity { get; set; }
        /// <summary>
        /// 第一次出现时间 
        /// </summary>
        public DateTime FIRSTTIME { get; set; }
        
        /// <summary>
        /// 扫描出现次数
        /// </summary>
        public Int64 SCANCOUNT { get; set; }
    }
}
