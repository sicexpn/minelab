using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class UserLogin
    {

        //public String AcctSessionId { get; set; }
        //public String SSID { get; set; }
        ///// <summary>
        ///// groupName
        ///// </summary>
        public String CalledStationId { get; set; }
        //public String AdId { get; set; }

        public String UserName { get; set; }
        /// <summary>
        /// UserType字段//0-freeuser;1-qq;2-微博;3-微信;4-other
        /// </summary>
        public int UserType { get; set; }
    }
}
