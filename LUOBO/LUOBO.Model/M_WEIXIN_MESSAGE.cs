using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace LUOBO.Model
{
    [XmlRoot("xml")]
    [DataContract(Namespace = "")]
    public class M_WEIXIN_MESSAGE
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// text,image,voice,video,link
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 图片消息媒体id,语音消息媒体id,视频消息媒体id，
        /// </summary>
        public string MediaId { get; set; }

        #region 文本消息
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
        #endregion

        #region 图片消息
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }
        #endregion

        #region 图片消息
        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }
        #endregion

        #region 视频消息
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }
        #endregion

        #region 地理位置消息
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        #endregion

        #region 链接消息
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
        #endregion

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }

    }
}
