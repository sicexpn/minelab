using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 组管理的限制
    /// </summary>
    public class RadGroupReply
    {
        public Int64 Id { get; set; }
        public String GroupName { get; set; }
        public String Attribute { get; set; }
        public String Op { get; set; }
        public String Value { get; set; }
    }
}
