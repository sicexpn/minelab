using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LUOBO.Entity
{
    public class TAPI_TodayImage
    {
        //{
        //    "bdjuuid":"7fb4ddcce2ef4214b1f28228d4f43ed3",
        //    "context":"小妹妹和雕像跳舞",
        //    "createDate":"2012-11-11 21:21:02",
        //    "id":"15567",
        //    "imgUrl":"http://ww2.sinaimg.cn/large/82450bb6jw1dyguqpxjmjj.jpg",
        //    "type":"囧创意"
        //}
        [BsonId]
        public ObjectId MyId;

        public string id { get; set; }
        
        public string bdjuuid { get; set; }
        public string context { get; set; }
        public string imgUrl { get; set; }
        public string type { get; set; }
        public string createDate { get; set; }
    }
}
