using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.AdNews
{
    /// <summary>
    /// 广告列表
    /// </summary>
    public class AdEntity
    {

        /// <summary>
        /// 标识
        /// </summary>        
        [JsonProperty("AdId", NullValueHandling = NullValueHandling.Ignore)]
        public int? Adid { get; set; }

        /// <summary>
        /// 广告名称
        /// </summary>        
        [JsonProperty("AdName", NullValueHandling = NullValueHandling.Ignore)]
        public string AdName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>        
        [JsonProperty("CreatedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Adcreatetime { get; set; }

        /// <summary>
        /// 0 默认 (有效)  1下架(无效)
        /// </summary>        
        [JsonProperty("AdState", NullValueHandling = NullValueHandling.Ignore)]
        public int? AdState { get; set; }

        /// <summary>
        /// AdTypeId
        /// </summary>        
        [JsonProperty("AdTypeId", NullValueHandling = NullValueHandling.Ignore)]
        public int? AdTypeId { get; set; }

        /// <summary>
        /// 广告路径
        /// </summary>        
        [JsonProperty("AdPath", NullValueHandling = NullValueHandling.Ignore)]
        public string AdPath { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>        
        [JsonProperty("AdLink", NullValueHandling = NullValueHandling.Ignore)]
        public string AdLink { get; set; }

    }
}
