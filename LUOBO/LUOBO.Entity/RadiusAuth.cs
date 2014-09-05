using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class RadiusAuth
    {
        //用户QQ、微博唯一ID
        public String OpenID { get; set; }
        /// <summary>
        /// UserType字段//0-freeuser;1-qq;2-微博;3-微信;4-other
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// 机构ID
        /// </summary>
        public Int64 OID { get; set; }
        public String UserMac { get; set; }
    }
}
