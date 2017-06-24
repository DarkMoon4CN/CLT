using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Share
{
    /// <summary>
    /// 请求分享的账号（手机号）实体
    /// </summary>
    public class RequestQRCode
    {
        /// <summary>
        /// 手机号
        /// </summary>
       public string UserId { get; set; }
    }
}
