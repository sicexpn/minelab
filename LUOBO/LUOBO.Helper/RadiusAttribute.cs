using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Helper
{
    public class RadiusAttribute
    {

        /// <summary>
        /// 允许同时连接数目
        /// </summary>
        static public String Simultaneous_Use { get { return "Simultaneous-Use"; } }
        /// <summary>
        /// 上行字节
        /// </summary>
        static public String Acct_Input_Octets { get { return "Acct-Input-Octets"; } }
        /// <summary>
        /// 下行字节
        /// </summary>
        static public String Acct_Output_Octets { get { return "Acct-Output-Octets"; } }
        /// <summary>
        /// 用户可用的剩余时间，以秒为单位；
        /// </summary>
        static public String Session_Timeout { get { return "Session_Timeout"; } }
        /// <summary>
        /// 用户的闲置切断时间，以秒为单位
        /// </summary>
        static public String Idel_Timeout { get { return "Idel-Timeout"; } }
        /// <summary>
        /// 实时计费的间隔，以秒为单位。
        /// </summary>
        static public String Acct_Interim_Interval { get { return "Acct_Interim_Interval"; } }
        /// <summary>
        /// 下行带宽
        /// </summary>
        static public String ChilliSpot_Bandwidth_Max_Down { get { return "ChilliSpot-Bandwidth-Max-Down"; } }
        /// <summary>
        /// 上行带宽
        /// </summary>
        static public String ChilliSpot_Bandwidth_Max_Up { get { return "ChilliSpot-Bandwidth-Max-Up"; } }
        /// <summary>
        /// 密码
        /// </summary>
        static public String User_Password { get { return "User-Password"; } }

    }
}
