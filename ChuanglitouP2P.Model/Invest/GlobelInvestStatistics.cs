using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.Invest
{
    /// <summary>
    /// 全局方法-投资统计实体类
    /// </summary>
    public class GlobelInvestStatistics
    {
        /// <summary>
        /// 平台累积交易金额(0000,0000,000.00元)
        /// </summary>
        public string InvestTotalAmount { get; set; }

        /// <summary>
        /// 平台累积投资总人数(000人)
        /// </summary>
        public string InvestTotalPeople { get; set; }
    }
}
