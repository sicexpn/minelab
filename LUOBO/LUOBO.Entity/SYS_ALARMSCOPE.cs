using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_ALARMSCOPE
    {
        public Int64 ID { get; set; }
        public Int64 ALID { get; set; }
        public Int64 APID { get; set; }
        public Int64 SSID { get; set; }
        public Int64 VCOUNT { get; set; }
        public DateTime CURRENTTIME { get; set; }
    }
}
