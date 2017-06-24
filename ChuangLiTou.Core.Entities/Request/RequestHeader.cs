
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Request
{
    /// <summary>
    /// 接口公共参数实体
    /// </summary>
    public class RequestHeader
    {

        /// <summary>
        /// 应用id
        /// </summary> 
        public int? appId { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>         
        public string appSecret { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string accessToken { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timeStamp { get; set; }

        /// <summary>
        /// 特殊戳，普通Controller必须要带
        /// </summary>
        public string specialStamp { get; set; }


    }

}
