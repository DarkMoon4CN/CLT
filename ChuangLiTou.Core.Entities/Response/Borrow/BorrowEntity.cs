using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Borrow
{
    public class BorrowEntity
    {
        /// <summary>
        /// 标ID
        /// </summary>
        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public int? targetId { get; set; }
        /// <summary>
        /// 借款人id
        /// </summary>
        [JsonProperty("borrowerRegisterId", NullValueHandling = NullValueHandling.Ignore)]
        public int? borrower_registerid { get; set; }
        /// <summary>
        /// 借款人姓名
        /// </summary>
        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string userName { get; set; }
        /// <summary>
        /// 用于合同编号
        /// </summary>
        [JsonProperty("loanNumber", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? loan_number { get; set; }
        /// <summary>
        /// 借款标题
        /// </summary>
        [JsonProperty("borrowingTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string borrowing_title { get; set; }
        /// <summary>
        /// 借款缩略图
        /// </summary>
        [JsonProperty("borrowing_thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string borrowing_thumbnail { get; set; }
        /// <summary>
        /// 项目类型id
        /// </summary>
        [JsonProperty("typeId", NullValueHandling = NullValueHandling.Ignore)]
        public int? project_type_id { get; set; }
        /// <summary>
        /// 年利率
        /// </summary>
        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? annual_interest_rate { get; set; }
        /// <summary>
        /// 借款金额
        /// </summary>
        [JsonProperty("balance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? borrowing_balance { get; set; }
        /// <summary>
        /// 借款期限
        /// </summary>
        [JsonProperty("deadLine", NullValueHandling = NullValueHandling.Ignore)]
        public int? life_of_loan { get; set; }
        /// <summary>
        ///单位(月/天) 1 月  3 天
        /// </summary>
        [JsonProperty("unitDay", NullValueHandling = NullValueHandling.Ignore)]
        public int? unit_day { get; set; }
        /// <summary>
        /// 借款日期
        /// </summary>
        [JsonProperty("releaseDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? release_date { get; set; }
        /// <summary>
        /// 统一起息日
        /// </summary>
        [JsonProperty("valueDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? value_date { get; set; }
        /// <summary>
        /// 每月付息日期 
        /// </summary>
        [JsonProperty("monthPaymentDate", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_payment_date { get; set; }
        /// <summary>
        /// 还款日期
        /// </summary>
        [JsonProperty("repaymentDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? repayment_date { get; set; }
        /// <summary>
        /// 标的投资单笔最小额度限制。0为不限制 
        /// </summary>
        [JsonProperty("minNum", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum { get; set; }
        /// <summary>
        /// 标的投资单笔最大额度限制。0为不限制 
        /// </summary>
        [JsonProperty("maxNum", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maxmum { get; set; }
        /// <summary>
        /// 担保公司id
        /// </summary>
        [JsonProperty("companyid", NullValueHandling = NullValueHandling.Ignore)]
        public int? companyid { get; set; }
        /// <summary>
        /// 担保方式id
        /// </summary>
        [JsonProperty("guarantee_way_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? guarantee_way_id { get; set; }
        /// <summary>
        /// 1 按月等额本息 3 每月还息，到期还本  4 一次性还本付息
        /// </summary>
        [JsonProperty("paymentOption", NullValueHandling = NullValueHandling.Ignore)]
        public int? payment_options { get; set; }
        /// <summary>
        /// 招标开始时间
        /// </summary>
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? start_time { get; set; }
        /// <summary>
        /// 招标结束时间
        /// </summary>
        [JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? end_time { get; set; }
        /// <summary>
        /// 资询服务费费率
        /// </summary>
        [JsonProperty("service_charge", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? service_charge { get; set; }
        /// <summary>
        /// 借款帐户管理费费率
        /// </summary>
        [JsonProperty("loan_management_fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? loan_management_fee { get; set; }
        /// <summary>
        /// 投资帐户管理费费率
        /// </summary>
        [JsonProperty("investors_management_fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? investors_management_fee { get; set; }
        /// <summary>
        /// 普通逾期管理费费率
        /// </summary>
        [JsonProperty("ordinary_overdue_management_fees", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ordinary_overdue_management_fees { get; set; }
        /// <summary>
        /// seriously_overdue_management_fees
        /// </summary>
        [JsonProperty("seriously_overdue_management_fees", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? seriously_overdue_management_fees { get; set; }
        /// <summary>
        /// ordinary_overdue_penalty
        /// </summary>
        [JsonProperty("ordinary_overdue_penalty", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ordinary_overdue_penalty { get; set; }
        /// <summary>
        /// seriously_overdue_penalty
        /// </summary>
        [JsonProperty("seriously_overdue_penalty", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? seriously_overdue_penalty { get; set; }
        /// <summary>
        /// transfer_Expenses
        /// </summary>
        [JsonProperty("transfer_Expenses", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? transfer_Expenses { get; set; }
        /// <summary>
        /// 根据和投资明细表触发器计算 已筹款
        /// </summary>
        [JsonProperty("fundraisingAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? fundraising_amount { get; set; }
        /// <summary>
        ///  2 复审通过(开标上线)     3 满标 (还款中)     4放款 (还款中)   5 已还清   
        /// </summary>
        [JsonProperty("tenderState", NullValueHandling = NullValueHandling.Ignore)]
        public int? tender_state { get; set; }
        /// <summary>
        /// 0 未放款  1已放款
        /// </summary>
        [JsonProperty("full_scale_loan", NullValueHandling = NullValueHandling.Ignore)]
        public int? full_scale_loan { get; set; }
        /// <summary>
        /// 0 未满标 1 未返还
        /// </summary>
        [JsonProperty("flow_return", NullValueHandling = NullValueHandling.Ignore)]
        public int? flow_return { get; set; }
        /// <summary>
        /// recommend
        /// </summary>
        [JsonProperty("recommend", NullValueHandling = NullValueHandling.Ignore)]
        public int? recommend { get; set; }
        /// <summary>
        /// sys_time
        /// </summary>
        [JsonProperty("SysTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? sys_time { get; set; }
        /// <summary>
        /// 默认为 0   在生成原款计划时触发器自动生成还款期数总数
        /// </summary>
        [JsonProperty("repaymentperiods", NullValueHandling = NullValueHandling.Ignore)]
        public int? repaymentperiods { get; set; }
        /// <summary>
        /// reviewremarks
        /// </summary>
        [JsonProperty("reviewremarks", NullValueHandling = NullValueHandling.Ignore)]
        public string reviewremarks { get; set; }
        /// <summary>
        /// recheckremarks
        /// </summary>
        [JsonProperty("recheckremarks", NullValueHandling = NullValueHandling.Ignore)]
        public string recheckremarks { get; set; }
        /// <summary>
        /// guarantee_fee
        /// </summary>
        [JsonProperty("guarantee_fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? guarantee_fee { get; set; }
        /// <summary>
        /// consultingAMT
        /// </summary>
        [JsonProperty("consultingAMT", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? consultingAMT { get; set; }
        /// <summary>
        /// guaranteeAMT
        /// </summary>
        [JsonProperty("guaranteeAMT", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? guaranteeAMT { get; set; }
        /// <summary>
        /// B_Rates
        /// </summary>
        [JsonProperty("B_Rates", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? B_Rates { get; set; }
        /// <summary>
        /// H_Repayment_Amt
        /// </summary>
        [JsonProperty("repaymentAmt", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? H_Repayment_Amt { get; set; }
        /// <summary>
        /// Repay_Time
        /// </summary>
        [JsonProperty("Repay_Time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Repay_Time { get; set; }
        /// <summary>
        /// G_contract_Path
        /// </summary>
        [JsonProperty("G_contract_Path", NullValueHandling = NullValueHandling.Ignore)]
        public string G_contract_Path { get; set; }
        /// <summary>
        /// IsUse
        /// </summary>
        [JsonProperty("IsUse", NullValueHandling = NullValueHandling.Ignore)]
        public int? IsUse { get; set; }
        /// <summary>
        /// indexorder
        /// </summary>
        [JsonProperty("indexorder", NullValueHandling = NullValueHandling.Ignore)]
        public int? indexorder { get; set; }

        /// <summary>
        /// 投标数
        /// </summary> 

        [JsonProperty("investCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? investCount { get; set; }


        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedOn { get; set; }

        [JsonProperty("ServerTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ServerTime { get { return DateTime.Now; } }
    }
}
