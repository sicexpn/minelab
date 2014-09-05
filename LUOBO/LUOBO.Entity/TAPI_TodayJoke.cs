using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LUOBO.Entity
{
    public class TAPI_TodayJoke
    {
        //{
        //    "contextText":"1、好的防晒产品，推荐水宝宝；2、外出装备（帽子、伞、眼镜等）；3、避免暴晒；4、洗澡；5、晒后修护；6、芦荟敷肤；7、裸露皮肤都要防晒；8、多喝水；9、多吃高VC食物；10、保证充足睡眠；11、良好生活习惯；12、饮食用药注意",
        //    "createDate":"2013-08-21 15:34:48",
        //    "id":"54063",
        //    "juuid":"7a45eb2ce026458e8762131325e1931f",
        //    "title":"好的防晒产品",
        //    "type":"爆笑大杂烩"
        //}
        [BsonId]
        public ObjectId MyId;

        public string id { get; set; }
        public string juuid { get; set; }
        public string title { get; set; }
        public string contextText { get; set; }
        public string type { get; set; }
        public string createDate { get; set; }
    }
}
