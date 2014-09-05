using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class StatisticalAP
    {
        public Int64 ID{get;set;}
        public string NAME { get; set; }
        public List<Int64> NUM { get; set; }
    }

    public class StatisticalAP<T>
    {
        public Int64 ID { get; set; }
        public string NAME { get; set; }
        public List<T> VALUE { get; set; }
    }
}
