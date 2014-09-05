using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Model
{
    public class M_APSETTING_VIEW
    {
        public Entity.SYS_APDEVICE ap_device { get; set; }
        public List<Entity.SYS_SSID> ssid { get; set; }
    }
}
