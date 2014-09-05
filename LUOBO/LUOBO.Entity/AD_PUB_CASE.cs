using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class AD_PUB_CASE
    {
        public Int64 AC_ID { get; set; }

        public Int64 ORG_ID { get; set; }

        public String AC_TITLE { get; set; }

        public Int64 AD_ID { get; set; }

        public String AD_TITLE { get; set; }

        public String AD_SSID { get; set; }

        public String AD_PATH { get; set; }

        public String AD_PORTAL { get; set; }

        public Int32 IS_COPYSSID { get; set; }
    }
}
