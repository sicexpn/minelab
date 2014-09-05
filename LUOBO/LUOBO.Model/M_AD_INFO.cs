using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.Model
{
    public class M_AD_INFO
    {
        public List<AD_INFO> APCTList { get; set; }
        public Int32 AllCount { get; set; }
    }

    public class M_AD_PUB
    {
        public AD_INFO ADInfo { get; set; }
        public Int32 PubCount { get; set; }
    }

    public class M_AD_PUBINFO
    {
        public List<M_AD_PUB> ADList { get; set; }
        public Int32 AllCount { get; set; }
    }
}
