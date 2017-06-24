using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Record
{
    /// <summary>
    /// Class RecordEntity.
    /// </summary>
    public class RecordEntity
    {
        /// <summary>
		/// bid_records_id
        /// </summary>        
        [JsonProperty("bid_records_id",NullValueHandling = NullValueHandling.Ignore)]	
        public int? bid_records_id{get;set;}   
				       
             /// <summary>
		/// borrower_registerid
        /// </summary>        
        [JsonProperty("borrower_registerid",NullValueHandling = NullValueHandling.Ignore)]	
        public int? borrower_registerid{get;set;}   
				       
             /// <summary>
		/// targetid
        /// </summary>        
        [JsonProperty("targetid",NullValueHandling = NullValueHandling.Ignore)]	
        public int? targetid{get;set;}   
				       
             /// <summary>
		/// loan_number
        /// </summary>        
        [JsonProperty("loan_number",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal? loan_number{get;set;}   
				       
             /// <summary>
		/// annual_interest_rate
        /// </summary>        
        [JsonProperty("annual_interest_rate",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal? annual_interest_rate{get;set;}   
				       
             /// <summary>
		/// 分期总数
        /// </summary>        
        [JsonProperty("current_period",NullValueHandling = NullValueHandling.Ignore)]	
        public int? current_period{get;set;}   
				       
        /// <summary>
        /// 投资金额
        /// </summary>        
        [JsonProperty("investmentAmount",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal investment_amount{get;set;}   
				       
             /// <summary>
		/// value_date
        /// </summary>        
        [JsonProperty("value_date",NullValueHandling = NullValueHandling.Ignore)]	
        public DateTime? value_date{get;set;}   
				       
             /// <summary>
		/// investment_maturity
        /// </summary>        
        [JsonProperty("investment_maturity",NullValueHandling = NullValueHandling.Ignore)]	
        public DateTime? investment_maturity{get;set;}   
				       
        /// <summary>
        /// 投资时间
        /// </summary>        
        [JsonProperty("investedOn",NullValueHandling = NullValueHandling.Ignore)]	
        public DateTime invest_time{get;set;}   
				       
             /// <summary>
		/// 1 成功  2 失败  3流标返还  如果是投标返还的话通过触发器更改对应金额问题
        /// </summary>        
        [JsonProperty("invest_state",NullValueHandling = NullValueHandling.Ignore)]	
        public int? invest_state{get;set;}   
				       
             /// <summary>
		/// 1 无返还  2已返还
        /// </summary>        
        [JsonProperty("flow_return",NullValueHandling = NullValueHandling.Ignore)]	
        public int? flow_return{get;set;}   
				       
             /// <summary>
		/// repayment_amount
        /// </summary>        
        [JsonProperty("repayment_amount",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal? repayment_amount{get;set;}   
				       
             /// <summary>
		/// repayment_period
        /// </summary>        
        [JsonProperty("repayment_period",NullValueHandling = NullValueHandling.Ignore)]	
        public DateTime? repayment_period{get;set;}   
				       
             /// <summary>
		/// investor_registerid
        /// </summary>        
        [JsonProperty("investor_registerid",NullValueHandling = NullValueHandling.Ignore)]	
        public int? investor_registerid{get;set;}   
				       
             /// <summary>
		/// 0 未还款    1 借款人自己还款  2 平台代还
        /// </summary>        
        [JsonProperty("payment_status",NullValueHandling = NullValueHandling.Ignore)]	
        public int? payment_status{get;set;}   
				       
             /// <summary>
		/// withoutinterest
        /// </summary>        
        [JsonProperty("withoutinterest",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal? withoutinterest{get;set;}   
				       
             /// <summary>
		/// haveinterest
        /// </summary>        
        [JsonProperty("haveinterest",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal? haveinterest{get;set;}   
				       
             /// <summary>
		/// contractid
        /// </summary>        
        [JsonProperty("contractid",NullValueHandling = NullValueHandling.Ignore)]	
        public int? contractid{get;set;}   
				       
             /// <summary>
		/// contractpath
        /// </summary>        
        [JsonProperty("contractpath",NullValueHandling = NullValueHandling.Ignore)]				
        public string contractpath{get;set;}   
				       
             /// <summary>
		/// invitationcode
        /// </summary>        
        [JsonProperty("invitationcode",NullValueHandling = NullValueHandling.Ignore)]				
        public string invitationcode{get;set;}   
				       
             /// <summary>
		/// OrdId
        /// </summary>        
        [JsonProperty("OrdId",NullValueHandling = NullValueHandling.Ignore)]	
        public decimal? OrdId{get;set;}   
				       
             /// <summary>
		/// ordstate
        /// </summary>        
        [JsonProperty("ordstate",NullValueHandling = NullValueHandling.Ignore)]	
        public int? ordstate{get;set;}   
				       
             /// <summary>
		/// IsLoans
        /// </summary>        
        [JsonProperty("IsLoans",NullValueHandling = NullValueHandling.Ignore)]	
        public int? IsLoans{get;set;}   
    }
}
