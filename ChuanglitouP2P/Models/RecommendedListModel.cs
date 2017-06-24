using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuanglitouP2P.Models
{
    /// <summary>
    /// 推荐人列表视图模型类
    /// </summary>
    public class RecommendedListViewModel
    {
        /// <summary>
        /// 推荐人
        /// </summary>
        public string Referee { get; set; }
        /// <summary>
        /// 邀请人数
        /// </summary>
        public int RecommendCount { get; set; }
        /// <summary>
        /// 投资次数
        /// </summary>
        public int InvestedTimes { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public string InvestTotalAmount { get; set; }
    }
}