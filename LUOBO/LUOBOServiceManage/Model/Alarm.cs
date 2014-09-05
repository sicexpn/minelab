using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.ServiceManage.Model
{
    public class Alarm
    {
        //<Alarm Type="1" Mode="1">
        //  <Name>广州邮储</Name>
        //  <Remark>没有访问</Remark>
        //  <Time Type="h">1</Time>
        //  <Min>0</Min>
        //  <Max>0</Max>
        //</Alarm>
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Int64 Time { get; set; }
        /// <summary>
        /// 时间单位
        /// y=年,M=月,d=天,h=时,m=分,s=秒,ms=毫秒
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public Int64 Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public Int64 Max { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime STime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ETime { get; set; }
        /// <summary>
        /// 类型
        /// 0=默认,1=自定义,-99=关闭
        /// </summary>
        public Int16 Type { get; set; }
        /// <summary>
        /// 模式
        /// 0=智能告警,1=自定义
        /// </summary>
        public Int16 Mode { get; set; }

        public override string ToString()
        {
            return Name + (Type == 0 ? "(默认)" : "");
        }
    }
}
