using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_Result
    {
        /// <summary>
        /// Code：0成功，1失败
        /// </summary>
        public int ResultCode { get; set; }
        public String ResultMsg { get; set; }
        public Object ResultOBJ { get; set; }
    }

    public class M_WCF_Result<T>
    {
        /// <summary>
        /// Code：0成功，1失败
        /// </summary>
        public int ResultCode { get; set; }
        public String ResultMsg { get; set; }
        public T ResultOBJ { get; set; }
    }
}
