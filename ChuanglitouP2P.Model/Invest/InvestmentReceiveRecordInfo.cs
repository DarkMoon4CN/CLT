using System;

namespace ChuanglitouP2P.Model.Invest
{
    /// <summary>
    /// 投资记录信息
    /// </summary>
    public class InvestmentReceiveRecordInfo
    {

        /// <summary>
        /// 实际收到日期
        /// </summary>
        public DateTime ActualReceiveDate { get; set; }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 应收总额
        /// </summary>
        public decimal Amount { get; set; }


        /// <summary>
        /// 应收本息
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 当前的分期数
        /// </summary>
        public int CurrentInstallments { get; set; }

        public string ID { get; set; }

        
      
        /// <summary>
        /// 应收利息
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// 利息状态
        /// </summary>
        public int InterestStatus { get; set; }

        /// <summary>
        /// 投资id
        /// </summary>
        public string InvestmentID { get; set; }

        /// <summary>
        /// 备忘录
        /// </summary>
        public string Memo { get; set; }


        /// <summary>
        /// 当前期的投资起息日期
        /// </summary>
        public DateTime interestvalue_date { get; set; }

        
        /// <summary>
        /// 回报日期
        /// </summary>
        public DateTime NominalReceiveDate { get; set; }

        /// <summary>
        /// 管理费用
        /// </summary>
        public decimal Overheads { get; set; }

        /// <summary>
        /// 应收本金
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        /// 本金状态
        /// </summary>
        public int PrincipalStatus { get; set; }

        /// <summary>
        /// 分期付款总计
        /// </summary>
        public int TotalInstallments { get; set; }

        /// <summary>
        /// 计息天数
        /// </summary>
        public int TotalDays { get; set; }

        public DateTime UpdateTime { get; set; }

    }
}
