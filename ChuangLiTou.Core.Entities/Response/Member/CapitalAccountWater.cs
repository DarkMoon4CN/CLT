using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Member
{
    /// <summary>
    /// 资金流水
    /// </summary>
    public class CapitalAccountWater
    {
        /// <summary>
        /// 帐户流水id
        /// </summary>        
        [JsonProperty("waterId", NullValueHandling = NullValueHandling.Ignore)]
        public int? account_water_id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>        
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public int? membertable_registerid { get; set; }

        /// <summary>
        /// 收入
        /// </summary>        
        [JsonProperty("income", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? income { get; set; }

        /// <summary>
        /// 支出
        /// </summary>        
        [JsonProperty("expenditure", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? expenditure { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>        
        [JsonProperty("occurrencedOn", NullValueHandling = NullValueHandling.Ignore)]
        public string time_of_occurrence { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>        
        [JsonProperty("accountBalance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? account_balance { get; set; }

        /// <summary>
        /// 资金类型
        /// </summary>        
        [JsonProperty("fundType", NullValueHandling = NullValueHandling.Ignore)]
        public int? types_Finance { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>        
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? createtime { get; set; }

        /// <summary>
        /// 主键id
        /// </summary>        
        [JsonProperty("keyId", NullValueHandling = NullValueHandling.Ignore)]
        public int? keyid { get; set; }

        /// <summary>
        /// 备注
        /// </summary>        
        [JsonProperty("remarks", NullValueHandling = NullValueHandling.Ignore)]
        public string remarks { get; set; }   
				       
    }
}
