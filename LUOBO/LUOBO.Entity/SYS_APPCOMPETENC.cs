using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class SYS_APPCOMPETENC
    {
        //Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
        //ID	ID	bigint			TRUE	FALSE	TRUE
        //APPID	APPID	bigint			FALSE	TRUE	FALSE
        //名称	NAME	varchar(200)	200		FALSE	FALSE	FALSE
        //Controller名称	CONTROLLER	varchar(128)	128		FALSE	FALSE	FALSE
        //Action名称	ACTION	varchar(128)	128		FALSE	FALSE	FALSE
        /// <summary>
        /// ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Int64 APPID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// Controller名称
        /// </summary>
        public string CONTROLLER { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        public string ACTION { get; set; }
    }
}
