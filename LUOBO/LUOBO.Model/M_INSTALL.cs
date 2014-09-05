using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_INSTALL
    {
        #region 设备
        /// <summary>
        /// 设备名称
        /// </summary>
        public string AP_ALIAS { get; set; }
        /// <summary>
        /// 设备MAC
        /// </summary>
        public string AP_MAC { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string AP_ADDRESS { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double AP_LAT { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double AP_LON { get; set; }
        /// <summary>
        /// 心跳间隔
        /// </summary>
        public Int64 AP_HBINTERVAL { get; set; }
        #endregion

        #region 机构
        /// <summary>
        /// 机构类型
        /// </summary>
        public Int32 ORG_TYPE { get; set; }
        /// <summary>
        /// 机构编号
        /// </summary>
        public Int64 ORG_ID { get; set; }
        /// <summary>
        /// 父机构编号
        /// </summary>
        public Int64 ORG_PID { get; set; }
        /// <summary>
        /// 机构全称
        /// </summary>
        public string ORG_FULLNAME { get; set; }
        /// <summary>
        /// 机构简称
        /// </summary>
        public string ORG_SIMPLENAME { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string ORG_PROVINCE { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string ORG_CITY { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string ORG_COUNTIES { get; set; }
        /// <summary>
        /// 行业(字典)
        /// </summary>
        public Int32 ORG_INDUSTRY { get; set; }
        /// <summary>
        /// 面积(字典)
        /// </summary>
        public Int32 ORG_AREA { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ORG_CONTACTER { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ORG_CONTACT { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string ORG_QQ { get; set; }
        /// <summary>
        /// 微博
        /// </summary>
        public string ORG_WEIBO { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string ORG_WEIXIN { get; set; }
        /// <summary>
        /// 是否有管理权(true:新增机构并创建管理员 false:使用原有机构)
        /// </summary>
        public bool ORG_ISMANAGE { get; set; }
        #endregion

        #region 用户
        /// <summary>
        /// 管理员帐号
        /// </summary>
        public string USER_ACCOUNT { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        public string USER_PWD { get; set; }
        #endregion

        /// <summary>
        /// SSID列表
        /// </summary>
        public List<M_SSID> SSIDLIST { get; set; }

        public string DEFAULT { get; set; }
    }
}
