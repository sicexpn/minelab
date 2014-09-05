using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.Model
{
    public class M_ORG_EXT_PROPERTY
    {
        public Int64 ORG_ID { get; set; }

        public int PRO_ID { get; set; }

        public String PRO_NAME { get; set; }

        public List<M_ORG_EXT_PROPERTY_ITEM> PRO_ITEM { get; set; }
    }

    public class M_ORG_EXT_PROPERTY_ITEM
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public String ProValue { get; set; }

        /// <summary>
        /// 属性项目
        /// </summary>
        public SYS_DICT_EXTPROP PropertyInfo { get; set; }
    }
}
