using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class AD_INFO_FREEHOST
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 AD_ID { get; set; }

        public Int64 ORG_ID { get; set; }

        public string AD_Title { get; set; }

        public string AD_SSID { get; set; }

        public string AD_HomePage { get; set; }

        public Int32 AD_Type { get; set; }

        public Int32 AD_Model { get; set; }

        public DateTime AD_Time { get; set; }

        public Int32 AD_Stat { get; set; }

        public Int32 AD_Release_Count { get; set; }

        public String AD_PUBPATH { get; set; }

        public Int64 AF_ID { get; set; }
        public string F_Host { get; set; }
        public string F_Domain { get; set; }
        public string F_Default { get; set; }
    }
}
