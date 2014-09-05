using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_LOG_APNEAR
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
        public DateTime FIRSTTIME { get; set; }
        //public DateTime LASTTIME { get; set; }
        public Int64 SCANCOUNT { get; set; }
    }
}
