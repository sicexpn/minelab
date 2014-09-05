using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
     //   `id` int(11) NOT NULL,
     //`province` varchar(20) DEFAULT NULL,
     //`city` varchar(20) DEFAULT NULL,
     //`town` varchar(20) DEFAULT NULL,
     //PRIMARY KEY (`id`)
     public class SYS_DICT_ZONE
     {
          public Int64 ID { get; set; }
          public String Province { get; set; }
          public String City { get; set; }
          public String Town { get; set; }
     }
}
