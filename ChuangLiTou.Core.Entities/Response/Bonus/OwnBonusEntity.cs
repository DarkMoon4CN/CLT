using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Bonus
{
    /// <summary>
    /// 个人代金券列表数据
    /// </summary>
    public class OwnBonusEntity
    {
        /// <summary>
        /// 券编号
        /// </summary>        
        [JsonProperty("UserAct", NullValueHandling = NullValueHandling.Ignore)]
        public int UserAct { get; set; }
        /// <summary>
        /// 券类型
        /// </summary>        
        [JsonProperty("RewTypeID", NullValueHandling = NullValueHandling.Ignore)]
        public int RewTypeID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>        
        [JsonProperty("Amt", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amt { get; set; }
        /// <summary>
        /// 活动有效时间
        /// </summary>        
        [JsonProperty("ActStarttime", NullValueHandling = NullValueHandling.Ignore)]
        public string ActTime { get; set; }
        /// <summary>
        /// 来源
        /// </summary>        
        [JsonProperty("TypeName", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName { get; set; }
        /// <summary>
        /// 使用状态 0 未使用   1已使用  2 已过期 3 锁定中
        /// </summary>        
        [JsonProperty("UseState", NullValueHandling = NullValueHandling.Ignore)]
        public string UseState { get; set; }

        /// <summary>
        /// 使用限制-限制可使用的标的期限(最小月-最大月)。要求大于等于最小月且小于等于最大月
        /// </summary>
        [JsonProperty("UseLifeLoan", NullValueHandling = NullValueHandling.Ignore)]
        public string UseLifeLoan { get; set; }
        /// <summary>
        /// 使用限制-限制可使用的标的期限，配套描述信息
        /// </summary>
        [JsonProperty("UseLifeLoanMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string UseLifeLoanMessage { get; set; }

        /// <summary>
        /// 使用说明
        /// </summary>
        [JsonProperty("Remark", NullValueHandling = NullValueHandling.Include)]
        public string Remark { get; set; }
    }
}
