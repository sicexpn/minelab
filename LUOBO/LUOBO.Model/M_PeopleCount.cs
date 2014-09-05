using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_PeopleCount
    {
        ///// <summary>
        ///// 安装人次
        ///// </summary>
        //public Int64 InstalPeopleCount { get; set; }
        ///// <summary>
        ///// 安装人数
        ///// </summary>
        //public Int64 InstalPeopleNum { get; set; }
        ///// <summary>
        ///// 开机人次
        ///// </summary>
        //public Int64 StartPeopleCount { get; set; }
        ///// <summary>
        ///// 开机人数
        ///// </summary>
        //public Int64 StartPeopleNum { get; set; }
        /// <summary>
        /// 在线人数
        /// </summary>
        public Int64 OnlinePeopleNum { get; set; }
        /// <summary>
        /// 总访问量
        /// </summary>
        public Int64 AllVisitCounts { get; set; }
        /// <summary>
        /// 日均访问量
        /// </summary>
        public Int64 DayAvageVisitCounts { get; set; }
        /// <summary>
        /// 平均访问时常
        /// </summary>
        public Int64 AvageVisitTime { get; set; }


    }
}
