using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.NormalArea
{
    /// <summary>
    /// 省市区
    /// </summary>
    public class NormalAreaEntity
    {
        /// <summary>
        /// 区域Id
        /// </summary>        
        [JsonProperty("areaId", NullValueHandling = NullValueHandling.Ignore)]
        public int? AreaId { get; set; }

        /// <summary>
        /// 区域码
        /// </summary>        
        [JsonProperty("areaCode", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaCode { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>        
        [JsonProperty("areaName", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaName { get; set; }

        /// <summary>
        /// 父类id
        /// </summary>        
        [JsonProperty("parentId", NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentId { get; set; }

        /// <summary>
        /// 等级 省1市2县3
        /// </summary>        
        [JsonProperty("areaLevel", NullValueHandling = NullValueHandling.Ignore)]
        public int? AreaLevel { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>        
        [JsonProperty("areaOrder", NullValueHandling = NullValueHandling.Ignore)]
        public int? AreaOrder { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>        
        [JsonProperty("areaNameEn", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaNameEn { get; set; }

        /// <summary>
        /// 英文简写
        /// </summary>        
        [JsonProperty("areaShortNameEn", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaShortNameEn { get; set; }

    }
}
