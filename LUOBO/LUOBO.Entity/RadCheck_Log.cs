using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{/*`ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `OpenID` varchar(200) DEFAULT NULL COMMENT '用户唯一标识（微博、微信、QQ）',
  `OID` bigint(20) DEFAULT NULL COMMENT '机构ID',
  `UserMac` varchar(50) DEFAULT NULL,
  `CreatTime` datetime DEFAULT NULL,
  * */
    public class RadCheck_Log
    {
        public Int64 ID { get; set; }
        public Int64 OID { get; set; }
        public String OpenID { get; set; }
        public int UserType { get; set; }
        public String UserMac { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
