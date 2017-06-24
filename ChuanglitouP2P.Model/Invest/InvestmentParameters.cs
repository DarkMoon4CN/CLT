using System;

namespace ChuanglitouP2P.Model.Invest
{
    public class InvestmentParameters
    {

        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 期限
        /// </summary>
        public int Circle { get; set; }

        /// <summary>
        /// 期限类型 1月 / 3天  
        /// </summary>
        public int CircleType { get; set; }

        /// <summary>
        /// 投资日期
        /// </summary>
        public DateTime InvestDate { get; set; }

        /// <summary>
        /// 借款日期
        /// </summary>
        public string ReleaseDate { get; set; }


        /// <summary>
        /// 每月付息日
        /// </summary>
        public int Payinterest  { get; set; }


        /// <summary>
        /// 投资到期结束日期
        /// </summary>
        public DateTime Investmentenddate { get; set; }


        /// <summary>
        /// 投资身份对象  1投资人需要不进行四舍五入，2借款人需进行四舍五入
        /// </summary>
        public int InvestObject { get; set; }


        public bool IsThirtyDayMonth { get; set; }

        /// <summary>
        /// 年利率
        /// </summary>
        public double NominalYearRate { get; set; }


        /// <summary>
        /// 管理费率
        /// </summary>
        public double OverheadsRate { get; set; }

        /// <summary>
        /// 还款方式
        /// </summary>
        public int RepaymentMode { get; set; }

        /// <summary>
        /// 奖厉比例
        /// </summary>
        public double RewardRate { get; set; }


    }
}
