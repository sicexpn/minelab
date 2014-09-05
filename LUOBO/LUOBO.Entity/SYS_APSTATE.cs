using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /*Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
ID	ID	bigint			TRUE	FALSE	TRUE
MAC	MAC	varchar(32)	32		FALSE	FALSE	FALSE
设备状态	DEVICESTATE	int			FALSE	FALSE	FALSE
名称	NAME	varchar(32)	32		FALSE	FALSE	FALSE
CPU占用率%	CPU	varchar(30)	30		FALSE	FALSE	FALSE
空闲内存	MEMFREE	varchar(30)	30		FALSE	FALSE	FALSE
开机时常秒	POWERTIME	bigint			FALSE	FALSE	FALSE
待机时常秒	FREETIME	bigint			FALSE	FALSE	FALSE
开机后总流量	NETWORKTOTAL	varchar(30)	30		FALSE	FALSE	FALSE
网络速率	NETWORKRATE	varchar(30)	30		FALSE	FALSE	FALSE
历史时常	HISTORYTIME	bigint			FALSE	FALSE	FALSE
心跳时间	LASTHB	date			FALSE	FALSE	FALSE
     * */
    public class SYS_APSTATE
    {
        public Int64 ID { get; set; }
        public String MAC { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public int DEVICESTATE { get; set; }
        public String NAME { get; set; }
        /// <summary>
        /// CPU占用率%
        /// </summary>
        public String CPU { get; set; }
        /// <summary>
        /// 空闲内存
        /// </summary>
        public String MEMFREE { get; set; }
        /// <summary>
        /// 开机时常秒
        /// </summary>
        public Int64 POWERTIME { get; set; }
        /// <summary>
        /// 待机时常秒
        /// </summary>
        public Int64 FREETIME { get; set; }
        /// <summary>
        /// 开机后总流量
        /// </summary>
        public String NETWORKTOTAL { get; set; }
        /// <summary>
        /// 网络速率
        /// </summary>
        public String NETWORKRATE { get; set; }
        /// <summary>
        /// 历史时常
        /// </summary>
        public Int64 HISTORYTIME { get; set; }
        /// <summary>
        /// 心跳时间
        /// </summary>
        public DateTime LASTHB { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime POWERDATETIME { get; set; }
    }
}
