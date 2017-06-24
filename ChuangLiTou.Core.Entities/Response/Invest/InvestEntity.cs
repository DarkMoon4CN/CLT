using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Invest
{
    public class InvestEntity
    {
        /// <summary>
        /// 应收利息
        /// </summary>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal receivableInterest { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>        
        [JsonProperty("recordId", NullValueHandling = NullValueHandling.Ignore)]
        public int? recordId { get; set; }

        /// <summary>
        /// 标ID
        /// </summary>        
        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public int? targetId { get; set; }
        /// <summary>
        /// 标的标题
        /// </summary>        
        [JsonProperty("targetTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string targetTitle { get; set; }

        /// <summary>
        /// 投资用户id
        /// </summary>        
        [JsonProperty("investUserId", NullValueHandling = NullValueHandling.Ignore)]
        public int? investMemberId { get; set; }

        /// <summary>
        /// 投资人姓名
        /// </summary>        
        [JsonProperty("investUserName", NullValueHandling = NullValueHandling.Ignore)]
        public string investMemberName { get; set; }

        /// <summary>
        /// 期限
        /// </summary>        
        [JsonProperty("deadLine", NullValueHandling = NullValueHandling.Ignore)]
        public int? deadLine { get; set; }

        /// <summary>
        /// 单位(月/天) 1 月  3 天
        /// </summary>        
        [JsonProperty("unitDay", NullValueHandling = NullValueHandling.Ignore)]
        public int? unitDay { get; set; }

        /// <summary>
        /// 年化收益
        /// </summary>        
        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? rate { get; set; }
        /// <summary>
        /// 项目总额
        /// </summary>        
        [JsonProperty("borrowTotalAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? borrowTotalAmount { get; set; }
        /// <summary>
        /// 已投总金额
        /// </summary>        
        [JsonProperty("fundTotalAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? fundTotalAmount { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>        
        [JsonProperty("investAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? investAmount { get; set; }

        /// <summary>
        /// 0 未还款    1 借款人自己还款  2 平台代还
        /// </summary>        
        [JsonProperty("paymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public int? paymentStatus { get; set; }

        /// <summary>
        /// 开始计算利息日期
        /// </summary>        
        [JsonProperty("rateBeginOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? rateBeginOn { get; set; }

        /// <summary>
        /// 投资到期日
        /// </summary>        
        [JsonProperty("investMaturity", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? investMaturity { get; set; }

        /// <summary>
        /// 投资时间
        /// </summary>        
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? createdOn { get; set; }


        /// <summary>
        /// 已购买人数
        /// </summary>        
        [JsonProperty("investNumber", NullValueHandling = NullValueHandling.Ignore)]
        public int? investNumber { get; set; }

        /// <summary>
        /// 还款日期
        /// </summary>        
        [JsonProperty("paymentDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? paymentDate { get; set; }


        /// <summary>
        /// 付息日
        /// </summary>        
        [JsonProperty("valueDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? valueDate { get; set; }

        /// <summary>
        /// 1 按月等额本息  3 每月还息，到期还本   4 一次性还本付息
        /// </summary>        
        [JsonProperty("paymentOption", NullValueHandling = NullValueHandling.Ignore)]
        public int? paymentOption { get; set; }

        /// <summary>
        /// 担保类型
        /// </summary>
        [JsonProperty("guaranteeWayId", NullValueHandling = NullValueHandling.Ignore)]
        public int? guarantee_way_id { get; set; }

        /// <summary>
        /// 每月付息日
        /// </summary>
        [JsonProperty("monthPaymentDate", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_payment_date { get; set; }

        /// <summary>
        /// 每月付息日
        /// </summary>
        [JsonProperty("jiaxiNum", NullValueHandling = NullValueHandling.Ignore)]
        public decimal jiaxiNum { get; set; }
        /// <summary>
        /// 每月付息日
        /// </summary>
        [JsonProperty("BonusAmt", NullValueHandling = NullValueHandling.Ignore)]
        public decimal BonusAmt { get; set; }


    }





}
