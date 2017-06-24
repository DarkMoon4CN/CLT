using System;
using System.Collections.Generic;

namespace ChuangLitouP2P.Models
{

    /// <summary>
    /// 活动现金奖励
    /// </summary>
    public class MActCash
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActName { get; set; }


        /// <summary>
        /// 活动时间 开始
        /// </summary>
        public DateTime ActStarttime { get; set; }

        /// <summary>
        /// 活动时间 开始
        /// </summary>
        public DateTime ActEndtime { get; set; }


        /// <summary>
        /// 面向用户
        /// </summary>
        public int ActUser { get; set; }

        /// <summary>
        /// 投资金额要求 1 按百分比赠送  2 投投资赠送
        /// </summary>
        public int require { get; set; }

        /// <summary>
        /// 封顶金额
        /// </summary>
        public decimal TopAmt { get; set; }

        /// <summary>
        /// 封顶人数
        /// </summary>
        public decimal TopNum { get; set; }


        /// <summary>
        /// 赠送金额阶段列表
        /// </summary>
        public List<MAmtList> MAmtList { get; set; }

        /// <summary>
        /// 邀请好友 续投按一定金额赠送
        /// </summary>
        public int require1 { get; set; }



        /// <summary>
        /// 单笔佣金
        /// </summary>
        public decimal TopAmt1 { get; set; }

        /// <summary>
        /// 邀请首笔投资赠送金额
        /// </summary>
        public decimal Cash { get; set; }

    }
}
