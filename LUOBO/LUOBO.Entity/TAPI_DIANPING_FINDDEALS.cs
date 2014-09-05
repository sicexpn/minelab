using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class TAPI_DIANPING_FINDDEALS
    {
        //count: 20
        //deals: Array[20]
        //status: "OK"
        //total_count: 358

        /// <summary>
        /// 本次API访问所获取的单页团购数量
        /// </summary>
        public Int32 count { get; set; }
        /// <summary>
        /// 团购单列表
        /// </summary>
        public List<TAPI_DIANPING_DEAL> deals { get; set; }
        /// <summary>
        /// 本次API访问状态，如果成功返回"OK"，并返回结果字段，如果失败返回"ERROR"，并返回错误说明
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 所有页面团购总数
        /// </summary>
        public Int64 total_count { get; set; }
    }

    public class TAPI_DIANPING_DEAL
    {
        //businesses: Array[40]
        //categories: Array[1]
        //city: "北京"
        //commission_ratio: 0.03
        //current_price: 81
        //deal_h5_url: "http://lite.m.dianping.com/Lph3w7KujQ"
        //deal_id: "2-6215011"
        //deal_url: "http://dpurl.cn/p/gvNDeeoxcL"
        //description: "汉拿山 仅售81元，价值100元北京地区代金券，不限时段通用，节假日通用！（另有其他套餐可选）"
        //distance: 862
        //image_url: "http://t2.dpfile.com/pc/mc/af6277055b5741b78e43ef1f01f957e0(640x1024)/thumb.jpg"
        //list_price: 100
        //publish_date: "2014-06-20"
        //purchase_count: 26612
        //purchase_deadline: "2014-09-19"
        //regions: Array[10]
        //s_image_url: "http://t2.dpfile.com/pc/mc/af6277055b5741b78e43ef1f01f957e0(640x1024)/thumb_1.jpg"
        //title: "汉拿山"

        /// <summary>
        /// 团购所适用的商户列表
        /// </summary>
        public List<TAPI_DIANPING_BUSINESSES> businesses { get; set; }
        /// <summary>
        /// 团购所属分类
        /// </summary>
        public List<string> categories { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 当前团单的佣金比例
        /// </summary>
        public double commission_ratio { get; set; }
        /// <summary>
        /// 团购价格
        /// </summary>
        public double current_price { get; set; }
        /// <summary>
        /// 团购HTML5页面链接，适用于移动应用和联网车载应用
        /// </summary>
        public string deal_h5_url { get; set; }
        /// <summary>
        /// 团购单ID
        /// </summary>
        public string deal_id { get; set; }
        /// <summary>
        /// 团购Web页面链接，适用于网页应用
        /// </summary>
        public string deal_url { get; set; }
        /// <summary>
        /// 团购描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 团购单所适用商户中距离参数坐标点最近的一家与坐标点的距离，单位为米；如不传入经纬度坐标，结果为-1；如团购单无关联商户，结果为MAXINT
        /// </summary>
        public Int64 distance { get; set; }
        /// <summary>
        /// 团购图片链接，最大图片尺寸450×280
        /// </summary>
        public string image_url { get; set; }
        /// <summary>
        /// 团购包含商品原价值
        /// </summary>
        public double list_price { get; set; }
        /// <summary>
        /// 团购发布上线日期
        /// </summary>
        public string publish_date { get; set; }
        /// <summary>
        /// 团购当前已购买数
        /// </summary>
        public Int64 purchase_count { get; set; }
        /// <summary>
        /// 团购单的截止购买日期
        /// </summary>
        public string purchase_deadline { get; set; }
        /// <summary>
        /// 团购适用商户所在行政区
        /// </summary>
        public List<string> regions { get; set; }
        /// <summary>
        /// 小尺寸团购图片链接，最大图片尺寸160×100
        /// </summary>
        public string s_image_url { get; set; }
        /// <summary>
        /// 团购标题
        /// </summary>
        public string title { get; set; }
    }

    public class TAPI_DIANPING_BUSINESSES
    {
        //city: "北京"
        //h5_url: "http://lite.m.dianping.com/svSVGrmHRl"
        //id: 510895
        //latitude: 39.985077
        //longitude: 116.36994
        //name: "汉拿山(花园路总店)"
        //url: "http://dpurl.cn/p/rO21Xkumr8"

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 商户页链接，HTML5页面链接，适用于移动应用和联网车载应用
        /// </summary>
        public string h5_url { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public Int64 id { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 商户名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 商户页链接
        /// </summary>
        public string url { get; set; }
    }
}
