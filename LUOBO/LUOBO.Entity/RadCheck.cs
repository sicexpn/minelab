using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    /*id	int	11	0	0	-1	-1	0	0		0					-1	0
    username	varchar	64	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
    attribute	varchar	64	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
    op	char	2	0	0	0	0	0	0	==	0		utf8	utf8_unicode_ci		0	0
    value	varchar	253	0	0	0	0	0	0		0		utf8	utf8_unicode_ci		0	0
    userType	int	1	0	0	0	0	0	0		0					0	0
    */
    /// <summary>
    /// 验证完成后，将生产的密码写入数据库
    /// </summary>
    public class RadCheck
    {
        public Int64 ID { get; set; }
        public String UserName { get; set; }
        /// <summary>
        /// User-Password
        /// </summary>
        public String Attribute { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// //0-freeuser;1-qq;2-微博;3-微信;4-other
        /// </summary>
        public Int64 UserType { get; set; }
    }
}
