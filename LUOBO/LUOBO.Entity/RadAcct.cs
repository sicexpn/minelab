using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 从RadAcct表中读取SessionId，从而判断当前用户，继而进一步验证
    /// </summary>
    /* radacctid	bigint	21	0	0	-1	0	0	0		0					-1	0
acctsessionid	varchar	64	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
acctuniqueid	varchar	32	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
username	varchar	64	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
groupname	varchar	64	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
realm	varchar	64	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
nasipaddress	varchar	15	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
nasportid	varchar	15	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
nasporttype	varchar	32	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
acctstarttime	datetime	0	0	-1	0	0	0	0		0					0	0
acctstoptime	datetime	0	0	-1	0	0	0	0		0					0	0
acctsessiontime	int	12	0	-1	0	0	0	0		0					0	0
acctauthentic	varchar	32	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
connectinfo_start	varchar	50	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
connectinfo_stop	varchar	50	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
acctinputoctets	bigint	20	0	-1	0	0	0	0		0					0	0
acctoutputoctets	bigint	20	0	-1	0	0	0	0		0					0	0
calledstationid	varchar	50	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
callingstationid	varchar	50	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
acctterminatecause	varchar	32	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
servicetype	varchar	32	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
framedprotocol	varchar	32	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
framedipaddress	varchar	15	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
acctstartdelay	int	12	0	-1	0	0	0	0		0					0	0
acctstopdelay	int	12	0	-1	0	0	0	0		0					0	0
xascendsessionsvrkey	varchar	10	0	-1	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
*/
    public class RadAcct
    {
        public Int64 RadAcctId { get; set; }
        /// <summary>
        /// SessionId
        /// </summary>
        public String AcctSessionId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }
        public String AcctUniqueId { get; set; }
        public String NasIpAddress { get; set; }
        public String NasPortType { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime AcctStartTime { get; set; }
        public DateTime AcctStopTime { get; set; }
        public Int32 AcctSessionTime { get; set; }
        /// <summary>
        /// 流量
        /// </summary>
        public Int64 AcctInputOctets { get; set; }
        public Int64 AcctOutputOctets { get; set; }
        public String CalledStationId { get; set; }
        public String CallingStationId { get; set; }
    }
}
