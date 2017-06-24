using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Bonus
{
    public class BonusEntity
    {
        /// <summary>
        /// 奖励帐户id
        /// </summary>        
        [JsonProperty("bonusId", NullValueHandling = NullValueHandling.Ignore)]
        public int? bonus_account_id { get; set; }

        /// <summary>
        /// 活动计划表id
        /// </summary>        
        [JsonProperty("scheduleId", NullValueHandling = NullValueHandling.Ignore)]
        public int? activity_schedule_id { get; set; }

        /// <summary>
        /// 用户注册id
        /// </summary>        
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public int? membertable_registerid { get; set; }

        /// <summary>
        /// 活动计划名称
        /// </summary>
        [JsonProperty("scheduleName", NullValueHandling = NullValueHandling.Ignore)]
        public string activity_schedule_name { get; set; }

        /// <summary>
        /// 奖励金额
        /// </summary>        
        [JsonProperty("scheduleAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? amount_of_reward { get; set; }

        /// <summary>
        /// 使用下限
        /// </summary>        
        [JsonProperty("useLimit", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? use_lower_limit { get; set; }

        /// <summary>
        /// 奖励方式 0 常规(只奖励单方)  1 邀请奖励双方
        /// </summary>        
        [JsonProperty("reward", NullValueHandling = NullValueHandling.Ignore)]
        public int? reward { get; set; }

        /// <summary>
        /// 有效开始日期
        /// </summary>        
        [JsonProperty("beginOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? start_date { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>        
        [JsonProperty("endOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? end_date { get; set; }

        /// <summary>
        /// 奖励状态 0 未使用   1已使用  2 已过期
        /// </summary>        
        [JsonProperty("rewardState", NullValueHandling = NullValueHandling.Ignore)]
        public int? reward_state { get; set; }

        /// <summary>
        /// 奖励备注
        /// </summary>        
        [JsonProperty("remark", NullValueHandling = NullValueHandling.Ignore)]
        public string reward_remarks { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>        
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? entry_time { get; set; }
        /// <summary>
        /// 使用状态 0 未使用  1 已使用  3 已过期
        /// </summary>        
        [JsonProperty("useState", NullValueHandling = NullValueHandling.Ignore)]
        public int? act_state { get; set; }
        /// <summary>
        /// 应用项目
        /// </summary>        
        [JsonProperty("proid", NullValueHandling = NullValueHandling.Ignore)]
        public int? proid { get; set; }

        /// <summary>
        /// 是否选中　１选中　０未选中
        /// </summary>
        public int selectedItem { get; set; }

        /// <summary>
        /// 使用限制-限制可使用的标的期限(最小月-最大月)。要求大于等于最小月且小于等于最大月
        /// </summary>
        public string UseLifeLoan { get; set; }
        /// <summary>
        /// 使用限制-限制可使用的标的期限，配套描述信息
        /// </summary>
        public string UseLifeLoanMessage { get; set; }
    }

    public class RateBonusEntity
    {
        /// <summary>
        /// LogId
        /// </summary>        
        [JsonProperty("LogId", NullValueHandling = NullValueHandling.Ignore)]
        public int? LogId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>        
        [JsonProperty("UserId", NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }

        /// <summary>
        /// ActivityId
        /// </summary>        
        [JsonProperty("ActivityId", NullValueHandling = NullValueHandling.Ignore)]
        public int? ActivityId { get; set; }

        /// <summary>
        /// ActType
        /// </summary>        
        [JsonProperty("ActType", NullValueHandling = NullValueHandling.Ignore)]
        public int? ActType { get; set; }

        /// <summary>
        /// ActivityName
        /// </summary>        
        [JsonProperty("ActivityName", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityName { get; set; }

        /// <summary>
        /// UseBeginOn
        /// </summary>        
        [JsonProperty("UseBeginOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UseBeginOn { get; set; }

        /// <summary>
        /// UseEndOn
        /// </summary>        
        [JsonProperty("UseEndOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UseEndOn { get; set; }

        /// <summary>
        /// RecordId
        /// </summary>        
        [JsonProperty("RecordId", NullValueHandling = NullValueHandling.Ignore)]
        public int? RecordId { get; set; }

        /// <summary>
        /// TargetId
        /// </summary>        
        [JsonProperty("TargetId", NullValueHandling = NullValueHandling.Ignore)]
        public int? TargetId { get; set; }

        /// <summary>
        /// UsedRecordId
        /// </summary>        
        [JsonProperty("UsedRecordId", NullValueHandling = NullValueHandling.Ignore)]
        public int? UsedRecordId { get; set; }

        /// <summary>
        /// UsedTargetId
        /// </summary>        
        [JsonProperty("UsedTargetId", NullValueHandling = NullValueHandling.Ignore)]
        public int? UsedTargetId { get; set; }

        /// <summary>
        /// AddRate
        /// </summary>        
        [JsonProperty("AddRate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? AddRate { get; set; }

        /// <summary>
        /// 0 可用 1 已用 2已过期
        /// </summary>        
        [JsonProperty("UseStatus", NullValueHandling = NullValueHandling.Ignore)]
        public int? UseStatus { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>        
        [JsonProperty("CreateOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreateOn { get; set; }


        /// <summary>
        /// 是否选中　１选中　０未选中
        /// </summary>
        public int selectedItem { get; set; }

        /// <summary>
        /// 使用限制-限制可使用的标的期限(最小月-最大月)。要求大于等于最小月且小于等于最大月
        /// </summary>
        public string UseLifeLoan { get; set; }

        /// <summary>
        /// 使用限制-限制可使用的标的期限，配套描述信息
        /// </summary>
        public string UseLifeLoanMessage { get; set; }
    }
}
