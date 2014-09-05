using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;

namespace LUOBO.Model
{
    public class M_SYS_USER
    {
        public List<SYS_USER_VIEW> UserList { get; set; }
        public Int32 AllCount { get; set; }
    }
}
