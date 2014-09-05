using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUOBO.Entity;
namespace LUOBO.Model
{
    public class M_ORGAPP
    {
        public List<SYS_APPLICATION> appsAuth { get; set; }
        public List<SYS_APPLICATION> appsNoAuth { get; set; }
    }
}
