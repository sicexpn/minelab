using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_SECURITY_SSID_Filter
    {
        /// <summary>
        /// 查询条件：
        /// 1，可疑
        /// 2，可信
        /// 3，中文
        /// 4，今日新增
        /// 5，对手
        /// 其他，全部
        /// </summary>
        public String filtermod { get; set; }
        /// <summary>
        /// 排序字段
        /// 字段名称
        /// </summary>
        public String ordercul { get; set; }
        /// <summary>
        /// 排序规则
        /// asc：正序
        /// desc：倒序
        /// </summary>
        public String sortstr { get; set; }
        /// <summary>
        /// 是否查询子机构
        /// </summary>
        public Boolean isSubOrg { get; set; }
        /// <summary>
        /// 查询的设备ID
        /// </summary>
        public Int64 apid { get; set; }
        /// <summary>
        /// 是否实时
        /// </summary>
        public Boolean isRealTime { get; set; }
    }
}
