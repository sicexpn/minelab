using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_DeviceStatiscal
    {
        /// <summary>
        /// AP总数量
        /// </summary>
        public Int64 ApAllCounts { get; set; }
        /// <summary>
        /// AP在线数量
        /// </summary>
        public Int64 APOnlineCounts { get; set; }
        /// <summary>
        /// 总连接人数
        /// </summary>
        public Int64 PeopleConnectedCounts { get; set; }
    }
}
