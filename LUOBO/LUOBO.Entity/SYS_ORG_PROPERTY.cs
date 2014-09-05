using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Mvc;
using LUOBO.Helper;
namespace LUOBO.Entity
{
    /*
Name	Code	Data Type	Length	Precision	Primary	Foreign Key	Mandatory
ID	ID	bigint			TRUE	FALSE	TRUE
机构ID	OID	bigint			FALSE	TRUE	FALSE
类别	PTYPE	varchar(50)	50		FALSE	FALSE	FALSE
属性名称	PNAME	varchar(50)	50		FALSE	FALSE	FALSE
属性值	PVALUE	varchar(512)	512		FALSE	FALSE	FALSE
     * */
    //[DataContract]
    //[ModelBinder(typeof(JsonModelBinder))]
    public class SYS_ORG_PROPERTY
    {
        [DataMember]
        public Int64 ID { get; set; }
        [DataMember]
        public Int64 OID { get; set; }
        [DataMember]
        public String PTYPE { get; set; }
        [DataMember]
        public String PNAME { get; set; }
        [DataMember]
        public String PVALUE { get; set; }

    }
    [DataContract]
    //[ModelBinder(typeof(JsonModelBinder))]
    public class SYS_ORG_PROPERTY_VIEW
    {
        [DataMember]
        public SYS_ORG_PROPERTY[] Items { get; set; }
    }
}
