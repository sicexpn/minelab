using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 控制用户的流量、session
    /// </summary>
    public class RadReply
    {
        public Int64 Id { get; set; }
        public String UserName { get; set; }
        public String Attribute { get; set; }
        public String Op { get; set; }
        public String Value { get; set; }
    }
}
