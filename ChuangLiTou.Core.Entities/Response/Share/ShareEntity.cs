using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Share
{
    /// <summary>
    /// 微信分享信息实体类
    /// </summary>
    public class ShareEntity
    {
        /// <summary>
        /// 分享信息的标题
        /// </summary>
        public string shareTitle { get; set; }
        /// <summary>
        /// 分享信息的右侧图片
        /// </summary>
        public string shareImg { get; set; }
        /// <summary>
        /// 分享信息的内容
        /// </summary>
        public string shareContent
        {
            get; set;
        }
    }
    public class WeiXinEntity
    {
        public string imageUrl { get; set; }

    }

    /// <summary>
    /// 二维码返回实体
    /// </summary>
    public class QRCodeEntity
    {
        /// <summary>
        ///  图片地址
        /// </summary>
        public string LinkUrl { get; set; }
    }
}

