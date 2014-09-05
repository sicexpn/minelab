using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class APControl
    {
        /// <summary>
        /// groupName
        /// </summary>
        public String CalledStationId { get; set; }
        /// <summary>
        /// 允许同时连接数目
        /// </summary>
        public String Simultaneous_Use { get; set; }
        /// <summary>
        /// 上行字节
        /// </summary>
        public String Acct_Input_Octets { get; set; }
        /// <summary>
        /// 下行字节
        /// </summary>
        public String Acct_Output_Octets { get; set; }
        /// <summary>
        /// 用户可用的剩余时间，以秒为单位；
        /// </summary>
        public String Session_Timeout { get; set; }
        /// <summary>
        /// 用户的闲置切断时间，以秒为单位
        /// </summary>
        public String Idel_Timeout { get; set; }
        /// <summary>
        /// 实时计费的间隔，以秒为单位。
        /// </summary>
        public String Acct_Interim_Interval { get; set; }
    }
}
