using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LUOBO.Model
{
    [XmlRoot("xml")]
    public class M_WeixinMsgText
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
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
    }
}
