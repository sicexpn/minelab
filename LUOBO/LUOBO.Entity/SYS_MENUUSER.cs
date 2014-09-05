using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /// <summary>
    /// 用户菜单权限
    /// </summary>
    public class SYS_MENUUSER
    {
        public Int64 ID { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Int64 M_ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Int64 U_ID { get; set; }
    }
}
