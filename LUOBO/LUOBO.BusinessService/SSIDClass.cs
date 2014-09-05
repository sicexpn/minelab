using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.BusinessService
{
    /// <summary>
    /// SSID名称修改参数类
    /// </summary>
    public class SSIDNameSave
    {
        /// <summary>
        /// SSID编号
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// SSID名称
        /// </summary>
        public string Name { get; set; }
    }

    public class SSIDValidation
    {
        private string _APNAME = "";
        private string _COLUMN = "";
        private string _ORDERBY = "";
        private Int64 _CurrentPage = 1;
        private Int64 _PageSize = 50;

        /// <summary>
        /// 关键字
        /// </summary>
        public string KEY { get; set; }
        /// <summary>
        /// 类型 AP、SSID
        /// </summary>
        public string TYPE { get; set; }
        /// <summary>
        /// 审核状态(枚举)
        /// </summary>
        public Int32 STATE { get; set; }
        /// <summary>
        /// 设备MAC地址
        /// </summary>
        public string APMAC
        {
            get { return _APNAME; }
            set { _APNAME = value; }
        }
        /// <summary>
        /// 排序列名
        /// </summary>
        public string COLUMN
        {
            get { return _COLUMN; }
            set { _COLUMN = value; }
        }
        /// <summary>
        /// 排序  ASC:升序 DESC:降序 空:不排序
        /// </summary>
        public string ORDERBY
        {
            get { return _ORDERBY; }
            set { _ORDERBY = value; }
        }
        /// <summary>
        /// 当前页
        /// </summary>
        public Int64 CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }
        /// <summary>
        /// 显示条数
        /// </summary>
        public Int64 PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
    }
}