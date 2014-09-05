using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class TAPI_YOUKU_SHOWCATEGORY
    {
        //total: "2842"
        //shows: Array[20]
        /// <summary>
        /// 符合条件的节目数量
        /// </summary>
        public string total { get; set; }
        public List<TAPI_YOUKU_SHOWCATEGORY_ITEM> shows { get; set; }
    }

    public class TAPI_YOUKU_SHOWCATEGORY_ITEM
    {
        //category: "电视剧"
        //comment_count: "286017"
        //completed: 0
        //episode_count: "50"
        //episode_updated: "34"
        //favorite_count: "258379"
        //hasvideotype: Array[5]   0: "正片"1: "预告片"2: "花絮"3: "MV"4: "首映式"
        //id: "9ffa9418853611e2a19e"
        //last_play_link: "http://v.youku.com/v_show/id_XNzYyODM3MjI0.html"
        //lastupdate: "2014-08-28 10:53:45"
        //link: "http://www.youku.com/show_page/id_z9ffa9418853611e2a19e.html"
        //name: "古剑奇谭"
        //paid: 0
        //play_link: "http://v.youku.com/v_show/id_XNzM0ODIyMTM2.html"
        //poster: "http://r1.ykimg.com/050D000053ABC58767379F1488056361"
        //published: "2014-07-03"
        //released: "2014-07-02"
        //score: "9.971"
        //streamtypes: Array[4]
        //thumbnail: "http://r4.ykimg.com/050B00005214876967583961AE07EEC8"
        //view_count: "978157063"

        /// <summary>
        /// 节目ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 节目名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 节目链接
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// 节目首个正片播放链接
        /// </summary>
        public string play_link { get; set; }
        /// <summary>
        /// 节目最新正片播放链接
        /// </summary>
        public string last_play_link { get; set; }
        /// <summary>
        /// 节目海报
        /// </summary>
        public string poster { get; set; }
        /// <summary>
        /// 节目图片
        /// </summary>
        public string thumbnail { get; set; }
        /// <summary>
        /// 流格式 flvhd flv 3gphd 3gp hd hd2
        /// </summary>
        public List<string> streamtypes { get; set; }
        /// <summary>
        /// 总集数
        /// </summary>
        public string episode_count { get; set; }
        /// <summary>
        /// 更新至
        /// </summary>
        public string episode_updated { get; set; }
        /// <summary>
        /// 总播放数
        /// </summary>
        public string view_count { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public string score { get; set; }
        /// <summary>
        /// 是否付费 0.否 1.是
        /// </summary>
        public string paid { get; set; }
        /// <summary>
        /// 节目发行时间(YYYY-MM-DD)
        /// </summary>
        public string released { get; set; }
        /// <summary>
        /// 优酷发行时间(YYYY-MM-DD)
        /// </summary>
        public string published { get; set; }
        /// <summary>
        /// 最后更新时间(YYYY-MM-DD HH:MM:SS)
        /// </summary>
        public string lastupdate { get; set; }
        /// <summary>
        /// 评论次数
        /// </summary>
        public string comment_count { get; set; }
        /// <summary>
        /// 收藏次数
        /// </summary>
        public string favorite_count { get; set; }
        /// <summary>
        /// 拥有视频类型
        /// </summary>
        public List<string> hasvideotype { get; set; }
        /// <summary>
        /// 完结 0:未完结 1:完结
        /// </summary>
        public string completed { get; set; }
    }
}
