using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Invest
{
    public class ResponseIncomeEntity
    {
        /// <summary>
        /// 年月
        /// </summary>
        public string shortDate { get; set; }
        public List<ResponseInvestIncomeEntity> lst { get; set; }

    }


    public class ResponseInvestIncomeEntity
    {

        /// <summary>
        /// 标Id
        /// </summary>        
        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public int? targetid { get; set; }

        /// <summary>
        /// 合同号
        /// </summary>        
        [JsonProperty("loanNumber", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? loan_number { get; set; }

        /// <summary>
        /// 标题
        /// </summary>        
        [JsonProperty("borrowingTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string borrowing_title { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>        
        [JsonProperty("borrowingBalance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? borrowing_balance { get; set; }

        /// <summary>
        /// 投资金额
        /// </summary>        
        [JsonProperty("investmentAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? investment_amount { get; set; }

        /// <summary>
        /// income_statement_id
        /// </summary>        
        [JsonProperty("income_statement_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? income_statement_id { get; set; }

        /// <summary>
        /// 借款人Id
        /// </summary>        
        [JsonProperty("borrowerRegisterid", NullValueHandling = NullValueHandling.Ignore)]
        public int? borrower_registerid { get; set; }

        /// <summary>
        /// annual_revenue
        /// </summary>        
        [JsonProperty("annual_revenue", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? annual_revenue { get; set; }

        /// <summary>
        /// 投资时间
        /// </summary>        
        [JsonProperty("investedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? invest_time { get; set; }

        /// <summary>
        /// current_investment_period
        /// </summary>        
        [JsonProperty("current_investment_period", NullValueHandling = NullValueHandling.Ignore)]
        public int? current_investment_period { get; set; }

        /// <summary>
        /// 起息日
        /// </summary>        
        [JsonProperty("valueDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? value_date { get; set; }


        /// <summary>
        /// 预期还款日
        /// </summary>        
        [JsonProperty("interestPaymentDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime interest_payment_date;

        /// <summary>
        /// 真实还款时间
        /// </summary>        
        [JsonProperty("repaymentOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime repayment_period { get; set; }
        /// <summary>
        /// 付息日
        /// </summary>        
        [JsonProperty("day", NullValueHandling = NullValueHandling.Ignore)]
        public string day { get; set; }


        /// <summary>
        /// 还款金额
        /// </summary>        
        [JsonProperty("repaymentAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? repayment_amount { get; set; }


        /// <summary>
        /// 投资人Id
        /// </summary>        
        [JsonProperty("investorUserId", NullValueHandling = NullValueHandling.Ignore)]
        public int? investor_registerid { get; set; }

        /// <summary>
        /// 还款状态 1提前还款 2待还款 3延迟（已还款） 4已还款 5 延迟（待还款）
        /// </summary>        
        [JsonProperty("paymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public int? payment_status { get; set; }

        /// <summary>
        /// 记录Id
        /// </summary>        
        [JsonProperty("bid_records_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? bid_records_id { get; set; }

        /// <summary>
        /// 年化
        /// </summary>        
        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? annual_interest_rate { get; set; }

        /// <summary>
        /// registerid
        /// </summary>        
        [JsonProperty("registerid", NullValueHandling = NullValueHandling.Ignore)]
        public int? registerid { get; set; }

        /// <summary>
        /// available_balance
        /// </summary>        
        [JsonProperty("available_balance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_balance { get; set; }

        /// <summary>
        /// payment_options
        /// </summary>        
        [JsonProperty("payment_options", NullValueHandling = NullValueHandling.Ignore)]
        public int? payment_options { get; set; }

        /// <summary>
        /// OutCustId
        /// </summary>        
        [JsonProperty("OutCustId", NullValueHandling = NullValueHandling.Ignore)]
        public string OutCustId { get; set; }

        /// <summary>
        /// loan_management_fee
        /// </summary>        
        [JsonProperty("loan_management_fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? loan_management_fee { get; set; }

        /// <summary>
        /// Principal
        /// </summary>        
        [JsonProperty("Principal", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Principal { get; set; }

        /// <summary>
        /// interestDay
        /// </summary>        
        [JsonProperty("interestDay", NullValueHandling = NullValueHandling.Ignore)]
        public int? interestDay { get; set; }

        /// <summary>
        /// TotalInstallments
        /// </summary>        
        [JsonProperty("TotalInstallments", NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalInstallments { get; set; }

        /// <summary>
        /// ordstate
        /// </summary>        
        [JsonProperty("ordstate", NullValueHandling = NullValueHandling.Ignore)]
        public int? ordstate { get; set; }

        /// <summary>
        /// username
        /// </summary>        
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string username { get; set; }

        /// <summary>
        /// realname
        /// </summary>        
        [JsonProperty("realname", NullValueHandling = NullValueHandling.Ignore)]
        public string realname { get; set; }

        /// <summary>
        /// mobile
        /// </summary>        
        [JsonProperty("mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string mobile { get; set; }

        /// <summary>
        /// orderid
        /// </summary>        
        [JsonProperty("orderid", NullValueHandling = NullValueHandling.Ignore)]
        public string orderid { get; set; }
    }
}
