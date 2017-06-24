using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuanglitouP2P.Models
{
    /// <summary>
    /// 返现统计视图模型类
    /// </summary>
    public class CashbackStatisticsViewModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string NO { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 项目期限
        /// </summary>
        public string ProjectTerm { get; set; }
        /// <summary>
        /// 累积投资金额
        /// </summary>
        public string InvestTotalAmount { get; set; }
        /// <summary>
        /// 累积投资笔数
        /// </summary>
        public string InvestTotalCount { get; set; }
        /// <summary>
        /// 累积应返现
        /// </summary>
        public string CashbackTotalAmount { get; set; }
        /// <summary>
        /// 返现状态
        /// </summary>
        public string Status { get; set; }

    }
}