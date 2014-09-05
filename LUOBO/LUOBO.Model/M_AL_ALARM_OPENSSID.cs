using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_AL_ALARM_OPENSSID
    {
        public Int64 SSID { get; set; }
        public Int64 OID { get; set; }
        public string SSIDNAME { get; set; }
        public Int64 APID { get; set; }
        public string ALIAS { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
