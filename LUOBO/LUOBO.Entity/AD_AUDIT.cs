using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class AD_AUDIT
    {
        public Int64 AUD_ID { get; set; }
        public Int64 ORG_ID { get; set; }
        public string ORG_NAME_V { get; set; }
        public Int64 AD_ID { get; set; }
        public string AD_NAME_V { get; set; }
        public string SSID_NAME { get; set; }
        public string AD_SSID_V { get; set; }
        public Int32 PUB_TYPE { get; set; }
        public String PUB_LIST { get; set; }
        public Int32 ISCOPYNAME { get; set; }
        public Int64 FROM_ORG_ID { get; set; }
        public string FROM_ORG_NAME_V { get; set; }
        public String FROM_USER { get; set; }
        public DateTime FROM_DATE { get; set; }
        public string FROM_DATE_S
        {
            get
            {
                return FROM_DATE.ToString("yyyy-MM-dd hh:mm");
            }
        }
        public Int32 FROM_TYPE { get; set; }
        public Int64 TO_ORG_ID { get; set; }
        public String AUD_CONTENT { get; set; }
        public Int32 AUD_STAT { get; set; }
        public Int64 AUD_PARENTID { get; set; }
    }
}
