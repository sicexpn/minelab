using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUOBO.BusinessService
{
    public class APValidation
    {
        private string _NAME = "";
        private string _COLUMN = "";
        private string _ORDERBY = "";
        /// <summary>
        /// AP名称
        /// </summary>
        public string NAME
        {
            get { return _NAME; }
            set { _NAME = value; }
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
    }
}