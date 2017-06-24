using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.UserAddress
{
    /// <summary>
    /// Class ResponseUserAddressEntity.
    /// </summary>
    public class ResponseUserAddressEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public int? userId { get; set; }

        /// <summary>
        /// 省级id
        /// </summary>        
        [JsonProperty("provinceId", NullValueHandling = NullValueHandling.Ignore)]
        public int? provinceId { get; set; }

        /// <summary>
        /// 省级名称
        /// </summary>        
        [JsonProperty("provinceName", NullValueHandling = NullValueHandling.Ignore)]
        public string provinceName { get; set; }

        /// <summary>
        /// 市级id
        /// </summary>        
        [JsonProperty("cityId", NullValueHandling = NullValueHandling.Ignore)]
        public int? cityId { get; set; }

        /// <summary>
        /// 市级名称
        /// </summary>        
        [JsonProperty("cityName", NullValueHandling = NullValueHandling.Ignore)]
        public string cityName { get; set; }

        /// <summary>
        /// 县级id
        /// </summary>        
        [JsonProperty("countyId", NullValueHandling = NullValueHandling.Ignore)]
        public int? countyId { get; set; }

        /// <summary>
        /// 县级名称
        /// </summary>        
        [JsonProperty("countyName", NullValueHandling = NullValueHandling.Ignore)]
        public string countyName { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>        
        [JsonProperty("detailAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string detailAddress { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>        
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? createdOn { get; set; }

        /// <summary>
        /// updatedOn
        /// </summary>        
        [JsonProperty("updatedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? updatedOn { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>        
        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string userName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>        
        [JsonProperty("mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string mobile { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>        
        [JsonProperty("zipCode", NullValueHandling = NullValueHandling.Ignore)]
        public string zipCode { get; set; }

    }
}
