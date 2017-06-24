using System.Collections.Generic;

namespace ChuangLiTou.Core.Helpers.Entity.Invest
{
    /// <summary>
    /// 投资的总收入
    /// </summary>
    public class InvestmentTotalIncome
    {
        /// <summary>
        /// 月化收益率
        /// </summary>
        public double ActualMonthRate { get; set; }

        public double ActualQuartRate { get; set; }

        /// <summary>
        /// 年化收益率
        /// </summary>
        public double ActualYearRate { get; set; }

       
        public decimal CleanTotalEarnings { get; set; }

        public decimal CleanTotalIncome { get; set; }

        /// <summary>
        /// 利息
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// 管理费用
        /// </summary>
        public decimal Overheads { get; set; }

        public List<InvestmentReceiveRecordInfo> ReceiveRecords { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Reward { get; set; }

        /// <summary>
        /// 收益总计
        /// </summary>
        public decimal TotalEarnings { get; set; }

        /// <summary>
        /// 还款总计
        /// </summary>
        public decimal TotalIncome { get; set; }

    }
}
