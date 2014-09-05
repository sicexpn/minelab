using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_OrgApTime
    {
        public Int64 OID { get; set; }
        public String MAC { get; set; }
        public DateTime? ApPowerTime { get; set; }
        public DateTime? ApStartTime { get; set; }
        public Int64 ONLINEPEOPLENUM { get; set; }
    }
}
