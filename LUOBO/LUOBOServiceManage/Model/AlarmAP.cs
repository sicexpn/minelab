using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.ServiceManage.Model
{
    public class AlarmAP
    {
        //<AP Alarm="广州邮储">
        //  <ID>40</ID>
        //  <OID>10049</OID>
        //  <Alias>梅花园邮局营业部</Alias>
        //  <MAC>E0-05-C5-B1-E5-89</MAC>
        //  <LAT>113.3202160000</LAT>
        //  <LON>23.1796980000</LON>
        //</AP>
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        /// <summary>
        /// 告警分组名称
        /// </summary>
        public string Alarm { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double LAT { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double LON { get; set; }
    }
}
