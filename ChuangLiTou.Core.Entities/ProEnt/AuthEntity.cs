using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.ProEnt
{
   public class AuthEntity
    {
        /// <summary>
        /// 应用id
        /// </summary>
        /// <value>The application identifier.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? appId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("appName", NullValueHandling = NullValueHandling.Ignore)]
        public string appName { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>        
        [JsonProperty("appSecret", NullValueHandling = NullValueHandling.Ignore)]
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
        /// 安全码
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("appSafeCode", NullValueHandling = NullValueHandling.Ignore)]
        public string appSafeCode { get; set; }

        /// <summary>
        /// 应用ip
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("appServerIps", NullValueHandling = NullValueHandling.Ignore)]
        public string appServerIps { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("isDelete", NullValueHandling = NullValueHandling.Ignore)]
        public int? isDelete { get; set; }

        /// <summary>
        /// 应用状态
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("appStatus", NullValueHandling = NullValueHandling.Ignore)]
        public int? appStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? createdOn { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>        
        [JsonIgnore]
        [JsonProperty("updatedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? updatedOn { get; set; }
    }
}
