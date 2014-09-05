using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.Model
{
    public class M_SYS_MENU
    {
        public Int64 M_ID { get; set; }
        public Int64 APP_ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string M_NAME { get; set; }
        /// <summary>
        /// 父菜单ID(-1为第一级)
        /// </summary>
        public Int64 M_PID { get; set; }
        /// <summary>
        /// 菜单级数(1为第一级)
        /// </summary>
        public Int32 M_LEVEL { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public Int32 M_ORDER { get; set; }
        /// <summary>
        /// 菜单类型
        /// 0:普通菜单
        /// 1:右键菜单
        /// </summary>
        public Int32 M_TYPE { get; set; }
        /// <summary>
        /// 页面路径
        /// </summary>
        public string M_URL { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string M_REMARK { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string M_ICON { get; set; }
        /// <summary>
        /// 菜单图标类型
        /// 0:无图标
        /// 1:样式图标
        /// 2:文件图标
        /// </summary>
        public Int32 M_ICONTYPE { get; set; }
        /// <summary>
        /// 是否生效
        /// </summary>
        public bool M_ISON { get; set; }
        /// <summary>
        /// 子级菜单
        /// </summary>
        public List<M_SYS_MENU> SubMenuList { get; set; }
    }
}
