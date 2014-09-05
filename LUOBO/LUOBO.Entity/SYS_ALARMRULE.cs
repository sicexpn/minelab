using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_ALARMRULE
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int64 AL_ID { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string AL_NAME { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string AL_REMARK { get; set; }
        /// <summary>
        /// 类型   0=全部都检查,1=只检查心跳，2只检查异常
        /// </summary>
        public Int16 AL_MODE { get; set; }
        /// <summary>
        /// 类型 0=默认,1=自定义,-99=关闭
        /// </summary>
        public Int16 AL_TYPE { get; set; }
        /// <summary>
        /// 间隔 
        /// </summary>
        public Int64 AL_TIME { get; set; }
        /// <summary>
        /// 间隔单位 y=年,M=月,d=天,h=时,m=分,s=秒,ms=毫秒
        /// </summary>
        public string AL_UNIT { get; set; }
        /// <summary>
        /// 告警小于阈值
        /// </summary>
        public Int64 AL_MIN { get; set; }
        /// <summary>
        /// 告警大于阈值
        /// </summary>
        public Int64 AL_MAX { get; set; }
        /// <summary>
        /// 生效起始时间
        /// </summary>
        public TimeSpan AL_STIME { get; set; }
        /// <summary>
        /// 生效结束时间
        /// </summary>
        public TimeSpan AL_ETIME { get; set; }
        /// <summary>
        /// 生效规则 目前按星期生效，2进制低位起始7位标识周一到周日，1位生效，0为不生效
        /// </summary>
        public Int32 AL_DATERULE { get; set; }
    }
}
