using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_WCF_SSIDGroupAP_VIEW
    {
        /// <summary>
        /// AP编号
        /// </summary>
        public Int64 APID { get; set; }
        /// <summary>
        /// AP名称
        /// </summary>
        public string APNAME { get; set; }
        /// <summary>
        /// 对应的SSID列表
        /// </summary>
        public List<M_WCF_SSID_VIEW> SSIDList { get; set; }
    }
}
