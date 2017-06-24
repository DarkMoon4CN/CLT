using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Bonus
{
    /// <summary>
    /// 现金券、优惠券详情
    /// </summary>
    public class OwnBonusDetailEntity
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        [JsonProperty("ActName", NullValueHandling = NullValueHandling.Include)]
        public string ActName { get; set; }
        /// <summary>
        /// 使用说明
        /// </summary>
        [JsonProperty("Remark", NullValueHandling = NullValueHandling.Include)]
        public string Remark { get; set; }
        /// <summary>
        /// 有效时间  （2016/10/19  暂不使用）
        /// </summary>
        [JsonProperty("ActTime", NullValueHandling = NullValueHandling.Include)]
        public string ActTime { get; set; }

        /// <summary>
        /// 有效时间 （2016/10/19  启用）
        /// </summary>
        [JsonProperty("AmtEndTime", NullValueHandling = NullValueHandling.Include)]
        public string AmtEndTime { get; set; }

        /// <summary>
        /// 券类型
        /// </summary>
        [JsonProperty("RewTypeID", NullValueHandling = NullValueHandling.Include)]
        public int RewTypeID { get; set; }

        /// <summary>
        /// 使用范围
        /// </summary>
        [JsonProperty("ActRangeID", NullValueHandling = NullValueHandling.Include)]
        public int? ActRangeID { get { return 1; } }

        /// <summary>
        /// 使用范围说明
        /// </summary>
        [JsonProperty("ActRangeRemarks", NullValueHandling = NullValueHandling.Include)]
        public string ActRangeRemarks { get { return "除新手标都可使用"; } }

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
    }
}
