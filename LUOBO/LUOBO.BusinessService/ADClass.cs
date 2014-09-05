using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.BusinessService
{
    public class ADAudit
    {
        public Int64 ad_id{get;set;}
        public int pub_type { get; set; }
        public String ids { get; set; }
        public int ascase { get; set; }
        public int isCopyName { get; set; }
    }

    public class ADKeyPage
    {
        public int adaudit { get; set; }
        public String keystr { get; set; }
        public int pagenum { get; set; }
        public int pagesize { get; set; }
    }
}