using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.BusinessService
{
    /// <summary>
    /// 用户登陆对象参数
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PWD { get; set; }
        /// <summary>
        /// 用户MAC
        /// </summary>
        public string MAC { get; set; }
    }

    /// <summary>
    /// 用户扩展属性
    /// </summary>
    public class UserExtProperty
    {
        /// <summary>
        /// 扩展属性ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 扩展属性类型
        /// </summary>
        public string ProType { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ProID { get; set; }
        /// <summary>
        /// 扩展属性值
        /// </summary>
        public string ProValue { get; set; }
    }
}