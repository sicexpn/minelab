using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_DICT_EXTPROP
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Int32 PROP_ID { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public Int32 PROP_TYPE { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string PROP_NAME { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PROP_CODE { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string PROP_DEFAULTVALUE { get; set; }
        /// <summary>
        /// 是否可以为空
        /// </summary>
        public Int32 PROP_ISNULL { get; set; }
        /// <summary>
        /// 校验规则正则表达式
        /// </summary>
        public string PROP_REGEX { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string PROP_COMMENT { get; set; }
        /// <summary>
        /// 用户是否可编辑
        /// </summary>
        public Int32 USER_CANEDIT { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public Int32 PROP_SORT { get; set; }
        /// <summary>
        /// 输入类型
        /// </summary>
        public Int32 PROP_INPUTTYPE { get; set; }
        /// <summary>
        /// 用户是否可见
        /// </summary>
        public Int32 USER_CANVIEW { get; set; }
        /// <summary>
        /// 帮助信息
        /// </summary>
        public string PROP_HELP { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string PROP_DATA { get; set; }
    }
}
